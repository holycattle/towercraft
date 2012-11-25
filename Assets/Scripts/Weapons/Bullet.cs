using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public int damage = 1;
	public int range = 32;

	// Starting Position
	private Vector3 _startingPos;

	void Start() {
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
		}
		Destroy(gameObject);
	}
}
