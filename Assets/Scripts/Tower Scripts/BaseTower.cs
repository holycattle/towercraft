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

	// Tower Stats BaseLines
	public const float BASE_RANGE = 4.0f;

	// Tower Stat Multipliers
//	public const int MULT_DAMAGE = 2;
//	public const int MULT_RANGE = 8;		// 1-8 = L1, 9-16 = L2
//	public const int MULT_FIRINGRATE = 1;

	// Tower Parts
	private TowerComponent[] _towerComponents;

	// Tower Properties (Derived from Parts)
	public string towerName;
	private ModifiedStat[] stats;
	public float towerRange;		// (meters)
	public float firingInterval;	// (seconds wait per shot)
	public GameObject missile;
	public GameObject statusAilment;
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
		// Try to Fire
		if (_timeTillFire <= 0 && isFiring) {
			if (_target == null) {
				_target = FindClosestTarget();
			} else {
				if (_enemiesInRange.Contains(_target)) {
					// Target IN range
					// Do not change target.
				} else {
					// Target OUT of range
					// Change target.
					_target = FindClosestTarget();
				}
			}

			// Missile Firing
			if (_target != null) {
				GameObject g = Instantiate(missile, _missileSource.position, Quaternion.identity) as GameObject;

				// Set target
				BaseMissile b = g.GetComponent<BaseMissile>();
				b.Target = _target;
				b.damage = (int)stats[(int)Stat.Damage].AdjustedBaseValue;
				b.statusAilment = this.statusAilment;

				_timeTillFire += firingInterval;
			}
		}

		// Make the Cannon face the Enemy
		if (_target != null) {
			float distance = Vector2.Distance(new Vector2(_target.transform.position.x, _target.transform.position.z), new Vector2(transform.position.x, transform.position.z));
			if (distance < stats[(int)Stat.Range].AdjustedBaseValue * LevelController.TILE_SIZE) {
				_missileSource.rotation = Quaternion.Slerp(_missileSource.rotation, Quaternion.LookRotation(_missileSource.position - _target.transform.position), 0.5f);
			}
		}

		// Update FiringCounter
		if (_timeTillFire > 0)
			_timeTillFire -= Time.deltaTime;
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Enemy") {
			_enemiesInRange.Add(other.transform.gameObject);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Enemy")
			_enemiesInRange.Remove(other.transform.gameObject);
	}

	private GameObject FindClosestTarget() {
		if (_enemiesInRange.Count > 0) {
			// Get closest
			GameObject closest = null;
			float closestDistance = 9999999;
			foreach (GameObject g in _enemiesInRange) {
				if (g != null) {
					if (Vector3.Distance(g.transform.position, transform.position) < closestDistance) {
						closest = g;
						closestDistance = Vector3.Distance(g.transform.position, transform.position);
					}
				}
			}
			return closest;
		}
		return null;
	}

	private void UpdateStats() {
//		Debug.Log("Update Stats!");
//		for (int i = 0; i < TOWER_COMPLETE; i++) {
//			Debug.Log("Tower: " + _towerComponents[i].componentName);
//		}

		if (_towerComponents[TOWER_TURRET] == null) {
			_missileSource = null;
		} else {
			_missileSource = _towerComponents[TOWER_TURRET].transform.Find("MissileSource");
		}

		// Clear all current modifiers
		foreach (Stat s in Enum.GetValues(typeof(Stat))) {
			stats[(int)s].ClearModifiers();
		}

		// Add modifiers
		if (_towerComponents[TOWER_TURRET] == null) {

		} else {
			TowerTurret temp = (TowerTurret)_towerComponents[TOWER_TURRET];
			foreach (ModifyingAttribute m in temp.attributes) {
				stats[(int)m.stat].AddModifier(m);
			}
		}

		// Set Firing Interval
		if (stats[(int)Stat.FiringRate].AdjustedBaseValue == 0) {
			firingInterval = 0;
		} else {
			firingInterval = 1f / stats[(int)Stat.FiringRate].AdjustedBaseValue;
		}

		// Set Collider Range
		if (stats[(int)Stat.Range].AdjustedBaseValue == 0) {

		} else {
			GetComponent<SphereCollider>().radius = stats[(int)Stat.Range].AdjustedBaseValue * LevelController.TILE_SIZE;
		}


		if (_towerComponents[TOWER_TURRET] == null) {
			missile = null;
			statusAilment = null;
		} else {
			// Load the Missile
			TowerTurret temp = (TowerTurret)_towerComponents[TOWER_TURRET];
			if (temp.missile != null) {
				missile = temp.missile;
			}
			// Load Status Ailment
			statusAilment = temp.statusAilment;
		}

		if (_towerComponents[TOWER_TURRET] == null) {
			isFiring = false;
		} else {
			isFiring = true;
		}
		// Debug Out all Stats
//		for (int i = 0; i < Enum.GetValues(typeof(Stat)).Length; i++) {
//			Debug.Log(((Stat)i).ToString() + ": " + stats[i].AdjustedBaseValue);
//		}
	}

	public void AddNextComponent(TowerComponent tPrefab) {
		int next = GetNextComponent();

		// Instantiate the Game Object
//		TowerComponent towerInstance = Instantiate(tPrefab, transform.position + GetNextComponentPosition(), Quaternion.identity) as TowerComponent;
		tPrefab.transform.position = transform.position + GetNextComponentPosition();
		tPrefab.transform.rotation = Quaternion.identity;
		_towerComponents[next] = tPrefab;	// Note: You can't use t here because it is the component of the prefab.

		tPrefab.gameObject.SetActive(true);

		// Copy the PREFAB's ModifyingAttributes to the INSTANCE
//		if (towerInstance.componentType == TOWER_TURRET) {
//			foreach (ModifyingAttribute m in ((TowerTurret) tPrefab).attributes) {
//				((TowerTurret)towerInstance).attributes.Add(m);
//			}
//		}

//		towerInstance.gameObject.SetActive(true);

		// Destroy the Prefab (Save on space)
//		Destroy(tPrefab.gameObject);

//		if (tPrefab.componentType == TOWER_TURRET) {
//			Debug.Log("Component PREFAB Count: " + ((TowerTurret)tPrefab).attributes.Count);
//			Debug.Log("Component INSTANCE Count: " + ((TowerTurret)towerInstance).attributes.Count);
//		}

		if (next == TOWER_TURRET) {
			UpdateStats();
		}
	}

	public TowerComponent DeactivateComponent(int type) {
		_towerComponents[type].transform.position = new Vector3(0, 50, 0);	// Move to a far away place
		_towerComponents[type].transform.parent = null;	// DE-Parent
		_towerComponents[type].gameObject.SetActive(false);
		TowerComponent t = _towerComponents[type];
		_towerComponents[type] = null;
		return t;
	}

	public TowerComponent SwapComponent(TowerComponent tPrefab) {
		int swapType = tPrefab.componentType;

//		_towerComponents[swapType].transform.position = new Vector3(0, 50, 0);	// Move to a far away place
//		_towerComponents[swapType].transform.parent = null;	// DE-Parent
//		_towerComponents[swapType].gameObject.SetActiveRecursively(false);
//		TowerComponent swappedOutComponent = _towerComponents[swapType].GetComponent<TowerComponent>();
		TowerComponent swappedOutComponent = DeactivateComponent(swapType);

		// TODO: Change to SetActive
		tPrefab.transform.position = transform.position + GetNextComponentPosition();
		tPrefab.transform.rotation = Quaternion.identity;
		_towerComponents[swapType] = tPrefab;	// Note: You can't use t here because it is the component of the prefab.

		tPrefab.gameObject.SetActive(true);

//		TowerComponent towerInstance = Instantiate(tPrefab, transform.position + GetNextComponentPosition(swapType), Quaternion.identity) as TowerComponent;
//		towerInstance.transform.parent = transform;
//		_towerComponents[swapType] = towerInstance;	// Note: You can't use t here because it is the component of the prefab.
//
//		// Copy the PREFAB's ModifyingAttributes to the INSTANCE
//		if (tPrefab.componentType == TOWER_TURRET) {
//			foreach (ModifyingAttribute m in ((TowerTurret) tPrefab).attributes) {
//				((TowerTurret)towerInstance).attributes.Add(m);
//			}
//		}

		UpdateStats();
		return swappedOutComponent;
	}

	public TowerComponent BreakTopComponent() {
		TowerComponent t = null;
		switch (GetNextComponent()) {
			case TOWER_COMPLETE:
				t = DeactivateComponent(TOWER_TURRET);
				break;
			case TOWER_TURRET:
				t = DeactivateComponent(TOWER_BASE);
				break;
		}
		UpdateStats();
		return t;
	}

	public TowerComponent GetTopComponent() {
		switch (GetNextComponent()) {
			case TOWER_COMPLETE:
				return _towerComponents[TOWER_TURRET];
			case TOWER_TURRET:
				return _towerComponents[TOWER_BASE];
			case TOWER_BASE:
				return null;
		}
		return null;
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

	public float GetRange() {
		return stats[(int)Stat.Range].AdjustedBaseValue;
	}

	/*
	 *	STATIC FUNCTIONS
	 */
	public static float JiggleStat(float amt, float jiggle) {
		float min = amt * (1 - jiggle);
		float max = amt * (1 + jiggle);
		return UnityEngine.Random.Range(min, max);
	}
}

public enum Stat {
	Damage,
	Range,
	FiringRate
//	SplashRange
}