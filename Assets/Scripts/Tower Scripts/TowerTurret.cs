using UnityEngine;
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

		for (int i = 0; i < System.Enum.GetValues(typeof(Stat)).Length; i++) {
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

	public string GenerateName() {
		string[] guns = {"Turret", "Buster"};
		string[] s_damage = {"Powerful", "Potent", "Strengthy"};			// Damage
		string[] s_range = {"All-Seeing", "Remote"};					// Range
		string[] s_firing = {"Rapid-Fire", "Accelerated", "Crazy"};	// ROF

		string s = "";

		Stat max = Stat.Damage;
		int level = 0;
		foreach (ModifyingAttribute m in attributes) {
			if (BaseTower.CalculateStatLevel(m.stat, m.amount) > level) {
				max = m.stat;
				level = BaseTower.CalculateStatLevel(m.stat, m.amount);
			}
		}

		switch (max) {
			case Stat.Damage:
				s += s_damage[Random.Range(0, s_damage.Length)];
				break;
			case Stat.Range:
				s += s_range[Random.Range(0, s_range.Length)];
				break;
			case Stat.FiringRate:
				s += s_firing[Random.Range(0, s_firing.Length)];
				break;
		}

		s += " " + guns[Random.Range(0, guns.Length)] + " v" + Random.Range(1, 10);

		return s;
	}

	public int CalculateCost() {
		int total = 0;
		foreach (ModifyingAttribute m in attributes) {
			total += BaseTower.CalculateStatLevel(m.stat, m.amount);
		}
		return total;
	}
}
