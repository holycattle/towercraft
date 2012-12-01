using UnityEngine;
using System.Collections;

public class WeaponStatItem : WeaponItem {

	// Stat Modifiers
//	public float accuracy;
	public int magSize;
	public float range;
	public float reloadTime;

	public WeaponStatItem (int cost) : base(STAT) {
		magSize = Random.Range(16, 16 + cost * 10);
		reloadTime = ((magSize - 14f) / (cost * 10f)) * 4f;
		range = 64;

		if (magSize - 16 < (cost * 10) * 0.5f) {
			weaponName = "Quick Mechanism";
		} else {
			weaponName = "Large Mechanism";
		}

		weaponName += " v" + Random.Range(1, 10) + "." + Random.Range(1, 10);
	}

	public override string GetTooltip() {
		return weaponName + "\n" +
			"Magazine Size: " + magSize + "\n" +
			"Reload Time: +" + reloadTime + "\n" +
			"Range: +" + range;
	}
}
