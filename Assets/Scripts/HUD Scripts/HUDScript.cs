using UnityEngine;
using System.Collections;

public class HUDScript : MonoBehaviour {

	public const int LIVES_WIDTH = 100;
	public const int LIVES_HEIGHT = 30;
	
	private GameController _gameController;
	
	void Awake() {
		_gameController = GetComponent<GameController>();
	}
	
	void OnGUI() {
		GUI.Box(new Rect(Screen.width - LIVES_WIDTH, 0, LIVES_WIDTH, LIVES_HEIGHT), "Lives Left: " + _gameController.Lives);
	}
}
