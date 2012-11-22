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

	// Crafting Constants
	private const int CRAFT_COMBINE = 0;
	private const int CRAFT_CLONE = 1;
	private const float COST_COMBINE = 0.9f;
	private const float COST_CLONE = 1.1f;
	private const int NUM_CRAFTABLES = 4;

	// Game
	protected GameController _game;

	// Inventory
	private Item[] inventory;

	// GUI Elements
	private Rect[] drawRects;
	private Rect toCloneRect;
	private Rect[] toDestroyRects;
	private Rect cloneButtonRect;
	private Rect modeChangeButtonRect;
	private Rect tooltipRect;

	// Cloning / Combining
	private int craftingMode;
	private Item toClone;
	private Item[] toDestroy;

	void Start() {
		_game = GameObject.Find(" GameController").GetComponent<GameController>();

		inventory = new Item[WIDTH * HEIGHT];

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

		// Cloning Rectangles
		toCloneRect = new Rect(sx + totalWidth + GRIDWIDTH, sy, GRIDWIDTH, GRIDHEIGHT);
		toDestroyRects = new Rect[NUM_CRAFTABLES];
		for (int i = 0; i < NUM_CRAFTABLES; i++) {
			toDestroyRects[i] = new Rect(sx + totalWidth + GRIDWIDTH, sy + (GRIDHEIGHT + SPACE) * (2 + i), GRIDWIDTH, GRIDHEIGHT);
		}
		cloneButtonRect = new Rect(sx + totalWidth + GRIDWIDTH, sy + (GRIDHEIGHT + SPACE) * HEIGHT, GRIDWIDTH, GRIDHEIGHT);
		modeChangeButtonRect = new Rect(sx + totalWidth + GRIDWIDTH, sy - (GRIDHEIGHT + SPACE), GRIDWIDTH, GRIDHEIGHT);
		tooltipRect = new Rect(sx, sy + totalHeight + SPACE, GRIDWIDTH * 2, GRIDHEIGHT * 4);

		Pickup(new Item(0, 4));
//		Pickup(new Item(0, 1));
		Pickup(new Item(1, 4));
		Pickup(new Item(2, 4));
		Pickup(new Item(2, 4));
		Pickup(new Item(1, 4));
		Pickup(new Item(2, 4));
		Pickup(new Item(2, 4));
		Pickup(new Item(1, 4));
		Pickup(new Item(2, 4));
		Pickup(new Item(2, 4));
		Pickup(new Item(1, 4));
		Pickup(new Item(2, 4));
		Pickup(new Item(2, 4));

		toDestroy = new Item[NUM_CRAFTABLES];

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

				craftingMode = CRAFT_COMBINE;
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
			if (GUI.Button(modeChangeButtonRect, craftingMode == CRAFT_CLONE ? "Clone Mode" : "Combine Mode")) {
				craftingMode = (craftingMode + 1) % 2;
				if (craftingMode == CRAFT_CLONE) {

				} else if (craftingMode == CRAFT_COMBINE) {
					Pickup(toClone);
					toClone = null;
				}
			}

			// Draw Inventory
			for (int i = 0; i < WIDTH * HEIGHT; i++) {
				if (inventory[i] == null)
					continue;
				if (GUI.Button(drawRects[i], new GUIContent(inventory[i].Name, inventory[i].Tooltip))) {
					if (craftingMode == CRAFT_COMBINE) {
						// Combine Mode
						if (Pickup(toDestroy, inventory[i])) {
							Remove(inventory[i]);
						}
					} else if (craftingMode == CRAFT_CLONE) {
						// Clone Mode
						if (toClone == null) {
							toClone = inventory[i];
							Remove(inventory[i]);
						} else {
							if (Pickup(toDestroy, inventory[i])) {
								Remove(inventory[i]);
							}
						}
					}
				}
			}
			GUI.Label(tooltipRect, GUI.tooltip);

			// Draw ToClone
			if (craftingMode == CRAFT_CLONE) {
				if (GUI.Button(toCloneRect, toClone == null ? "" : toClone.Name)) {
					Pickup(toClone);
					toClone = null;
				}
			}

			// Draw ToDestroy
			for (int i = 0; i < toDestroy.Length; i++) {
				if (toDestroy[i] == null)
					continue;
				if (GUI.Button(toDestroyRects[i], toDestroy[i].Name)) {
					Pickup(toDestroy[i]);
					toDestroy[i] = null;
				}
			}

			// Clone Button
			if (craftingMode == CRAFT_CLONE) {
				if (GUI.Button(cloneButtonRect, toClone == null ? "" : toClone.GetTowerComponent().cost + "<< == >>" + SumDestroyCost())) {
					if (toClone.GetTowerComponent().cost <= SumDestroyCost()) {
						Pickup(new Item(ComponentGenerator.Get().TowerClone(toClone.GetTowerComponent())));
						toDestroy = new Item[NUM_CRAFTABLES];
					}
				}
			} else if (craftingMode == CRAFT_COMBINE) {
				if (GUI.Button(cloneButtonRect, "COMBINE")) {
					if (CountInventory(toDestroy) >= 2) {
						Pickup(new Item(ComponentGenerator.Get().GenerateComponent(Random.Range(0, BaseTower.TOWER_COMPLETE), SumDestroyCost())));
						toDestroy = new Item[NUM_CRAFTABLES];
					}
				}
			}
		}
	}

	private int SumDestroyCost() {
		int cost = 0;
		for (int i= 0; i < toDestroy.Length; i++) {
			if (toDestroy[i] != null) {
				cost += toDestroy[i].GetTowerComponent().cost;
			}
		}
		return cost;
	}

	private void SetInventoryOpen(bool setted) {
		if (!setted) {
			Pickup(toClone);
			foreach (Item i in toDestroy) {
				Pickup(i);
			}
			toClone = null;
			toDestroy = new Item[NUM_CRAFTABLES];
			UpdateInventory();
		}
	}

	private void UpdateInventory() {
		SortInventory();
	}

	public TowerComponent[] GetGameObjects(int type) {
		List<TowerComponent> objects = new List<TowerComponent>();
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory[i] != null) {
//				Debug.Log("Checking: " + inventory[i].GetTowerComponent().componentName + " > " + inventory[i].GetTowerComponent().componentType);
				if (inventory[i].GetTowerComponent().componentType == type) {
					objects.Add(inventory[i].GetTowerComponent());
				}
			}
		}
		TowerComponent[] temp = objects.ToArray();
		return temp;
	}

	public bool Pickup(Item it) {
		bool p = Pickup(inventory, it);
		UpdateInventory();
		return p;
	}

	private bool Pickup(Item[] list, Item it) {
		if (it == null)
			return true;
		for (int i = 0; i < list.Length; i++) {
			if (list[i] == null) {
				list[i] = it;
				return true;
			}
		}
		return false;
	}

	public void Remove(Item it) {
		this.Remove(inventory, it);
		UpdateInventory();
	}

	public void Remove(TowerComponent t) {
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory[i] != null) {
				if (inventory[i].GetTowerComponent() == t) {
					inventory[i] = null;
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
				inventory[i] = null;
				return;
			}
		}
	}

	public bool CanPickup() {
		return CanPickup(inventory);
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
		return CountInventory(inventory);
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

	public void SortInventory() {
		// Note: Using Bubble Sort (xD)
		for (int i = 0; i < inventory.Length; i++) {
			for (int j  = 0; j < inventory.Length - 1; j++) {
				if (inventory[j] == null) {
					// j is greater = Swap
					inventory[j] = inventory[j + 1];
					inventory[j + 1] = null;
				} else if (inventory[j + 1] == null) {
					// Do Nothing
				} else if (inventory[j].GetTowerComponent().componentType > inventory[j + 1].GetTowerComponent().componentType) {
					// j is greater = Swap
					Item it = inventory[j];
					inventory[j] = inventory[j + 1];
					inventory[j + 1] = it;
				}
			}
		}
	}
}
