using UnityEngine;
using System.Collections;

public class AmmoScript : PickupableScript {

	public int amount;

	protected override void Start() {
		base.Start();

		if (amount == 0) {
			amount = 10;
		}
	}

	public override void Pickup(ItemCollector ic) {
		if (ic != null) 
			ic.gameObject.transform.root.GetComponentInChildren<Weapon>().AddAmmo(amount);
	}

	public override bool CanPickup(ItemCollector ic) {
		return true;
	}
}
