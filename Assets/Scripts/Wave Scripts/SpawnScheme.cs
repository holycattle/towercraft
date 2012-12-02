using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SpawnScheme {
	private const int INTERVAL_COEFF = 7;
	
	// moveSpeed range - to be tweaked later on
	public const int MIN_SPEED = 3;
	public const int MAX_SPEED = 13;
	
	//maxHealth coefficient and multiplier
	public const int HEALTH_COEFF = 200; 			//this determines the scale of the HP
	public const float HEALTH_MULTIPLIER = 0.2f; 	//every wave, health increases by current_health * HEALTH_MULTIPLIER

	private LevelController _levelController;
//	private WaveController _waveController;
	protected List<MobSpawn> _spawnScheme;
	public float _timeSinceLastSpawn;

	// Mob Lists
	public Hashtable mobTable = new Hashtable();
	public Transform mobRoot = null;

	public SpawnScheme (LevelController gameController, GameObject[] mobs, int cost, int waveNumber) {
		//Debug.Log("SpawnScheme mob type 0 = " + mobs[0].ToString());
		//initialize mobTable - see WaveController parameters in Unity Editor for details of what each "mobs" index represent
		mobTable["Creepling"] = mobs[1];
		mobTable["Tank"] = mobs[3];
		mobTable["Speedster"] = mobs[2];

		// Get Controllers
		_levelController = gameController;

		//
		_spawnScheme = new List<MobSpawn>();

//		Debug.Log("SpawnScheme moveSpeed : " + moveSpeed);
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

	public int GetINTERVAL_COEFF {
		get { return INTERVAL_COEFF; }
	}
	
	public float GetTimeSinceLastSpawn {
		get { return _timeSinceLastSpawn; }
	}
	
	public LevelController getLevelController() {
		return _levelController;
	}
	
	protected class MobSpawn {
		private GameObject _mobToSpawn; // Mob to spawn at this instance
		private float _waitTime;		// How much time to wait before spawning this mob

		// Mob's Stats
		private float mobMoveSpeed;		// Mob's Move Speed
		private int mobHealth;			// Mob's Health
		private int mobLevel;			// Mob's Level
		private int resistanceType;
		private float resistanceAmt;
		public MobSpawn (GameObject g, float wait, float mspd, int hp, int level, int resistanceType, float resistanceAmt) {
			_mobToSpawn = g;
			_waitTime = wait;

			// Set Stats
			mobMoveSpeed = mspd;
			mobHealth = hp;
			mobLevel = level;
			this.resistanceType = resistanceType;
			this.resistanceAmt = resistanceAmt;
		}

		public GameObject Spawn(Vector3 v, Quaternion q) {
			GameObject g = GameObject.Instantiate(_mobToSpawn, v, q) as GameObject;
			BaseEnemy m = g.GetComponent<BaseEnemy>();

			// Set Stats
			m.MoveSpeed = mobMoveSpeed;
			m.maxLife = mobHealth;
			m.level = mobLevel;
			//resistanceType = BaseEnemy.BURN_TYPE;
			//resistanceAmt = 1;
			
			switch(resistanceType) {
				//remove int typecast later
				case BaseEnemy.BURN_TYPE:
					m.heatResistance = resistanceAmt;
					m.slowResistance = (1 - resistanceAmt)/2;
					m.stunResistance = (1 - resistanceAmt)/2;
					break;
				case BaseEnemy.FREEZE_TYPE:
					m.slowResistance = resistanceAmt;
					m.stunResistance = (1 - resistanceAmt)/2;
					m.heatResistance = (1 - resistanceAmt)/2;
					break;
				case BaseEnemy.STUN_TYPE:
					m.stunResistance = resistanceAmt;
					m.heatResistance = (1 - resistanceAmt)/2;
					m.slowResistance = (1 - resistanceAmt)/2;
					break;
			}
			return g;
		}
	
		public float WaitTime {
			get { return _waitTime; }
		}
	}
}

