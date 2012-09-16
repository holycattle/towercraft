using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SpawnScheme {

	private GameController _gameController;
	protected List<MobSpawn> _spawnScheme;
	private float _timeSinceLastSpawn;

	public SpawnScheme (GameController gameController, GameObject[] mobs, int cost) {
		_gameController = gameController;
		_spawnScheme = new List<MobSpawn>();

		// Create the Scheme
//		int i = 0;
//		MobType[] types = Enum.GetValues(typeof(MobType)) as MobType[];
//		while (cost > 0) {
//			// Choose which mob to spawn.
//			GameObject g = null;
//			foreach (GameObject tg in mobs) {
//				if (tg.GetComponent<BaseEnemy>().type == types[i]) {
//					g = tg;
//					break;
//				}
//			}
//			float interval = 0.8f;
//
//			_spawnScheme.Add(new MobSpawn(g, interval));
//			cost -= g.GetComponent<BaseEnemy>().WaveCost;
//
//			i = (i + 1) % Enum.GetValues(typeof(MobType)).Length;
//		}

		_timeSinceLastSpawn = 0;
	}
	
	public bool Update() {
		if (_spawnScheme.Count > 0) {
			while (_spawnScheme.Count > 0 && _spawnScheme[0].WaitTime <= _timeSinceLastSpawn) {
				GameObject g = _spawnScheme[0].Spawn(
					new Vector3(_gameController.startXPosition + 2 + UnityEngine.Random.Range(-1f, 1f),
					2, _gameController.startYPosition - 2 + UnityEngine.Random.Range(-1f, 1f)),
					Quaternion.identity);

				BaseEnemy m = g.GetComponent<BaseEnemy>();
				m.Path = _gameController.Path;

				_timeSinceLastSpawn -= _spawnScheme[0].WaitTime;
				_spawnScheme.RemoveAt(0);
			}

			_timeSinceLastSpawn += Time.deltaTime;

			return true;
		}
		return false;
	}

	protected class MobSpawn {
		private GameObject _mobToSpawn; // Mob to spawn at this instance
		private float _waitTime;		// How much time to wait before spawning this mob

		public MobSpawn (GameObject g, float wait) {
			_mobToSpawn = g;
			_waitTime = wait;
		}

		public GameObject Spawn(Vector3 v, Quaternion q) {
			return GameObject.Instantiate(_mobToSpawn, v, q) as GameObject;
		}
	
		public float WaitTime {
			get { return _waitTime; }
		}
	}
}

