using UnityEngine;
using System.Collections;

public class BaseMissile : MonoBehaviour {
	
	public float moveSpeed;
	public int damage;
	public int damageType;
	public float splashRadius;
	public GameObject explosionEffect;
	public GameObject statusAilment;
	private GameObject _target;
	
	void Update() {
		if (_target != null) {
			// Rotation
			transform.LookAt(_target.transform);
			// Translation
			transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
		} else {
			// Self Destruct
			Destroy(this.gameObject);

			// Retargetting Code (Untested)
			/*GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			
			if (enemies != null) {
				float minDist = Vector3.Distance(enemies[0].transform.position, transform.position);
				int minIndex = 0;
				for (int i = 1; i < enemies.Length; i++) {
					float dist = Vector3.Distance(enemies[i].transform.position, transform.position);
					if (dist < minDist) {
						minIndex = i;
						minDist = dist;
					}
				}
				
				_target = enemies[minIndex];
				
				// Rotation
				transform.LookAt(_target.transform);
				// Translation
				transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
			} else {
				// No enemies found, Self-Destruct
				Destroy(this.gameObject);
			}*/
		}
	}

	public GameObject Target {
		set { _target = value; }
	}
	
	void OnCollisionEnter(Collision collision) {
		if (_target != null) {
			if (collision.gameObject.tag == "Enemy") {
				collision.transform.gameObject.GetComponent<BaseEnemy>().Damage(damage, damageType);

				// Add Status Effect
				if (statusAilment != null) {
					GameObject g = Instantiate(statusAilment, collision.gameObject.transform.position, Quaternion.identity) as GameObject;
					g.transform.parent = collision.gameObject.transform;
				}

				// Add Particle Effects
				Instantiate(explosionEffect, this.transform.position, Quaternion.identity);
				Destroy(this.gameObject);
			}
		} else {
			Destroy(this.gameObject);
		}
	}
}
