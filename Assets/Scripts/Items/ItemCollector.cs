using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemCollector : MonoBehaviour {
	// GUI Constants
	private const int WIDTH = 4;
	private const int HEIGHT = 6;
	private const int GRIDWIDTH = 128;
	private const int GRIDHEIGHT = 32;
	private const int SPACE = 4;

	// Inventory Constants
	private const int INV_TOWER = 0;
	private const int INV_CRAFT = 1;
	private const int INV_WEAPON = 2;
	private const int NUM_TYPES = 3;
	private static string[] INVENTORYTYPES = {"Tower Items", "Crafting Items", "Weapons"};

	// Crafting Constants
//	private const int CRAFT_COMBINE = 0;
//	private const int CRAFT_CLONE = 1;
//	private const float COST_COMBINE = 0.9f;
//	private const float COST_CLONE = 1.1f;
	private const int NUM_CRAFTABLES = 3;

	// Game
	protected GameController _game;

	// Inventories
	private int activeInv;
	private Item[] activeInventory;
	private TowerItem[] towerInventory;
	private CraftableItem[] craftInventory;
	private WeaponItem[] weaponInventory;

	// GUI Elements
	private Rect changeInventoryRect;
	private Rect[] drawRects;
	private Rect[] toDestroyRects;
	private Rect cloneButtonRect;
	private Rect modeChangeButtonRect;
	private Rect tooltipRect;

	// Cloning / Combining
	private int craftingMode;
	private TowerItem toClone;
	private Item[] craftingRecipe;

	void Start() {
		_game = GameObject.Find(" GameController").GetComponent<GameController>();

		// Init the inventories
		towerInventory = new TowerItem[WIDTH * HEIGHT];
		craftInventory = new CraftableItem[WIDTH * HEIGHT];
		weaponInventory = new WeaponItem[WIDTH * HEIGHT];
		activeInv = INV_TOWER;
		activeInventory = towerInventory;

		/*
		 *	GUI Initialization
		 */
		// Init Rects for Inventory Items
		drawRects = new Rect[WIDTH * HEIGHT];
		int totalWidth = WIDTH * GRIDWIDTH + (WIDTH - 1) * SPACE;
		int totalHeight = HEIGHT * GRIDHEIGHT + (HEIGHT - 1) * SPACE;
		int sx = Screen.width / 2 - totalWidth / 2 - GRIDWIDTH;
		int sy = Screen.height / 2 - totalHeight / 2 - (GRIDHEIGHT + SPACE);
		for (int y = 0; y < HEIGHT; y++) {
			for (int x = 0; x < WIDTH; x++) {
				drawRects[y * WIDTH + x] = new Rect(sx + x * (GRIDWIDTH + SPACE), sy + y * (GRIDHEIGHT + SPACE), GRIDWIDTH, GRIDHEIGHT);
			}
		}

		changeInventoryRect = new Rect(sx, sy - (GRIDHEIGHT * 2 + SPACE), GRIDWIDTH, GRIDHEIGHT * 2);

		// Crafting Rectangles
		toDestroyRects = new Rect[NUM_CRAFTABLES];
		for (int i = 0; i < NUM_CRAFTABLES; i++) {
			toDestroyRects[i] = new Rect(sx + totalWidth + GRIDWIDTH, sy + (GRIDHEIGHT + SPACE) * (2 + i), GRIDWIDTH, GRIDHEIGHT);
		}
		cloneButtonRect = new Rect(sx + totalWidth + GRIDWIDTH, sy + (GRIDHEIGHT + SPACE) * HEIGHT, GRIDWIDTH, GRIDHEIGHT);
		modeChangeButtonRect = new Rect(sx + totalWidth + GRIDWIDTH, sy - (GRIDHEIGHT + SPACE), GRIDWIDTH, GRIDHEIGHT);
		tooltipRect = new Rect(sx, sy + totalHeight + SPACE, GRIDWIDTH * 2, GRIDHEIGHT * 4);

		//deactivate Messenger engine for now
		
		_game.Messenger.enabled = false;

		Pickup(new TowerItem(0, 4));
		Pickup(new TowerItem(0, 4));
		Pickup(new TowerItem(0, 4));
		Pickup(new TowerItem(1, 4));
		Pickup(new TowerItem(1, 4));
		Pickup(new TowerItem(1, 4));
		Pickup(new TowerItem(1, 4));
		
//		Pickup(new CraftableItem(0, 1));
//		Pickup(new CraftableItem(1, 1));
//		Pickup(new CraftableItem(2, 1));
//		Pickup(new CraftableItem(0, 1));
//		Pickup(new CraftableItem(1, 1));
//		Pickup(new CraftableItem(2, 1));

		craftingRecipe = new Item[NUM_CRAFTABLES];

		UpdateInventory();
		_game.Messenger.enabled = true; //enable messenger again
	}

	void OnTriggerEnter(Collider other) {
		if (CanPickup()) {
			if (other.gameObject.tag == "Item") {
				PickupableScript i = other.gameObject.GetComponent<PickupableScript>();
				if (i.CanPickup(this))
					StartCoroutine("PickupDelay", other.gameObject);
			}
		}
	}

	private IEnumerator PickupDelay(GameObject g) {
		PickupableScript i = g.GetComponent<PickupableScript>();
		i.Target = transform;

		yield return new WaitForSeconds(0.3f);
		i.Pickup(this);
		DestroyObject(g);
	}

	void Update() {
		if (_game.ActiveMenu == Menu.Inventory || _game.ActiveMenu == Menu.Game) {
			if (Input.GetKeyDown(KeyCode.E)) {
				_game.ActiveMenu = _game.ActiveMenu == Menu.Inventory ? Menu.Game : Menu.Inventory;
				SetInventoryOpen(_game.ActiveMenu == Menu.Inventory);

				activeInv = INV_TOWER;
			}
		}
	}

	/*
	 *	Inventory System
	 */
	void OnGUI() {
		if (_game.ActiveMenu == Menu.Inventory) {
//			int width = WIDTH * GRIDWIDTH + (WIDTH - 1) * SPACE;
//			int height = HEIGHT * GRIDHEIGHT + (HEIGHT - 1) * SPACE;

			// Draw Mode Swap Button
//			if (GUI.Button(modeChangeButtonRect, craftingMode == CRAFT_CLONE ? "Clone Mode" : "Combine Mode")) {
//				craftingMode = (craftingMode + 1) % 2;
//				if (craftingMode == CRAFT_CLONE) {
//
//				} else if (craftingMode == CRAFT_COMBINE) {
//					Pickup(toClone);
//					toClone = null;
//				}
//			}

			// Draw Inventory
			for (int i = 0; i < WIDTH * HEIGHT; i++) {
				if (activeInventory[i] == null) {
					GUI.Button(drawRects[i], "");
					continue;
				}

				if (GUI.Button(drawRects[i], new GUIContent(activeInventory[i].GetName(), activeInventory[i].GetTooltip()))) {
					if (Pickup(craftingRecipe, activeInventory[i])) {
						Remove(activeInventory[i]);
					}
				}
			}
			GUI.Label(tooltipRect, GUI.tooltip);

			// Draw ToClone
//			if (craftingMode == CRAFT_CLONE) {
//				if (GUI.Button(toCloneRect, toClone == null ? "" : toClone.GetName())) {
//					Pickup(toClone);
//					toClone = null;
//				}
//			}

			// Draw ToDestroy
			for (int i = 0; i < craftingRecipe.Length; i++) {
				if (craftingRecipe[i] == null)
					continue;
				if (GUI.Button(toDestroyRects[i], craftingRecipe[i].GetName())) {
					Pickup(craftingRecipe[i]);
					craftingRecipe[i] = null;
				}
			}

			// Clone Button
//			if (craftingMode == CRAFT_CLONE) {
//				if (GUI.Button(cloneButtonRect, toClone == null ? "" : toClone.GetTowerComponent().level + "<< == >>" + SumDestroyCost())) {
//					if (toClone.GetTowerComponent().level <= SumDestroyCost()) {
//						Pickup(new Item(ComponentGenerator.Get().TowerClone(toClone.GetTowerComponent())));
//						craftingRecipe = new Item[NUM_CRAFTABLES];
//					}
//				}
//			} else if (craftingMode == CRAFT_COMBINE) {
			if (GUI.Button(changeInventoryRect, INVENTORYTYPES[activeInv])) {
				activeInv = (activeInv + 1) % NUM_TYPES;
				SetActiveInventory(activeInv);
			}
//			}
		}
	}

	private void SetActiveInventory(int i) {
		activeInv = i;
		switch (i) {
			case INV_TOWER:
				activeInventory = towerInventory;
				break;
			case INV_CRAFT:
				activeInventory = craftInventory;
				break;
			case INV_WEAPON :
				activeInventory = weaponInventory;
				break;
		}
	}

	private void SetInventoryOpen(bool setted) {
		if (!setted) {
			Pickup(toClone);
			foreach (Item i in craftingRecipe) {
				Pickup(i);
			}
			toClone = null;
			craftingRecipe = new Item[NUM_CRAFTABLES];
			UpdateInventory();
		}
	}

	private void UpdateInventory() {
		Sort(activeInventory);
	}

	public TowerComponent[] GetGameObjects(int type) {
		List<TowerComponent> objects = new List<TowerComponent>();
		for (int i = 0; i < towerInventory.Length; i++) {
			if (towerInventory[i] != null) {
//				Debug.Log("Checking: " + inventory[i].GetTowerComponent().componentName + " > " + inventory[i].GetTowerComponent().componentType);
				if (((TowerItem)towerInventory[i]).GetTowerComponent().componentType == type) {
					objects.Add(((TowerItem)towerInventory[i]).GetTowerComponent());
				}
			}
		}
		TowerComponent[] temp = objects.ToArray();
		return temp;
	}

	public bool Pickup(Item it) {
		bool p = Pickup(towerInventory, it);
		UpdateInventory();
		return p;
	}

	private bool Pickup(Item[] list, Item it) {
		if (it == null) {
			return true;
		}
		for (int i = 0; i < list.Length; i++) {
			if (list[i] == null) {
				list[i] = it;
				_game.Messenger.ItemMessage(it.GetName());
				
				return true;
			}
		}
		return false;
	}

	public void Remove(Item it) {
		this.Remove(towerInventory, it);
		UpdateInventory();
	}

	public void Remove(TowerComponent t) {
		for (int i = 0; i < towerInventory.Length; i++) {
			if (towerInventory[i] != null) {
				if (((TowerItem)towerInventory[i]).GetTowerComponent() == t) {
					towerInventory[i] = null;
					SortInventory();
					return;
				}
			}
		}
	}

	private void Remove(Item[] list, Item it) {
		if (it == null)
			return;
		for (int i = 0; i < list.Length; i++) {
			if (it == list[i]) {
				towerInventory[i] = null;
				return;
			}
		}
	}

	public bool CanPickup() {
		return CanPickup(towerInventory);
	}

	private bool CanPickup(Item[] list) {
		foreach (Item i in list) {
			if (i == null) {
				return true;
			}
		}
		return false;
	}

	public int CountInventory() {
		return CountInventory(towerInventory);
	}

	private int CountInventory(Item[] list) {
		int counter = 0;
		for (int i = 0; i < list.Length; i++) {
			if (list[i] != null) {
				counter++;
			}
		}
		return counter;
	}

	public void Sort(Item[] list) {
		// Note: Using Bubble Sort (xD)
		for (int i = 0; i < list.Length; i++) {
			for (int j  = 0; j < list.Length - 1; j++) {
				if (list[j] == null) {
					// j is greater = Swap
					list[j] = list[j + 1];
					list[j + 1] = null;
				} else if (list[j + 1] == null) {
					// Do Nothing
				} else if (list[j + 1].isLessThan(list[j])) {
					// j is greater = Swap
					Item it = list[j];
					list[j] = list[j + 1];
					list[j + 1] = it;
				}
			}
		}
	}

	public void SortInventory() {
		// Note: Using Bubble Sort (xD)
		for (int i = 0; i < towerInventory.Length; i++) {
			for (int j  = 0; j < towerInventory.Length - 1; j++) {
				if (towerInventory[j] == null) {
					// j is greater = Swap
					towerInventory[j] = towerInventory[j + 1];
					towerInventory[j + 1] = null;
				} else if (towerInventory[j + 1] == null) {
					// Do Nothing
				} else if (((TowerItem)towerInventory[j]).GetTowerComponent().componentType > ((TowerItem)towerInventory[j + 1]).GetTowerComponent().componentType) {
					// j is greater = Swap
//					Item it = towerInventory[j];
//					towerInventory[j] = towerInventory[j + 1];
//					towerInventory[j + 1] = it;
				}
			}
		}
	}
}
