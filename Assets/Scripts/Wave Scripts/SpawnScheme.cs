using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SpawnScheme {
	// DPS Constant
	public const float DPS_CONSTANT = 5;

	// Movespeed Range
	public const int MIN_SPEED = 8;
	public const int MAX_SPEED = 24;

	//maxHealth coefficient and multiplier
	public const int HEALTH_COEFF = 200; 			//this determines the scale of the HP
	public const float HEALTH_MULTIPLIER = 0.4f; 	//every wave, health increases by current_health * HEALTH_MULTIPLIER
	public const int HEALTH_DPSCOEFF = 100;
	public const float HEALTH_DPSMULTIPLIER = 0.1f;

	//
	private LevelController _levelController;
	protected List<MobSpawn> _spawnScheme;
	public float _timeSinceLastSpawn;

	// Mob Lists
	public Hashtable mobTable = new Hashtable();
	public Transform mobRoot = null;

	public SpawnScheme (LevelController gameController, GameObject[] mobs, float cost, int waveNumber) {
		//Debug.Log("SpawnScheme mob type 0 = " + mobs[0].ToString());
		//initialize mobTable - see WaveController parameters in Unity Editor for details of what each "mobs" index represent
		mobTable["Creepling"] = mobs[1];
		mobTable["Tank"] = mobs[3];
		mobTable["Speedster"] = mobs[2];

		// Get Controllers
		_levelController = gameController;

		//
		_spawnScheme = new List<MobSpawn>();

		_timeSinceLastSpawn = 0;

		Debug.Log("WAVENUMBER: " + waveNumber);

		// Mob Root
		mobRoot = GameObject.Find("[Root]Mobs").transform;
	}
	
	public virtual bool Update() {
		if (_spawnScheme.Count > 0) {
			while (_spawnScheme.Count > 0 && _spawnScheme[0].WaitTime <= _timeSinceLastSpawn) {
				Vector3 offset = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f));

				// Spawn the Mob and Set Parent
				GameObject g = _spawnScheme[0].Spawn(_levelController.mobSpawnPoint + offset, Quaternion.identity);
				g.transform.parent = mobRoot;

				// Procedurally assign new Enemy entity maxLife based on moveSpeed
				BaseEnemy m = g.GetComponent<BaseEnemy>();
				m.MotionPath = _levelController.MotionPath; // set enemy path to path determined by A* search

				// Update Spawn Timer
				_timeSinceLastSpawn -= _spawnScheme[0].WaitTime;
				_spawnScheme.RemoveAt(0);
			}
			_timeSinceLastSpawn += Time.deltaTime;
			return true;
		}
		return false;
	}

	public static MobType determineEnemyType(float moveSpeed) {
		if (moveSpeed >= MIN_SPEED && moveSpeed < MIN_SPEED + 3)
			return MobType.Tank;
		else if (moveSpeed >= MIN_SPEED + 3 && moveSpeed < MIN_SPEED + 6)
			return MobType.Creepling;
		else
			return MobType.Speedster;
	}

	public LevelController getLevelController() {
		return _levelController;
	}
	
	protected class MobSpawn {
		private GameObject _mobToSpawn; // Mob to spawn at this instance
		private float _spawnInterval;		// How much time to wait before spawning this mob

		// Mob's Stats
		private Item[] mobDrops;
		private float mobMoveSpeed;		// Mob's Move Speed
		private int mobHealth;			// Mob's Health
		private int resistanceType;
		private float resistanceAmt;

		public MobSpawn (GameObject g, Item[] items, float spawnInterval, float mspd, int hp, int resistanceType, float resistanceAmt) {
			_mobToSpawn = g;
			_spawnInterval = spawnInterval;

			// Set Stats
			mobDrops = items;
			mobMoveSpeed = mspd;
			mobHealth = hp;
			this.resistanceType = resistanceType;
			this.resistanceAmt = resistanceAmt;
		}

		public GameObject Spawn(Vector3 v, Quaternion q) {
			GameObject g = GameObject.Instantiate(_mobToSpawn, v, q) as GameObject;
			BaseEnemy m = g.GetComponent<BaseEnemy>();

			// Set Stats
			m.MoveSpeed = mobMoveSpeed;
			m.maxLife = mobHealth;
			switch (resistanceType) {
				case BaseEnemy.BURN_TYPE:
					m.heatResistance = resistanceAmt;
					m.slowResistance = m.stunResistance = (1 - resistanceAmt) / 2;
					break;
				case BaseEnemy.FREEZE_TYPE:
					m.slowResistance = resistanceAmt;
					m.stunResistance = m.heatResistance = (1 - resistanceAmt) / 2;
					break;
				case BaseEnemy.STUN_TYPE:
					m.stunResistance = resistanceAmt;
					m.heatResistance = m.slowResistance = (1 - resistanceAmt) / 2;
					break;
			}

			m.drops = mobDrops;

			return g;
		}
	
		public float WaitTime {
			get { return _spawnInterval; }
		}
	}
}

