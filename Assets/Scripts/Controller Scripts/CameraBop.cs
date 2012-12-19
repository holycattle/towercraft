using UnityEngine;
using System.Collections;

public class CameraBop : MonoBehaviour {
//	private float impulseAmount = 0.4f;
	private Vector3 originalLocalPosition;
	private float recoverySpeed = 1 / 0.0125f;
	private bool _positiveTransform;
	private float _currentVelocity;
	private float _currentPosition;

	void Start() {
		originalLocalPosition = transform.localPosition;
		_currentVelocity = 0;
		_currentPosition = 0;
	}

	void Update() {
		if (_currentVelocity == 0) {
		} else {
			if (_positiveTransform) {
				_currentVelocity -= recoverySpeed * Time.deltaTime;
			} else {
				_currentVelocity += recoverySpeed * Time.deltaTime;
			}
			float deltaPos = _currentVelocity * Time.deltaTime;
			if (_currentPosition == 0 || Mathf.Sign(_currentPosition + deltaPos) == Mathf.Sign(_currentPosition)) {
				_currentPosition += deltaPos;
			} else {
				_currentPosition = 0;
				_currentVelocity = 0;
			}

			transform.localPosition = new Vector3(transform.localPosition.x, originalLocalPosition.y + _currentPosition, transform.localPosition.z);
		}
	}

	public void OnLand() {
		_positiveTransform = false;
		_currentVelocity = -12f;
//		_currentImpulse = -1;
//		iTween.PunchPosition(gameObject, iTween.Hash("amount", new Vector3(0, -verticalImpulse, 0), "time", 1f, "space", Space.Self));
//		iTween.MoveFrom(gameObject,
//			iTween.Hash("position", transform.localPosition + new Vector3(0, -impulseAmount, 0),
//			"time", 0.5f, "islocal", true, "easetype", iTween.EaseType.easeOutCubic));
	}

	public void OnJump() {
//		_positiveTransform = true;
//		_currentImpulse = 1;
//		iTween.PunchPosition(gameObject, iTween.Hash("amount", new Vector3(0, verticalImpulse, 0), "time", 1f, "space", Space.Self));
//		iTween.MoveFrom(gameObject,
//			iTween.Hash("position", transform.localPosition + new Vector3(0, impulseAmount, 0),
//			"time", 1f, "islocal", true, "easetype", iTween.EaseType.easeOutCubic));
	}
}
