using UnityEngine;
using System.Collections;

public class Assorted : SpawnScheme {
	MobType newEnemyType;
	int costLeft = 0;
	public Assorted (LevelController gameController, GameObject[] mobs, int cost) : base(gameController, mobs, cost) {
		costLeft = cost;
	}
	
	public override bool Update() {
		moveSpeed = UnityEngine.Random.Range(MIN_SPEED, MAX_SPEED);
		if(costLeft > 0) {
			newEnemyType = determineEnemyType();
			// Choose which mob to spawn.
			GameObject g = null;
			g = (GameObject)mobTable[newEnemyType.ToString()]; //optimized assigning of new mob by using a Hashtable
			Debug.Log("newEnemyType = " + newEnemyType.ToString() + " g = " + g.ToString() + " moveSpeed = " + moveSpeed);
			float interval = ((1f / moveSpeed) * GetINTERVAL_COEFF);
			Debug.Log("Spawn Interval = " + interval.ToString());

			_spawnScheme.Add(new MobSpawn(g, interval, moveSpeed));
			costLeft -= g.GetComponent<BaseEnemy>().WaveCost;
		}
		
		while (_spawnScheme.Count > 0 && _spawnScheme[0].WaitTime <= _timeSinceLastSpawn) {
			Vector3 offset = new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f));
			GameObject g = _spawnScheme[0].Spawn(getLevelController().mobSpawnPoint + offset, Quaternion.identity);
			BaseEnemy m = g.GetComponent<BaseEnemy>();
			//random moveSpeed generated from constructor
				
			m.moveSpeed = _spawnScheme[0].moveSpeed;
				
			//procedurally assign new Enemy entity maxLife based on moveSpeed
			WaveController waveController = getLevelController().GetComponent<WaveController>();
			m.maxLife = (int)(((1f / m.moveSpeed) * HEALTH_COEFF) * (1 + (waveController.waveNumber * HEALTH_MULTIPLIER)));
			
			m.MotionPath = getLevelController().MotionPath; //set enemy path to path determined by A* search
			_timeSinceLastSpawn -= _spawnScheme[0].WaitTime;
			_spawnScheme.RemoveAt(0);
		}

		_timeSinceLastSpawn += Time.deltaTime;

		return true;
		
	}
}
