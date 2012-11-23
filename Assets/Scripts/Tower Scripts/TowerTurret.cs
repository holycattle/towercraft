using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TowerTurret : TowerComponent {
	public List<ModifyingAttribute> attributes;

	protected override void Awake() {
		base.Awake();

		attributes = new List<ModifyingAttribute>();
	}

	public override string GetTooltipString() {
		string s = "---" + componentName + "---\n";

		for (int i = 0; i < Enum.GetValues(typeof(Stat)).Length; i++) {
			s += ((Stat)i).ToString() + ": ";
			int amt = 0;
			foreach (ModifyingAttribute m in attributes) {
				if (m.stat == ((Stat)i)) {
					amt += m.amount;
				}
			}
			s += amt + "\n";
		}

		return s;
	}
}
