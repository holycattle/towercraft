using UnityEngine;
using System.Collections;

public class GameTool : MonoBehaviour {
	protected GameController _game;
	protected WeaponController _weapon;
	protected LevelController _level;
	public int bullets;

	// Recoil Movement (USER CONSTANTS)
	private float backRecoil = 0.5f;		// World Space moved (World Units)
	private float recoilRecovery = 0.95f; 	// 0 = Instant Recovery, 1 = No Recovery. Domain: [0, 1]
	private float recoilRecovery2 = 0.01f; 	// 0 = Instant Recovery, 1 = No Recovery. Domain: [0, 1]
	private float recoilAmount = 0.4f;		// 0 = No Recoil, 1 = Instant Max Recoil. Domain: [0, 1]
	private const float RECOIL_ASYMPTOTE = 0.7f;

	// Recoil Movement (DYNAMIC)
	private float _currentRecoil = 0; 		// 0 = No Recoil, 1 = Max Recoil - Range: [0, 1]
	private Vector3 baseRotation;
	private Vector3 basePosition;

	protected virtual void Awake() {
		_weapon = GameObject.Find("Player").GetComponentInChildren<WeaponController>();
		_game = GameObject.Find(" GameController").GetComponent<GameController>();
		_level = GameObject.Find(" GameController").GetComponent<LevelController>();

		bullets = 0;
	}

	protected virtual void Start() {
		// Gun Movement
		baseRotation = transform.rotation.eulerAngles;
		basePosition = transform.localPosition;
	}

	protected virtual void OnGUI() {
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
//		_currentRecoil *= recoilRecovery;
		_currentRecoil -= recoilRecovery2;
		if (_currentRecoil < 0) {
			_currentRecoil = 0;
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
