using UnityEngine;
using System.Collections;

public class CraftableItem : Item {
	// Constants
	public const float CRAFT_RANDOMNESS = 0.2f;
	public const int PART_DPS = 0;
	public const int PART_RANGE = 1;
	public const int PART_MODIFIER = 2;	// ROF = Rate of Fire
	public const int PART_MAX = 3;

	// Values
	public string craftableName;
	private int _type;		// Type of Craftable Item
	private int _level;
	private float _stat = 0;	// Modifier Value
	private string _tooltip;

	public CraftableItem (int level) : this(Random.Range(0, PART_MAX), level) {
	}

	public CraftableItem (int type, int level) : base(ITEM_CRAFT) {
		_type = type;
		_level = level;

		craftableName = "";
		_tooltip = "";
		switch (_type) {
			case PART_DPS:
				craftableName += "Energizer";
				_stat = (SpawnScheme.HEALTH_COEFF * (1 + level * SpawnScheme.HEALTH_MULTIPLIER)) / ((BaseTower.BASE_RANGE * 2) * ComponentGenerator.Get().PASSESTOKILL);
				break;
			case PART_RANGE:
				craftableName += "Stem";
				_stat = Mathf.Round(BaseTower.JiggleStat(BaseTower.BASE_RANGE, 0.1f) * 10f) / 10f;
				_tooltip = "Range will scale depending on other components.";
				break;
			case PART_MODIFIER:
				craftableName += "Capacitor";
				_stat = Random.Range(0, 4);
				switch ((int)_stat) {
					case 0:
						_tooltip = "Turret will have a HIGH Rate of fire.";
						break;
					case 1:
						_tooltip = "Turret will have a HIGH Damage per shot.";
						break;
					case 2:
						_tooltip = "Turret will have a HIGH Range.";
						break;
					case 3:
						_tooltip = "Turret will have a small chance to get an elemental.";
						break;
				}
				break;
		}

		craftableName += " v" + level + "." + Random.Range(10, 100);
	}

	public override bool isLessThan(Item t) {
		return _type < ((CraftableItem)t).CraftableType;
	}

	public int CraftableType {
		get { return _type; }
	}

	public float Modifier {
		get { return _stat; }
	}

	public int Level {
		get { return _level; }
	}

	public override string GetName() {
		return craftableName;
	}

	public override string GetTooltip() {
		string s = GetName() + "\nLevel: " + _level + "\n";
		switch (_type) {
			case PART_DPS:
				s += "DPS: ";
				s += _stat;
				s += "\n";
				break;
			case PART_RANGE:
				s += "Range: ";
				s += _stat;
				s += "\n";
				break;
			case PART_MODIFIER:
				break;
		}
		s += "-----\n";
		s += _tooltip;
		return s;
	}

	public string RawTooltip {
		get { return _tooltip; }
	}
}
