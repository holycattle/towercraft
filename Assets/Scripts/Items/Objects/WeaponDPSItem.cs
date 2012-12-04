using UnityEngine;
using System.Collections;

public class WeaponDPSItem : WeaponItem {

	public const int PASSESTOKILL = 2;

	// Stat Modifiers
	public int damage;
	public float firingRate;
	public int energyConsumption;
	public GameObject statusEffect;

	public WeaponDPSItem (int level) : base(DPS) {
		weaponName = "";

		float dps = (SpawnScheme.HEALTH_COEFF * (1 + level * SpawnScheme.HEALTH_MULTIPLIER)) / ((BaseTower.BASE_RANGE * 2) * PASSESTOKILL);
		dps = BaseTower.JiggleStat(dps, 0.1f);
		int randomDPSMultiplier = Random.Range(1, 2);
		dps *= randomDPSMultiplier;


		// Add Status Ailment
		if (Random.Range(0, 3) == 0) {
			dps -= (dps / randomDPSMultiplier) / 2f;

			int i = Random.Range(0, Ailment.STUN + 1);
			string[] s = {"Burn", "Slow", "Stun"};

			statusEffect = Resources.Load("Prefabs/StatusAilments/" + s[i], typeof(GameObject)) as GameObject;
			switch (i) {
				case Ailment.BURN:
					weaponName += "Incendiary ";
					break;
				case Ailment.SLOW:
					weaponName += "Cryogenic ";
					break;
				case Ailment.STUN:
					weaponName += "Stasis ";
					break;
			}
		}

		// Damage
		// -Multiply with DPSMULT so FiringRate : (0, 1]
		int s_dmg = Random.Range(1, (int)(dps / 2));

		// Firing Rate (# of bullets per second)
		float s_firingRate = Mathf.Round((dps / s_dmg) * 10f) / 10f;

		if (weaponName == "") {
			if (s_dmg < 0.25f * dps)
				weaponName += "Repeater ";
			else
				weaponName += "Overcharged ";
		}

		weaponName += "Barrel";
		weaponName += " v" + Random.Range(1, 10) + "." + Random.Range(1, 10);

		damage = s_dmg;
		firingRate = s_firingRate;
		energyConsumption = randomDPSMultiplier;
	}

	public override string GetTooltip() {
		return weaponName + "\n" +
			"DPS: " + damage * firingRate + "\n" +
			"Damage: +" + damage + "\n" +
			"Firing Rate: +" + firingRate + "\n" +
			"Energy Consumption: " + energyConsumption;
	}
}
