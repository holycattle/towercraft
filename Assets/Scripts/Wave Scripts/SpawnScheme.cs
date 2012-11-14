using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SpawnScheme {

	private LevelController _levelController;
	protected List<MobSpawn> _spawnScheme;
	private float _timeSinceLastSpawn;
	protected float moveSpeed;

	public SpawnScheme (LevelController gameController, GameObject[] mobs, int cost) {
		_levelController = gameController;
		_spawnScheme = new List<MobSpawn>();

		moveSpeed = UnityEngine.Random.Range(5, 12);
//		moveSpeed = 6;
		Debug.Log("SpawnScheme moveSpeed : " + moveSpeed);
		_timeSinceLastSpawn = 0;
	}
	
	public bool Update() {
		if (_spawnScheme.Count > 0) {
			while (_spawnScheme.Count > 0 && _spawnScheme[0].WaitTime <= _timeSinceLastSpawn) {
				Vector3 offset = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f));

				GameObject g = _spawnScheme[0].Spawn(_levelController.mobSpawnPoint + offset, Quaternion.identity);

				BaseEnemy m = g.GetComponent<BaseEnemy>();
				m.moveSpeed = (int)moveSpeed;
				m.MotionPath = _levelController.MotionPath; //set enemy path to path determined by A* search

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

