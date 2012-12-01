using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	float forwardSpeed = 32;
	private Camera minimapCam;
	private Camera firstPersonCam;

	// Controllers
	private GameController _game;
	private LevelController _level;
	private PlayerController _player;
	private WeaponController _weapon;

	// Path Drawing
	private const float INTERVAL = 0.25f;
	public GameObject pathDrawer;
	private float pathInterval = 0;

	void Start() {
		minimapCam = GameObject.Find("Minimap Camera").GetComponent<Camera>();
		firstPersonCam = GameObject.Find("Main Camera").GetComponent<Camera>();

		// Controller Inits
		_game = GameObject.Find(" GameController").GetComponent<GameController>();
		_level = _game.gameObject.GetComponent<LevelController>();
		_player = GameObject.Find("Player").GetComponent<PlayerController>();
		_weapon = _player.gameObject.GetComponentInChildren<WeaponController>();

		// Path Drawer
		pathDrawer = Resources.Load("Prefabs/Tools/PathDrawer", typeof(GameObject)) as GameObject;

		firstPersonCam.enabled = true;
		minimapCam.enabled = false;
	}

	void Update() {
		// Toggle Camera View
		if (Input.GetKeyDown(KeyCode.C)) {
			SetOverviewCamera(!minimapCam.enabled);
		}

		if (minimapCam.enabled) {
			float forwardMoveAmount = Input.GetAxis("Vertical") * forwardSpeed * Time.deltaTime;
			float sideMoveAmount = Input.GetAxis("Horizontal") * forwardSpeed * Time.deltaTime;
			transform.Translate(sideMoveAmount, 0, forwardMoveAmount, Space.World);

			// Path Drawers
			pathInterval -= Time.deltaTime;
			if (pathInterval <= 0) {
				pathInterval = INTERVAL;

				GameObject g = Instantiate(pathDrawer) as GameObject;
				g.GetComponent<PathFollower>().path = _level.MotionPath;
			}
		}
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

			pathInterval = 0;
		} else {
			/*
			 * First Person Camera
			 */
			_weapon.SetActiveWeapon(WeaponController.TOOL_WEAPON);
			_weapon.WeaponSwapLock = false;
			_game.CaptureCursor = true;

			// Re-Enable the Player
			_player.SetEnabled(true);

			// Destroy all Path Drawers
			GameObject[] g = GameObject.FindGameObjectsWithTag("PathDrawer");
			foreach (GameObject tg in g) {
				GameObject.Destroy(tg);
			}
		}
	}
}