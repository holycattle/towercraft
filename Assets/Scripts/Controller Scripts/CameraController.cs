using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	Material DEFAULT_SKYBOX = null;
	const float SCROLL_WIDTH = 16f;
	const float SCROLL_HEIGHT = 16f;
	const int SCROLL_SPEED = 32;

	//map restraints
	const float MAP_MIN_X = -18.7f;
	const float MAP_MAX_X = 27f;
	const float MAP_MIN_Z = -70f;
	const float MAP_MAX_Z = 42f;
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
	private GameObject pathDrawer;
	private float pathInterval = 0;
	private Vector3 mousePos;
	private float terrainWidth;
	private float terrainHeight;
	
	void Start() {
		Debug.Log("terrain width = " + terrainWidth.ToString());
		Debug.Log("terrain height = " + terrainHeight.ToString());
		minimapCam = GameObject.Find("Minimap Camera").GetComponent<Camera>();
		firstPersonCam = GameObject.Find("Main Camera").GetComponent<Camera>();
		//set default skybox
		DEFAULT_SKYBOX = Resources.Load("Skyboxes/Skybox18", typeof(Material)) as Material;
		GameObject.Find("Main Camera").GetComponent<Skybox>().material = DEFAULT_SKYBOX;
		GameObject.Find("Minimap Camera").GetComponent<Skybox>().material = DEFAULT_SKYBOX;
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
		if (Input.GetKeyDown(KeyCode.C) && !_player.isDead) {
			SetOverviewCamera(!minimapCam.enabled);
		}
		
		int top = 0; //1 if going up, -1 if going down
		int left = 0; //1 if going right, -1 if going left
		float forwardMoveAmount;
		float sideMoveAmount;
		if (minimapCam.enabled) {
			mousePos = Input.mousePosition;
			if (mousePos.x < SCROLL_WIDTH) {
				left = -1;
			} else if (mousePos.x >= Screen.width - SCROLL_WIDTH)
				left = 1;
			else
				left = 0;
			
			if (mousePos.y < SCROLL_HEIGHT) {
				top = -1;
			} else if (mousePos.y >= Screen.height - SCROLL_HEIGHT)
				top = 1;
			else
				top = 0;
			
			forwardMoveAmount = top * SCROLL_SPEED * Time.deltaTime;
			sideMoveAmount = left * SCROLL_SPEED * Time.deltaTime;
			//legacy support for WSAD movement
			if (Input.GetAxis("Vertical") != 0)
				forwardMoveAmount = Input.GetAxis("Vertical") * SCROLL_SPEED * Time.deltaTime;
			if (Input.GetAxis("Horizontal") != 0)
				sideMoveAmount = Input.GetAxis("Horizontal") * SCROLL_SPEED * Time.deltaTime;
			moveCamera(sideMoveAmount, 0, forwardMoveAmount);

			// Path Drawers
			if (Input.GetKeyDown(KeyCode.Q)) {
				GameObject g = Instantiate(pathDrawer) as GameObject;
				g.GetComponent<PathFollower>().path = _level.MotionPath;
			}
//			pathInterval -= Time.deltaTime;
//			if (pathInterval <= 0) {
//				pathInterval = INTERVAL;
//
//				GameObject g = Instantiate(pathDrawer) as GameObject;
//				g.GetComponent<PathFollower>().path = _level.MotionPath;
//			}
		}
	}

	public void moveCamera(float x, float y, float z) {
		if (x + transform.position.x < MAP_MIN_X) {
			x = MAP_MIN_X - transform.position.x;
		} else if (x + transform.position.x > MAP_MAX_X) {
			x = MAP_MAX_X - transform.position.x;
		}

		if (z + transform.position.z < MAP_MIN_Z) {
			z = MAP_MIN_Z - transform.position.z;
		} else if (z + transform.position.z > MAP_MAX_Z) {
			z = MAP_MAX_Z - transform.position.z;
		}

		transform.Translate(x, y, z, Space.World);
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

			// Enable the Grid
			_level.DrawGrid = true;

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
	
			// Disable the Grid
			_level.DrawGrid = false;
	
			// Destroy all Path Drawers
			GameObject[] g = GameObject.FindGameObjectsWithTag("PathDrawer");
			foreach (GameObject tg in g) {
				GameObject.Destroy(tg);
			}
		}
		
	}
}