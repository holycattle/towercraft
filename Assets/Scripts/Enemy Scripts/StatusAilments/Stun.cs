using UnityEngine;
using System.Collections;

public class Stun : Ailment {

	private float deltaMoveSpeed;

	protected override void Start() {
		base.Start();
		Interval = 1;
	}

	protected override void BeginStatus() {
		if(_enemy.stunResistance == 0) {
			deltaMoveSpeed = _enemy.MoveSpeed;
			_enemy.MoveSpeed = 0;		// Setting this to 0.
		}
	}

	protected override void Update() {
		if(_enemy.stunResistance == 0) {
			Debug.Log("Stunned!");
			if (_enemy.MoveSpeed > 0) {
				deltaMoveSpeed += _enemy.MoveSpeed;
				_enemy.MoveSpeed -= _enemy.MoveSpeed;
			}
			base.Update();
		}
	}

	protected override void EndStatus() {
		_enemy.MoveSpeed += deltaMoveSpeed;
		base.EndStatus();
	}
}