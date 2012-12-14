using UnityEngine;
using System.Collections;

public class ComponentGenerator {
	public float PASSESTOKILL = 2f;
	private static ComponentGenerator _cgen;	// Creating a Singleton
	private TowerComponent[][] _towerParts;
	private Object[] _bases;
	private Object[] _stems;
	private Object[] _turrets;

	// Turret Constants
	private Vector3 buildSpot = new Vector3(0, 50, 0);
	private Transform partsRoot;

	// Turret Parts
	private GameObject turretBase;
	private GameObject spinningBase;
	private TowerStem[] stems;
	private Object[] missileSources;
	private Object[] barrels;
	private Object[] spinningParts;
	private Object[] missiles;

	public ComponentGenerator () {
		string[] dirs = {"Base", "Turret"};
		_towerParts = new TowerComponent[BaseTower.TOWER_COMPLETE][];

		for (int i = 0; i < dirs.Length; i++) {
			Object[] loaded = Resources.LoadAll("Prefabs/Towers/" + dirs[i], typeof(GameObject));
			_towerParts[i] = new TowerComponent[loaded.Length];
			for (int o = 0; o < loaded.Length; o++) {
				_towerParts[i][o] = ((GameObject)loaded[o]).GetComponent<TowerComponent>();
				_towerParts[i][o].componentType = i;
				_towerParts[i][o].componentName = ((GameObject)loaded[o]).name;
			}
		}

		/*
		 *	Turret Parts
		 */
		string s = "Prefabs/Towers/Turret Parts/";
		turretBase = Resources.Load(s + "Turret Base", typeof(GameObject)) as GameObject;
		spinningBase = Resources.Load(s + "Spinning Base", typeof(GameObject)) as GameObject;

		missileSources = Resources.LoadAll(s + "Missile Source", typeof(GameObject));
		barrels = Resources.LoadAll(s + "Barrel", typeof(GameObject));
		spinningParts = Resources.LoadAll(s + "Spinning", typeof(GameObject));
		missiles = Resources.LoadAll(s + "Missiles", typeof(GameObject));
		Object[] loadedStems = Resources.LoadAll(s + "Stems", typeof(GameObject));
		stems = new TowerStem[loadedStems.Length];
		for (int i = 0; i < loadedStems.Length; i++) {
			stems[i] = ((GameObject)loadedStems[i]).GetComponent<TowerStem>();
		}

		Debug.Log("ComponentGenerator: " + _towerParts[0].Length + "/" + _towerParts[1].Length + " / S:" + stems.Length);

		partsRoot = GameObject.Find("[Root]Parts").transform;
	}

	public static ComponentGenerator Get() {
		if (_cgen == null)
			_cgen = new ComponentGenerator();
		return _cgen;
	}

	public TowerTurret UpgradeTurret(CraftableItem[] parts, TowerTurret t) {
		return null;
	}

