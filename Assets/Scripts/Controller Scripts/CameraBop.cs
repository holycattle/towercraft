using UnityEngine;
using System.Collections;

public class CameraBop : MonoBehaviour {
//	private float impulseAmount = 0.4f;

	void Start() {
		iTween.Init(gameObject);
	}

	public void OnLand() {
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
