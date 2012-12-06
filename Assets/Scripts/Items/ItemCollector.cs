using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemCollector : MonoBehaviour {
	// GUI Constants
	private const int WIDTH = 4;
	private const int HEIGHT = 6;
	public const int GRIDWIDTH = 160;
	public const int GRIDHEIGHT = 32;
	public const int TOOLTIPWIDTH = 256;
	public const int TOOLTIPHEIGHT = 128;
	public const int SPACE = 4;

	// Inventory Constants
	private const int INV_TOWER = 0;
	private const int INV_CRAFT = 1;
	private const int INV_WEAPON = 2;
	private const int NUM_TYPES = 3;
	private static string[] INVENTORYTYPES = {"Tower Items", "Crafting Items", "Weapons"};

	// Crafting Constants
//	private const int CRAFT_COMBINE = 0;
//	private const int CRAFT_CLONE = 1;
//	private const float COST_CLONE = 1.1f;
	private const int NUM_CRAFTABLES = 3;

	// References
	protected GameController _game;
	protected Weapon _gun;

	// Inventories
	private int activeInv;
	private Item[] activeInventory;
	private TowerItem[] towerInventory;
	private CraftableItem[] craftInventory;
	private WeaponItem[] weaponInventory;

	// GUI Elements
	private GUISkin inventorySkin;
	private Rect changeInventoryRect;
	private Rect[] drawRects;
	private Rect[] craftingRects;
	private Rect cloneButtonRect;
//	private Rect modeChangeButtonRect;
	private Rect tooltipRect;


	// Cloning / Combining
	private int craftingMode;
	private TowerItem toClone;
	private CraftableItem[] craftingRecipe;

	void Start() {
		_game = GameObject.Find(" GameController").GetComponent<GameController>();
		_gun = GameObject.Find("Player").GetComponentInChildren<Weapon>();

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
		inventorySkin = Resources.Load("Fonts/InventorySkin", typeof(GUISkin)) as GUISkin;
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
		craftingRects = new Rect[NUM_CRAFTABLES];
		for (int i = 0; i < NUM_CRAFTABLES; i++) {
			craftingRects[i] = new Rect(sx + totalWidth + GRIDWIDTH, sy + (GRIDHEIGHT + SPACE) * i, GRIDWIDTH, GRIDHEIGHT);
		}
		cloneButtonRect = new Rect(sx + totalWidth + GRIDWIDTH, sy + (GRIDHEIGHT + SPACE) * HEIGHT, GRIDWIDTH, GRIDHEIGHT);
//		modeChangeButtonRect = new Rect(sx + totalWidth + GRIDWIDTH, sy - (GRIDHEIGHT + SPACE), GRIDWIDTH, GRIDHEIGHT);
		tooltipRect = new Rect(sx, sy + totalHeight + SPACE, TOOLTIPWIDTH, TOOLTIPHEIGHT);

		// Deactivate Messenger: So there aren't any messages
		_game.Messenger.enabled = false;
		Pickup(new TowerItem(0, 1));
		Pickup(new TowerItem(0, 1));
		Pickup(new TowerItem(0, 1));
		Pickup(new TowerItem(0, 1));
		Pickup(new TowerItem(0, 1));
		Pickup(new TowerItem(0, 1));
		Pickup(new TowerItem(1, 2));
		Pickup(new TowerItem(1, 2));
		Pickup(new TowerItem(1, 2));
		Pickup(new TowerItem(1, 2));

		Pickup(new CraftableItem(0, 1));
		Pickup(new CraftableItem(1, 1));
		Pickup(new CraftableItem(2, 1));
		Pickup(new CraftableItem(0, 4));
		Pickup(new CraftableItem(1, 4));
		Pickup(new CraftableItem(2, 4));

		Pickup(new WeaponDPSItem(1));
		Pickup(new WeaponDPSItem(1));
		Pickup(new WeaponDPSItem(1));
		Pickup(new WeaponStatItem(1));
		Pickup(new WeaponStatItem(1));
		Pickup(new WeaponStatItem(1));
		_game.Messenger.enabled = true; //enable messenger again

		craftingRecipe = new CraftableItem[CraftableItem.PART_MAX];

		Sort(towerInventory);
		Sort(craftInventory);
		Sort(weaponInventory);

		UpdateInventory();
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

				SetActiveInventory(INV_TOWER);
			}
		}
	}

	/*
	 *	Inventory System
	 */
	void OnGUI() {
		if (_game.ActiveMenu == Menu.Inventory) {
			GUI.skin = inventorySkin;

			// Draw Mode Swap Button
			if (GUI.Button(changeInventoryRect, INVENTORYTYPES[activeInv])) {
				activeInv = (activeInv + 1) % NUM_TYPES;
				SetActiveInventory(activeInv);
			}

			// Draw Inventory
			for (int i = 0; i < WIDTH * HEIGHT; i++) {
				if (activeInventory[i] == null) {
					GUI.Button(drawRects[i], "");
					continue;
				}

				if (GUI.Button(drawRects[i], new GUIContent(activeInventory[i].GetName(), activeInventory[i].GetTooltip()))) {
					if (Input.GetKey(KeyCode.LeftControl)) {
						// Destroy Item
						if (activeInv == INV_TOWER) {
							Destroy(((TowerItem)activeInventory[i]).GetTowerComponent().gameObject);
						}
						Remove(activeInventory[i]);
					} else {
						switch (activeInv) {
							case INV_CRAFT:
								CraftableItem c = ((CraftableItem)activeInventory[i]);
								if (craftingRecipe[c.CraftableType] == null) {
									// If there is no item CraftableType slot, then ADD
									craftingRecipe[c.CraftableType] = c;
									Remove(c);
								} else {
									// Else, SWAP the clicked with the one in the slot
									Pickup(craftingRecipe[c.CraftableType]);
									craftingRecipe[c.CraftableType] = c;
									Remove(c);
								}
								break;
							case INV_TOWER:
								break;
							case INV_WEAPON:
								WeaponItem w = ((WeaponItem)activeInventory[i]);
								WeaponItem prevEquipped = _gun.equippedWeapons[w.weaponType];
								_gun.equippedWeapons[w.weaponType] = w;
								Remove(activeInventory, w);
								Pickup(activeInventory, prevEquipped);
								_gun.RecalculateStats();
								break;
						}
					}
				}
			}

			if (activeInv == INV_WEAPON) {
				// Draw Equipped Weapons
				for (int i = 0; i < _gun.equippedWeapons.Length; i++) {
					if (_gun.equippedWeapons[i] == null) {
						GUI.Button(craftingRects[i], "");
						continue;
					}
					GUI.Button(craftingRects[i], new GUIContent(_gun.equippedWeapons[i].GetName(), _gun.equippedWeapons[i].GetTooltip()));
				}
			} else {
				// Draw Crafting Recipe
				for (int i = 0; i < craftingRecipe.Length; i++) {
					if (craftingRecipe[i] == null) {
						GUI.Button(craftingRects[i], "");
						continue;
					}
					if (GUI.Button(craftingRects[i], new GUIContent(craftingRecipe[i].GetName(), craftingRecipe[i].GetTooltip()))) {
						Pickup(craftingRecipe[i]);
						craftingRecipe[i] = null;
					}
				}
			}

			/*
			 *	Craft Button
			 */
			if (CraftComplete()) {
				if (GUI.Button(cloneButtonRect, new GUIContent("Build", CraftPreviewString()))) {
					Pickup(towerInventory, new TowerItem(ComponentGenerator.Get().GenerateTurret(craftingRecipe)));
					craftingRecipe = new CraftableItem[CraftableItem.PART_MAX];
				}
			} else {
//				GUI.enabled = false;
				if (GUI.Button(cloneButtonRect, new GUIContent("Build", CraftPreviewString()))) {
				}
//				GUI.enabled = true;
			}

			GUI.Label(tooltipRect, GUI.tooltip);

			GUI.skin = null;
		}
	}

	private string CraftPreviewString() {
		string s = "";
		foreach (CraftableItem i in craftingRecipe) {
			if (i == null) {
				return "Incomplete Recipe!";
			}


			switch (i.CraftableType) {
				case CraftableItem.PART_DPS:
					s += "DPS: " + i.Modifier;
					break;
				case CraftableItem.PART_RANGE:
					s += "Range: " + i.Modifier;
					break;
				case CraftableItem.PART_MODIFIER:
					s += "Modifier: " + i.RawTooltip;
					break;
			}
			s += "\n";
		}

		int level = craftingRecipe[0].Level;
		foreach (CraftableItem i in craftingRecipe) {
			if (i.Level != level) {
				s += "Varying Levels in Parts Detected.\nStats may vary.";
				break;
			}
		}

		return s;
	}

	private bool CraftComplete() {
		foreach (Item i in craftingRecipe) {
			if (i == null)
				return false;
		}
		return true;
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
			craftingRecipe = new CraftableItem[NUM_CRAFTABLES];
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
		if (it == null)
			return true;

		bool p = false;
		switch (it.ItemType) {
			case Item.ITEM_TOWER:
				p = Pickup(towerInventory, it);
				break;
			case Item.ITEM_CRAFT:
				p = Pickup(craftInventory, it);
				break;
			case Item.ITEM_WEAPON:
				p = Pickup(weaponInventory, it);
				break;
		}
		UpdateInventory();
		return p;
	}

	private bool Pickup(Item[] list, Item it) {
		if (it == null)
			return true;

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
		this.Remove(activeInventory, it);
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
				list[i] = null;
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
