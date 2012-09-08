using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerScript : MonoBehaviour {
	
	// Tower Properties
	public float towerRange; 
	public float firingInterval;
	
	public GameObject missile;
	
	// Tower Logic
	private List<GameObject> _enemiesInRange;	// List of enemies in range
	private float _timeSinceFired;				// # of seconds since you last fired
	
	
	void Start () {
		_enemiesInRange = new List<GameObject>();
		
		_timeSinceFired = 0;
		
		// Update the Radius of the Capsule Collider (Range)
		GetComponent<CapsuleCollider>().radius = towerRange;
	}
	
	void Update () {
		if (_enemiesInRange.Count > 0) {
			GameObject target = _enemiesInRange[0];
			while (!(target != null)) { // If the target has already been destroyed
				_enemiesInRange.RemoveAt(0);
				if (_enemiesInRange.Count > 0) {
					target = _enemiesInRange[0];
				} else {
					target = null;
					break;
				}
			}
			if (target != null && _timeSinceFired <= 0) {
				// Fire Missile
				GameObject g = Instantiate(missile, transform.FindChild("MissileSource").position, Quaternion.identity) as GameObject;
				
				// Set target
				g.GetComponent<BaseMissile>().Target = _enemiesInRange[0];
				
				_timeSinceFired += firingInterval;
			}
		}
		
		if (_timeSinceFired > 0) 
			_timeSinceFired -= Time.deltaTime;
	}
	
	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Enemy")
			_enemiesInRange.Add(other.transform.root.gameObject);
	}
	
	void OnTriggerExit(Collider other) {
		if (other.gameObject.tag == "Enemy")
			_enemiesInRange.Remove(other.transform.root.gameObject);
	}
}
