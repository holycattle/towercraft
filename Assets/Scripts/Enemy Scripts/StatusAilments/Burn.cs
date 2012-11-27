using UnityEngine;
using System.Collections;

public class Burn : Ailment {

	private int damage = 1;
	private float damageInterval = 1;

	protected override void Start() {
		base.Start();
		Interval = 10;
	}

	protected override void Update() {
		base.Update();

		damageInterval -= Time.deltaTime;
		if (damageInterval <= 0) {
			_enemy.AddLife(-damage);
			damageInterval += 1;
		}
	}
}
