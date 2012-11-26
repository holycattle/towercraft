using UnityEngine;
using System.Collections;

public class Slow : Ailment {

	private float slowPercentage = 0.5f;
	private float initMoveSpeed;

	protected override void Start() {
		base.Start();
		Interval = 10;
	}

	protected override void BeginStatus() {
		initMoveSpeed = _enemy.MoveSpeed;
		_enemy.MoveSpeed = Mathf.Max(0.5f, (int)(initMoveSpeed * slowPercentage));
	}

	protected override void EndStatus() {
		_enemy.MoveSpeed = initMoveSpeed;
		_enemy.UpdateStats();
		base.EndStatus();
	}
}
