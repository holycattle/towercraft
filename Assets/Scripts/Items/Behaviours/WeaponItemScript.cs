using UnityEngine;
using System.Collections;

public class WeaponItemScript : PickupableScript {
	public WeaponItem item;

	protected override void Start() {
		base.Start();
//		if (item == null) {
//			item = new WeaponItem();
//		}
	}

	public override void Pickup(ItemCollector ic) {
		ic.Pickup(item);
	}

	public override bool CanPickup(ItemCollector ic) {
		return ic.CanPickup();
	}
}
