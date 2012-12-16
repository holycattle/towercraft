using UnityEngine;
using System.Collections;

public class Tank : SpawnScheme {
	// Distance to Kill Constant (Do not change)
	// -Doesnt seem to make much of a difference.
	public const int DIST_TO_KILL = 64;

	//
	public const float LEVELCONST_MULT = 1.0f;

	// Number of mobs to spawn
	public const int MOBCOUNT_MIN = 12;
	public const int MOBCOUNT_MAX = 40;
	public const int MOBCOUNT_MEDIAN = (int)((MOBCOUNT_MIN + MOBCOUNT_MAX) / 2);

	// Health Multiplier
	public const float HEALTHMULT_MIN = 0.6f;
	public const float HEALTHMULT_MAX = 2.5f;

	// Number of Bases to Drop Per Wave
	public const int BASESDROP_MIN = 2;
	public const int BASESDROP_MAX = 4;

	// Number of Turrets to Drop Per Wave
	public const int TURRETDROP_MIN = 1;
	public const int TURRETDROP_MAX = 1;

	// Number of Weapons to Drop Per Wave
	public const int WEAPONDROP_MIN = 1;
	public const int WEAPONDROP_MAX = 1;

	// Number of Craftable Parts to Drop Per Wave
	public const int CRAFTDROP_MIN = 4;
	public const int CRAFTDROP_MAX = 8;

	// Droppable Item Types
	public const int BASEDROP = 0;
	public const int TURRETDROP = 1;
	public const int WEAPONDROP = 2;
	public const int CRAFTDROP = 3;
	public const int DROPPABLETYPES = 4;

	public Tank (LevelController gameController, GameObject[] mobs, float cost, int waveNumber, int resistanceType) : base(gameController, mobs, cost, waveNumber) {
		Debug.Log("---Scheme = Tank---");

		Debug.Log("Cost: " + cost);
		Debug.Log("Wave#: " + waveNumber);

		int numberOfMobs = Random.Range(MOBCOUNT_MIN, MOBCOUNT_MAX + 1);
		float singleMobCost = (float)cost / numberOfMobs;

		Debug.Log("# Mobs: " + numberOfMobs);
		Debug.Log("1 Mob Cost: " + singleMobCost);

		// Create the Scheme
		MobType enemyType = MobType.Tank;
		float moveSpeed = UnityEngine.Random.Range(MIN_SPEED, MAX_SPEED);
		float healthMultiplier = Random.Range(HEALTHMULT_MIN, HEALTHMULT_MAX);
		int health = (int)CalculateHP(moveSpeed, singleMobCost);
		health = (int)(health * healthMultiplier);

		Debug.Log("HPM: " + healthMultiplier);
		Debug.Log("HP: " + health);
		Debug.Log("Mspd: " + moveSpeed);

		float timeToKill = health / (DPS_CONSTANT * singleMobCost);
		float interval = timeToKill - ((DIST_TO_KILL - timeToKill * moveSpeed) / (moveSpeed * (numberOfMobs - 1)));

		Debug.Log("Int: " + interval);

		Item[][] itemDrops = GenerateItemDropArray(numberOfMobs, waveNumber);

		for (int i = 0; i < numberOfMobs; i++) {
			// Choose which mob to spawn.
			GameObject g = (GameObject)mobTable[enemyType.ToString()]; // Optimized assigning of new mob by using a Hashtable
			_spawnScheme.Add(new MobSpawn(g, itemDrops[i], interval, moveSpeed, health, resistanceType, UnityEngine.Random.Range(0.5f, 1f)));
		}
	}

	protected Item[][] GenerateItemDropArray(int numMobs, int waveNumber) {
		int[] droppables = new int[4];
		droppables[0] = Random.Range(BASESDROP_MIN, BASESDROP_MAX + 1);
		droppables[1] = Random.Range(TURRETDROP_MIN, TURRETDROP_MAX + 1);
		droppables[2] = Random.Range(WEAPONDROP_MIN, WEAPONDROP_MAX + 1);
		droppables[3] = Random.Range(CRAFTDROP_MIN, CRAFTDROP_MAX + 1);
		Debug.Log("Drops: " + droppables[0] + "/" + droppables[1] + "/" + droppables[2] + "/" + droppables[3]);
		Item[][] drops = new Item[numMobs][];

		float itemLevel = waveNumber * LEVELCONST_MULT + WaveController.WAVESTART_PERMOB_COST;

		int mobNumber;	// Which mob is going to drop the item
		for (int dropCounter = 0; dropCounter < droppables.Length; dropCounter++) {
			for (int i = 0; i < droppables[dropCounter]; i++) {
				Item toDrop = null;
				switch (dropCounter) {
					case BASEDROP:
						toDrop = new TowerItem(BaseTower.TOWER_BASE, itemLevel);
						break;
					case TURRETDROP:
						toDrop = new TowerItem(BaseTower.TOWER_TURRET, itemLevel);
						break;
					case WEAPONDROP:
						toDrop = WeaponItem.RandomItem(itemLevel);
						break;
					case CRAFTDROP:
						toDrop = new CraftableItem(itemLevel);
						break;
				}

				mobNumber = Random.Range(0, numMobs);
				if (drops[mobNumber] == null) {
					drops[mobNumber] = new Item[1];
					drops[mobNumber][0] = toDrop;
				} else {
					Item[] temp = drops[mobNumber];
					drops[mobNumber] = new Item[temp.Length + 1];
					int o = 0;
					for (; o < temp.Length; o++) {
						drops[mobNumber][o] = temp[o];
					}
					drops[mobNumber][o] = toDrop;
				}
			}
		}
		return drops;
	}

	protected int CalculateHP(float movespeed, float level) {
		float hp = (ComponentGenerator.Get().PASSESTOKILL * (DPS_CONSTANT * level) * BaseTower.BASE_RANGE) / movespeed;
		return (int)hp;
	}
}
