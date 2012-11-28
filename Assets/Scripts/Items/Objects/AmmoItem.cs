using UnityEngine;
using System.Collections;

public class AmmoItem : Item {
	public const int SMALL_CORE = 0;
	public const int MEDIUM_CORE = 1;
	public const int BIG_CORE = 2;
	private string ammoName = "Nuclear Core";
//	private int amount = 0;

	public AmmoItem () : base(ITEM_AMMO) {
		int coreType = UnityEngine.Random.Range(0, 2);

		switch (coreType) {
			case SMALL_CORE:
				ammoName = "Small " + ammoName;
//				amount = 10;
				break;
			case MEDIUM_CORE:
				ammoName = "Medium " + ammoName;
//				amount = 20;
				break;
			case BIG_CORE:
				ammoName = "Big " + ammoName;
//				amount = 30;
				break;
		}
	}

	public override string GetName() {
		return ammoName;
	}
}
