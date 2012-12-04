using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour {
	private GameController _game;
	private WaveController _wave;
	private PlayerController _player;
	private Weapon _weapon;
	public const int PADDING = 48;
	public const int LIVES_WIDTH = 256;
	public const int LIVES_HEIGHT = 32;
	public const int AMMO_WIDTH = 128;
	public const int AMMO_HEIGHT = 32;
	public const int TEXT_HEIGHT = 30;
	public GUIStyle style;

	// Life Bar
	private Rect lifeBarRect;
	public Texture2D lifeEmpty;
	public Texture2D lifeFull;

	// Ammo Bar
	private Rect ammoBarRect;
	public Texture2D ammoEmpty;
	public Texture2D ammoFull;

	void Start() {
		_game = GameObject.Find(" GameController").GetComponent<GameController>();
		_wave = GameObject.Find(" GameController").GetComponent<WaveController>();
		_player = GameObject.Find("Player").GetComponent<PlayerController>();
		_weapon = GameObject.Find("LaserGun").GetComponent<Weapon>();

		lifeEmpty = Resources.Load("Textures/GUI/Health/empty6", typeof(Texture)) as Texture2D;
		lifeFull = Resources.Load("Textures/GUI/Health/full6", typeof(Texture)) as Texture2D;
		lifeBarRect = new Rect(PADDING, Screen.height - LIVES_HEIGHT - PADDING, LIVES_WIDTH, LIVES_HEIGHT);

		ammoEmpty = Resources.Load("Textures/GUI/Health/ammo00", typeof(Texture)) as Texture2D;
		ammoFull = Resources.Load("Textures/GUI/Health/ammo01", typeof(Texture)) as Texture2D;
		ammoBarRect = new Rect(PADDING, Screen.height - LIVES_HEIGHT - AMMO_HEIGHT - PADDING, AMMO_WIDTH, AMMO_HEIGHT);
	}

	void OnGUI() {
		GUI.backgroundColor = Color.grey;
		
		GUI.backgroundColor = Color.grey;
		GUI.Box(new Rect(0, 0, LIVES_WIDTH, LIVES_HEIGHT), "Lives Left: " + _game.Lives);
		if (!_wave._waveActive) {
			GUI.Box(new Rect(Screen.width - LIVES_WIDTH, 0, LIVES_WIDTH, LIVES_HEIGHT), "Incoming wave: " + _wave.getNextWave());
			GUI.Box(new Rect(Screen.width - LIVES_WIDTH, TEXT_HEIGHT * 2, LIVES_WIDTH, LIVES_HEIGHT), "Approaching in: " + (_wave.TimeTillNextWavex100 / 100f).ToString("F2"));
			GUI.Box(new Rect(Screen.width - LIVES_WIDTH, TEXT_HEIGHT * 3, LIVES_WIDTH, LIVES_HEIGHT), "Press N to continue to proceed to next wave.");
		}
		// Draw Bullet Count
//		GUI.Box(new Rect(0, Screen.height - TEXT_HEIGHT - LIVES_HEIGHT, LIVES_WIDTH, LIVES_HEIGHT), "Bullets: " + _weapon.Ammo + " / " + _weapon.totalAmmo);

		// Life Bar
		GUI.BeginGroup(lifeBarRect);
		GUI.DrawTexture(new Rect(0, 0, LIVES_WIDTH, LIVES_HEIGHT), lifeEmpty);
		GUI.BeginGroup(new Rect(0, 0, LIVES_WIDTH * ((float)_player.Life / PlayerController.MAX_LIFE), LIVES_HEIGHT));
		GUI.DrawTexture(new Rect(0, 0, LIVES_WIDTH, LIVES_HEIGHT), lifeFull);
		GUI.EndGroup();
		GUI.EndGroup();
		GUI.Box(lifeBarRect, "LIFE: " + _player.Life, style);

		// Ammo Bar Bar
		GUI.BeginGroup(ammoBarRect);
		GUI.DrawTexture(new Rect(0, 0, AMMO_WIDTH, AMMO_HEIGHT), ammoEmpty);
		GUI.BeginGroup(new Rect(0, 0, AMMO_WIDTH * ((float)_weapon.Ammo / _weapon.MagazineSize), AMMO_HEIGHT));
		GUI.DrawTexture(new Rect(0, 0, AMMO_WIDTH, AMMO_HEIGHT), ammoFull);
		GUI.EndGroup();
		GUI.EndGroup();
		GUI.Box(ammoBarRect, "Bullets: " + _weapon.Ammo + " / " + _weapon.TotalAmmo, style);
	}
}
