using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemCollector : MonoBehaviour {
	private const int WIDTH = 5;
	private const int HEIGHT = 5;
	private const int GRIDSIZE = 64;
	private const int SPACE = 4;
	private Item[] inventory;
	protected GameController _game;

	void Start() {
		inventory = new Item[WIDTH * HEIGHT];
		_game = GameObject.Find(" GameController").GetComponent<GameController>();

		Pickup(new Item(0, 4));
//		Pickup(new Item(0, 1));
		Pickup(new Item(1, 4));
		Pickup(new Item(2, 4));
		Pickup(new Item(2, 4));
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

	public TowerComponent[] GetGameObjects(int type) {
		List<TowerComponent> objects = new List<TowerComponent>();
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory[i] != null) {
				Debug.Log("Checking: " + inventory[i].GetTowerComponent().componentName + " > " + inventory[i].GetTowerComponent().componentType);
				if (inventory[i].GetTowerComponent().componentType == type) {
					objects.Add(inventory[i].GetTowerComponent());
				}
			}
		}
		TowerComponent[] temp = objects.ToArray();
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

	public void Remove(Item it) {
		for (int i = 0; i < inventory.Length; i++) {
			if (it == inventory[i]) {
				inventory[i] = null;
				Debug.Log("Removed Successful!");
				return;
			}
		}
		Debug.Log("Removed FAIL!");
	}

	public void Remove(TowerComponent t) {
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory[i] != null) {
				if (inventory[i].GetTowerComponent() == t) {
					inventory[i] = null;
					Debug.Log("Removed Successful!");
					return;
				}
			}
		}
		Debug.Log("Removed FAIL!");
	}

	public bool Pickup(Item it) {
		for (int i = 0; i < inventory.Length; i++) {
			if (inventory[i] == null) {
				inventory[i] = it;
				return true;
			}
		}
		return false;
	}

	public bool CanPickup() {
		foreach (Item i in inventory) {
			if (i == null) {
				return true;
			}
		}
		return false;
	}
}
