using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemCollector : MonoBehaviour {
	private const int WIDTH = 3;
	private const int HEIGHT = 4;
	private const int GRIDSIZE = 64;
	private const int SPACE = 4;
	private Item[] inventory;
	protected GameController _game;

	void Start() {
		inventory = new Item[WIDTH * HEIGHT];
		_game = GameObject.Find(" GameController").GetComponent<GameController>();

		Pickup(new Item(0));
		Pickup(new Item(1));
		Pickup(new Item(2));
		Pickup(new Item(0));
		Pickup(new Item(1));
		Pickup(new Item(2));
		Pickup(new Item(0));
		Pickup(new Item(1));
		Pickup(new Item(2));
	}

	void OnTriggerEnter(Collider other) {
		if (CanPickup()) {
			if (other.gameObject.tag == "Item") {
//				Debug.Log("Item Collected");
				Pickup(other.gameObject.GetComponent<ItemScript>().item);
				DestroyObject(other.gameObject);
			}
		}
	}

	void Update() {
		if (_game.ActiveMenu == Menu.Inventory || _game.ActiveMenu == Menu.Game) {
			if (Input.GetKeyDown(KeyCode.E)) {
				_game.ActiveMenu = _game.ActiveMenu == Menu.Inventory ? Menu.Game : Menu.Inventory;
			}
		}
	}

	void OnGUI() {
		if (_game.ActiveMenu == Menu.Inventory) {
			int width = WIDTH * GRIDSIZE + (WIDTH - 1) * SPACE;
			int height = HEIGHT * GRIDSIZE + (HEIGHT - 1) * SPACE;
			GUI.SelectionGrid(new Rect(Screen.width / 2 - width / 2, Screen.height / 2 - width / 2, width, height), 0, MakeNameStrings(), WIDTH);
		}
	}

	public GameObject[] GetGameObjects(int type) {
		List<GameObject> objects = new List<GameObject>();

		for (int i = 0; i < inventory.Length; i++) {
			if (inventory[i] != null) {
				if (inventory[i].GetTowerComponent().Type == type) {
					objects.Add(inventory[i].GetTowerComponent().gameObject);
				}
			}
		}
		GameObject[] temp = objects.ToArray();
		return temp;
	}

	private string[] MakeNameStrings() {
		string[] s = new string[WIDTH * HEIGHT];
		for (int i = 0; i < WIDTH * HEIGHT; i++) {
			if (inventory[i] == null) {
				s[i] = "";
			} else {
				s[i] = inventory[i].Name;
			}
		}
		return s;
	}

	private bool Pickup(Item it) {
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory[i] == null) {
				inventory[i] = it;
				return true;
			}
		}
		return false;
	}

	private bool CanPickup() {
		foreach (Item i in inventory) {
			if (i == null) {
				return true;
			}
		}
		return false;
	}
}
