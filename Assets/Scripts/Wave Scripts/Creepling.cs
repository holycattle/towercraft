using UnityEngine;
using System.Collections;

public class Creepling : SpawnScheme {

	public Creepling (LevelController gameController, GameObject[] mobs, int cost) : base(gameController, mobs, cost) {
		// Create the Scheme
		GameObject g = null;
		foreach (GameObject tg in mobs) {
			if (tg.GetComponent<BaseEnemy>().type == MobType.Creepling) {
				g = tg;
				break;
			}
		}

		_spawnScheme.Add(new MobSpawn(g, 0f));
		while (cost > 0) {
			// Choose which mob to spawn.
			g = null;
			foreach (GameObject tg in mobs) {
				if (tg.GetComponent<BaseEnemy>().type == MobType.Creepling) {
					g = tg;
					break;
				}
			}

			float interval = 12f / moveSpeed;				// Equation Simplified
			_spawnScheme.Add(new MobSpawn(g, interval));
			cost -= g.GetComponent<BaseEnemy>().WaveCost;
		}
	}
}
