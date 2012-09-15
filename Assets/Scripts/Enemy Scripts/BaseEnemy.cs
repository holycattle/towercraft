using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseEnemy : MonoBehaviour {
	public const float MOV_OFFSET = 0.5f;

	// Enemy Stats (Given Default values, but you have to set it in the game object.)
	public float turnSpeed = 3 * Mathf.PI / 4;
	public int moveSpeed;
	public int maxLife;
	public int moneyReward;
	public int waveCost;

	// Private Variables
	private List<Vector2> _path;
	private int _activePoint;
	private Vector2 _offset;
	private int _life;

	void Start() {
		_activePoint = 0;
		_offset = new Vector2(Random.Range(-MOV_OFFSET, MOV_OFFSET), Random.Range(-MOV_OFFSET, MOV_OFFSET));
		_life = maxLife;
	}

	void Update() {
		// MOVEMENT
		if (_activePoint == _path.Count) {
			// Dont do anything anymore.
			return;
		}

		Vector3 targetPos = new Vector3(_path[_activePoint].x + _offset.x, transform.position.y, _path[_activePoint].y + _offset.y);
		float dist = Vector3.Distance(targetPos, transform.position);

		//TODO!: Make the look rotation smoother
		// -Do this by making them start to turn as they APPROACH their target
		if (dist < moveSpeed * Time.deltaTime) {
			transform.rotation = Quaternion.Slerp(transform.rotation,
				Quaternion.LookRotation(targetPos - transform.position), turnSpeed * Time.deltaTime);
			transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);

			// Move the extra
			_activePoint++;
			_offset = new Vector2(Random.Range(-MOV_OFFSET, MOV_OFFSET), Random.Range(-MOV_OFFSET, MOV_OFFSET));
			if (_activePoint != _path.Count) {
				transform.rotation = Quaternion.Slerp(transform.rotation,
					Quaternion.LookRotation(targetPos - transform.position), turnSpeed * Time.deltaTime);
				transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
			}
		} else {
			transform.rotation = Quaternion.Slerp(transform.rotation,
				Quaternion.LookRotation(targetPos - transform.position), turnSpeed * Time.deltaTime);
			transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
		}

//		float forwardMoveAmount = 0;
//		float zDiff = _path[_activePoint].y - transform.position.z;
//		float sideMoveAmount = 0;
//		float xDiff = _path[_activePoint].x - transform.position.x;
//
//		// Determine how much the entity moves in either direction.
//		forwardMoveAmount = CalculateDirection(zDiff) * MOVESPEED * Time.deltaTime;
//		sideMoveAmount = CalculateDirection(xDiff) * MOVESPEED * Time.deltaTime;
//
//		if (forwardMoveAmount > Mathf.Abs(zDiff)) {
//			forwardMoveAmount = zDiff;
//			_activePoint++;
//
//			if (_activePoint != _path.Count) {
//				zDiff = _path[_activePoint].y - (transform.position.z + forwardMoveAmount);
//				xDiff = _path[_activePoint].x - (transform.position.x + sideMoveAmount);
//				forwardMoveAmount += CalculateDirection(zDiff) * MOVESPEED * Time.deltaTime;
//				sideMoveAmount += CalculateDirection(xDiff) * MOVESPEED * Time.deltaTime;
//			}
//		} else if (sideMoveAmount > Mathf.Abs(xDiff)) {
//			sideMoveAmount = xDiff;
//			_activePoint++;
//
//			if (_activePoint != _path.Count) {
//				zDiff = _path[_activePoint].y - (transform.position.z + forwardMoveAmount);
//				xDiff = _path[_activePoint].x - (transform.position.x + sideMoveAmount);
//				forwardMoveAmount += CalculateDirection(zDiff) * MOVESPEED * Time.deltaTime;
//				sideMoveAmount += CalculateDirection(xDiff) * MOVESPEED * Time.deltaTime;
//			}
//		}


//		transform.Translate(sideMoveAmount, 0, forwardMoveAmount, Space.World);

		// LIVING
		if (_life <= 0) {
			Destroy(this.transform.root.gameObject);
		}
	}

	#region Life Management
	public void AddLife(int amt) {
		_life += amt;
		if (_life <= 0)
			kill();
	}

	public void SubLife(int amt) {
		_life -= amt;
		if (_life <= 0)
			kill();
	}

	public int Life {
		get { return _life; }
	}

	public void kill() {
		GameObject.Find(" GameController").GetComponent<GameController>().AddMoney(moneyReward);
		Destroy(this.transform.root.gameObject);
	}
	#endregion

	private int CalculateDirection(float diff) {
		if (diff > 0)
			return 1;
		else if (diff < 0)
			return -1;
		return 0;
	}

	public int WaveCost {
		get { return waveCost; }
	}

	public List<Vector2> Path {
		set { _path = value; }
	}

}
