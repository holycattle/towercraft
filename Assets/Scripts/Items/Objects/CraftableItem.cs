using UnityEngine;
using System.Collections;

public class CraftableItem : Item {
	// Constants
	public const int PART_DAMAGE = 0;
	public const int PART_RANGE = 1;
	public const int PART_ROF = 2;	// ROF = Rate of Fire
	public const int PART_MAX = 3;

	// Values
	public string craftableName;
	private int _type;		// Type of Craftable Item
	private int _stat = 0;	// Modifier Value

	public CraftableItem (int type, int cost) : base(ITEM_CRAFT) {
		_type = type;
		_stat = cost;
	}

	public override bool isLessThan(Item t) {
		return _type < ((CraftableItem)t).CraftableType;
	}

	public int CraftableType {
		get { return _type; }
	}

	public int Modifier {
		get { return _stat; }
	}
}
