using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	float forwardSpeed = 32;
	private Camera minimapCam;
	private Camera firstPersonCam;

	void Start() {
		minimapCam = GameObject.Find("Minimap Camera").GetComponent<Camera>();
		firstPersonCam = GameObject.Find("Main Camera").GetComponent<Camera>();

		firstPersonCam.enabled = true;
		minimapCam.enabled = false;
	}

	void Update() {
		float forwardMoveAmount = Input.GetAxis("Vertical") * forwardSpeed * Time.deltaTime;
		float sideMoveAmount = Input.GetAxis("Horizontal") * forwardSpeed * Time.deltaTime;

		transform.Translate(sideMoveAmount, 0, forwardMoveAmount, Space.World);

		if (Input.GetKeyDown(KeyCode.C)) {
			minimapCam.enabled = !minimapCam.enabled;
			firstPersonCam.enabled = !firstPersonCam.enabled;
		}
	}

	void OnMouseDown() {
		Debug.Log("Hello World!");
	}
	
	void OnCollisionEnter(Collision collision) {
		Debug.Log("Collided With: " + collision.gameObject.name);
	}
}