using UnityEngine;
using System.Collections;

public class TowerBase : TowerComponent {

	public Vector3 baseNextComponentPosition;	// Position of next component relative to this component

	public override Vector3 NextComponentPosition() {
		return baseNextComponentPosition;
	}

	public override string GetTooltipString() {
		string s = "---" + componentName + "---\nLevel: " + level;

		return s;
	}
}