	public TowerTurret GenerateTurret(CraftableItem[] parts) {
		/*
		 *	Parts Generation
		 */
		int chStem = Random.Range(0, stems.Length);
		int chMissile = Random.Range(0, missileSources.Length);
		int chBarrel = Random.Range(0, barrels.Length);
		int chSpinner = Random.Range(0, spinningParts.Length);

		// Turret Base
		GameObject g = GameObject.Instantiate(turretBase, buildSpot, Quaternion.identity) as GameObject;

		// Stem
		TowerStem tStem = GameObject.Instantiate(stems[chStem], buildSpot, Quaternion.identity) as TowerStem;
		tStem.transform.parent = g.transform;
		Vector3 stemOffest = tStem.baseNextComponentPosition;

		// Spinner Base
		GameObject gSpin = GameObject.Instantiate(spinningBase, buildSpot + stemOffest, Quaternion.identity) as GameObject;
		gSpin.transform.parent = g.transform;

		// Missile
		GameObject gMiss = GameObject.Instantiate((GameObject)missileSources[chMissile], buildSpot + stemOffest, Quaternion.identity) as GameObject;
		gMiss.transform.parent = g.transform;
		gMiss.name = "MissileSource";

		// Barrel
		GameObject gBarr = GameObject.Instantiate((GameObject)barrels[chBarrel], buildSpot + stemOffest, Quaternion.identity) as GameObject;
		gBarr.transform.parent = gMiss.transform;	// Barrel is a CHILD of missileSource

		// Spinning Thing
		int numSpinners = Random.Range(2, 7);	// 2 - 6 possible spinners
		float angle = (360 / numSpinners) * Mathf.Deg2Rad;
		for (int i = 0; i < numSpinners; i++) {
			Vector3 offset = new Vector3(Mathf.Cos(i * angle) * 2, 1, Mathf.Sin(i * angle) * 2);
			GameObject gSpinner = GameObject.Instantiate((GameObject)spinningParts[chSpinner], buildSpot + stemOffest + offset, Quaternion.identity) as GameObject;
			gSpinner.transform.parent = gSpin.transform;
		}

		/*
		 *	Stats Generation
		 */
		TowerTurret t = g.GetComponent<TowerTurret>();
		string name = "";

		// Get average level of the three components
		float avgLevel = (parts[CraftableItem.PART_DPS].Level + parts[CraftableItem.PART_RANGE].Level + parts[CraftableItem.PART_MODIFIER].Level) / 3f;

		// Range
		float s_range;
		s_range = (parts[CraftableItem.PART_RANGE].Level / avgLevel) * parts[CraftableItem.PART_RANGE].Modifier;
		if ((int)parts[CraftableItem.PART_MODIFIER].Modifier == 2) {
			s_range *= 1.5f;
		}
		s_range = BaseTower.JiggleStat(s_range, 0.05f);
		s_range = Mathf.Round(s_range * 10) / 10f;

		// Determine DPS for the current level
		float dps = parts[CraftableItem.PART_DPS].Modifier;
		float DPSMULT = 2f;

		if ((int)parts[CraftableItem.PART_MODIFIER].Modifier == 3) {
			// Add Status Ailment
			if (Random.Range(0, 3) == 0) {
				dps /= 2f;

				int i = Random.Range(0, Ailment.STUN + 1);
				string[] s = {"Burn", "Slow", "Stun"};
				GameObject statEffect = Resources.Load("Prefabs/StatusAilments/" + s[i], typeof(GameObject)) as GameObject;
				t.missile = Resources.Load("Prefabs/Towers/Turret Parts/MissilesStatus/" + s[i] + "Missile", typeof(GameObject)) as GameObject;
				switch (i) {
					case Ailment.BURN:
					//statEffect.GetComponent<Burn>().damage = 2;
						name += "Incendiary ";
						break;
					case Ailment.SLOW:
					//statEffect.GetComponent<Slow>().slowPercentage = 0.5f;
						name += "Slowness ";
						break;
					case Ailment.STUN:
						name += "Stunning ";
						break;
				}
				t.statusAilment = statEffect;
			} else {
				name += "Basic ";
			}
		}

		// Damage
		int s_dmg;
		if ((int)parts[CraftableItem.PART_MODIFIER].Modifier == 1) {
			s_dmg = Random.Range((int)dps, (int)(dps * DPSMULT));
			name += "Advanced ";
		} else if ((int)parts[CraftableItem.PART_MODIFIER].Modifier == 0) {
			s_dmg = Random.Range(1, (int)(dps * 0.75f));
			name += "Remote ";
		} else {
			s_dmg = Random.Range(1, (int)(dps * DPSMULT));
		}

		// Firing Rate (# of bullets per second)
		float s_firingRate = Mathf.Round((dps / s_dmg) * 10f) / 10f;

		t.attributes.Add(new ModifyingAttribute(Stat.Damage, s_dmg));
		t.attributes.Add(new ModifyingAttribute(Stat.Range, s_range));
		t.attributes.Add(new ModifyingAttribute(Stat.FiringRate, s_firingRate));

		t.toolTipMessage = "DPS: " + (s_dmg * s_firingRate) + t.toolTipMessage;

		/*
		 *	Stats Generation
		 */

		t.componentName = name + "Turret";
		t.componentType = BaseTower.TOWER_TURRET;
		t.level = (int)avgLevel;

		g.SetActive(false);
		t.transform.parent = partsRoot;
		return t;
	}

