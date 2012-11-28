using UnityEngine;
using System.Collections;

public class Tank : SpawnScheme {

	public Tank (LevelController gameController, GameObject[] mobs, int cost, int m) : base(gameController, mobs, cost, m) {
		// Create the Scheme
		GameObject g = null;
		MobType enemyType = MobType.Speedster;
		
		while (cost > 0) {
			// Choose which mob to spawn.
			g = null;
			foreach (GameObject tg in mobs) {
				if (tg.GetComponent<BaseEnemy>().type == enemyType) {
					g = tg;
					break;
				}
			}

			float interval = ((1f / moveSpeed) * GetINTERVAL_COEFF);
			Debug.Log("Spawn Interval = " + interval.ToString());
//			interval = 16f;
			_spawnScheme.Add(new MobSpawn(g, interval));
			cost -= g.GetComponent<BaseEnemy>().WaveCost;
			Debug.Log(cost);
		}
	}
}