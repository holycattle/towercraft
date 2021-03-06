using UnityEngine;
using System.Collections;

public class Stun : Ailment {
	
	public const float LENGTH = 0.5f;
	private float deltaMoveSpeed;

	protected override void Start() {
		base.Start();
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
		_enemy.MoveSpeed += deltaMoveSpeed;
		base.EndStatus();
	}
}