using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerTurret : TowerComponent {

	// Turret Stats
	public List<ModifyingAttribute> attributes;
	public GameObject missile;
	public GameObject statusAilment;

	// Tooltip
	public string toolTipMessage;

	protected override void Awake() {
		base.Awake();

		attributes = new List<ModifyingAttribute>();
		Debug.Log("Checkered");
	}

	public override string GetTooltipString() {
		if (attributes == null) {
			Debug.Log("Nulled In Tooltip");
		}

		string s = "---" + componentName + "---\n";
		s += "Level: " + level + "\n";

		// Add Stat Messages
		for (int i = 0; i < System.Enum.GetValues(typeof(Stat)).Length; i++) {
			s += ((Stat)i).ToString() + ": ";
			float amt = 0;
			foreach (ModifyingAttribute m in attributes) {
				if (m.stat == ((Stat)i)) {
					amt += m.amount;
				}
			}
			amt = (Mathf.Round(amt * 10f) / 10f);
			s += amt + "\n";
		}

		return s + toolTipMessage;
	}
}
