using System.Collections.Generic;// For List<>

public class ModifiedStat : BaseStat {
	private List<ModifyingAttribute> _mods;	// List of attributes that modify this stat.
	private float _modValue;					// Amount added to the baseValue from the modifiers
	
	public ModifiedStat () {
		_mods = new List<ModifyingAttribute>();
		_modValue = 0;
	}

	public void AddModifier(ModifyingAttribute mod) {
		_mods.Add(mod);
		CalculateModValue();
	}

	public void ClearModifiers() {
		_mods.Clear();
		CalculateModValue();
	}
	
	private void CalculateModValue() {
		_modValue = 0;

		if (_mods.Count > 0) {
			foreach (ModifyingAttribute att in _mods) {
				_modValue += att.amount;
			}
		}
	}
	
	public float AdjustedBaseValue {
		get { return BaseValue + _modValue; }
	}
	
	// Note: Not the same update as MonoBehaviour
	public void Update() {
		CalculateModValue();
	}
}

public struct ModifyingAttribute {
	public Stat stat;
	public float amount;

	public ModifyingAttribute (Stat s, float a) {
		stat = s;
		amount = a;
	}

	public string DebugLog() {
		return stat.ToString() + ": " + amount;
	}
}