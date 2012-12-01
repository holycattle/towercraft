using UnityEngine;
using System.Collections;

public class Assorted : SpawnScheme {

	public Assorted (LevelController gameController, GameObject[] mobs, int cost, int waveNumber) : base(gameController, mobs, cost, waveNumber) {
		Debug.Log("Scheme = Assorted");
		

		while (cost > 0) {
			float moveSpeed = UnityEngine.Random.Range(MIN_SPEED, MAX_SPEED);
			int health = (int)(((1f / moveSpeed) * HEALTH_COEFF) * (1 + (waveNumber * HEALTH_MULTIPLIER)));
			MobType enemyType = SpawnScheme.determineEnemyType(moveSpeed);
			float interval = ((1f / moveSpeed) * GetINTERVAL_COEFF);

			// Choose which mob to spawn.
			GameObject g = (GameObject)mobTable[enemyType.ToString()]; //optimized assigning of new mob by using a Hashtable
			
			if(enemyType == MobType.Tank) {
				_spawnScheme.Add(new MobSpawn(g, interval, moveSpeed, health, waveNumber, BaseEnemy.FREEZE_TYPE, Random.Range(0, 2)));
			} else if(enemyType == MobType.Creepling) {
				_spawnScheme.Add(new MobSpawn(g, interval, moveSpeed, health, waveNumber, BaseEnemy.STUN_TYPE, Random.Range(0, 2)));
			} else if(enemyType == MobType.Speedster) {
				_spawnScheme.Add(new MobSpawn(g, interval, moveSpeed, health, waveNumber, BaseEnemy.BURN_TYPE, Random.Range(0, 2)));
			}
			
			cost -= g.GetComponent<BaseEnemy>().WaveCost;
		}
	}
}
