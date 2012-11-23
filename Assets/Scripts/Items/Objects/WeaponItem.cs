using UnityEngine;
using System.Collections;

public class WeaponItem : Item {
	
	public WeaponItem () : base(ITEM_WEAPON) {
	}
//
//	public override bool isLessThan(Item t) {
//		return _type < ((CraftableItem)t).CraftableType;
//	}
}
