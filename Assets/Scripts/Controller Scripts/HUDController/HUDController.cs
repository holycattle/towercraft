using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour {
	private GameController _game;
	private WaveController _wave;
	private PlayerController _player;
	private Weapon _weapon;
	
	public const int LIVES_WIDTH = 128;
	public const int LIVES_HEIGHT = 30;
	public const int TEXT_HEIGHT = 30;
	public GUIStyle style;
	
	void Start() {
		_game = GameObject.Find(" GameController").GetComponent<GameController>();
		_wave = GameObject.Find(" GameController").GetComponent<WaveController>();
		_player = GameObject.Find("Player").GetComponent<PlayerController>();
		_weapon = GameObject.Find("LaserGun").GetComponent<Weapon>();
	}

	void OnGUI() {
		GUI.backgroundColor = Color.grey;
		
		GUI.backgroundColor = Color.grey;
		GUI.Box(new Rect(0, 0, LIVES_WIDTH, LIVES_HEIGHT), "Lives Left: " + _game.Lives);
		GUI.Box(new Rect(0, TEXT_HEIGHT * 2, LIVES_WIDTH, LIVES_HEIGHT), "Next Wave: " + (_wave.TimeTillNextWavex100 / 100f).ToString("F2"));
		GUI.Box(new Rect(0, Screen.height - TEXT_HEIGHT, LIVES_WIDTH, LIVES_HEIGHT), "LIFE: " + _player.Life);
		
		// Draw Bullet Count
		GUI.Box(new Rect(0, 120, 128, 30), "Bullets: " + _weapon.Ammo + " / " + _weapon.totalAmmo);
	}
}
