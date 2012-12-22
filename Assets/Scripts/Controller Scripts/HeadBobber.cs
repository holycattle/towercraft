using UnityEngine;
using System.Collections;

public class HeadBobber: MonoBehaviour {
	PlayerController _player;
	CharacterMotor _motor;
	private float timer = 0.0f;
	private float bobbingSpeed;
//	private float multiplier
	float bobbingAmount = 0.2f;
	float startY;

	void Start() {
		_player = transform.root.GetComponentInChildren<PlayerController>();
		_motor = transform.root.GetComponentInChildren<CharacterMotor>();

		startY = transform.localPosition.y;
		bobbingSpeed = (2 * Mathf.PI) / transform.root.GetComponentInChildren<PlayerController>().footstepInterval;
	}

	void Update() {
		float waveslice = 0.0f;
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		Vector3 cSharpConversion = transform.localPosition;

		if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0) {
			timer = 0.0f;
		} else {
			waveslice = Mathf.Sin(timer * (bobbingSpeed * _player.footstepMultiplier));
			timer += Time.deltaTime;
//			if (timer > Mathf.PI * 2) {
//				timer -= (Mathf.PI * 2);
//			}
		}

		Debug.Log("WS: " + waveslice);
		if (waveslice != 0) {
			float translateChange = waveslice * bobbingAmount;
			float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
			totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
			translateChange = totalAxes * translateChange;
			cSharpConversion.y = startY + translateChange;
		} else {
			cSharpConversion.y = startY;
		}
		transform.localPosition = cSharpConversion;
	}
}