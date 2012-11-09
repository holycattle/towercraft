using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {
	private Vector3 SCREEN_CENTER;
	private string OnMouseEnter = "InputMouseEnter";
	private string OnMouseExit = "InputMouseExit";

	// RayCasting
	private int layerMask; // Temporarily Public
	private GameObject _activeTarget;

	// Game Controller
	private GameController _game;

	// Player Components
	public GameObject[] weapons;
	private int _activeTool;

	// Gun Movement (User Defined)
	private float verticalImpulse = 0.2f;	// World Space moved (World Units)
	private float impulseRecovery = 0.3f;
	//--
	private float backRecoil = 0.5f;		// World Space moved (World Units)
	private float weaponRotAmount = 0.4f;	// 0 = No Movement, 1 = Instant Movement. Domain: [0, 1]
	private float recoilRecovery = 0.95f; 	// 0 = Instant Recovery, 1 = No Recovery. Domain: [0, 1]
	private float recoilAmount = 0.3f;		// 0 = No Recoil, 1 = Instant Max Recoil. Domain: [0, 1]

	// Gun Movement (Non-User Defined)
	private float currentRecoil = 0; 		// 0 = No Recoil, 1 = Max Recoil - Range: [0, 1]
	private float currentImpulse = 0; 		// 0 = No Impulse, +-1 = Max Impulse - Range: [-1, 1]. Used for jumping and landing force.
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
		SetActiveWeapon(_activeTool);

		// Gun Movement
		baseRotation = transform.rotation.eulerAngles;
		basePosition = transform.localPosition;
	}

	void OnGUI() {
		GUI.backgroundColor = Color.grey;
		GUI.Box(new Rect(0, 120, 128, 30), "Bullets: " + ActiveTool.bullets);
	}

	void FixedUpdate() {
		if (_game.ActiveMenu == Menu.Game) {
			/*
			 * -----Keyboard Presses-----
			 */
			// Changing of Active Weapon
			if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKey(KeyCode.Alpha1)) {			// TODO: Why do you have to use both GetKey + GetKeyDown?
				SetActiveWeapon(0);
			} else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKey(KeyCode.Alpha2)) {
				SetActiveWeapon(1);
			}

			// ----TESTING!!!----
			if (Input.GetKeyDown(KeyCode.P) || Input.GetKey(KeyCode.P)) {
				ComponentGenerator.Get().GenerateComponent(0);
			}

			/*
			 * ----Mouse Clicks-----
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

			RecoilUpdate();		// Affects localPosition (should affect localRotation eventually)
			WeaponRotation();	// Affects localRotation
		}
	}

	public void Recoil() {
		currentRecoil += (1 - currentRecoil) * recoilAmount;
	}

	private void RecoilUpdate() {
		currentRecoil *= recoilRecovery;
		currentImpulse *= impulseRecovery;
		transform.localPosition = new Vector3(basePosition.x,
												basePosition.y + currentImpulse * verticalImpulse,
												basePosition.z - currentRecoil * backRecoil);
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
		Ray ray = Camera.main.ScreenPointToRay(SCREEN_CENTER);
		RaycastHit hit;

		// Casts the ray and get the first game object hit
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
			return hit.transform.gameObject;
		else
			return null;
	}

	public void OnLand() {
		currentImpulse = -1;
	}

	public void OnJump() {
		currentImpulse = 1;
	}

	public int RaycastLayer {
		set { layerMask = 1 << value; }
		get { return layerMask; }
	}

	public void SetActiveWeapon(int i) {
		if (i < 0 || i >= 2)
			return;

		foreach (GameObject g in weapons) {
			g.SetActiveRecursively(false);
			g.GetComponent<GameTool>().WhenUnequipped();
		}
		_activeTool = i;
		weapons[_activeTool].SetActiveRecursively(true);
		ActiveTool.WhenEquipped();
	}

	public GameObject ActiveToolObject {
		get { return weapons[_activeTool]; }
	}

	public GameTool ActiveTool {
		get { return weapons[_activeTool].GetComponent<GameTool>(); }
	}
}
