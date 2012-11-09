using UnityEngine;
using System.Collections;

public class Inventory {

	private Item[] items;

	public Inventory (int width, int height) {
		items = new Item[width * height];
	}

	public bool Pickup(Item it) {
		for (int i = 0; i < items.Length; i++) {
			if (items[i] == null) {
				items[i] = it;
				return true;
			}
		}
		return false;
	}

	public bool CanPickup() {
		foreach (Item i in items) {
			if (i == null) {
				return true;
			}
		}
		return false;
	}
}