	public TowerComponent GenerateComponent(int type, int level) {
		if (type < 0 || type >= BaseTower.TOWER_COMPLETE)
			return null;

		if (type == BaseTower.TOWER_TURRET) {
			int chStem = Random.Range(0, stems.Length);
			int chMissileSource = Random.Range(0, missileSources.Length);
			int chBarrel = Random.Range(0, barrels.Length);
			int chSpinner = Random.Range(0, spinningParts.Length);

			// Turret Base
			GameObject g = GameObject.Instantiate(turretBase, buildSpot, Quaternion.identity) as GameObject;

			// Stem
			TowerStem tStem = GameObject.Instantiate(stems[chStem], buildSpot, Quaternion.identity) as TowerStem;
			tStem.transform.parent = g.transform;
			Vector3 stemOffest = tStem.baseNextComponentPosition;

			// Spinner Base
			GameObject gSpin = GameObject.Instantiate(spinningBase, buildSpot + stemOffest, Quaternion.identity) as GameObject;
			gSpin.transform.parent = g.transform;

			// Missile Source
			GameObject gMiss = GameObject.Instantiate((GameObject)missileSources[chMissileSource], buildSpot + stemOffest, Quaternion.identity) as GameObject;
			gMiss.transform.parent = g.transform;
			gMiss.name = "MissileSource";

			// Barrel
			GameObject gBarr = GameObject.Instantiate((GameObject)barrels[chBarrel], buildSpot + stemOffest, Quaternion.identity) as GameObject;
			gBarr.transform.parent = gMiss.transform;	// Barrel is a CHILD of missileSource

			// Spinning Thing
			int numSpinners = Random.Range(2, 7);	// 2 - 6 possible spinners
			float angle = (360 / numSpinners) * Mathf.Deg2Rad;
			for (int i = 0; i < numSpinners; i++) {
				Vector3 offset = new Vector3(Mathf.Cos(i * angle) * 2, 1, Mathf.Sin(i * angle) * 2);
				GameObject gSpinner = GameObject.Instantiate((GameObject)spinningParts[chSpinner], buildSpot + stemOffest + offset, Quaternion.identity) as GameObject;
				gSpinner.transform.parent = gSpin.transform;
			}

			/*
			 *	Stats Generation
			 */
			TowerTurret t = g.GetComponent<TowerTurret>();
			string name = "";

			// Range
			float s_range;
			s_range = BaseTower.JiggleStat(BaseTower.BASE_RANGE, 0.15f);
			s_range = Mathf.Round(s_range * 10) / 10f;

			// Determine DPS for the current level
			float dps = (SpawnScheme.HEALTH_COEFF * (1 + level * SpawnScheme.HEALTH_DPSMULTIPLIER)) / ((BaseTower.BASE_RANGE * 2) * PASSESTOKILL);
			float DPSMULT = 2f;

			// Add Status Ailment
			if (Random.Range(0, 4) == 0) {
				dps *= 0.65f;

				int i = Random.Range(0, Ailment.STUN + 1);
				string[] s = {"Burn", "Slow", "Stun"};
				GameObject statEffect = Resources.Load("Prefabs/StatusAilments/" + s[i], typeof(GameObject)) as GameObject;
				t.missile = Resources.Load("Prefabs/Towers/Turret Parts/MissilesStatus/" + s[i] + "Missile", typeof(GameObject)) as GameObject;
				switch (i) {
					case Ailment.BURN:
						//statEffect.GetComponent<Burn>().damage = 2;
						name += "Incendiary ";
						break;
					case Ailment.SLOW:
						//statEffect.GetComponent<Slow>().slowPercentage = 0.5f;
						name += "Slowness ";
						break;
					case Ailment.STUN:
						name += "Stunning ";
						break;
				}
				t.statusAilment = statEffect;
			} else {
				// No Status Ailment
				int chMissile = Random.Range(0, missiles.Length);
				t.missile = (GameObject)missiles[chMissile];
			}

			// Damage
			int s_dmg = Random.Range(1, (int)(dps * DPSMULT));	// Multiply to allow for FiringRate : (0, 1]

			// Firing Rate (# of bullets per second)
			float s_firingRate = Mathf.Round((dps / s_dmg) * 10f) / 10f;

			t.attributes.Add(new ModifyingAttribute(Stat.Damage, s_dmg));
			t.attributes.Add(new ModifyingAttribute(Stat.Range, s_range));
			t.attributes.Add(new ModifyingAttribute(Stat.FiringRate, s_firingRate));

			t.toolTipMessage = "DPS: " + (s_dmg * s_firingRate) + t.toolTipMessage;

			/*
			 *	Name Generation
			 */

			string[] n_damage = {"Advanced", "Prototype", "Overcharged", "Energized"};			// Damage
			string[] n_firing = {"Capacitor"};	// ROF
			if (s_firingRate < 0) {
				name += n_damage[Random.Range(0, n_damage.Length)];
			} else {
				name += n_firing[Random.Range(0, n_firing.Length)];
			}
			name += " Turret";

			t.componentName = name;
			t.componentType = BaseTower.TOWER_TURRET;
			t.level = level;

			g.SetActive(false);
			t.transform.parent = partsRoot;
			return t;
		} else if (type == BaseTower.TOWER_BASE) {
			int part = Random.Range(0, _towerParts[type].Length);
			GameObject g = GameObject.Instantiate(_towerParts[type][part].gameObject, buildSpot, Quaternion.identity) as GameObject;

			TowerComponent t = g.GetComponent<TowerComponent>();
			t.componentName = _towerParts[type][part].gameObject.name;
			t.componentType = BaseTower.TOWER_BASE;
			t.level = level;

			g.SetActive(false);
			t.transform.parent = partsRoot;
//			Debug.Log("Base After Count: " + t.attributes.Count);
			return t;
		}

		return null;
	}

//	public TowerComponent TowerClone(TowerComponent t) {
//		// Instantiate the Game Object
//		TowerComponent towerInstance = GameObject.Instantiate(t, buildSpot, Quaternion.identity) as TowerComponent;
//
//		if (t.componentType == BaseTower.TOWER_BASE) {
//			// Do Nothing.
//		} else if (t.componentType == BaseTower.TOWER_TURRET) {
//			// Copy the PREFAB's ModifyingAttributes to the INSTANCE
//			foreach (ModifyingAttribute m in ((TowerTurret) t).attributes) {
//				((TowerTurret)towerInstance).attributes.Add(m);
//			}
//		}
//
//		towerInstance.gameObject.SetActive(false);
//		return towerInstance;
//	}
}
