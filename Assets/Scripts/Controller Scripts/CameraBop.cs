using UnityEngine;
using System.Collections;

public class CameraBop : MonoBehaviour {
//	private float impulseAmount = 0.4f;
	private Vector3 originalLocalPosition;
	private float recoverySpeed = 1;
	private bool _positiveTransform;
	private float _currentVelocity;
	private float _currentPosition;

	void Start() {
		originalLocalPosition = transform.localPosition;
		_currentVelocity = 0;
		_currentPosition = 0;
	}

	void Update() {
		Debug.Log("VelPos: " + _currentPosition + " > " + _currentVelocity);

		if (_currentVelocity == 0) {
		} else {
			if (_positiveTransform) {
				_currentVelocity -= recoverySpeed * Time.deltaTime;
			} else {
				_currentVelocity += recoverySpeed * Time.deltaTime;
			}
			Debug.Log("VelPos2: " + _currentPosition + " > " + _currentVelocity);

			if (Mathf.Sign(_currentPosition + _currentVelocity) != Mathf.Sign(_currentPosition)) {
				_currentPosition = 0;
				_currentVelocity = 0;
			} else {
				_currentPosition += _currentVelocity * Time.deltaTime;
			}

			transform.localPosition = new Vector3(transform.localPosition.x, originalLocalPosition.y + _currentPosition, transform.localPosition.z);
		}
	}

	public void OnLand() {
		_positiveTransform = false;
		_currentVelocity = -1f;
		Debug.Log("Maxtor");

//		_currentImpulse = -1;
//		iTween.PunchPosition(gameObject, iTween.Hash("amount", new Vector3(0, -verticalImpulse, 0), "time", 1f, "space", Space.Self));
//		iTween.MoveFrom(gameObject,
//			iTween.Hash("position", transform.localPosition + new Vector3(0, -impulseAmount, 0),
//			"time", 0.5f, "islocal", true, "easetype", iTween.EaseType.easeOutCubic));
	}

	public void OnJump() {
//		_currentImpulse = 1;
//		iTween.PunchPosition(gameObject, iTween.Hash("amount", new Vector3(0, verticalImpulse, 0), "time", 1f, "space", Space.Self));
//		iTween.MoveFrom(gameObject,
//			iTween.Hash("position", transform.localPosition + new Vector3(0, impulseAmount, 0),
//			"time", 1f, "islocal", true, "easetype", iTween.EaseType.easeOutCubic));
	}
}
