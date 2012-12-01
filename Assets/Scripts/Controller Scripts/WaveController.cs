using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveController : MonoBehaviour {
	private const int SPEEDSTER = 0;
	private const int CREEPLING = 1;
	private const int TANK = 2;
	private const int ASSORTED = 3;
	private const int MIN_SPEED = 2;
	private const int MAX_SPEED = 12;
	private const int WAVESTART_COST = 128;
	private const float WAVE_INCREASE = 1.5f;
	private const int WAVE_INTERVAL = 10; // In seconds

	public GameObject[] mobs; // Mobs to choose from.
	//public Hashtable mobTable; 

	private LevelController _gameController;
	private SpawnScheme _spawnScheme;		// Current Wave Scheme
	private int _waveNumber;
	private int _nextWaveCost;
	private float _timeTillNextWave;
	private bool _waveActive;

	void Start() {
		_gameController = GetComponent<LevelController>();
		_nextWaveCost = WAVESTART_COST;
		_waveNumber = 0;
		_timeTillNextWave = 5;
		_waveActive = false;
	}

	void Update() {
		if (_waveActive) {
			if (!_spawnScheme.Update()) {
				// Check if there are still enemies on the board.
				if (GameObject.FindGameObjectWithTag("Enemy") == null) {
					_waveActive = false;
					_timeTillNextWave = WAVE_INTERVAL;
				}
			}
		} else {
			_timeTillNextWave -= Time.deltaTime;
			if (_timeTillNextWave <= 0) {
				_timeTillNextWave = 0;
				NextWave();
			}
		}
	}
	
	public int GetMIN_SPEED {
		get { return MIN_SPEED;}
	}
	
	public int GetMAX_SPEED {
		get { return MAX_SPEED;}
	}
	
	public int GetMED_SPEED { //get median speed
		get { return MAX_SPEED / MIN_SPEED;}
	}
	
	private int determineEnemyType(int moveSpeed) {
		if (moveSpeed >= GetMIN_SPEED && moveSpeed < GetMIN_SPEED + 3) {
			return TANK;
		} else if (moveSpeed >= GetMIN_SPEED + 3 && moveSpeed < GetMIN_SPEED + 6) {
			return CREEPLING;
		} else
			return SPEEDSTER;
	}

	private void NextWave() {
		_waveNumber++;
		_nextWaveCost = (int)(_nextWaveCost * WAVE_INCREASE);
		_waveActive = true;

		// Create the Spawn Scheme

		// Note: Random.Range(x, y) Generates a random number from [x, y). Inclusive X, Exclusive Y.
		switch (UnityEngine.Random.Range(SPEEDSTER, ASSORTED + 1)) { //change this later to randomly go through all enemy types
//		switch (ASSORTED) {
			case TANK:
				_spawnScheme = new Tank(_gameController, mobs, _nextWaveCost, _waveNumber);
				break;
			case CREEPLING:
				_spawnScheme = new Creepling(_gameController, mobs, _nextWaveCost, _waveNumber);
				break;
			case SPEEDSTER:
				_spawnScheme = new Speedster(_gameController, mobs, _nextWaveCost, _waveNumber);
				break;
			case ASSORTED:
				_spawnScheme = new Assorted(_gameController, mobs, _nextWaveCost, _waveNumber);
				break;
		}
	}
	
	public int waveNumber {
		get { return _waveNumber;}
	}
	
	public int TimeTillNextWavex100 {
		get { return (int)(_timeTillNextWave * 100); }
	}
}