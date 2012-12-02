using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour {
	private GameController _game;
	private WaveController _wave;
	private PlayerController _player;
	public const int LIVES_WIDTH = 128;
	public const int LIVES_HEIGHT = 30;
	public const int TEXT_HEIGHT = 30;
	public GUIStyle style;
	
	void Start() {
		_game = GameObject.Find(" GameController").GetComponent<GameController>();
		_wave = GameObject.Find(" GameController").GetComponent<WaveController>();
		_player = GameObject.Find("Player").GetComponent<PlayerController>();
		
	}

	void OnGUI() {
		GUI.backgroundColor = Color.grey;
		
		GUI.backgroundColor = Color.grey;
		GUI.Box(new Rect(0, 0, LIVES_WIDTH, LIVES_HEIGHT), "Lives Left: " + _game.Lives, style);
		GUI.Box(new Rect(0, TEXT_HEIGHT * 2, LIVES_WIDTH, LIVES_HEIGHT), "Next Wave: " + (_wave.TimeTillNextWavex100 / 100f).ToString("F2"), style);
		GUI.Box(new Rect(0, Screen.height - TEXT_HEIGHT, LIVES_WIDTH, LIVES_HEIGHT), "LIFE: " + _player.Life, style);
	}
}
