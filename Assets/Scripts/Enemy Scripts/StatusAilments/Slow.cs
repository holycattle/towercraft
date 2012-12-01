using UnityEngine;
using System.Collections;

public class Slow : Ailment {

	public float slowPercentage = 0.5f;
	private float deltaMoveSpeed;

	protected override void Start() {
		base.Start();
		Interval = 10;
	}

	protected override void BeginStatus() {
		if(_enemy.slowResistance == 0) {
			Debug.Log("Slowed!");
			deltaMoveSpeed = Mathf.Max(0.5f, (int)(deltaMoveSpeed * slowPercentage));
			_enemy.MoveSpeed -= deltaMoveSpeed;
		}
	}

	protected override void EndStatus() {
		if(_enemy.slowResistance == 0) {
			_enemy.MoveSpeed += deltaMoveSpeed;
			base.EndStatus();
		}
	}
}
