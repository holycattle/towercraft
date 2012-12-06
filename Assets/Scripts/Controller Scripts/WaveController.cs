using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveController : MonoBehaviour {
	protected float PASSESTOKILL_ADDER = 2f;
	protected float PASSESTOKILL_ADDER_GROWVALUE = 1.5f;
	private const int SPEEDSTER = 0;
	private const int CREEPLING = 1;
	private const int TANK = 2;
	private const int ASSORTED = 3;
	
	//change this once we've come up with official names
	private const string SPEEDSTER_NAME = "Speedster";
	private const string CREEPLING_NAME = "Creepling";
	private const string TANK_NAME = "Tank";
	private const int MIN_SPEED = 2;
	private const int MAX_SPEED = 12;
	private const int WAVESTART_COST = 128;
	private const float WAVE_INCREASE = 1.15f;
	private const int WAVE_INTERVAL = 60; // In seconds
	
	public int incomingWave;
	public int incomingWaveResistanceType;
	public GameObject[] mobs; // Mobs to choose from.
	//public Hashtable mobTable; 

	private LevelController _gameController;
	private SpawnScheme _spawnScheme;		// Current Wave Scheme
	public int _waveNumber;
	private int _nextWaveCost;
	public float _timeTillNextWave;
	public bool _waveActive;

	void Start() {
		_gameController = GetComponent<LevelController>();
		_nextWaveCost = WAVESTART_COST;
		_waveNumber = 0;
		_timeTillNextWave = 120f;
		incomingWaveResistanceType = UnityEngine.Random.Range(BaseEnemy.BURN_TYPE, BaseEnemy.STUN_TYPE + 1);
//		incomingWave = UnityEngine.Random.Range(SPEEDSTER, ASSORTED + 1);
//		incomingWave = CREEPLING;
		_waveActive = false;
	}

	void Update() {
		if (_waveActive) {
			if (!_spawnScheme.Update()) {
				// Check if there are still enemies on the board.
				if (GameObject.FindGameObjectWithTag("Enemy") == null) {
					_waveActive = false;
					_timeTillNextWave = WAVE_INTERVAL;
					incomingWaveResistanceType = UnityEngine.Random.Range(BaseEnemy.BURN_TYPE, BaseEnemy.STUN_TYPE + 1);
//					incomingWave = UnityEngine.Random.Range(SPEEDSTER, ASSORTED + 1);
					incomingWave = CREEPLING;
//					Debug.Log("incoming resistance = " + incomingWaveResistanceType.ToString());
				}
			}
		} else {
			_timeTillNextWave -= Time.deltaTime;
			if (Input.GetKeyDown(KeyCode.N)) {
				_timeTillNextWave = 0;
				CameraController c = GameObject.Find("Minimap Camera").GetComponent<CameraController>();
				if (c.minimapCam.enabled)
					c.SetOverviewCamera(false);
			}
			if (_timeTillNextWave <= 0) {
				_timeTillNextWave = 0;
				CameraController c = GameObject.Find("Minimap Camera").GetComponent<CameraController>();
				if (c.minimapCam.enabled) {
					c.SetOverviewCamera(false);
				}
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
		Debug.Log("Creating new: P2K = " + ComponentGenerator.Get().PASSESTOKILL);

		_waveNumber++;
		_nextWaveCost = (int)(_nextWaveCost * WAVE_INCREASE);
		_waveActive = true;

		// Create the Spawn Scheme
		switch (incomingWave) {
			case TANK:
				_spawnScheme = new Tank(_gameController, mobs, _nextWaveCost, _waveNumber, incomingWaveResistanceType);
				break;
			case CREEPLING:
				_spawnScheme = new Creepling(_gameController, mobs, _nextWaveCost, _waveNumber, incomingWaveResistanceType);
				break;
			case SPEEDSTER:
				_spawnScheme = new Speedster(_gameController, mobs, _nextWaveCost, _waveNumber, incomingWaveResistanceType);
				break;
			case ASSORTED:
				_spawnScheme = new Assorted(_gameController, mobs, _nextWaveCost, _waveNumber);
				break;
		}
		ComponentGenerator.Get().PASSESTOKILL += PASSESTOKILL_ADDER;
		PASSESTOKILL_ADDER *= PASSESTOKILL_ADDER_GROWVALUE;


		// Clear all items from the stage
		GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
		foreach (GameObject i in items) {
			Destroy(i);
		}

	}
	
	public int waveNumber {
		get { return _waveNumber;}
	}
	
	public string getNextWave() {
		string nw = "";
		
		switch (incomingWaveResistanceType) {
			case BaseEnemy.BURN_TYPE:
				nw += "Burn-resistant";
				break;
			case BaseEnemy.STUN_TYPE:
				nw += "Stun-resistant";
				break;
			case BaseEnemy.FREEZE_TYPE:
				nw += "Slow-resistant";
				break;
		}
		
		switch (incomingWave) {
			case TANK:
				nw += " " + TANK_NAME + "s";
				break;
			case CREEPLING:
				nw += " " + CREEPLING_NAME + "s";
				break;
			case SPEEDSTER:
				nw += " " + SPEEDSTER_NAME + "s";
				break;
			case ASSORTED:
				return "Random Wave!";
		}
		
		return nw;
	}
	
	public int TimeTillNextWavex100 {
		get { return (int)(_timeTillNextWave * 100); }
	}
}