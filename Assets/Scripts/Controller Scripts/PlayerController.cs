using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public const int LIVES_WIDTH = 128;
	public const int LIVES_HEIGHT = 30;
	public const int TEXT_HEIGHT = 30;
	private Vector3 DISABLETRANSFORM = new Vector3(0, 100, 0);

	// Game Controllers
	private GameController _game;
	private WaveController _wave;
	private WeaponController _weapon;

	// Player Variables
	public const int MAX_LIFE = 100;
	private int _life;

	// GUI
	public Texture2D menu;
	public Texture2D item;
	
	// Damage bars
	private MyTexture2D leftDamage;
	private MyTexture2D rightDamage;
//	private Rect leftDmgBox;
//	private Rect rightDmgBox;
	private float timeLeft;
	private float timeDamage = 0.25f;

	void Start() {
		_game = GameObject.Find(" GameController").GetComponent<GameController>();
		_wave = GameObject.Find(" GameController").GetComponent<WaveController>();
		_weapon = GetComponentInChildren<WeaponController>();

		Screen.showCursor = true;

		Texture2D left = Resources.Load("Textures/Weapon/TakeDamage0", typeof(Texture2D)) as Texture2D;
		Texture2D right = Resources.Load("Textures/Weapon/TakeDamage1", typeof(Texture2D)) as Texture2D;
		Color c = new Color(0.8f, 0, 0, 1f);
		leftDamage = new MyTexture2D(left,
			new Rect((Screen.width / 2) - left.width, (Screen.height - left.height) / 2, left.width, left.height), c);
		rightDamage = new MyTexture2D(right,
			 new Rect(Screen.width / 2, (Screen.height - right.height) / 2, right.width, right.height), c);

		// Player Variables
		_life = MAX_LIFE;
	}

	void Update() {
		if (timeLeft > 0) {
			timeLeft -= Time.deltaTime;
			leftDamage.Alpha = timeLeft / timeDamage;
			rightDamage.Alpha = timeLeft / timeDamage;
		}

		if (_life <= 0) {
			_life = MAX_LIFE;
			transform.position = new Vector3(0, 50, 0);
		}
	}

	void OnGUI() {
		GUI.backgroundColor = Color.grey;

		if (timeLeft > 0) {
			leftDamage.DrawMe();
			rightDamage.DrawMe();
//			GUI.DrawTexture(leftDmgBox, leftDamage);
//			GUI.DrawTexture(rightDmgBox, rightDamage);
		}
	}

	public void SetEnabled(bool b) {
		(GetComponent("NewCharacterMotor") as MonoBehaviour).enabled = b;
		MouseLook[] mLook = GetComponentsInChildren<MouseLook>();
		foreach (MouseLook m in mLook) {
			m.enabled = b;
		}

		transform.position += DISABLETRANSFORM * (b ? 1 : -1);
		_weapon.drawCrosshair = b;
	}

	public void AddLife(int amt) {
		_life += amt;

		if (amt < 0 && timeLeft <= 0) {
			timeLeft = timeDamage;
		}

		if (_life >= MAX_LIFE) {
			_life = MAX_LIFE;
		}
//		if (_life <= 0)
//			Lose();
	}

	public int Life {
		get { return _life; }
	}
}
