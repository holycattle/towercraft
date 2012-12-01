using UnityEngine;
using System.Collections;

public class Speedster : SpawnScheme {

	public Speedster (LevelController gameController, GameObject[] mobs, int cost, int waveNumber) : base(gameController, mobs, cost, waveNumber) {
		Debug.Log("Scheme = Speedster");
		
		// Create the Scheme
		MobType enemyType = MobType.Speedster;
		float moveSpeed = UnityEngine.Random.Range(MAX_SPEED - 3, MAX_SPEED);
		int health = (int)(((1f / moveSpeed) * HEALTH_COEFF) * (1 + (waveNumber * HEALTH_MULTIPLIER)));
		float interval = ((1f / moveSpeed) * GetINTERVAL_COEFF);

		while (cost > 0) {
			// Choose which mob to spawn.
			GameObject g = (GameObject)mobTable[enemyType.ToString()]; //optimized assigning of new mob by using a Hashtable
				_spawnScheme.Add(new MobSpawn(g, interval, moveSpeed, health, waveNumber, BaseEnemy.BURN_TYPE, Random.Range(0f, 1f)));
			cost -= g.GetComponent<BaseEnemy>().WaveCost;
		}
	}
}
