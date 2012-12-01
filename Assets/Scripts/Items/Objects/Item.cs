using UnityEngine;
using System.Collections;

public class Item {
	public const int ITEM_TOWER = 0;
	public const int ITEM_CRAFT = 1;
	public const int ITEM_WEAPON = 2;
	public const int ITEM_AMMO = 3;

	// Item Variables
	public int itemType;
	public Texture texture;

	public Item (int type) {
		itemType = type;
	}

	public virtual bool isLessThan(Item t) {
		Debug.Log("ERROR! YOU SHOULDNT SEE THIS!");
		return true;
	}

	public virtual string GetTooltip() {
		return "None.";
	}

	public virtual string GetName() {
		return "None.";
	}

	public int ItemType {
		get { return itemType; }
	}

	public static void Instantiate(int type, Vector3 position, Quaternion rot) {
		switch (type) {
			case ITEM_TOWER:
				break;
			case ITEM_CRAFT:
				break;
			case ITEM_WEAPON:
				break;
			case ITEM_AMMO:
				break;
		}
	}
}