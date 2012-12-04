using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	Material DEFAULT_SKYBOX = null;
	
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
	//private float terrainWidth;
	//private float terrainHeight;
	const float SCROLL_WIDTH = 10f;
	const float SCROLL_HEIGHT = 10f;
	const int SCROLL_SPEED = 32;

	void Start() {
		//Terrain t = GameObject.Find("[Root]World").GetComponent<Terrain>();
		//terrainWidth = t.terrainData.size.y;
		//terrainHeight = t.terrainData.size.x;
		//Debug.Log("terrain width = " + terrainWidth.ToString());
		//Debug.Log("terrain height = " + terrainHeight.ToString());
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
		if (Input.GetKeyDown(KeyCode.C)) {
			SetOverviewCamera(!minimapCam.enabled);
		}
		
		int top = 0; //1 if going up, -1 if going down
		int left = 1; //1 if going right, -1 if going left
		float forwardMoveAmount;
		float sideMoveAmount;
		if (minimapCam.enabled) {
			mousePos = Input.mousePosition;
			//Debug.Log("x: " + mousePos.x.ToString() + "; y: " + mousePos.y.ToString());
			//Debug.Log("screen width: " + Screen.width.ToString() + "; screen height: " + Screen.height.ToString());
			if(mousePos.x < SCROLL_WIDTH) {
				left = -1;
			} else if(mousePos.x >= Screen.width - SCROLL_WIDTH)
				left = 1;
			else left = 0;
			
			if(mousePos.y <= 0) {
				top = -1;
			} else if(mousePos.y >= Screen.height)
				top = 1;
			else top = 0;
			
			forwardMoveAmount = top * SCROLL_SPEED * Time.deltaTime;
			sideMoveAmount = left * SCROLL_SPEED * Time.deltaTime;
			//legacy support for WSAD movement
			forwardMoveAmount = Input.GetAxis("Vertical") * SCROLL_SPEED * Time.deltaTime;
			sideMoveAmount = Input.GetAxis("Horizontal") * SCROLL_SPEED * Time.deltaTime;
			transform.Translate(sideMoveAmount, 0, forwardMoveAmount, Space.World);
			
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