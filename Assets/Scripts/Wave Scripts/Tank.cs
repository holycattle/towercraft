using UnityEngine;
using System.Collections;

public class Tank : SpawnScheme {

	public Tank (LevelController gameController, GameObject[] mobs, int cost, int waveNumber) : base(gameController, mobs, cost, waveNumber) {
		//resistant to slow
		Debug.Log("Scheme = Tank");

		// Create the Scheme
		MobType enemyType = MobType.Tank;
		float moveSpeed = UnityEngine.Random.Range(MIN_SPEED, MIN_SPEED + 3);
		int health = (int)(((1f / moveSpeed) * HEALTH_COEFF) * (1 + (waveNumber * HEALTH_MULTIPLIER)));
		float interval = ((1f / moveSpeed) * GetINTERVAL_COEFF);

		while (cost > 0) {
			// Choose which mob to spawn.
			GameObject g = (GameObject)mobTable[enemyType.ToString()]; //optimized assigning of new mob by using a Hashtable
			_spawnScheme.Add(new MobSpawn(g, interval, moveSpeed, health, waveNumber, BaseEnemy.FREEZE_TYPE, UnityEngine.Random.Range(0, 1)));
			cost -= g.GetComponent<BaseEnemy>().WaveCost;
		}
	}
}
