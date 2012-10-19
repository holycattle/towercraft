using UnityEngine;
using System.Collections;

public class Builder : GameTool {
	// Tower Data
	public GameObject[] towerBases;
	public GameObject[] towerStems;
	public GameObject[] towerTurrets;
	private GameObject[][] towerPrefabs;

	// Building Menu
	private const int BUTTON_WIDTH = 100;
	private const int BUTTON_HEIGHT = 50;
	private const int BUTTON_PADDING = 50;
	private Rect[] buttonRects;
	private int _activeMenu;
	private bool _isMenuActive;
	private Grid targettedGrid;

	protected override void Awake() {
		base.Awake();

		towerPrefabs = new GameObject[3][];
		towerPrefabs[BaseTower.TOWER_BASE] = towerBases;
		towerPrefabs[BaseTower.TOWER_STEM] = towerStems;
		towerPrefabs[BaseTower.TOWER_TURRET] = towerTurrets;
	}

	void OnGUI() {
		if (_isMenuActive) {
			for (int i = 0; i < towerPrefabs[_activeMenu].Length; i++) {
				if (GUI.Button(buttonRects[i], towerPrefabs[_activeMenu][i].GetComponent<TowerComponent>().componentName)) {
					_game.Active = true;
					_isMenuActive = false;

					BaseTower currentTower = targettedGrid.Tower;
					if (currentTower == null) {
						currentTower = targettedGrid.GenerateBaseTower();
					}
					currentTower.addNextComponent(towerPrefabs[_activeMenu][i]);
					targettedGrid.SetSelected(false);
					targettedGrid = null;
				}
			}
		}
	}

	public override void WhenEquipped() {
		_input.RaycastLayer = LayerMask.NameToLayer("Level");
	}

	public override void MouseClickOn(GameObject g) {
		if (!_isMenuActive && g != null) {
			_isMenuActive = true;
			_game.Active = false;
			targettedGrid = g.GetComponent<Grid>();

			BaseTower currentTower = targettedGrid.Tower;
			if (currentTower == null) {
				_activeMenu = BaseTower.TOWER_BASE;
			} else if (currentTower.getNextComponent() == BaseTower.TOWER_STEM) {
				_activeMenu = BaseTower.TOWER_STEM;
			} else if (currentTower.getNextComponent() == BaseTower.TOWER_TURRET) {
				_activeMenu = BaseTower.TOWER_TURRET;
			} else {
				// Tower is complete.
				_isMenuActive = false;
				_game.Active = true;
				targettedGrid = null;
				return;
			}

			targettedGrid.SetSelected(true);

			// Generate Rectangles
			Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
			Vector2 start = new Vector2(center.x - (BUTTON_WIDTH + BUTTON_PADDING) * (towerPrefabs.Length / 2), center.y - BUTTON_PADDING);

			buttonRects = new Rect[towerPrefabs[_activeMenu].Length];
			for (int i = 0; i < towerPrefabs[_activeMenu].Length; i++) {
				buttonRects[i] = new Rect(start.x + (BUTTON_WIDTH + BUTTON_PADDING) * i, start.y, BUTTON_WIDTH, BUTTON_HEIGHT);
			}
		}
	}
}
