using UnityEngine;
using System.Collections;

public class Item {
	public Texture texture;
	private TowerComponent _tc;

	public Item () {
		System.Random r = new System.Random();
		_tc = ComponentGenerator.Get().GenerateComponent(r.Next(BaseTower.TOWER_COMPLETE));
//		Debug.Log("Generated: " + _tc.componentName);
	}

	public Item (int i) {
		_tc = ComponentGenerator.Get().GenerateComponent(i);
//		Debug.Log("Generated: " + _tc.componentName);
	}

	public string Name {
		get { return _tc.ComponentName; }
	}

	public TowerComponent GetTowerComponent() {
		return _tc;
	}

	public GameObject GetComponentPrefab() {
		return _tc.gameObject;
	}
}