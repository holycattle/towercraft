using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TowerComponent : MonoBehaviour {
	public string componentName;
	public int componentType;
	public int level;
	public List<ModifyingAttribute> attributes;
	public Vector3 baseNextComponentPosition;	// Position of next component relative to this component

	void Awake() {
		attributes = new List<ModifyingAttribute>();
	}

	public string GetTooltipString() {
		// TODO: Add Cost

		string s = "";

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
