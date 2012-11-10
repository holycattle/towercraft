using UnityEngine;
using System.Collections;

public class TowerComponent : MonoBehaviour {

	public string componentName;
	private int _componentType;
	public ModifyingAttribute[] attributes;
	public Vector3 baseNextComponentPosition;	// Position of next component relative to this component

	void Awake() {	// << AWAKE WAS THE SOLUTION ALL ALONG!
		attributes = new ModifyingAttribute[2];
		attributes[0] = new ModifyingAttribute(Stat.Range, 2);
		attributes[1] = new ModifyingAttribute(Stat.Damage, 1);
	}

	public int Type {
		get { return _componentType; }
		set { _componentType = value; }
	}
}
