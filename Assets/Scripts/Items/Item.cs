using UnityEngine;
using System.Collections;

public class Item {
	public Texture texture;
	private TowerComponent _tc;

	public Item () {
		System.Random r = new System.Random();
		_tc = ComponentGenerator.Get().GenerateComponent(r.Next(BaseTower.TOWER_COMPLETE), 0);
//		Debug.Log("Item Gen: " + _tc.componentName + "> ");
//		Debug.Log("Item Gen: " + _tc.componentName + "> " + _tc.attributes.Count);
//		Debug.Log("Generated: " + _tc.componentName);
	}

	public Item (int i, int cost) {
		_tc = ComponentGenerator.Get().GenerateComponent(i, cost);
//		Debug.Log("Item Gen: " + _tc.componentName + "> ");
//		Debug.Log("Item Gen: " + _tc.componentName + "> " + _tc.attributes.Count);
//		Debug.Log("Generated: " + _tc.componentName);
	}

	public Item (TowerComponent t) {
		_tc = t;
	}

	public string Name {
		get { return _tc.componentName; }
	}

	public TowerComponent GetTowerComponent() {
		return _tc;
	}
}