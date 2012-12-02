using UnityEngine;
using System.Collections;

public class Item {
	public const int ITEM_TOWER = 0;
	public const int ITEM_CRAFT = 1;
	public const int ITEM_WEAPON = 2;
	public const int ITEM_AMMO = 3;
	public const int ITEM_HEALTH = 4;

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

	public void ItemInstantiate(Vector3 position, Quaternion rot) {
		GameObject g = null;
		switch (itemType) {
			case ITEM_TOWER:
				g = GameObject.Instantiate(Resources.Load("Prefabs/Items/Item", typeof(GameObject)) as GameObject, position, rot) as GameObject;
				g.GetComponent<TowerItemScript>().item = (TowerItem)this;
				break;
			case ITEM_CRAFT:
				g = GameObject.Instantiate(Resources.Load("Prefabs/Items/Craftable", typeof(GameObject)) as GameObject, position, rot) as GameObject;
				g.GetComponent<CraftableScript>().item = (CraftableItem)this;
				break;
			case ITEM_WEAPON:
				g = GameObject.Instantiate(Resources.Load("Prefabs/Items/Weapon", typeof(GameObject)) as GameObject, position, rot) as GameObject;
				g.GetComponent<WeaponItemScript>().item = (WeaponItem)this;
				break;
		}
	}
}