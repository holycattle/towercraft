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
	public GUIStyle style;

	void Start() {
		_game = GameObject.Find(" GameController").GetComponent<GameController>();
		_wave = GameObject.Find(" GameController").GetComponent<WaveController>();
		_weapon = GetComponentInChildren<WeaponController>();

		Screen.showCursor = true;

		// Player Variables
		_life = MAX_LIFE;
	}

	void Update() {
		if (_life <= 0) {
			_life = MAX_LIFE;
			transform.position = new Vector3(0, 50, 0);
		}
	}

	void OnGUI() {
		GUI.backgroundColor = Color.grey;
		GUI.Box(new Rect(0, 0, LIVES_WIDTH, LIVES_HEIGHT), "Lives Left: " + _game.Lives, style);
		GUI.Box(new Rect(0, TEXT_HEIGHT, LIVES_WIDTH, LIVES_HEIGHT), "Money: " + _game.Money, style);
		GUI.Box(new Rect(0, TEXT_HEIGHT * 2, LIVES_WIDTH, LIVES_HEIGHT), "Next Wave: " + (_wave.TimeTillNextWavex100 / 100f).ToString("F2"), style);
		GUI.Box(new Rect(0, Screen.height - TEXT_HEIGHT, LIVES_WIDTH, LIVES_HEIGHT), "LIFE: " + _life, style);
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
