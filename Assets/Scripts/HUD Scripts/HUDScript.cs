using UnityEngine;
using System.Collections;

public class HUDScript : MonoBehaviour {

	public const int LIVES_WIDTH = 100;
	public const int LIVES_HEIGHT = 30;
	public const int TEXT_HEIGHT = 30;
	private GameController _game;
	private WaveController _wave;

	// Skin
	public GUIStyle style;

	void Start() {
		_game = GetComponent<GameController>();
		_wave = GetComponent<WaveController>();
	}
	
	void OnGUI() {
		GUI.backgroundColor = Color.grey;
		GUI.Box(new Rect(0, 0, LIVES_WIDTH, LIVES_HEIGHT), "Lives Left: " + _game.Lives, style);
		GUI.Box(new Rect(0, TEXT_HEIGHT, LIVES_WIDTH, LIVES_HEIGHT), "Money: " + _game.Money, style);
		GUI.Box(new Rect(0, TEXT_HEIGHT * 2, LIVES_WIDTH, LIVES_HEIGHT), "Next Wave: " + _wave.TimeTillNextWave, style);
	}
}
