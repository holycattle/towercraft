using UnityEngine;
using System.Collections;

public class CraftableItem : Item {
	// Constants
	public const float CRAFT_RANDOMNESS = 0.2f;
	public const int PART_DAMAGE = 0;
	public const int PART_RANGE = 1;
	public const int PART_ROF = 2;	// ROF = Rate of Fire
	public const int PART_MAX = 3;

	// Values
	public string craftableName;
	private int _type;		// Type of Craftable Item
	private int _level;
	private int _stat = 0;	// Modifier Value

	public CraftableItem (int type, int cost) : base(ITEM_CRAFT) {
		_type = type;
		_level = cost;

		_stat = 1;

		craftableName = "";
		switch (_type) {
			case PART_DAMAGE:
				craftableName += "Energizer";
//				_stat = cost * BaseTower.MULT_DAMAGE;
				break;
			case PART_RANGE:
				craftableName += "Stem";
//				_stat = cost * BaseTower.MULT_RANGE;
				break;
			case PART_ROF:
				craftableName += "Capacitor";
//				_stat = cost * BaseTower.MULT_FIRINGRATE;
				break;
		}


		craftableName += " - v" + Random.Range(1, 10) + "." + Random.Range(100, 1000);
	}

	public override bool isLessThan(Item t) {
		return _type < ((CraftableItem)t).CraftableType;
	}

	public int CraftableType {
		get { return _type; }
	}

	public int Modifier {
		get { return _stat; }
	}

	public override string GetName() {
		return craftableName;
	}

	public override string GetTooltip() {
		string s = GetName() + "\nLevel: " + _level + "\n";
		switch (_type) {
			case PART_DAMAGE:
				s += "Damage: ";
				break;
			case PART_RANGE:
				s += "Range: ";
				break;
			case PART_ROF:
				s += "Firing Rate: ";
				break;
		}
		s += _stat;
		return s;
	}
}
