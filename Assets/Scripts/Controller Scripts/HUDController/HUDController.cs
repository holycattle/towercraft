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
	public const int ENEMY_LIFE_WIDTH = 128;
	public const int ENEMY_LIFE_HEIGHT = 32;
	public const int ENEMY_DATA_WIDTH = 192;
	public const int ENEMY_DATA_HEIGHT = 30;
	public const int AMMO_WIDTH = 128;
	public const int AMMO_HEIGHT = 32;
	public const int TEXT_HEIGHT = 30;
	public GUISkin uiSkin;
	public GUIStyle style;

	// Life Bar
	private Rect lifeBarRect;
	public Texture2D lifeEmpty;
	public Texture2D lifeFull;

	// Ammo Bar
	private Rect ammoBarRect;
	public Texture2D ammoEmpty;
	public Texture2D ammoFull;

	// Targetted Enemy
	private Rect enemyDataRect;
	private Rect eLifeBarRect;
	private Texture2D eLifeEmpty;
	private Texture2D eLifeFull;

	void Start() {
		_game = GameObject.Find(" GameController").GetComponent<GameController>();
		_wave = GameObject.Find(" GameController").GetComponent<WaveController>();
		_player = GameObject.Find("Player").GetComponent<PlayerController>();
		_weapon = GameObject.Find("LaserGun").GetComponent<Weapon>();

		uiSkin = Resources.Load("Fonts/UISkin", typeof(GUISkin)) as GUISkin;

		// Player Life
		lifeEmpty = Resources.Load("Textures/GUI/Health/empty6", typeof(Texture)) as Texture2D;
		lifeFull = Resources.Load("Textures/GUI/Health/full6", typeof(Texture)) as Texture2D;
		lifeBarRect = new Rect(PADDING, Screen.height - LIVES_HEIGHT - PADDING, LIVES_WIDTH, LIVES_HEIGHT);

		// Player Ammo
		ammoEmpty = Resources.Load("Textures/GUI/Health/ammo00", typeof(Texture)) as Texture2D;
		ammoFull = Resources.Load("Textures/GUI/Health/ammo01", typeof(Texture)) as Texture2D;
		ammoBarRect = new Rect(PADDING, Screen.height - LIVES_HEIGHT - AMMO_HEIGHT - PADDING, AMMO_WIDTH, AMMO_HEIGHT);

		// Enemy Life
		eLifeEmpty = Resources.Load("Textures/GUI/Health/eHPe-1", typeof(Texture)) as Texture2D;
		eLifeFull = Resources.Load("Textures/GUI/Health/eHPf-1", typeof(Texture)) as Texture2D;
		eLifeBarRect = new Rect((Screen.width - eLifeEmpty.width) / 2, Screen.height * 0.8f - eLifeEmpty.height, eLifeEmpty.width, eLifeEmpty.height);
		enemyDataRect = new Rect((Screen.width - ENEMY_DATA_WIDTH) / 2, eLifeBarRect.yMin - ENEMY_DATA_HEIGHT, ENEMY_DATA_WIDTH, ENEMY_DATA_HEIGHT);

		_game.Messenger.HUDMessage("Initializing HUD...", 5f);
	}

	void OnGUI() {
		GUI.skin = uiSkin;
		GUI.backgroundColor = Color.grey;
		
		GUI.backgroundColor = Color.grey;
		GUI.Box(new Rect(0, 0, LIVES_WIDTH, LIVES_HEIGHT), "Lives Left: " + _game.Lives);
		if (!_wave._waveActive) {
			GUI.Box(new Rect(Screen.width - LIVES_WIDTH, 0, LIVES_WIDTH, LIVES_HEIGHT), "Incoming wave: " + _wave.getNextWave());
			GUI.Box(new Rect(Screen.width - LIVES_WIDTH, TEXT_HEIGHT * 2, LIVES_WIDTH, LIVES_HEIGHT), "Approaching in: " + (_wave.TimeTillNextWavex100 / 100f).ToString("F2"));
			GUI.Box(new Rect(Screen.width - LIVES_WIDTH, TEXT_HEIGHT * 3, LIVES_WIDTH, LIVES_HEIGHT), "Press N to continue to proceed to next wave.");
		}
		
		if (_player.isDead) {
			GUI.Box(new Rect((Screen.width - LIVES_WIDTH) / 2, (Screen.height - TEXT_HEIGHT) / 2 - 50, LIVES_WIDTH, LIVES_HEIGHT), "Respawning in " + (_player.respawnCountdown).ToString("F2"));
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
		GUI.Label(lifeBarRect, "Life: " + _player.Life);

		// Ammo Bar Bar
		GUI.BeginGroup(ammoBarRect);
		GUI.DrawTexture(new Rect(0, 0, AMMO_WIDTH, AMMO_HEIGHT), ammoEmpty);
		GUI.BeginGroup(new Rect(0, 0, AMMO_WIDTH * ((float)_weapon.Ammo / _weapon.MagazineSize), AMMO_HEIGHT));
		GUI.DrawTexture(new Rect(0, 0, AMMO_WIDTH, AMMO_HEIGHT), ammoFull);
		GUI.EndGroup();
		GUI.EndGroup();
		GUI.Label(ammoBarRect, "Energy: " + _weapon.Ammo + " / " + _weapon.TotalAmmo);

		if (_weapon.TargettedEnemy != null) {
			GUI.Label(enemyDataRect, _weapon.TargettedEnemy.GetResistanceTypeAsString() + "-resistant " + _weapon.TargettedEnemy.Name);
			GUI.BeginGroup(eLifeBarRect);
			GUI.DrawTexture(new Rect(0, 0, ENEMY_LIFE_WIDTH, ENEMY_LIFE_HEIGHT), eLifeEmpty);
			GUI.BeginGroup(new Rect(0, 0, ENEMY_LIFE_WIDTH * ((float)_weapon.TargettedEnemy.Life / _weapon.TargettedEnemy.maxLife), ENEMY_LIFE_HEIGHT));
			GUI.DrawTexture(new Rect(0, 0, ENEMY_LIFE_WIDTH, ENEMY_LIFE_HEIGHT), eLifeFull);
			GUI.EndGroup();
			GUI.EndGroup();
			GUI.Label(eLifeBarRect, _weapon.TargettedEnemy.Life + " / " + _weapon.TargettedEnemy.maxLife);
		}

		GUI.skin = null;
	}
}
