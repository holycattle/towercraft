using UnityEngine;
using System.Collections;

public class Slow : Ailment {

	private float slowPercentage = 0.25f;
	private float deltaMoveSpeed;

	protected override void Start() {
		base.Start();
		Interval = 10;
	}

	protected override void BeginStatus() {
		deltaMoveSpeed = Mathf.Max(0.5f, (int)(deltaMoveSpeed * slowPercentage));
		_enemy.MoveSpeed -= deltaMoveSpeed;
	}

	protected override void EndStatus() {
		_enemy.MoveSpeed += deltaMoveSpeed;
		base.EndStatus();
	}
}
