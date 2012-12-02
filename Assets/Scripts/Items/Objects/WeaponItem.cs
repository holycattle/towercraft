using UnityEngine;
using System.Collections;

public class WeaponItem : Item {

	public const int DPS = 0;
	public const int STAT = 1;
	public string weaponName;
	public int weaponType;

	public WeaponItem (int type) : base(ITEM_WEAPON) {
		weaponType = type;

		weaponName = "" + Random.Range(100, 999);
	}

	public override bool isLessThan(Item t) {
		return true;
	}

	public override string GetName() {
		return weaponName;
	}

	public static WeaponItem RandomItem(int level) {
		if (Random.Range(0, 2) == 0) {
			return new WeaponDPSItem(level);
		} else {
			return new WeaponStatItem(level);
		}
	}
}
