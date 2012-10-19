using UnityEngine;
using System.Collections;

public class Weapon : GameTool {
	private Vector3 SCREEN_CENTER = new Vector3(Screen.width / 2, Screen.height / 2, 0);
	//
	public GameObject sparks;

	//
	private float _fireInterval;
	private float _timeTillFire;
	public int damage = 1;

	//
	private bool isFiring;

	protected override void Awake() {
		base.Awake();

		_fireInterval = 0.15f;
		_timeTillFire = 0;
		bullets = 30;
		isFiring = false;
	}

	public override void WhenEquipped() {
		_input.RaycastLayer = LayerMask.NameToLayer("Mob");
	}

	public override void MouseClickOn(GameObject g) {
		isFiring = true;
		TryToFire(g);
	}

	public override void MouseDownOn(GameObject g) {
		isFiring = true;
		TryToFire(g);
	}

	public override void MouseUpOn(GameObject g) {
		isFiring = false;
	}

	private void TryToFire(GameObject g) {
		if (_timeTillFire <= 0) {
			// Raycast
			Ray ray = Camera.main.ScreenPointToRay(SCREEN_CENTER);
			RaycastHit hit;

			// Casts the ray and get the first game object hit
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, _input.RaycastLayer)) {
				// Damage Game Object
				if (hit.transform.gameObject.GetComponent<BaseEnemy>() != null)
					g.GetComponent<BaseEnemy>().SubLife(damage);	// Collided with enemy, otherwise collided with terrain

				// Calculate Rotation Vector
				Vector3 path = transform.position - hit.point;
				Vector3 bounced = 2 * hit.normal * Vector3.Dot(hit.normal, path) - path;
				Instantiate(sparks, hit.point, Quaternion.LookRotation(bounced));
			}
			_timeTillFire = _fireInterval;
			bullets--;
			if (bullets == 0)
				bullets = 30;
		}
	}

	void Update() {
		_input.setBobbing(!isFiring);

		if (_timeTillFire > 0) {
			_timeTillFire -= Time.deltaTime;
			if (_timeTillFire < 0)
				_timeTillFire = 0;
		}
	}
}
