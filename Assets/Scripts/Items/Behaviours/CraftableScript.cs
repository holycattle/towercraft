using UnityEngine;
using System.Collections;

public class CraftableScript : PickupableScript {
	public CraftableItem item;

	protected override void Start() {
		base.Start();

		if (item == null) {
			item = new CraftableItem(Random.Range(0, CraftableItem.PART_MAX), Random.Range(2, 10));
		}
	}

	public override void Pickup(ItemCollector ic) {
		ic.Pickup(item);
	}

	public override bool CanPickup(ItemCollector ic) {
		return ic.CanPickup();
	}
}
