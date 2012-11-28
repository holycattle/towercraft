using UnityEngine;
using System.Collections;

public class Weapon : GameTool {
	private const int LIFE_WIDTH = 160;
	private const int LIFE_HEIGHT = 50;
	private Vector3 SCREEN_CENTER = new Vector3(Screen.width / 2, Screen.height / 2, 0);
	private const int NUM_WEAPONEQUIPS = 2;

	// Default Constants
	public const int DEFAULT_DAMAGE = 1;
	public const int DEFAULT_RANGE = 64;
	public const int DEFAULT_FIRINGRATE = 4;
	public const int DEFAULT_ACCURACY = 10;

	//
	public GameObject sparks;
	public GameObject muzzleFlashObj;
	public GameObject bullet;
	private ParticleEmitter emitter;

	// Equipped Modifiers
	public WeaponItem[] equippedWeapons;

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

		equippedWeapons = new WeaponItem[NUM_WEAPONEQUIPS];

		// Initialize Stats
		RecalculateStats();
	}

	public void RecalculateStats() {
		damage = DEFAULT_DAMAGE;
		range = DEFAULT_RANGE;
		firingRate = DEFAULT_FIRINGRATE;
		accuracy = DEFAULT_ACCURACY;

		for (int i = 0; i < equippedWeapons.Length; i++) {
			if (equippedWeapons[i] != null) {
				damage += equippedWeapons[i].damage;
				range += equippedWeapons[i].range;
				firingRate += equippedWeapons[i].firingRate;
				accuracy += equippedWeapons[i].accuracy;
			}
		}

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
			//check if there's ammo left
			if (bullets == 0) {
				//if there are bullets in inventory, reload
				//bullets = 30;
				//else display warning message and do nothing afterwards
				_game.Messenger.WarningMessage("No more ammo left.");
				return;
			}
			
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
