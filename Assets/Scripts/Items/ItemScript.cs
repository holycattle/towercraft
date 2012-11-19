using UnityEngine;
using System.Collections;

public class ItemScript : PickupableScript {
	public Item item;

	protected override void Start() {
		base.Start();
		item = new Item();
	}

	public override void Pickup(ItemCollector ic) {
		ic.Pickup(item);
	}

	public override bool CanPickup(ItemCollector ic) {
		return ic.CanPickup();
	}
}
