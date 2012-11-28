using UnityEngine;
using System.Collections;

public class GameTool : MonoBehaviour {
	private Vector2 SCREENCENTER = new Vector2(Screen.width / 2, Screen.height / 2);

	// Default Constants
	protected const int DEFAULT_crosshairOffet = 48;	// Maximum outward pixels crosshair can move
	protected const float DEFAULT_backRecoil = 0.25f;					// World Space moved (World Units)
	protected const float DEFAULT_recoilRecovery = 0.025f;	 			// 0 = Instant Recovery, 1 = No Recovery. Domain: [0, 1]
	protected const float DEFAULT_recoilAmount = 0.4f;				// 0 = No Recoil, 1 = Instant Max Recoil. Domain: [0, 1]


	// Objects
	protected GameController _game;
	protected WeaponController _weapon;
	protected LevelController _level;

	// GUI Elements
	private int[] X_OFFSET = {-1, 1, 1, -1};
	private int[] Y_OFFSET = {-1, -1, 1, 1};
	public Texture2D[] crosshair;
	private Rect[] crosshairRect;

	// Recoil Movement (USER CONSTANTS)
	protected int crosshairOffset;		// Screen units (also used for gun inaccuracy)
	protected float backRecoil;		// World Space moved (World Units)
//	private float recoilRecovery = 0.95f; 	// 0 = Instant Recovery, 1 = No Recovery. Domain: [0, 1]
	protected float recoilRecovery; 	// 0 = Instant Recovery, 1 = No Recovery. Domain: [0, 1]
	protected float recoilAmount;		// 0 = No Recoil, 1 = Instant Max Recoil. Domain: [0, 1]

	// Recoil Movement (DYNAMIC)
	private float _currentRecoil = 0; 		// 0 = No Recoil, 1 = Max Recoil - Range: [0, 1]
//	private Vector3 baseRotation;
	private Vector3 basePosition;

	protected virtual void Awake() {
		_weapon = GameObject.Find("Player").GetComponentInChildren<WeaponController>();
		_game = GameObject.Find(" GameController").GetComponent<GameController>();
		_level = GameObject.Find(" GameController").GetComponent<LevelController>();
	}

	protected virtual void Start() {
		// HUD Components
		crosshair = new Texture2D[4];
		crosshairRect = new Rect[4];
		for (int i= 0; i < crosshair.Length; i++) {
			crosshair[i] = Resources.Load("Textures/d" + i) as Texture2D;
			crosshairRect[i] = new Rect(Screen.width / 2 - crosshair[i].width / 2, Screen.height / 2 - crosshair[i].height / 2,
				crosshair[i].width, crosshair[i].height);
		}

		// Recoil Defaults
		crosshairOffset = DEFAULT_crosshairOffet;
		backRecoil = DEFAULT_backRecoil;
		recoilRecovery = DEFAULT_recoilRecovery;
		recoilAmount = DEFAULT_recoilAmount;

		// Gun Movement
//		baseRotation = transform.rotation.eulerAngles;
		basePosition = transform.localPosition;
	}

	protected virtual void OnGUI() {
		if (!_weapon.drawCrosshair)
			return;

		GUI.DrawTexture(crosshairRect[0], crosshair[0]);
		GUI.DrawTexture(crosshairRect[1], crosshair[1]);
		GUI.DrawTexture(crosshairRect[2], crosshair[2]);
		GUI.DrawTexture(crosshairRect[3], crosshair[3]);
	}

	protected virtual void FixedUpdate() {
		RecoilUpdate();		// Affects localPosition (should affect localRotation eventually)

//		Debug.Log("Recoil: " + CurrentRecoil);
	}

	protected virtual void Update() {
	}

	public virtual void WhenEquipped() {
	}

	public virtual void WhenUnequipped() {
	}

	public virtual void MouseClickOn(GameObject g) {
	}

	public virtual void MouseDownOn(GameObject g) {
	}

	public virtual void MouseUpOn(GameObject g) {
	}

	protected void RecoilUpdate() {
		transform.localPosition = new Vector3(basePosition.x,
												basePosition.y,
												basePosition.z - CurrentRecoil * backRecoil);

		_currentRecoil -= recoilRecovery;
		if (_currentRecoil < 0) {
			_currentRecoil = 0;
		}

		// Update position of crosshair
		int offset = (int)(CurrentRecoil * crosshairOffset);
		for (int i = 0; i < crosshairRect.Length; i++) {
			crosshairRect[i].center = SCREENCENTER + new Vector2(X_OFFSET[i], Y_OFFSET[i]) * offset;
		}
	}

	protected void Recoil() {
		_currentRecoil += Mathf.Sqrt(1 - _currentRecoil) * recoilAmount;
	}

	public float CurrentRecoil {
		get { return _currentRecoil; }
//		get { return _currentRecoil / RECOIL_ASYMPTOTE; }
	}
}
