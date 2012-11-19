using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowerComponent : MonoBehaviour {
	public string componentName;
	public int componentType;
	public List<ModifyingAttribute> attributes;
	public Vector3 baseNextComponentPosition;	// Position of next component relative to this component

	void Awake() {
		attributes = new List<ModifyingAttribute>();
//		attributes.Add(new ModifyingAttribute(Stat.Range, 2));
//		attributes.Add(new ModifyingAttribute(Stat.Damage, 1));
//		Debug.Log("Instantiation: " + attributes.Count);
	}

	void Start() {
	}

//	public void AddAttribute(ModifyingAttribute m) {
//		attributes.Add(m);
//	}
}
