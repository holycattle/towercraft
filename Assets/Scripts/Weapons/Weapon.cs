using UnityEngine;
using System.Collections;

public class Weapon : GameTool {
	//
	public Rigidbody bulletPrefab;
	public float bulletSpeed = 256;

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

//		_particles.particleEmitter.maxEnergy = _fireInterval;
	}

	public override void WhenEquipped() {
		_input.RaycastLayer = LayerMask.NameToLayer("Mob");
//		_particles.particleEmitter.emit = false;
	}

	public override void MouseClickedOn(GameObject g) {
//		_particles.particleEmitter.emit = true;
		isFiring = true;
		TryToFire(g);
	}

	public override void MouseDownOn(GameObject g) {
//		_particles.particleEmitter.emit = true;
		isFiring = true;
		TryToFire(g);
	}

	public override void MouseUpOn(GameObject g) {
//		_particles.particleEmitter.emit = false;
		isFiring = false;
	}

	private void TryToFire(GameObject g) {
		if (_timeTillFire <= 0) {
			Rigidbody r = Instantiate(bulletPrefab, transform.position, transform.rotation) as Rigidbody;
			r.velocity = transform.TransformDirection(Vector3.forward * bulletSpeed);
			Physics.IgnoreCollision(r.collider, transform.root.collider);

			if (g != null) {
				// Damage Game Object
//				g.GetComponent<BaseEnemy>().SubLife(damage);
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
