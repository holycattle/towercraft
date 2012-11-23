using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	float forwardSpeed = 32;
	private Camera minimapCam;
	private Camera firstPersonCam;

	// Controllers
	private GameController _game;
	private PlayerController _player;
	private WeaponController _weapon;

	void Start() {
		minimapCam = GameObject.Find("Minimap Camera").GetComponent<Camera>();
		firstPersonCam = GameObject.Find("Main Camera").GetComponent<Camera>();

		// Controller Inits
		_game = GameObject.Find(" GameController").GetComponent<GameController>();
		_player = GameObject.Find("Player").GetComponent<PlayerController>();
		_weapon = _player.gameObject.GetComponentInChildren<WeaponController>();

		firstPersonCam.enabled = true;
		minimapCam.enabled = false;
	}

	void Update() {
		// Toggle Camera View
		if (Input.GetKeyDown(KeyCode.C)) {
			SetOverviewCamera(!minimapCam.enabled);
		}

		float forwardMoveAmount = Input.GetAxis("Vertical") * forwardSpeed * Time.deltaTime;
		float sideMoveAmount = Input.GetAxis("Horizontal") * forwardSpeed * Time.deltaTime;

		transform.Translate(sideMoveAmount, 0, forwardMoveAmount, Space.World);
	}

	public void SetOverviewCamera(bool val) {
		minimapCam.enabled = val;
		firstPersonCam.enabled = !val;

		if (val) {
			/*
			 * Overview Camera
			 */

			// Lock tool to BuildTool
			_weapon.SetActiveWeapon(WeaponController.TOOL_BUILDER);
			_weapon.WeaponSwapLock = true;
			_game.CaptureCursor = false;

			// Set to position of the Player
			transform.position = new Vector3(_player.transform.position.x, 45, _player.transform.position.z);

			// Disable the Player
			_player.SetEnabled(false);
		} else {
			/*
			 * First Person Camera
			 */
			_weapon.SetActiveWeapon(WeaponController.TOOL_WEAPON);
			_weapon.WeaponSwapLock = false;
			_game.CaptureCursor = true;

			// Re-Enable the Player
			_player.SetEnabled(true);
		}
	}
}