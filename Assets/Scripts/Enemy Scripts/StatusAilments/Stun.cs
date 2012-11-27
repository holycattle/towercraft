using UnityEngine;
using System.Collections;

public class Stun : Ailment {

	private float deltaMoveSpeed;

	protected override void Start() {
		base.Start();
		Interval = 1;
	}

	protected override void BeginStatus() {
		deltaMoveSpeed = _enemy.MoveSpeed;
		_enemy.MoveSpeed = 0;		// Setting this to 0.
	}

	protected override void Update() {
		if (_enemy.MoveSpeed > 0) {
			deltaMoveSpeed += _enemy.MoveSpeed;
			_enemy.MoveSpeed -= _enemy.MoveSpeed;
		}
		base.Update();
	}

	protected override void EndStatus() {
		Debug.Log("Adding: " + deltaMoveSpeed + " to " + _enemy.MoveSpeed);
		_enemy.MoveSpeed += deltaMoveSpeed;
		base.EndStatus();
	}
}