using UnityEngine;
using System.Collections;

public class WeaponItem : Item {

	private string weaponName;

	// Stat Modifiers
	public int damage;
	public int range;
	public int firingRate;
	public int accuracy;

	public WeaponItem () : base(ITEM_WEAPON) {
		weaponName = "" + Random.Range(100, 999);

		damage = 2;
		range = 16;
		firingRate = 4;
		accuracy = 20;
	}

	public override bool isLessThan(Item t) {
		return true;
	}

	public override string GetName() {
		return weaponName;
	}

	public override string GetTooltip() {
		return weaponName + "\n" +
			"Damage: +" + damage + "\n" +
			"Range: +" + range + "\n" +
			"Firing Rate: +" + firingRate + "\n" +
			"Accuracy: +" + accuracy + "\n";
	}
}
