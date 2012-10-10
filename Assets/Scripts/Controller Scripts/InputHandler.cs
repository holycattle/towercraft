/* SAUCE: http://blog.gfx47.com/2011/04/04/detect-right-click-on-game-objects-in-unity3d/ */
using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {
	private CharacterMotor _pMotor;

	void Start() {
		_pMotor = GetComponent<CharacterMotor>();
	}

	void Update() {
		#region Player Movement
		Vector3 directionVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		if (directionVector != Vector3.zero) {
			// Get the length of the directon vector and then normalize it
			// Dividing by the length is cheaper than normalizing when we already have the length anyway
			var directionLength = directionVector.magnitude;
			directionVector = directionVector / directionLength;
		
			// Make sure the length is no bigger than 1
			directionLength = Mathf.Min(1, directionLength);

			// Make the input vector more sensitive towards the extremes and less sensitive in the middle
			// This makes it easier to control slow speeds when using analog sticks
			directionLength = directionLength * directionLength;
		
			// Multiply the normalized direction vector by the modified length
			directionVector = directionVector * directionLength;
		}
		// Apply the direction to the CharacterMotor
		_pMotor.inputMoveDirection = transform.rotation * directionVector;
		_pMotor.inputJump = Input.GetButton("Jump");
		#endregion
	}
}
