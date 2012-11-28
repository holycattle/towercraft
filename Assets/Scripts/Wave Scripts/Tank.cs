using UnityEngine;
using System.Collections;

public class Tank : SpawnScheme {

	public Tank (LevelController gameController, GameObject[] mobs, int cost) : base(gameController, mobs, cost) {
		// Create the Scheme
		GameObject g = null;
		MobType enemyType = MobType.Tank;
		moveSpeed = UnityEngine.Random.Range(MIN_SPEED, MIN_SPEED+3);
		while (cost > 0) {
			// Choose which mob to spawn.
			g = null;
			g = (GameObject)mobTable[enemyType.ToString()]; //optimized assigning of new mob by using a Hashtable

			float interval = ((1f / moveSpeed) * GetINTERVAL_COEFF);
			Debug.Log("Spawn Interval = " + interval.ToString());
//			interval = 16f;
			_spawnScheme.Add(new MobSpawn(g, interval));
			cost -= g.GetComponent<BaseEnemy>().WaveCost;
			Debug.Log(cost);
		}
	}
}
