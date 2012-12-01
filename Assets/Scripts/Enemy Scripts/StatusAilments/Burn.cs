using UnityEngine;
using System.Collections;

public class Burn : Ailment {

	public int damage = 1;
	private float damageInterval = 1;

	protected override void Start() {
		base.Start();
		Interval = 8;
	}

	protected override void Update() {
		base.Update();

		damageInterval -= Time.deltaTime;
		if(_enemy.heatResistance == 0) {
			Debug.Log("Burned!");
			if (damageInterval <= 0) {
				_enemy.AddLife(-damage);
				damageInterval += 1;
			}
		}
	}
}
