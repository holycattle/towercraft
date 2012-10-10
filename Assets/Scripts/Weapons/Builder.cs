using UnityEngine;
using System.Collections;

public class Builder : GameTool {
	// Tower Data
	public GameObject[] towerPrefabs;
	private BaseTower[] towerScripts;

	// Building Menu
	private const int BUTTON_WIDTH = 100;
	private const int BUTTON_HEIGHT = 50;
	private const int BUTTON_PADDING = 50;
	private Rect[] buttonRects;
	private bool _menuActive;
	private Grid targettedGrid;

	protected override void Awake() {
		base.Awake();

		towerScripts = new BaseTower[towerPrefabs.Length];
		buttonRects = new Rect[towerPrefabs.Length];

		Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
		Vector2 start = new Vector2(center.x - (BUTTON_WIDTH + BUTTON_PADDING) * (towerPrefabs.Length / 2), center.y - BUTTON_PADDING);
		for (int i = 0; i < towerPrefabs.Length; i++) {
			towerScripts[i] = towerPrefabs[i].GetComponent<BaseTower>();
			buttonRects[i] = new Rect(start.x + (BUTTON_WIDTH + BUTTON_PADDING) * i, start.y, BUTTON_WIDTH, BUTTON_HEIGHT);
		}
	}

	void OnGUI() {
		if (_menuActive) {
			for (int i = 0; i < towerPrefabs.Length; i++) {
				if (GUI.Button(buttonRects[i], towerScripts[i].towerName)) {
					_game.Active = true;
					_menuActive = false;
					targettedGrid.AddTower(towerPrefabs[i]);

					targettedGrid = null;
				}
			}
		}
	}

	public override void WhenEquipped() {
		_input.RaycastLayer = LayerMask.NameToLayer("Level");
	}

	public override void MouseClickedOn(GameObject g) {
		if (g != null) {
			_menuActive = true;
			_game.Active = false;
			targettedGrid = g.GetComponent<Grid>();
		}
	}

	public override void MouseDownOn(GameObject g) {
	}
}
