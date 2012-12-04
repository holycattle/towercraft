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

	/*
	 *	Gun Stats (do not have to be manipulated in other classes)
	 */
	// DPS Stats
	private int damage;
	private float firingRate;
	private int energyConsumption;
	private GameObject statusEffect;
	// Other Stats
	private float range;
	private int magSize;		// # Bullets per Magazine
	private float reloadTime = 2;	// Secs to Reload

	// Gun Variables
	private float _fireInterval;
	private float _timeTillFire;

	// Ammo/Reloading
	private int _bullets;			// Current Magazine
	private int _totalBullets;		// Total in Pack
	private float _reloadCounter;	// Secs currently Reloading

	// GUI Information
	private BaseEnemy _targetted;

	protected override void Awake() {
		base.Awake();
	}

	protected override void Start() {
		base.Start();

		emitter = GetComponentInChildren<ParticleEmitter>();
		emitter.emit = false;

		equippedWeapons = new WeaponItem[NUM_WEAPONEQUIPS];
		equippedWeapons[WeaponItem.DPS] = new WeaponDPSItem(1);
		equippedWeapons[WeaponItem.STAT] = new WeaponStatItem(1);

		// Initialize Stats
		RecalculateStats();

		// Init Ammo/Shooting
		_timeTillFire = 0;
		_totalBullets = 60;
		_bullets = magSize;
	}

	public void RecalculateStats() {

		damage = ((WeaponDPSItem)equippedWeapons[WeaponItem.DPS]).damage;
		firingRate = ((WeaponDPSItem)equippedWeapons[WeaponItem.DPS]).firingRate;
		statusEffect = ((WeaponDPSItem)equippedWeapons[WeaponItem.DPS]).statusEffect;
		energyConsumption = ((WeaponDPSItem)equippedWeapons[WeaponItem.DPS]).energyConsumption;

		magSize = ((WeaponStatItem)equippedWeapons[WeaponItem.STAT]).magSize;
		reloadTime = ((WeaponStatItem)equippedWeapons[WeaponItem.STAT]).reloadTime;
		range = ((WeaponStatItem)equippedWeapons[WeaponItem.STAT]).range;
//		accuracy = ((WeaponStatItem)equippedWeapons[WeaponItem.STAT]).accuracy;

		_fireInterval = 1.0f / firingRate;
	}

	protected override void Update() {
		base.Update();

		if (_reloadCounter > 0) {
			_reloadCounter -= Time.deltaTime;
			if (_reloadCounter <= 0) {
				_reloadCounter = 0;
				ReloadBullets();
			}
		} else {
			if (Input.GetKey(KeyCode.R) && _bullets < magSize) {
				Reload();
			}
		}


		if (_timeTillFire > 0) {
			_timeTillFire -= Time.deltaTime;
			if (_timeTillFire <= 0)
				_timeTillFire = 0;
		}
	}

	protected override void OnGUI() {
		base.OnGUI();

		if (_targetted != null) {
			GUI.TextArea(new Rect(Screen.width / 2 - LIFE_WIDTH / 2, 0, LIFE_WIDTH, LIFE_HEIGHT),
				_targetted.Name + "\n" + _targetted.Life + " / " + _targetted.maxLife + "\n" +
				_targetted.getResistanceTypeAsString() + "-resistant"
			);
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
		// Check if reloading
		if (_reloadCounter > 0) {
			// Currently Reloading
			return;
		}

		// Check if there's ammo left.
		if (_bullets <= 0) {
			Reload();
			return;
		}

		//damage = 1;

		if (_timeTillFire <= 0) {
			// Raycast
			int maxInaccuracy = (int)(CurrentRecoil * crosshairOffset);
			Ray ray = Camera.main.ScreenPointToRay(SCREEN_CENTER +
				new Vector3(Random.Range(-maxInaccuracy, maxInaccuracy), Random.Range(-maxInaccuracy, maxInaccuracy), 0));
			RaycastHit hit;

			// Casts the ray and get the first game object hit
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, _weapon.RaycastLayer)) {
				// Damage Game Object
				BaseEnemy b = hit.transform.gameObject.GetComponent<BaseEnemy>();
				if (b != null) {
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
				proj.GetComponent<Bullet>().statusAilment = statusEffect;
				Physics.IgnoreCollision(proj.collider, transform.root.collider);
			} else {
				// Gun Projectile
				GameObject proj = Instantiate(bullet, transform.position, Quaternion.LookRotation(transform.forward)) as GameObject;
				proj.GetComponent<Rigidbody>().velocity = (transform.forward).normalized * 64;
				proj.GetComponent<Bullet>().damage = damage;
				proj.GetComponent<Bullet>().range = range;
				proj.GetComponent<Bullet>().statusAilment = statusEffect;
				Physics.IgnoreCollision(proj.collider, transform.root.collider);
			}
			_timeTillFire += _fireInterval;
			_bullets -= energyConsumption;

			// Check if there's ammo left.
			if (_bullets <= 0) {
				_bullets = 0;
			}

			emitter.Emit(1);
			Recoil();
		}
	}

	public void AddAmmo(int i) {
		_totalBullets += i;
		_game.Messenger.ItemMessage("Picked up " + i + " Bullets!");
	}

	private void Reload() {
		if (_totalBullets > 0) {
			_game.Messenger.WarningMessage("Reloading!");
		} else {
			_game.Messenger.WarningMessage("No more ammo left.");
			return;
		}
		// Start Reload Timer
		_reloadCounter = reloadTime;
	}
	
	public int Ammo {
		get { return _bullets; }
	}

	public int MagazineSize {
		get { return magSize; }
	}

	public int TotalAmmo {
		get { return _totalBullets; }
	}
	
	private void ReloadBullets() {
		_totalBullets += _bullets;
		if (_totalBullets >= magSize) {
			_bullets = magSize;
		} else {
			_bullets = _totalBullets;
		}
		_totalBullets -= _bullets;
	}
}
