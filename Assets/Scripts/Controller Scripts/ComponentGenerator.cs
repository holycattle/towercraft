using UnityEngine;
using System.Collections;

public class ComponentGenerator {
	private static ComponentGenerator _cgen;	// Creating a Singleton
	private Object[] _bases;
	private Object[] _stems;
	private Object[] _turrets;

	public ComponentGenerator () {
		_bases = Resources.LoadAll("Prefabs/Towers/Base", typeof(GameObject));
		_stems = Resources.LoadAll("Prefabs/Towers/Stem", typeof(GameObject));
		_turrets = Resources.LoadAll("Prefabs/Towers/Turret", typeof(GameObject));

		Debug.Log("ComponentGenerator: " + _bases.Length + "/" + _stems.Length + "/" + _turrets.Length);
	}

	public static ComponentGenerator Get() {
		if (_cgen == null) {
			_cgen = new ComponentGenerator();
		}

		return _cgen;
	}

	public TowerComponent GenerateComponent(int type) {
		Debug.Log("Started Generation");

		switch (type) {
			case BaseTower.TOWER_BASE:


				break;
			case BaseTower.TOWER_STEM:
				break;
			case BaseTower.TOWER_TURRET:
				break;
		}

		return null;
	}
}
