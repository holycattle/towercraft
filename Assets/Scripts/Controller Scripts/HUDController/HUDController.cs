using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour {
	private GameController _game;
	private WaveController _wave;
	private PlayerController _player;
	private Weapon _weapon;
	public const int LIVES_WIDTH = 256;
	public const int LIVES_HEIGHT = 64;
	public const int TEXT_HEIGHT = 30;
	public GUIStyle style;

	// Life Bar
	private Rect lifeBarRect;
	private Vector2 pos = new Vector2(100, 100);
	private Vector2 size = new Vector2(312, 60);
	public Texture2D lifeEmpty;
	public Texture2D lifeFull;
	
	void Start() {
		_game = GameObject.Find(" GameController").GetComponent<GameController>();
		_wave = GameObject.Find(" GameController").GetComponent<WaveController>();
		_player = GameObject.Find("Player").GetComponent<PlayerController>();
		_weapon = GameObject.Find("LaserGun").GetComponent<Weapon>();

		lifeEmpty = Resources.Load("Textures/GUI/Health/empty", typeof(Texture)) as Texture2D;
		lifeFull = Resources.Load("Textures/GUI/Health/full", typeof(Texture)) as Texture2D;
		lifeBarRect = new Rect(0, Screen.height - LIVES_HEIGHT, LIVES_WIDTH, LIVES_HEIGHT);
	}

	void OnGUI() {
		GUI.backgroundColor = Color.grey;
		
		GUI.backgroundColor = Color.grey;
		GUI.Box(new Rect(0, 0, LIVES_WIDTH, LIVES_HEIGHT), "Lives Left: " + _game.Lives);
		GUI.Box(new Rect(Screen.width - LIVES_WIDTH, TEXT_HEIGHT * 2, LIVES_WIDTH, LIVES_HEIGHT), "Approaching in: " + (_wave.TimeTillNextWavex100 / 100f).ToString("F2"));
		// Draw Bullet Count
		GUI.Box(new Rect(0, Screen.height - TEXT_HEIGHT - LIVES_HEIGHT, LIVES_WIDTH, LIVES_HEIGHT), "Bullets: " + _weapon.Ammo + " / " + _weapon.totalAmmo);

		// Life Bar
		GUI.BeginGroup(lifeBarRect);
		GUI.DrawTexture(new Rect(0, 0, LIVES_WIDTH, LIVES_HEIGHT), lifeEmpty);
		GUI.BeginGroup(new Rect(0, 0, LIVES_WIDTH * ((float)_player.Life / PlayerController.MAX_LIFE), LIVES_HEIGHT));
		GUI.DrawTexture(new Rect(0, 0, LIVES_WIDTH, LIVES_HEIGHT), lifeFull);
		GUI.EndGroup();
		GUI.EndGroup();
		GUI.Box(lifeBarRect, "LIFE: " + _player.Life, style);
	}
}
