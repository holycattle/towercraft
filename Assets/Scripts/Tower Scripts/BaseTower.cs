using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseTower : MonoBehaviour {

	// Tower Properties
	public string towerName;
	public float towerRange;
	public float firingInterval;
	public GameObject missile;
	public bool isFiring;

	// Tower Logic
	private List<GameObject> _enemiesInRange;	// List of enemies in range
	private float _timeSinceFired;				// # of seconds since you last fired
	private GameObject _target;
	private Transform _missileSource;

	void Start() {
		_enemiesInRange = new List<GameObject>();

		_timeSinceFired = 0;

		// Update the Radius of the Capsule Collider (Range)
		GetComponent<CapsuleCollider>().radius = towerRange;

		_missileSource = transform.Find("MissileSource").transform;
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
					GameObject g = Instantiate(missile, transform.FindChild("MissileSource").position, Quaternion.identity) as GameObject;

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
}
