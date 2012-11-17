using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveController : MonoBehaviour {
	private const int WAVESTART_COST = 64;
	private const float WAVE_INCREASE = 1.5f;
	private const int WAVE_INTERVAL = 10; // In seconds

	public GameObject[] mobs; // Mobs to choose from.

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

	private void NextWave() {
		// Create the Spawn Scheme
		_spawnScheme = new Creepling(_gameController, mobs, _nextWaveCost);

		_waveNumber++;
		_nextWaveCost = (int)(_nextWaveCost * WAVE_INCREASE);
		_waveActive = true;
	}
	
	public int waveNumber {
		get { return _waveNumber;}
	}
	
	public int TimeTillNextWavex100 {
		get { return (int)(_timeTillNextWave * 100); }
	}
}