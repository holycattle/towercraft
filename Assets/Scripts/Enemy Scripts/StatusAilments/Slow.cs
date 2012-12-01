using UnityEngine;
using System.Collections;

public class Slow : Ailment {

	private float slowPercentage = 0.5f;
	private float deltaMoveSpeed;

	protected override void Start() {
		base.Start();
		Interval = 10;
		
		slowPercentage *= (1 - _enemy.slowResistance);
	}

	protected override void BeginStatus() {
		Debug.Log("Slowed!");
		deltaMoveSpeed = Mathf.Max(0.5f, (int)(deltaMoveSpeed * slowPercentage));
		_enemy.MoveSpeed -= deltaMoveSpeed;
	}

	protected override void EndStatus() {
		_enemy.MoveSpeed += deltaMoveSpeed;
		base.EndStatus();
	}
}
