using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {
	// Constants
	public const int TOOL_BUILDER = 0;
	public const int TOOL_WEAPON = 1;
	private Vector2 SCREEN_CENTER = new Vector2(Screen.width / 2, Screen.height / 2);
	private string OnMouseEnter = "InputMouseEnter";
	private string OnMouseExit = "InputMouseExit";

	// RayCasting
	private int layerMask;
	private GameObject _activeTarget;

	// Game Controller
	private GameController _game;

	// Player Components
	private GameObject[] weapons;
	private int _activeTool;
	private bool _weaponSwapLock;

	// HUD Components
//	public const int CROSSHAIR_OFFSET = 32;		// Screen units (also used for gun inaccuracy)
//	private int[] X_OFFSET = {-1, 1, 1, -1};
//	private int[] Y_OFFSET = {-1, -1, 1, 1};
//	public Texture2D crosshair0;
//	public Texture2D crosshair1;
//	public Texture2D crosshair2;
//	public Texture2D crosshair3;
//	private Rect[] crosshairRect;
	public bool drawCrosshair;

	// Gun Movement (User Defined)
	private float verticalImpulse = 0.06f;
	private float impulseRecovery = 0.3f;
	private float weaponRotAmount = 0.4f;	// 0 = No Movement, 1 = Instant Movement. Domain: [0, 1]

	//	// Gun Movement (Non-User Defined)
	private float _currentImpulse = 0; 		// 0 = No Impulse, +-1 = Max Impulse - Range: [-1, 1]. Used for jumping and landing force.
	private Vector3 baseRotation;
	private Vector3 basePosition;

	void Start() {
		SCREEN_CENTER = new Vector3(Screen.width / 2, Screen.height / 2, 0);

		_activeTarget = null;
		layerMask = 1 << 8;

		_game = GameObject.Find(" GameController").GetComponent<GameController>();

		weapons = new GameObject[transform.GetChildCount()];
		for (int i = 0; i < weapons.Length; i++) {
			weapons[i] = transform.GetChild(i).gameObject;
		}

		_activeTool = 0;
		SetActiveWeapon(0);
		SetActiveWeapon(1);
		SetActiveWeapon(_activeTool);

		// Gun Movement
		baseRotation = transform.rotation.eulerAngles;
		basePosition = transform.localPosition;

		drawCrosshair = true;

		// Idle Movement
//		Vector3[] idlePath = new Vector3[4];
//		idlePath[0] = transform.localPosition + new Vector3(0, -1, 0);
//		idlePath[1] = transform.localPosition + new Vector3(1, 0, 0);
//		idlePath[2] = transform.localPosition + new Vector3(0, 1, 0);
//		idlePath[3] = transform.localPosition + new Vector3(-1, 0, 0);
//		iTween.MoveFrom(gameObject, iTween.Hash("path", idlePath, "time", 1,
//			"islocal", true,
//			"looptype", iTween.LoopType.loop, "easetype", iTween.EaseType.linear));
	}

	void FixedUpdate() {
		if (_game.ActiveMenu == Menu.Game) {
			/*
			 *	Keyboard Presses
			 */

			if (!_weaponSwapLock) {
				/*
				 *	Changing of Active Weapon
				 */
				// TODO: Why do you have to use both GetKey + GetKeyDown?
				if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKey(KeyCode.Alpha1)) {
					SetActiveWeapon(0);
				} else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKey(KeyCode.Alpha2)) {
					SetActiveWeapon(1);
				}
			}
		}
	}

	public void OnStartSprint() {
		Animation a = GetComponent<Animation>();
		foreach (AnimationState st in a) {
			st.speed = 1f;
			st.time = 0;
		}
		a.Play("Gun_fronttoside");
	}

	public void OnEndSprint() {
		Animation a = GetComponent<Animation>();
		foreach (AnimationState st in a) {
			st.speed = -1f;
			st.time = st.length;
		}
		a.Play("Gun_fronttoside");
	}

	void Update() {
	}

	void LateUpdate() {
		if (_game.ActiveMenu == Menu.Game) {
			/*
			 *	Mouse CLicks
			 */
			// Firing of Gun
			GameObject rayCastedObject = RaycastGameObject();

			if (_activeTarget == rayCastedObject) {
				// Do Nothing
			} else {
				if (_activeTarget != null) {
					_activeTarget.SendMessage(OnMouseExit, null, SendMessageOptions.DontRequireReceiver);
				}
				if (rayCastedObject != null) {
					rayCastedObject.SendMessage(OnMouseEnter, null, SendMessageOptions.DontRequireReceiver);
				} else {
				}
				_activeTarget = rayCastedObject;
			}

			if (Input.GetMouseButtonDown(0)) {
				ActiveTool.MouseClickOn(rayCastedObject);
			} else if (Input.GetMouseButton(0)) {
				ActiveTool.MouseDownOn(rayCastedObject);
			} else if (Input.GetMouseButtonUp(0)) {
				ActiveTool.MouseUpOn(rayCastedObject);
			}

			ImpulseUpdate();		// Affects localPosition (should affect localRotation eventually)
			WeaponRotation();	// Affects localRotation
		}
	}

	private void ImpulseUpdate() {
		_currentImpulse *= impulseRecovery;
//		transform.localPosition = new Vector3(basePosition.x,
//												basePosition.y + _currentImpulse * verticalImpulse,
//												basePosition.z);
//		transform.localRotation = Quaternion.Euler(transform.localRotation.x - currentRecoil * upwardRecoil, transform.localRotation.y, transform.localRotation.z);
	}

	private void WeaponRotation() {
		int mouseClamp = 6;
		float mHoriz = -Input.GetAxis("Mouse X") * mouseClamp;
		float mVert = Input.GetAxis("Mouse Y") * mouseClamp;

		int keyClamp = 6; // Measured in degrees
		float kHoriz = Input.GetAxis("Horizontal") * keyClamp;
		float kRot = -Input.GetAxis("Horizontal") * keyClamp;
		float kVert = -Input.GetAxis("Vertical") * keyClamp;

		transform.localRotation = Quaternion.Slerp(transform.localRotation,
			Quaternion.Euler(baseRotation.x + kVert + mVert, baseRotation.y + kHoriz + mHoriz, baseRotation.z + kRot), weaponRotAmount);
	}

	private GameObject RaycastGameObject() {
		// Builds a ray from camera point of view to the mouse position
		Ray ray;

		if (_game.CaptureCursor) {
			ray = Camera.main.ScreenPointToRay(SCREEN_CENTER);
		} else {
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		}
		RaycastHit hit;

		// Casts the ray and get the first game object hit
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
			return hit.transform.gameObject;
		else
			return null;
	}

	public void OnLand() {
//		_currentImpulse = -1;
//		iTween.PunchPosition(gameObject, iTween.Hash("amount", new Vector3(0, -verticalImpulse, 0), "time", 1f, "space", Space.Self));
//		iTween.MoveFrom(gameObject,
//			iTween.Hash("position", transform.localPosition + new Vector3(0, -verticalImpulse, 0),
//			"time", 0.25f, "islocal", true, "easetype", iTween.EaseType.easeOutCubic));

//		iTween.MoveAdd(gameObject, iTween.Hash("amount", new Vector3(0, 1f, 0), "time", 3, "islocal", true));
	}

	public void OnJump() {
//		_currentImpulse = 1;
//		iTween.PunchPosition(gameObject, iTween.Hash("amount", new Vector3(0, verticalImpulse, 0), "time", 1f, "space", Space.Self));
//		iTween.MoveFrom(gameObject,
//			iTween.Hash("position", transform.localPosition + new Vector3(0, verticalImpulse / 2, 0),
//			"time", 1f, "islocal", true, "easetype", iTween.EaseType.easeOutCubic));
	}

	public int RaycastLayer {
		set { layerMask = value; }
		get { return layerMask; }
	}

	public void SetActiveWeapon(int i) {
		if (i < 0 || i >= 2)
			return;
		if (_activeTool == i)
			return;

		foreach (GameObject g in weapons) {
			g.SetActive(false);
			g.GetComponent<GameTool>().WhenUnequipped();
		}
		_activeTool = i;
		weapons[_activeTool].SetActive(true);
		ActiveTool.WhenEquipped();
	}

	public bool WeaponSwapLock {
		get { return _weaponSwapLock;}
		set { _weaponSwapLock = value;}
	}

	public GameObject ActiveToolObject {
		get { return weapons[_activeTool]; }
	}

	public GameTool ActiveTool {
		get { return weapons[_activeTool].GetComponent<GameTool>(); }
	}
}
