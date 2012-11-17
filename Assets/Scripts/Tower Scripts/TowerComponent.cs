using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerComponent : MonoBehaviour {
	private string _componentName;
	private int _componentType;
	public List<ModifyingAttribute> attributes;
	public Vector3 baseNextComponentPosition;	// Position of next component relative to this component

	void Awake() {
		attributes = new List<ModifyingAttribute>();
		attributes.Add(new ModifyingAttribute(Stat.Range, 2));
		attributes.Add(new ModifyingAttribute(Stat.Damage, 1));
	}

	public void AddAttribute(ModifyingAttribute m) {
		attributes.Add(m);
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
