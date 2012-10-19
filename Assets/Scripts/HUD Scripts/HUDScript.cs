using UnityEngine;
using System.Collections;

public class HUDScript : MonoBehaviour {

	public const int LIVES_WIDTH = 128;
	public const int LIVES_HEIGHT = 30;
	public const int TEXT_HEIGHT = 30;
	public Texture2D crosshair;
	private Rect _crosshairPos;
	private GameController _game;
	private WaveController _wave;
	public Texture2D menu;
	public Texture2D item;

	// Skin
	public GUIStyle style;

	void Start() {
		_game = GameObject.Find(" GameController").GetComponent<GameController>();
		_wave = GameObject.Find(" GameController").GetComponent<WaveController>();

		Screen.showCursor = true;
		_crosshairPos = new Rect((Screen.width - crosshair.width) / 2, (Screen.height - crosshair.height) / 2,
			crosshair.width, crosshair.height);
	}

	void OnGUI() {
		GUI.backgroundColor = Color.grey;
		GUI.Box(new Rect(0, 0, LIVES_WIDTH, LIVES_HEIGHT), "Lives Left: " + _game.Lives, style);
		GUI.Box(new Rect(0, TEXT_HEIGHT, LIVES_WIDTH, LIVES_HEIGHT), "Money: " + _game.Money, style);
		GUI.Box(new Rect(0, TEXT_HEIGHT * 2, LIVES_WIDTH, LIVES_HEIGHT),
			"Next Wave: " + (_wave.TimeTillNextWavex100 / 100f).ToString("F2"), style);
//		GUI.Box(new Rect(0, TEXT_HEIGHT * 3, LIVES_WIDTH * 2, LIVES_HEIGHT), "Mode: " + LayerMask.LayerToName(_input.layerMask), style);

//		GUI.DrawTexture(new Rect(10, 10, menu.width, menu.height), menu);
//		GUI.DrawTexture(new Rect(74, 74, item.width, item.height), item);
		GUI.DrawTexture(_crosshairPos, crosshair);
	}
}
