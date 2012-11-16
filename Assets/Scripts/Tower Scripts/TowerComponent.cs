using UnityEngine;
using System.Collections;

public class TowerComponent : MonoBehaviour {
	private string _componentName;
	private int _componentType;
	public ModifyingAttribute[] attributes;
	public Vector3 baseNextComponentPosition;	// Position of next component relative to this component

	void Awake() {
		attributes = new ModifyingAttribute[2];
		attributes[0] = new ModifyingAttribute(Stat.Range, 2);
		attributes[1] = new ModifyingAttribute(Stat.Damage, 1);
	}

	public string ComponentName {
		get { return _componentName; }
		set { _componentName = value; }
	}

	public int Type {
		get { return _componentType; }
		set { _componentType = value; }
	}
}
