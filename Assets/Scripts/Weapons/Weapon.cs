using UnityEngine;
using System.Collections;

public class Weapon : GameTool {
	private Vector3 SCREEN_CENTER = new Vector3(Screen.width / 2, Screen.height / 2, 0);

	//
	public GameObject sparks;
	public GameObject muzzleFlashObj;
	public GameObject bullet;
	private ParticleEmitter emitter;

	//
	private float _fireInterval = 0.1f;
	private int damage = 1;
	private float _timeTillFire;

	protected override void Awake() {
		base.Awake();

		_timeTillFire = 0;
		bullets = 30;
	}

	void Start() {
		emitter = GetComponentInChildren<ParticleEmitter>();
		emitter.emit = false;
	}

	public override void WhenEquipped() {
		_input.RaycastLayer = LayerMask.NameToLayer("Mob");
	}

	public override void MouseClickOn(GameObject g) {
		TryToFire(g);
	}

	public override void MouseDownOn(GameObject g) {
		TryToFire(g);
	}

	public override void MouseUpOn(GameObject g) {
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

//				GameObject b = Instantiate(bullet, transform.position, Quaternion.LookRotation(hit.point - transform.position)) as GameObject;
//				b.GetComponent<Rigidbody>().velocity = (hit.point - transform.position).normalized * 128;
//				Physics.IgnoreCollision(b.collider, transform.root.collider);
			}
			_timeTillFire += _fireInterval;
			bullets--;
			if (bullets == 0)
				bullets = 30;

			emitter.Emit(1);
			_input.Recoil();
		}
	}

	void Update() {
		if (_timeTillFire > 0) {
			_timeTillFire -= Time.deltaTime;
			if (_timeTillFire <= 0)
				_timeTillFire = 0;
		}
	}
}
