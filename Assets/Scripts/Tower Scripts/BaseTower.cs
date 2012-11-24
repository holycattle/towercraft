using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BaseTower : MonoBehaviour {
	// Tower Part Constants
	public const int TOWER_BASE = 0;
	public const int TOWER_TURRET = 1;
	public const int TOWER_COMPLETE = 2;	// Also acts as the max # of components per tower

	// Tower Stat Constants (replacing the Enum)
	public const int STAT_DAMAGE = 0;
	public const int STAT_RANGE = 1;
	public const int STAT_FIRINGRANGE = 2;

	// Tower Stat Multipliers
	public const int MULT_DAMAGE = 1;
	public const int MULT_RANGE = 8;
	public const int MULT_FIRINGRANGE = 1;

	// Tower Parts
	private TowerComponent[] _towerComponents;

	// Tower Properties (Derived from Parts)
	public string towerName;
	private ModifiedStat[] stats;
	public float towerRange;		// (meters)
	public float firingInterval;	// (seconds wait per shot)
	public GameObject missile;
	public bool isFiring;

	// Tower Logic
	private List<GameObject> _enemiesInRange;	// List of enemies in range
	private float _timeTillFire;				// # of seconds till you can fire again
	private GameObject _target;
	private Transform _missileSource;

	void Awake() {
		_towerComponents = new TowerComponent[TOWER_COMPLETE];

		stats = new ModifiedStat[Enum.GetValues(typeof(Stat)).Length];
		for (int i = 0; i < Enum.GetValues(typeof(Stat)).Length; i++) {
			stats[i] = new ModifiedStat();
		}
	}

	void Start() {
		_enemiesInRange = new List<GameObject>();
		_timeTillFire = 0;
	}

	void Update() {
		// Make the Cannon face the Enemy
		if (_target != null) {
			float distance = Vector2.Distance(new Vector2(_target.transform.position.x, _target.transform.position.z), new Vector2(transform.position.x, transform.position.z));
			if (distance < stats[(int)Stat.Range].AdjustedBaseValue * LevelController.TILE_SIZE) {
				_missileSource.rotation = Quaternion.Slerp(_missileSource.rotation, Quaternion.LookRotation(_missileSource.position - _target.transform.position), 0.5f);
			}
		}

//		Debug.Log("Count: " + _enemiesInRange.Count);

		// Try to Fire
		if (_timeTillFire <= 0 && isFiring) {
			if (_enemiesInRange.Count > 0) {
				_target = _enemiesInRange[0];
				while (!(_target != null)) { // If the target has already been destroyed
					_enemiesInRange.RemoveAt(0);
					if (_enemiesInRange.Count > 0) {
						_target = _enemiesInRange[0];
					} else {
						_target = null;
						break;
					}
				}

				// Missile Firing
				if (_target != null && _timeTillFire <= 0) {
					GameObject g = Instantiate(missile, _missileSource.position, Quaternion.identity) as GameObject;

					// Set target
					BaseMissile b = g.GetComponent<BaseMissile>();
					b.Target = _enemiesInRange[0];
					b.damage = stats[(int)Stat.Damage].AdjustedBaseValue;

					_timeTillFire += firingInterval;
				}
			}
		}

		// Update FiringCounter
		if (_timeTillFire > 0)
			_timeTillFire -= Time.deltaTime;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Enemy") {
			_enemiesInRange.Add(other.transform.root.gameObject);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Enemy")
			_enemiesInRange.Remove(other.transform.root.gameObject);
	}

	private void UpdateStats() {
		Debug.Log("Update Stats!");
		for (int i = 0; i < TOWER_COMPLETE; i++) {
			Debug.Log("Tower: " + _towerComponents[i].componentName);
		}

		_missileSource = _towerComponents[TOWER_TURRET].transform.Find("MissileSource");

		// Clear all current modifiers
		foreach (Stat s in Enum.GetValues(typeof(Stat))) {
			stats[(int)s].ClearModifiers();
		}

		// Add modifiers
		TowerTurret temp = (TowerTurret)_towerComponents[TOWER_TURRET];
		Debug.Log("Length Attributes: " + temp.componentName + " > " + temp.attributes.Count);
		foreach (ModifyingAttribute m in temp.attributes) {
			stats[(int)m.stat].AddModifier(m);
		}

		// Set Firing Interval
		firingInterval = 1f;

		// Set Collider Range
		GetComponent<SphereCollider>().radius = stats[(int)Stat.Range].AdjustedBaseValue * LevelController.TILE_SIZE / MULT_RANGE;

		isFiring = true;

		// Debug Out all Stats
		for (int i = 0; i < Enum.GetValues(typeof(Stat)).Length; i++) {
			Debug.Log(((Stat)i).ToString() + ": " + stats[i].AdjustedBaseValue);
		}
	}

	public void AddNextComponent(TowerComponent tPrefab) {
		int next = GetNextComponent();
//		TowerComponent comp = g.GetComponent<TowerComponent>();
//		switch (next) {
//			case TOWER_BASE:
//				comp = g.GetComponent<TowerBase>();
//				break;
//			case TOWER_STEM:
//				comp = g.GetComponent<TowerStem>();
//				break;
//			case TOWER_TURRET:
//				comp = g.GetComponent<TowerTurret>();
//				break;
//		}

		Debug.Log("Adding Component: " + tPrefab.componentName);

		// Instantiate the Game Object
		TowerComponent towerInstance = Instantiate(tPrefab, transform.position + GetNextComponentPosition(), Quaternion.identity) as TowerComponent;
		towerInstance.transform.parent = transform;
		_towerComponents[next] = towerInstance;	// Note: You can't use t here because it is the component of the prefab.

		// Copy the PREFAB's ModifyingAttributes to the INSTANCE
		if (towerInstance.componentType == TOWER_TURRET) {
			foreach (ModifyingAttribute m in ((TowerTurret) tPrefab).attributes) {
				((TowerTurret)towerInstance).attributes.Add(m);
//				Debug.Log("Adding Attribute to INSTANCE!");
			}
		}

		// Destroy the Prefab (Save on space)
		Destroy(tPrefab.gameObject);

		if (tPrefab.componentType == TOWER_TURRET) {
			Debug.Log("Component PREFAB Count: " + ((TowerTurret)tPrefab).attributes.Count);
			Debug.Log("Component INSTANCE Count: " + ((TowerTurret)towerInstance).attributes.Count);
		}

		if (next == TOWER_TURRET) {
			UpdateStats();
		}
	}

	public TowerComponent SwapComponent(TowerComponent tPrefab) {
		int swapType = tPrefab.componentType;

		_towerComponents[swapType].transform.position = new Vector3(0, 50, 0);	// Move to a far away place
		_towerComponents[swapType].transform.parent = null;	// DE-Parent
		_towerComponents[swapType].gameObject.SetActiveRecursively(false);
		TowerComponent swappedOutComponent = _towerComponents[swapType].GetComponent<TowerComponent>();

		TowerComponent towerInstance = Instantiate(tPrefab, transform.position + GetNextComponentPosition(swapType), Quaternion.identity) as TowerComponent;
		towerInstance.transform.parent = transform;
		_towerComponents[swapType] = towerInstance;	// Note: You can't use t here because it is the component of the prefab.

		// Copy the PREFAB's ModifyingAttributes to the INSTANCE
		if (tPrefab.componentType == TOWER_TURRET) {
			foreach (ModifyingAttribute m in ((TowerTurret) tPrefab).attributes) {
				((TowerTurret)towerInstance).attributes.Add(m);
			}
		}

		UpdateStats();
		return swappedOutComponent;
	}

	public int GetNextComponent() {
		if (_towerComponents[TOWER_BASE] == null) {
			return TOWER_BASE;
		} else if (_towerComponents[TOWER_TURRET] == null) {
			return TOWER_TURRET;
		} else {
			return TOWER_COMPLETE;
		}
	}

	private Vector3 GetNextComponentPosition(int type) {
		if (type == TOWER_BASE) {
			return Vector3.zero;
		} else if (type == TOWER_TURRET) {
			return  _towerComponents[TOWER_BASE].NextComponentPosition();
		}
		return Vector3.zero;
	}

	private Vector3 GetNextComponentPosition() {
		return GetNextComponentPosition(GetNextComponent());
	}

	public static int CalculateStatLevel(Stat s, int amt) {
		switch (s) {
			case Stat.Damage:
				return Math.Max(1, (int)Math.Ceiling((float)amt / BaseTower.MULT_DAMAGE));
			case Stat.Range:
				return Math.Max(1, (int)Math.Ceiling((float)amt / BaseTower.MULT_RANGE));
			case Stat.FiringRate:
				return  Math.Max(1, (int)Math.Ceiling((float)amt / BaseTower.MULT_FIRINGRANGE));
		}
		return 0;
	}
}

public enum Stat {
	Range,
	Damage,
	FiringRate,
	SplashRange
}