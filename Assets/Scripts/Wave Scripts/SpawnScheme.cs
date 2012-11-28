using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SpawnScheme {
	private const int INTERVAL_COEFF = 12;
	
	//moveSpeed range - to be tweaked later on
	private const int MIN_SPEED = 2;
	private const int MAX_SPEED = 12;
	
	//maxHealth coefficient and multiplier
	private const int HEALTH_COEFF = 64; //this determines the scale of the HP
	private const float HEALTH_MULTIPLIER = 0.2f; //every wave, health increases by current_health * HEALTH_MULTIPLIER
	
	private LevelController _levelController;
	protected List<MobSpawn> _spawnScheme;
	private float _timeSinceLastSpawn;
	protected float moveSpeed;

	// Mob Root
	public Transform mobRoot;

	public SpawnScheme (LevelController gameController, GameObject[] mobs, int cost, int m) {
		_levelController = gameController;
		_spawnScheme = new List<MobSpawn>();

		moveSpeed = m;
		Debug.Log("SpawnScheme moveSpeed : " + moveSpeed);
		_timeSinceLastSpawn = 0;

		mobRoot = GameObject.Find("[Root]Mobs").transform;
	}
	
	public bool Update() {
		if (_spawnScheme.Count > 0) {
			while (_spawnScheme.Count > 0 && _spawnScheme[0].WaitTime <= _timeSinceLastSpawn) {
				Vector3 offset = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f));

				// Spawn the Mob and Set Parent
				GameObject g = _spawnScheme[0].Spawn(_levelController.mobSpawnPoint + offset, Quaternion.identity);
				g.transform.parent = mobRoot;

				BaseEnemy m = g.GetComponent<BaseEnemy>();
				//random moveSpeed
				m.moveSpeed = (int)moveSpeed;
				
				//procedurally assign new Enemy entity maxLife based on moveSpeed
				WaveController waveController = _levelController.GetComponent<WaveController>();
				m.maxLife = (int)(((1f / moveSpeed) * HEALTH_COEFF) * (1 + (waveController.waveNumber * HEALTH_MULTIPLIER)));
				
				m.MotionPath = _levelController.MotionPath; //set enemy path to path determined by A* search

				_timeSinceLastSpawn -= _spawnScheme[0].WaitTime;
				_spawnScheme.RemoveAt(0);
			}

			_timeSinceLastSpawn += Time.deltaTime;

			return true;
		}
		return false;
	}
	
	public int GetINTERVAL_COEFF {
		get { return INTERVAL_COEFF; }
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

