using UnityEngine;
using System.Collections;

public class Burn : Ailment {

	private int damage = 1;
	private float damageInterval = 0.5f;

	protected override void Start() {
		base.Start();
		Interval = 4;
		damage = Mathf.Max(1, (int)(_enemy.maxLife * 0.01f));
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
