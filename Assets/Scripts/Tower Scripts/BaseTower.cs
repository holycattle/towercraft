using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BaseTower : MonoBehaviour {
	// Tower Part Constants
	public const int TOWER_BASE = 0;
	public const int TOWER_STEM = 1;
	public const int TOWER_TURRET = 2;
	public const int TOWER_COMPLETE = 3;	// Also acts as the max # of components per tower

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
			Debug.Log("Trying to fire!");
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
		Debug.Log("Im here: " + other.name);
		if (other.gameObject.tag == "Enemy") {
			_enemiesInRange.Add(other.transform.root.gameObject);
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Enemy")
			_enemiesInRange.Remove(other.transform.root.gameObject);
	}

	private void UpdateStats() {
		_missileSource = _towerComponents[TOWER_TURRET].transform.Find("MissileSource");
//		foreach (Transform trans in _towerComponents[TOWER_TURRET].GetComponentsInChildren<Transform>()) {
//			if (trans.gameObject.name == "MissileSource") {
//				_missileSource = trans;
//				break;
//			}
//		}

		foreach (TowerComponent c in _towerComponents) {
			foreach (ModifyingAttribute m in c.attributes) {
				stats[(int)m.stat].AddModifier(m);
			}
		}

		// Set Firing Interval
		firingInterval = 1f;

		// Set Collider Range
		GetComponent<SphereCollider>().radius = stats[(int)Stat.Range].AdjustedBaseValue * LevelController.TILE_SIZE;
//		GetComponent<SphereCollider>().radius = 4 * LevelController.TILE_SIZE;

		isFiring = true;

		// Debug Out all Stats
		for (int i = 0; i < Enum.GetValues(typeof(Stat)).Length; i++) {
			Debug.Log(((Stat)i).ToString() + ": " + stats[i].AdjustedBaseValue);
		}
	}

	public void addNextComponent(GameObject g) {
		int next = getNextComponent();
		TowerComponent comp = g.GetComponent<TowerComponent>();
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

		// Proper Component is not Attached.
		if (comp == null) {
			Debug.Log("Not Attached");
			return;
		}

		// Instantiate the Game Object
		GameObject t = Instantiate(g, transform.position + getNextComponentPosition(), Quaternion.identity) as GameObject;
		t.transform.parent = transform;
		_towerComponents[next] = t.GetComponent<TowerComponent>();	// Note: You can't use comp here because it is the component of the prefab.

		if (next == TOWER_TURRET) {
			UpdateStats();
		}
	}

	public int getNextComponent() {
		if (_towerComponents[TOWER_BASE] == null) {
			return TOWER_BASE;
		} else if (_towerComponents[TOWER_STEM] == null) {
			return TOWER_STEM;
		} else if (_towerComponents[TOWER_TURRET] == null) {
			return TOWER_TURRET;
		} else {
			return TOWER_COMPLETE;
		}
	}

	private Vector3 getNextComponentPosition() {
		int next = getNextComponent();
		Vector3 offset = Vector3.zero;
		switch (next) {
			case TOWER_COMPLETE:
				break;
			case TOWER_TURRET:
				offset += _towerComponents[TOWER_STEM].baseNextComponentPosition;
				goto case TOWER_STEM;	// Using fallthrough, C# style.
			case TOWER_STEM:
				offset += _towerComponents[TOWER_BASE].baseNextComponentPosition;
				break;
			case TOWER_BASE:
				break;
		}
		return offset;
	}
}

public enum Stat {
	Range,
	Damage,
	FiringRate,
	SplashRange
}