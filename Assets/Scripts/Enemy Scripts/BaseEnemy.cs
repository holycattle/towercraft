using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseEnemy : MonoBehaviour {
	//
	public const int AMMO_DROP = 3;
	public const int HEALTH_DROP = 3;

	// Damage Types
//	public const int COUNT_DMGTYPES = 4;		// Only used for Iteration
//	public static readonly string[] NAME_ARMORTYPES = {"Human", "Alien", "Vehicle", "Pure"};
//	public static readonly string[] NAME_DMGTYPES = {"Plasma", "Laser", "Ballistic", "Pure"};
//	public static readonly float[][] DMG_MATRIX = {new float[] {1   , 2   , 0.5f, 1},
//											new float[] {0.5f, 1   , 2   , 1},
//											new float[] {2   , 0.5f, 1   , 1},
//											new float[] {1   , 1   , 1   , 1}};
//	public const int DMG_PLASMA = 0;
//	public const int DMG_LASER = 1;
//	public const int DMG_BALLISTIC = 2;
//	public const int DMG_PURE = 3;

	// Constants
	public const float MOV_OFFSET = 0f;
//	public static Vector3 LIFE_OFFSET = new Vector3(0, 2f, 0);

	// References
	private LevelController _level;
	private PlayerController _player;
	private readonly GameObject AMMO_PREFAB = Resources.Load("Prefabs/Items/Ammo", typeof(GameObject)) as GameObject;
	private readonly GameObject HEALTH_PREFAB = Resources.Load("Prefabs/Items/Health", typeof(GameObject)) as GameObject;

	// Enemy Stats (Given Default values, but you have to set it in the game object.)
	public MobType type;
	private float moveSpeed;
	public int maxLife;
	public int waveCost;
	public int level;
	public int armorType;
	public Item[] drops;
	private int damage = 2;
	private int accuracy = (int)(0.16f * 100);
	private float firingInterval = 1f;
	private float range = 32;

	// Vary-ables
	private int _life;					// Current Life

	// Attacking
	private float _timeTillFire;

	/*
	 *	Movement
	 */
	// Rotation
	private float turnSpeed;			// Defines how long it takes to turn 90 degrees.
	private float totalTurn;			// [0, 1] How far I have turned.
	private Quaternion originalRot;		// Rotation if totalturn == 0

	// Path Following
	private List<Vector2> _path;	// List of waypoints to follow
	private int _activePoint;		// Current point in the path
	private Vector2 _offset;		// Random offset for movement

	void Start() {
		_activePoint = 1;
		_offset = new Vector2(Random.Range(-MOV_OFFSET, MOV_OFFSET), Random.Range(-MOV_OFFSET, MOV_OFFSET));
		_life = maxLife;

		UpdateStats();

		originalRot = transform.rotation;

		_player = GameObject.Find("Player").GetComponent<PlayerController>();
		_level = GameObject.Find(" GameController").GetComponent<LevelController>();

		// Fix Name
		if (gameObject.name.EndsWith("(Clone)")) {
			gameObject.name = gameObject.name.Substring(0, gameObject.name.Length - 7);
		}
	}

	void Update() {
		/*
		 *	Movement
		 */
		if (_activePoint == _path.Count) {
			// Dont do anything anymore.
			Destroy(this.gameObject);
			return;
		}

		Vector3 targetPos = new Vector3(_path[_activePoint].x + _offset.x, transform.position.y, _path[_activePoint].y + _offset.y);
		float dist = Vector3.Distance(targetPos, transform.position);

		if (dist < moveSpeed * Time.deltaTime) {
			transform.rotation = Quaternion.Slerp(originalRot, Quaternion.LookRotation(targetPos - transform.position), totalTurn);
//			iTween.RotateTo(gameObject, iTween.Hash("rotation", Quaternion.LookRotation(targetPos - transform.position).eulerAngles, "time", turnTime));
			transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);

			// Move the extra
			_activePoint++;
			_offset = new Vector2(Random.Range(-MOV_OFFSET, MOV_OFFSET), Random.Range(-MOV_OFFSET, MOV_OFFSET));
			if (_activePoint != _path.Count) {
				targetPos = new Vector3(_path[_activePoint].x + _offset.x, transform.position.y, _path[_activePoint].y + _offset.y);
				originalRot = transform.rotation;
			}

			totalTurn = 0;
		} else {
			transform.rotation = Quaternion.Slerp(originalRot, Quaternion.LookRotation(targetPos - transform.position), totalTurn);
			transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
		}
		totalTurn += turnSpeed * Time.deltaTime;

		if (_path != null) {
			Vector3 p;
			int i = 0;
			for (; i < _path.Count - 1; i++) {
				p = new Vector3(_path[i].x, 1, _path[i].y);

				Debug.DrawLine(p, new Vector3(_path[i + 1].x, 1, _path[i + 1].y), Color.cyan);
				Debug.DrawLine(p, p + Vector3.up * 5, Color.cyan);
//				Debug.DrawLine(p, p + (Vector3.up + new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f))) * 5, Color.cyan);
			}
			p = new Vector3(_path[i].x, 1, _path[i].y);
			Debug.DrawLine(p, p + Vector3.up * 5);
		}


		/*
		 *	Attacking the Player
		 */
		if (Vector3.Distance(transform.position, _player.transform.position) <= range) {
			if (_timeTillFire <= 0) {
				if (Random.Range(0, 100) < accuracy) {
					// Hit
					_player.AddLife(-damage);
				}
				_timeTillFire += firingInterval;
			}
		}

		if (_timeTillFire > 0) {
			_timeTillFire -= Time.deltaTime;
		}
	}

	public void UpdateStats() {
		// Set Turn Speed
		turnSpeed = (moveSpeed / ((Mathf.Sqrt(2) / 2) * LevelController.TILE_SIZE)) * 2;
	}

	public void PathUpdate() {
		_path = _level.RecalculatePath(GridPosition);
		_activePoint = 2;
	}

	public void Damage(int amt, int damageType) {
		AddLife(-Mathf.Max(1, (int)(amt * DamageType.DMG_MATRIX[damageType][armorType])));
	}

	private void AddLife(int amt) {
		Debug.Log("Adding Life:" + amt);

		_life += amt;
		if (_life <= 0)
			Kill();
	}

	public void Kill() {
		Destroy(this.transform.gameObject);
		
		if (drops != null) {
			foreach (Item i in drops) {
				i.ItemInstantiate(transform.position, Quaternion.identity);
			}
		}

		if (UnityEngine.Random.Range(0, 10) < AMMO_DROP + HEALTH_DROP) {
			if (Random.Range(0, AMMO_DROP + HEALTH_DROP) < AMMO_DROP) {
				GameObject g = Instantiate(AMMO_PREFAB, transform.position, Quaternion.identity) as GameObject;
				g.GetComponent<AmmoScript>().amount = Random.Range(20, 40);
			} else {
				GameObject g = Instantiate(HEALTH_PREFAB, transform.position, Quaternion.identity) as GameObject;
				g.GetComponent<HealthScript>().health = Random.Range(5, 20);
			}
		}
	}

	private int CalculateDirection(float diff) {
		if (diff > 0)
			return 1;
		else if (diff < 0)
			return -1;
		return 0;
	}

	public Vector2 GridPosition {
		get { return _level.GridConvert(transform.position); }
	}

	public int Life {
		get { return _life; }
	}

	public string Name {
		get { return gameObject.name; }
	}

	public int WaveCost {
		get { return waveCost; }
	}

	public float MoveSpeed {
		get { return moveSpeed; }
		set {
			this.moveSpeed = value;
			UpdateStats();
		}
	}
	
	public string GetHUDName() {
		return DamageType.NAME_ARMORTYPES[armorType] + " " + Name;
	}

	public List<Vector2> MotionPath {
		set { _path = value; }
	}
}

public class DamageType {
	public const int COUNT_DMGTYPES = 4;		// Only used for Iteration
	public static readonly string[] NAME_ARMORTYPES = {"Human", "Alien", "Vehicle", "Pure"};
	public static readonly string[] NAME_DMGTYPES = {"Plasma", "Laser", "Ballistic", "Pure"};
	public static readonly float[][] DMG_MATRIX = {new float[] {1   , 2   , 0.5f, 1},
											new float[] {0.5f, 1   , 2   , 1},
											new float[] {2   , 0.5f, 1   , 1},
											new float[] {1   , 1   , 1   , 1}};
	public const int DMG_PLASMA = 0;
	public const int DMG_LASER = 1;
	public const int DMG_BALLISTIC = 2;
	public const int DMG_PURE = 3;
}

public enum MobType {
//	Basic,
	Creepling,
	Tank,
	Speedster
//	Healer,
//	Boss
}