using UnityEngine;
using System.Collections;

public class ComponentGenerator {
	private static ComponentGenerator _cgen;	// Creating a Singleton
	private TowerComponent[][] _towerParts;
	private Object[] _bases;
	private Object[] _stems;
	private Object[] _turrets;

	public ComponentGenerator () {
		string[] dirs = {"Base", "Stem", "Turret"};
		_towerParts = new TowerComponent[BaseTower.TOWER_COMPLETE][];

		for (int i = 0; i < dirs.Length; i++) {
			Object[] loaded = Resources.LoadAll("Prefabs/Towers/" + dirs[i], typeof(GameObject));
			_towerParts[i] = new TowerComponent[loaded.Length];
			for (int o = 0; o < loaded.Length; o++) {
				_towerParts[i][o] = ((GameObject)loaded[o]).GetComponent<TowerComponent>();
				_towerParts[i][o].Type = i;
			}
		}
//		Debug.Log("ComponentGenerator: " + _towerParts[0].Length + "/" + _towerParts[1].Length + "/" + _towerParts[2].Length);
	}

	public static ComponentGenerator Get() {
		if (_cgen == null)
			_cgen = new ComponentGenerator();
		return _cgen;
	}

	public TowerComponent GenerateComponent(int type) {
		if (type < 0 || type >= BaseTower.TOWER_COMPLETE)
			return null;
		return _towerParts[type][Random.Range(0, _towerParts[type].Length)];
	}
}
