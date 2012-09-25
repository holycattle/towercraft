using UnityEngine;
using System.Collections;

public class Creepling : SpawnScheme {

	public Creepling (LevelController gameController, GameObject[] mobs, int cost) : base(gameController, mobs, cost) {
		// Create the Scheme
		while (cost > 0) {
			// Choose which mob to spawn.
			GameObject g = null;
			foreach (GameObject tg in mobs) {
				if (tg.GetComponent<BaseEnemy>().type == MobType.Creepling) {
					g = tg;
					break;
				}
			}
			float interval = 0.3f;

			_spawnScheme.Add(new MobSpawn(g, interval));
			cost -= g.GetComponent<BaseEnemy>().WaveCost;
		}
	}
}
