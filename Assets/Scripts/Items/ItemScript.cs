using UnityEngine;
using System.Collections;

public class ItemScript : PickupableScript {
	public Item item;

	protected override void Start() {
		base.Start();
		item = new Item(Random.Range(0, BaseTower.TOWER_COMPLETE), Random.Range(2, 10));
	}

	public override void Pickup(ItemCollector ic) {
		ic.Pickup(item);
	}

	public override bool CanPickup(ItemCollector ic) {
		return ic.CanPickup();
	}
}
