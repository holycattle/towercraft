using UnityEngine;
using System.Collections;

public class HealthScript : PickupableScript {

	private int health;

	protected override void Start() {
		base.Start();

		health = 10;
	}

	public override void Pickup(ItemCollector ic) {
		ic.gameObject.transform.root.GetComponentInChildren<PlayerController>().AddLife(health);
	}

	public override bool CanPickup(ItemCollector ic) {
		return ic.gameObject.transform.root.GetComponentInChildren<PlayerController>().Life < PlayerController.MAX_LIFE;
	}
}
