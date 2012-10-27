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
	private float _timeSinceFired;				// # of seconds since you last fired
	private GameObject _target;
	public Transform _missileSource;

	void Awake() {
		_towerComponents = new TowerComponent[TOWER_COMPLETE];

		stats = new ModifiedStat[Enum.GetValues(typeof(Stat)).Length];
	}

	void Start() {
		_enemiesInRange = new List<GameObject>();
		_timeSinceFired = 0;

		// Update the Radius of the Capsule Collider (Range)
//		GetComponent<CapsuleCollider>().radius = towerRange;

//		_missileSource = transform.Find("MissileSource").transform;
	}

	void Update() {
		// Make the Cannon face the Enemy
		if (_target != null) {
			float distance = Vector2.Distance(new Vector2(_target.transform.position.x, _target.transform.position.z), new Vector2(transform.position.x, transform.position.z));
			if (distance < towerRange)
				_missileSource.rotation =
				Quaternion.Slerp(_missileSource.rotation,
					Quaternion.LookRotation(_missileSource.position - _target.transform.position), 90 * Time.deltaTime);
		}
		
		// Try to Fire
		if (_timeSinceFired <= 0 && isFiring) {
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
				if (_target != null && _timeSinceFired <= 0) {
					GameObject g = Instantiate(missile, _missileSource.position, Quaternion.identity) as GameObject;

					// Set target
					g.GetComponent<BaseMissile>().Target = _enemiesInRange[0];

					_timeSinceFired += firingInterval;
				}
			}
		}

		// Update FiringCounter
		if (_timeSinceFired > 0)
			_timeSinceFired -= Time.deltaTime;
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
		_missileSource = _towerComponents[TOWER_TURRET].transform.FindChild("MissileSource");

		// Set Firing Interval
		firingInterval = 1f;

		// Set Collider Range
//		GetComponent<SphereCollider>().radius = stats[Stat.Range].AdjustedBaseValue;
		GetComponent<SphereCollider>().radius = 4 * LevelController.TILE_SIZE;

		isFiring = true;
	}

	public void addNextComponent(GameObject g) {
		int next = getNextComponent();
		TowerComponent comp = null;
		switch (next) {
		case TOWER_BASE:
			comp = g.GetComponent<TowerBase>();
			break;
		case TOWER_STEM:
			comp = g.GetComponent<TowerStem>();
			break;
		case TOWER_TURRET:
			comp = g.GetComponent<TowerTurret>();
			break;
		}

		// Proper Component is not Attached.
		if (comp == null)
			return;

		// Instantiate the Game Object
		GameObject t = Instantiate(g, transform.position, Quaternion.identity) as GameObject;
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
}

public enum Stat {
	Range,
	Damage,
	FiringRate,
	SplashRange
}