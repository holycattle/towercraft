using UnityEngine;
using System.Collections;

public class Item {
	public const int ITEM_TOWER = 0;
	public const int ITEM_CRAFT = 1;
	public const int ITEM_WEAPON = 2;

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
}