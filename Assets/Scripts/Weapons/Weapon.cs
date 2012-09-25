using UnityEngine;
using System.Collections;

public class Weapon : GameTool {
	private float _fireInterval;
	private float _timeTillFire;
	public int damage = 1;

	public Weapon () : base() {
		_fireInterval = 0.15f;
		_timeTillFire = 0;
		bullets = 30;

		_particles.particleEmitter.maxEnergy = _fireInterval;
	}

	public override void WhenEquipped() {
		Debug.Log("Weapon Equipped");
		_input.RaycastLayer = LayerMask.NameToLayer("Mob");
		_particles.particleEmitter.emit = false;
	}

	public override void MouseClickedOn(GameObject g) {
		_particles.particleEmitter.emit = true;
		TryToFire(g);
	}

	public override void MouseDownOn(GameObject g) {
		_particles.particleEmitter.emit = true;
		TryToFire(g);
	}

	public override void MouseUpOn(GameObject g) {
		_particles.particleEmitter.emit = false;
	}

	private void TryToFire(GameObject g) {
		if (_timeTillFire <= 0) {
			if (g != null) {
				// Damage Game Object
				g.GetComponent<BaseEnemy>().SubLife(damage);
			}
			_timeTillFire = _fireInterval;
			bullets--;
			if (bullets == 0)
				bullets = 30;
		}
	}

	public override void Update() {
		if (_timeTillFire > 0) {
			_timeTillFire -= Time.deltaTime;
			if (_timeTillFire < 0)
				_timeTillFire = 0;
		}
	}
}
