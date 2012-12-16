using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TowerComponent : MonoBehaviour {
	public string componentName;
	public int componentType;
	public float level;

	protected virtual void Awake() {
	}

	protected virtual void Start() {
	}

	public virtual Vector3 NextComponentPosition() {
		return Vector3.zero;
	}

	public virtual string GetTooltipString() {
		return "NULL";

		// TODO: Add Cost

//		string s = "";
//
//		for (int i = 0; i < Enum.GetValues(typeof(Stat)).Length; i++) {
//			s += ((Stat)i).ToString() + ": ";
//			int amt = 0;
//			foreach (ModifyingAttribute m in attributes) {
//				if (m.stat == ((Stat)i)) {
//					amt += m.amount;
//				}
//			}
//			s += amt + "\n";
//		}
//
//		return s;
	}
}
