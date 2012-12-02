using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseEnemy : MonoBehaviour {
	//
	public const int AMMO_DROP = 5;
	public const int HEALTH_DROP = 5;

	// Constants
	public const float MOV_OFFSET = 2f;
	public const int BURN_TYPE = 0;
	public const int FREEZE_TYPE = 1;
	public const int STUN_TYPE = 2;
//	public static Vector3 LIFE_OFFSET = new Vector3(0, 2f, 0);

	// References
	private LevelController _level;
	private PlayerController _player;
	private static GameObject AMMO_PREFAB;
	private static GameObject HEALTH_PREFAB;

	// Enemy Stats (Given Default values, but you have to set it in the game object.)
	public MobType type;
	private float moveSpeed;
	public int maxLife;
	public int waveCost;
	public int level;
	public Item[] drops;
	private int damage = 2;
	private int accuracy = (int)(0.25f * 100);
	private float firingInterval = 1f;
	private float range = 32;

	//ailment resistance; domain = [0, 1]
	public float heatResistance;
	public float slowResistance;
	public float stunResistance;

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

		// TODO: Make this more optimal
//		if (ITEM_PREFAB == null)
//			ITEM_PREFAB = Resources.Load("Prefabs/Items/Item", typeof(GameObject)) as GameObject;
		if (HEALTH_PREFAB == null)
			HEALTH_PREFAB = Resources.Load("Prefabs/Items/Health", typeof(GameObject)) as GameObject;
		if (AMMO_PREFAB == null)
			AMMO_PREFAB = Resources.Load("Prefabs/Items/Ammo", typeof(GameObject)) as GameObject;
//		if (CRAFTABLE_PREFAB == null)
//			CRAFTABLE_PREFAB = Resources.Load("Prefabs/Items/Craftable", typeof(GameObject)) as GameObject;

		_player = GameObject.Find("Player").GetComponent<PlayerController>();
		_level = GameObject.Find(" GameController").GetComponent<LevelController>();
	}

	void Update() {
		/*
		 *	Movement
		 */
		if (_activePoint == _path.Count) {
			// Dont do anything anymore.
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

	public void AddLife(int amt) {
		_life += amt;
		if (_life <= 0)
			Kill();
	}

	public void Kill() {
		Destroy(this.transform.gameObject);
		
		if (drops != null) {
			foreach (Item i in drops) {
				if (i != null) {
					i.ItemInstantiate(transform.position, Quaternion.identity);
				}
			}
		}

		if (UnityEngine.Random.Range(0, 10) < AMMO_DROP) {
			GameObject g = Instantiate(AMMO_PREFAB, transform.position, Quaternion.identity) as GameObject;
			g.GetComponent<AmmoScript>().amount = Random.Range(20, 40);
		}
		if (UnityEngine.Random.Range(0, 10) < HEALTH_DROP) {
			GameObject g = Instantiate(HEALTH_PREFAB, transform.position, Quaternion.identity) as GameObject;
			g.GetComponent<HealthScript>().health = Random.Range(5, 20);
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
	
	public string getResistanceTypeAsString() {
		if (Mathf.Max(heatResistance, slowResistance, stunResistance) == heatResistance) {
			return "Heat";
		} else if (Mathf.Max(heatResistance, slowResistance, stunResistance) == slowResistance) {
			return "Slow";
		} else {
			return "Stun";
		}
	}

	public List<Vector2> MotionPath {
		set { _path = value; }
	}
}

public enum MobType {
	//Basic,
	Creepling,
	Tank,
	Speedster
//	Healer,
//	Boss
}