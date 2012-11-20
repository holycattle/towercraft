using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemCollector : MonoBehaviour {
	private const int WIDTH = 4;
	private const int HEIGHT = 6;
	private const int GRIDWIDTH = 128;
	private const int GRIDHEIGHT = 32;
	private const int SPACE = 4;
	private Item[] inventory;
	protected GameController _game;

	// GUI Elements
	private Rect[] drawRects;
	private Rect toCloneRect;
	private Rect toDestroyScrollRect;
	private Rect toDestroyRect;
	private Vector2 scrollPosition;
	private Rect cloneButtonRect;

	// Cloning
	private Item toClone;
	private List<Item> toDestroy;

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
		toDestroyRect = new Rect(0, 0, 0, 0);
		toDestroyScrollRect = new Rect(sx + totalWidth + GRIDWIDTH, sy + (GRIDHEIGHT + SPACE) * 2, GRIDWIDTH * 1.5f, (GRIDHEIGHT + SPACE) * (HEIGHT - 2));

		cloneButtonRect = new Rect(sx + totalWidth + GRIDWIDTH, sy + (GRIDHEIGHT + SPACE) * HEIGHT, GRIDWIDTH, GRIDHEIGHT);

		Pickup(new Item(0, 4));
//		Pickup(new Item(0, 1));
		Pickup(new Item(1, 4));
		Pickup(new Item(2, 4));
		Pickup(new Item(2, 4));

		toDestroy = new List<Item>();

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

			// Draw Inventory
			for (int i = 0; i < WIDTH * HEIGHT; i++) {
				if (inventory[i] == null)
					continue;
				if (GUI.Button(drawRects[i], inventory[i].Name)) {
					if (toClone == null) {
						toClone = inventory[i];
					} else {
						toDestroy.Add(inventory[i]);
						toDestroyRect = new Rect(0, 0, GRIDWIDTH, (GRIDHEIGHT + SPACE) * toDestroy.Count);
					}
					this.Remove(inventory[i]);
				}
			}

			// Draw ToClone
			if (GUI.Button(toCloneRect, toClone == null ? "" : toClone.Name)) {
				Pickup(toClone);
				toClone = null;
			}

			// Draw ToDestroy
			scrollPosition = GUI.BeginScrollView(toDestroyScrollRect, scrollPosition, toDestroyRect);
			for (int i = 0; i < toDestroy.Count; i++) {
				if (GUI.Button(new Rect(0, i * (GRIDHEIGHT + SPACE), GRIDWIDTH, GRIDHEIGHT), toDestroy[i].Name)) {
					Pickup(toDestroy[i]);
					toDestroy.RemoveAt(i);
				}
			}
			GUI.EndScrollView();

			// Clone Button
			if (GUI.Button(cloneButtonRect, toClone == null ? "" : toClone.GetTowerComponent().cost + "<< == >>" + SumDestroyCost())) {
				Pickup(new Item(ComponentGenerator.Get().TowerClone(toClone.GetTowerComponent())));
				toDestroy.Clear();
			}
		}
	}

	private int SumDestroyCost() {
		int cost = 0;
		for (int i= 0; i < toDestroy.Count; i++) {
			cost += toDestroy[i].GetTowerComponent().cost;
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
			toDestroy.Clear();
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
		if (it == null)
			return true;
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory[i] == null) {
				inventory[i] = it;
				UpdateInventory();
				return true;
			}
		}
		return false;
	}

	public void Remove(Item it) {
		for (int i = 0; i < inventory.Length; i++) {
			if (it == inventory[i]) {
				inventory[i] = null;
				UpdateInventory();
				return;
			}
		}
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

	public bool CanPickup() {
		foreach (Item i in inventory) {
			if (i == null) {
				return true;
			}
		}
		return false;
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

	public int CountInventory() {
		int counter = 0;
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory[i] != null) {
				counter++;
			}
		}
		return counter;
	}
}
