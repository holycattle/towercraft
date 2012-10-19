using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {
	private Vector3 SCREEN_CENTER = new Vector3(Screen.width / 2, Screen.height / 2, 0);
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

	// Gun Bobbing
	private float timer = 0;
	private float actualBobbingAmount;
	public float bobbingSpeed = 12f;
	public float bobbingAmount = 0.05f;
	public float bobbingMidpoint;

	// Recoil
	void Start() {
		_activeTarget = null;
		layerMask = 1 << 8;

		_game = GameObject.Find(" GameController").GetComponent<GameController>();

		weapons = new GameObject[transform.GetChildCount()];
		for (int i = 0; i < weapons.Length; i++) {
			weapons[i] = transform.GetChild(i).gameObject;
		}

		_activeTool = 0;
		SetActiveWeapon(_activeTool);

		// Gun Bobbing
		bobbingMidpoint = transform.localPosition.y;
		actualBobbingAmount = bobbingAmount;
	}

	void OnGUI() {
		GUI.backgroundColor = Color.grey;
		GUI.Box(new Rect(0, 120, 128, 30), "Bullets: " + ActiveTool.bullets);
	}

	void Update() {
		if (Screen.lockCursor) {
			if (_game.Active) {
				Time.timeScale = 1;
			} else {
				Screen.lockCursor = false;	// (Locked) AND (Not Active) = UNlock Cursor
				Time.timeScale = 0.6f;
			}
		} else {
			Time.timeScale = 1;
			Screen.lockCursor = _game.Active;
		}

		/*
		 * -----Keyboard Presses-----
		 */
		// Changing of Active Weapon
		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			SetActiveWeapon(0);
		} else if (Input.GetKeyDown(KeyCode.Alpha2)) {
			SetActiveWeapon(1);
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

		BobWeapon();
	}

	private void BobWeapon() {
		float waveslice = 0.0f;
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");

		if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0) {
			timer = 0;
		} else {
			waveslice = Mathf.Sin(timer); // Range: [-1, 1]
			timer += bobbingSpeed * Time.deltaTime;
			if (timer > Mathf.PI * 2) {
				timer -= (Mathf.PI * 2);
			}
		}

		if (waveslice != 0) {
			float translateChange = waveslice * actualBobbingAmount;
			float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
			totalAxes = Mathf.Clamp(totalAxes, 0, 1);
			translateChange = totalAxes * translateChange;

			transform.localPosition = new Vector3(transform.localPosition.x, bobbingMidpoint + translateChange, transform.localPosition.z);
		} else {
			transform.localPosition = new Vector3(transform.localPosition.x, bobbingMidpoint, transform.localPosition.z);
		}
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

	public void setBobbing(bool b) {
		if (b)
			actualBobbingAmount = bobbingAmount;
		else
			actualBobbingAmount = 0;
	}
}
