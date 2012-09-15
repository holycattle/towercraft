using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	float forwardSpeed = 32;

	void Update () {
		float forwardMoveAmount = Input.GetAxis("Vertical") * forwardSpeed * Time.deltaTime;
		float sideMoveAmount = Input.GetAxis("Horizontal") * forwardSpeed * Time.deltaTime;

		transform.Translate(sideMoveAmount, 0, forwardMoveAmount, Space.World);
	}

	void OnMouseDown () {
		Debug.Log("Hello World!");
	}
	
	void OnCollisionEnter (Collision collision) {
		Debug.Log("Collided With: " + collision.gameObject.name);
	}
}