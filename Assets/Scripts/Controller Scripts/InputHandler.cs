/* SAUCE: http://blog.gfx47.com/2011/04/04/detect-right-click-on-game-objects-in-unity3d/ */
using UnityEngine;
using System.Collections;

public class InputHandler : MonoBehaviour {
	private Vector3 SCREEN_CENTER = new Vector3(Screen.width / 2, Screen.height / 2, 0);
	private Vector3 offset = new Vector3(5, 5, 0);
	private string OnLeftClickMethodName = "OnLeftClick";
	private string OnRightClickMethodName = "OnRightClick";
	private string OnMouseEnter = "InputMouseEnter";
	private string OnMouseExit = "InputMouseExit";

	// RayCasting
	public int layerMask; // Temporarily Public
	private GameObject _activeTarget;

	// Game Controller
	private GameController _game;

	// Player Components
	private PlayerController _player;
	private CharacterMotor _pMotor;

	void Start() {
		_activeTarget = null;
		layerMask = 1 << 8;
		_player = GetComponent<PlayerController>();
		_pMotor = GetComponent<CharacterMotor>();

		_game = GameObject.Find(" GameController").GetComponent<GameController>();
	}

	void Update() {
		if (Screen.lockCursor) {
			if (_game.Active) {
				Time.timeScale = 1;
			} else {
				Time.timeScale = 0.6f;
			}
		} else {
			Time.timeScale = 0;
			Screen.lockCursor = true;
		}

		/* Keyboard Presses */
		if (Input.GetKeyDown(KeyCode.F1)) {
			Debug.Log("Layer Mask Value = " + layerMask);
			if (layerMask == 1 << 8) {
				layerMask = 1 << 9;
			} else {
				layerMask = 1 << 8;
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			_player.SetActiveWeapon(0);
		} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
			_player.SetActiveWeapon(1);
		}

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

		/* Mouse Clicks */
		GameObject rayCastedObject = GetClickedGameObject();
		if (_activeTarget == rayCastedObject) {
			// Do Nothing
		} else {
			if (_activeTarget != null) {
				_activeTarget.SendMessage(OnMouseExit, null, SendMessageOptions.DontRequireReceiver);
			}
			if (rayCastedObject != null) {
				rayCastedObject.SendMessage(OnMouseEnter, null, SendMessageOptions.DontRequireReceiver);
			}
			_activeTarget = rayCastedObject;
		}

		if (Input.GetMouseButtonDown(0)) {
			_player.ActiveTool.MouseClickedOn(rayCastedObject);
		} else if (Input.GetMouseButton(0)) {
			_player.ActiveTool.MouseDownOn(rayCastedObject);
		} else if (Input.GetMouseButtonUp(0)) {
			_player.ActiveTool.MouseUpOn(rayCastedObject);
		}
//		else if (Input.GetMouseButtonDown(1)) {
//			_player.ActiveTool.MouseClickedOn(rayCastedObject);
//		} else if (Input.GetMouseButton(1)) {
//			_player.ActiveTool.MouseClickedOn(rayCastedObject);
//		}
	}

	GameObject GetClickedGameObject() {
		// Builds a ray from camera point of view to the mouse position
		Ray ray = Camera.main.ScreenPointToRay(SCREEN_CENTER);
		RaycastHit hit;

		// Casts the ray and get the first game object hit
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
			return hit.transform.gameObject;
		else
			return null;
	}

	public int RaycastLayer {
		set { layerMask = 1 << value; }
	}
}
