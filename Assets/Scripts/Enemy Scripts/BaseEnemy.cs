using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseEnemy : MonoBehaviour {
	// Constants
	public const float MOV_OFFSET = 0.5f;
	public static Vector3 LIFE_OFFSET = new Vector3(0, 2f, 0);

	// References
	private LevelController _level;
	private PlayerController _player;
	private static GameObject _item;

	// Enemy Stats (Given Default values, but you have to set it in the game object.)
	public float moveSpeed;
	public int maxLife;
	private int _life;					// Current Life
	public int moneyReward;
	public int waveCost;
	private int damage = 1;
	private float firingInterval = 1f;
	private float range = 32;

	// Movement Variables
	private Vector2 _gridPosition;
	private float turnSpeed;
	private float totalTurn;
	private Quaternion originalRot;
	public MobType type;
	private float _timeTillFire;

	// Path Following
	private List<Vector2> _path;
	private int _activePoint;
	private Vector2 _offset;

	void Start() {
		_activePoint = 1;
		_offset = new Vector2(Random.Range(-MOV_OFFSET, MOV_OFFSET), Random.Range(-MOV_OFFSET, MOV_OFFSET));
		_life = maxLife;

		// Set Turn Speed
		turnSpeed = (moveSpeed / ((Mathf.Sqrt(2) / 2) * LevelController.TILE_SIZE)) * 2;
//		Debug.Log("Move/Turn Speed Init: " + moveSpeed + " / " + turnSpeed);

		originalRot = transform.rotation;
		_gridPosition = new Vector2(-100, -100);

		if (_item == null)
			_item = Resources.Load("Prefabs/Items/Item", typeof(GameObject)) as GameObject;

		_player = GameObject.Find("Player").GetComponent<PlayerController>();
		_level = GameObject.Find(" GameController").GetComponent<LevelController>();
	}

	void Update() {
		if (_path == null) {
			Debug.Log("NULL WHY?");
		}

		// MOVEMENT
		if (_activePoint == _path.Count) {
			// Dont do anything anymore.
			return;
		}

		Vector3 targetPos = new Vector3(_path[_activePoint].x + _offset.x, transform.position.y, _path[_activePoint].y + _offset.y);
		float dist = Vector3.Distance(targetPos, transform.position);

		if (dist < moveSpeed * Time.deltaTime) {
			transform.rotation = Quaternion.Slerp(originalRot, Quaternion.LookRotation(targetPos - transform.position), totalTurn);
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

		// Update current grid position
		Vector2 currentPos = _level.GridConvert(transform.position);
		if (currentPos != _gridPosition) {
			_gridPosition = currentPos;
		}

		if (_path != null) {
			Vector3 p;
			int i = 0;
			for (; i < _path.Count - 1; i++) {
				p = new Vector3(_path[i].x, 1, _path[i].y);

				Debug.DrawLine(p, new Vector3(_path[i + 1].x, 1, _path[i + 1].y), Color.cyan);
				Debug.DrawLine(p, p + Vector3.up * 5, Color.cyan);
			}
			p = new Vector3(_path[i].x, 1, _path[i].y);
			Debug.DrawLine(p, p + Vector3.up * 5);
		}

		// ATTACKING
		if (Vector3.Distance(transform.position, _player.transform.position) <= range) {
			if (_timeTillFire <= 0) {
				if (Random.Range(0, 1) == 0) {
					// Hit
					_player.SubLife(damage);
				}

				_timeTillFire += firingInterval;
			}
		}

		if (_timeTillFire > 0) {
			_timeTillFire -= Time.deltaTime;
		}
	}

	public void PathUpdate() {
		_path = _level.RecalculatePath(_gridPosition);
		_activePoint = 2;

		if (_path != null) {
			Debug.Log("Path Updated");
		} else {
			Debug.Log("UKNOWN");
		}
	}

	#region Life Management
	public void AddLife(int amt) {
		_life += amt;
		if (_life <= 0)
			Kill();
	}

	public void SubLife(int amt) {
		_life -= amt;
		if (_life <= 0)
			Kill();
	}

	public int Life {
		get { return _life; }
	}

	public void Kill() {
		GameObject.Find(" GameController").GetComponent<GameController>().AddMoney(moneyReward);
		Destroy(this.transform.root.gameObject);

		if (Random.Range(0, 10) < 10) {
			Instantiate(_item, transform.position, Quaternion.identity);
		}
	}
	#endregion

	private int CalculateDirection(float diff) {
		if (diff > 0)
			return 1;
		else if (diff < 0)
			return -1;
		return 0;
	}

	public string Name {
		get { return gameObject.name; }
	}

	public int WaveCost {
		get { return waveCost; }
	}

	public List<Vector2> MotionPath {
		set { _path = value; }
	}
}

public enum MobType {
	Basic,
	Creepling,
	Tank,
	Speedster
//	Healer,
//	Boss
}