using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {
	public int damage = 1;

	void Start() {
	}

	void OnCollisionEnter(Collision collision) {
		BaseEnemy b = collision.gameObject.GetComponent<BaseEnemy>();
		if (b != null) {
			b.SubLife(damage);
		}
		Destroy(gameObject);
	}
}
