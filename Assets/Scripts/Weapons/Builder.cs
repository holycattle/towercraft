using UnityEngine;
using System.Collections;

public class Builder : GameTool {

	// Tower Data
//	public GameObject[] towerBases;
//	public GameObject[] towerStems;
//	public GameObject[] towerTurrets;
//	private GameObject[][] towerPrefabs;
	private TowerComponent[] _displayArray;

	// Inventory
	private ItemCollector _inventory;

	// Building Menu
	private const int BUTTON_WIDTH = 100;
	private const int BUTTON_HEIGHT = 50;
	private const int BUTTON_PADDING = 50;
	private Rect[] buttonRects;
	private int _activeMenu;
	private bool _buildMode;	// TRUE = Building. FALSE = Swapping.
	private Grid targettedGrid;

	// Laser Sight
	private LineRenderer laserSight;
	
	protected override void Update() {
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) {
			_game.ActiveMenu = Menu.Game;
			Screen.lockCursor = false;
			Time.timeScale = 0.0f;
		}
	}
	
	protected override void Awake() {
		base.Awake();

//		towerPrefabs = new GameObject[3][];
//		towerPrefabs[BaseTower.TOWER_BASE] = towerBases;
//		towerPrefabs[BaseTower.TOWER_STEM] = towerStems;
//		towerPrefabs[BaseTower.TOWER_TURRET] = towerTurrets;

//		laserSight = GetComponent<LineRenderer>();
//		laserSight.SetVertexCount(2);
	}

	protected override void Start() {
		base.Start();

		_inventory = transform.root.gameObject.GetComponentInChildren<ItemCollector>();
	}

	protected override void OnGUI() {
		base.OnGUI();
		if (_game.ActiveMenu == Menu.Builder) {
			for (int i = 0; i < _displayArray.Length; i++) {
				if (GUI.Button(buttonRects[i], _displayArray[i].componentName)) {
//					Debug.Log("Display Array: " + _displayArray[i].componentName + " > " + _displayArray[i].attributes.Count);
					_game.ActiveMenu = Menu.Game;

					BaseTower currentTower = targettedGrid.Tower;

					if (_buildMode) {
						// Build Mode
						if (currentTower == null) {
							currentTower = targettedGrid.GenerateBaseTower();
						}
						currentTower.AddNextComponent(_displayArray[i]);
					} else {
						// Swap Mode
						TowerComponent t = currentTower.SwapComponent(_displayArray[i]);
						if (t != null) {
							_inventory.Pickup(new TowerItem(t));
						}
					}
					_inventory.Remove(_displayArray[i]);
					targettedGrid = null;
				}
			}

			// If the targetted grid is no longer targetted,
			//  it means that the player has already chosen a component to build
			//  therefore, nullify the display array.
			if (targettedGrid == null)
				_displayArray = null;
		}
	}

	public override void WhenEquipped() {
		_weapon.RaycastLayer = 1 << LayerMask.NameToLayer("Level");
	}

	public override void MouseDownOn(GameObject g) {
		MouseClickOn(g);
	}

	public override void MouseClickOn(GameObject g) {
		if (_game.ActiveMenu == Menu.Game && g != null) {
			targettedGrid = g.GetComponent<Grid>();

			// 1) CHECK if the clicked object is a grid
			if (targettedGrid == null) {
				Debug.Log("Not Clicked Correctly");
				return;
			}

			// 2) CHECK if you can build at that spot
			targettedGrid.TempTower = true;
			if (!_level.HasPath()) {
				Debug.Log("Prevent Building! No Possible Path");
//				targettedGrid.TempTower = false;
				return;
			}

			// 3) CHECK if there is a mob at that spot OR if a mob gets trapped
			GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
			for (int i= 0; i < enemies.Length; i++) {
				BaseEnemy b = enemies[i].GetComponent<BaseEnemy>();
				if (b == null)
					continue;

				if (b.GridPosition == targettedGrid.GridValue) {
					Debug.Log("Prevent Building! Mob on Grid");
					return;
				} else {
					if (!_level.HasPath(b.GridPosition)) {
						Debug.Log("Prevent Building! Mob Trap");
						return;
					}
				}
			}
//			targettedGrid.TempTower = false;

			BaseTower currentTower = targettedGrid.Tower;

			_game.ActiveMenu = Menu.Builder;
			_buildMode = true;
			if (currentTower == null) {
				_activeMenu = BaseTower.TOWER_BASE;
			} else if (currentTower.GetNextComponent() == BaseTower.TOWER_TURRET) {
				_activeMenu = BaseTower.TOWER_TURRET;
			} else {
				// Tower is Complete.
				// SWAP MODE!
				_activeMenu = BaseTower.TOWER_TURRET; // TODO: This will be changable.
				_buildMode = false;
//				// Tower is complete.
//				_game.ActiveMenu = Menu.Game;
//				targettedGrid = null;
//				return;
			}
//			targettedGrid.SetSelected(true);

			_displayArray = _inventory.GetGameObjects(_activeMenu);
			
			if (_displayArray.Length == 0) {
				_game.ActiveMenu = Menu.Game;
				Screen.lockCursor = false;
				Time.timeScale = 0.0f;
				_game.Messenger.WarningMessage("No items in your inventory.");
				return;	
			}

			// Generate Rectangles
			Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
			Vector2 start = new Vector2(center.x - (BUTTON_WIDTH + BUTTON_PADDING) * (_displayArray.Length / 2), center.y - BUTTON_PADDING);

			buttonRects = new Rect[_displayArray.Length];
			for (int i = 0; i < _displayArray.Length; i++) {
				buttonRects[i] = new Rect(start.x + (BUTTON_WIDTH + BUTTON_PADDING) * i, start.y, BUTTON_WIDTH, BUTTON_HEIGHT);
			}
		}
	}
}
