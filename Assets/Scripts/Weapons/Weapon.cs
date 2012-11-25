using UnityEngine;
using System.Collections;

public class Weapon : GameTool {
	private const int LIFE_WIDTH = 160;
	private const int LIFE_HEIGHT = 50;
	private Vector3 SCREEN_CENTER = new Vector3(Screen.width / 2, Screen.height / 2, 0);

	//
	public GameObject sparks;
	public GameObject muzzleFlashObj;
	public GameObject bullet;
	private ParticleEmitter emitter;

	// Gun Stats
	public int damage;
	public int range;
	public int firingRate;
	public int accuracy;

	// Gun Variables
	private float _fireInterval;
	private float _timeTillFire;

	// GUI Information
	private BaseEnemy _targetted;

	protected override void Awake() {
		base.Awake();

		_timeTillFire = 0;
		bullets = 30;
	}

	protected override void Start() {
		base.Start();

		emitter = GetComponentInChildren<ParticleEmitter>();
		emitter.emit = false;

		// Set Default Variables
		damage = 1;
		range = 64;
		firingRate = 4;
		accuracy = 10;
		RecalculateStats();
	}

	private void RecalculateStats() {
		_fireInterval = 1.0f / firingRate;
	}

	protected override void OnGUI() {
		base.OnGUI();

		if (_targetted != null) {
			GUI.TextArea(new Rect(Screen.width / 2 - LIFE_WIDTH / 2, 0, LIFE_WIDTH, LIFE_HEIGHT), _targetted.Name + "\n" + _targetted.Life + " / " + _targetted.maxLife);
		}
	}

	public override void WhenEquipped() {
		_weapon.RaycastLayer = 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Mob");
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
			int maxInaccuracy = (int)(CurrentRecoil * crosshairOffset);
//			Debug.Log("Max Inaccuracy: " + maxInaccuracy);
			Ray ray = Camera.main.ScreenPointToRay(SCREEN_CENTER +
				new Vector3(Random.Range(-maxInaccuracy, maxInaccuracy), Random.Range(-maxInaccuracy, maxInaccuracy), 0));
			RaycastHit hit;

			// Casts the ray and get the first game object hit
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, _weapon.RaycastLayer)) {
				// Damage Game Object
				BaseEnemy b = hit.transform.gameObject.GetComponent<BaseEnemy>();
				if (b != null) {
//					b.AddLife(-damage);	// Collided with enemy, otherwise collided with terrain
					_targetted = b;
				}

				// Calculate Rotation Vector
//				Vector3 path = transform.position - hit.point;
//				Vector3 bounced = 2 * hit.normal * Vector3.Dot(hit.normal, path) - path;
//				Instantiate(sparks, hit.point, Quaternion.LookRotation(bounced));

				// Gun Projectile
				GameObject proj = Instantiate(bullet, transform.position, Quaternion.LookRotation(hit.point - transform.position)) as GameObject;
				proj.GetComponent<Rigidbody>().velocity = (hit.point - transform.position).normalized * 64;
				proj.GetComponent<Bullet>().damage = damage;
				proj.GetComponent<Bullet>().range = range;
				Physics.IgnoreCollision(proj.collider, transform.root.collider);
			} else {
				// Gun Projectile
				GameObject proj = Instantiate(bullet, transform.position, Quaternion.LookRotation(transform.forward)) as GameObject;
				proj.GetComponent<Rigidbody>().velocity = (transform.forward).normalized * 64;
				proj.GetComponent<Bullet>().damage = damage;
				proj.GetComponent<Bullet>().range = range;
				Physics.IgnoreCollision(proj.collider, transform.root.collider);
			}
			_timeTillFire += _fireInterval;
			bullets--;
			if (bullets == 0)
				bullets = 30;

			emitter.Emit(1);
			Recoil();
		}
	}

	protected override void Update() {
		base.Update();

		if (_timeTillFire > 0) {
			_timeTillFire -= Time.deltaTime;
			if (_timeTillFire <= 0)
				_timeTillFire = 0;
		}
	}
}
