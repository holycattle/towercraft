using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	// Particle Effects
	private static GameObject sparks;

	// Bullet Stats
	public int damage = 1;
	public int range = 32;
	
	// Starting Position
	private Vector3 _startingPos;

	void Start() {
		if (sparks == null) {
			sparks = Resources.Load("Prefabs/Particles/Sparks", typeof(GameObject)) as GameObject;
		}

		_startingPos = transform.position;
	}

	void Update() {
		if (Vector3.Distance(_startingPos, transform.position) > range) {
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter(Collision collision) {
		BaseEnemy b = collision.gameObject.GetComponent<BaseEnemy>();
		if (b != null) {
			b.AddLife(-damage);

			// Add Random Status Ailment
			Ailment.AddRandomStatusAilment(b);
		}

		Vector3 path = _startingPos - transform.position;
		Vector3 bounced = 2 * collision.contacts[0].normal * Vector3.Dot(collision.contacts[0].normal, path) - path;
		Instantiate(sparks, collision.contacts[0].point, Quaternion.LookRotation(bounced));

		Destroy(gameObject);
	}
}
