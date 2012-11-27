using UnityEngine;
using System.Collections;

public class Assorted : SpawnScheme {

	public Assorted (LevelController gameController, GameObject[] mobs, int cost, int m) : base(gameController, mobs, cost, m) {
		/*
		// Create the Scheme
		GameObject g = null;
		MobType enemyType = determineEnemyType();

		while (cost > 0) {
			// Choose which mob to spawn.
			g = null;
			foreach (GameObject tg in mobs) {
				if (tg.GetComponent<BaseEnemy>().type == enemyType) {
					g = tg;
					break;
				}
			}

			float interval = ((1f / moveSpeed) * GetINTERVAL_COEFF) - genRandomIntervalFactor();
			Debug.Log("Spawn Interval = " + interval.ToString());
//			interval = 16f;
			_spawnScheme.Add(new MobSpawn(g, interval));
			cost -= g.GetComponent<BaseEnemy>().WaveCost;

			// Cycle through mob types
			/*if (enemyType == MobType.Tank)
				enemyType = MobType.Creepling;
			else if (enemyType == MobType.Creepling)
				enemyType = MobType.Speedster;
			else
				enemyType = MobType.Tank;*/
			/*enemyType = determineEnemyType();
		}*/
	}
}
