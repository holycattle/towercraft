using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseEnemy : MonoBehaviour {
	public const float MOV_OFFSET = 0.0f;
	public static Vector3 LIFE_OFFSET = new Vector3(0, 2f, 0);
	public const float OFFSETABLE = 0.5f;

	// Enemy Stats (Given Default values, but you have to set it in the game object.)
	private float turnSpeed;
	public float totalTurn;
	private Quaternion originalRot;
	public int moveSpeed;
	public int maxLife;
	public int moneyReward;
	public int waveCost;
	public MobType type;

	// Private Variables
	private List<Vector2> _path;
	private int _activePoint;
	private Vector2 _offset;
	private int _life;

	void Start() {
		_activePoint = 1;
		_offset = new Vector2(Random.Range(-MOV_OFFSET, MOV_OFFSET), Random.Range(-MOV_OFFSET, MOV_OFFSET));
		_life = maxLife;

		// Set Turn Speed
		turnSpeed = moveSpeed;

		originalRot = transform.rotation;
	}

	void Update() {
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
				// Set turn speed
				if (_activePoint >= 2 && _activePoint < _path.Count - 1) {
					Vector2 v1 = _path[_activePoint - 2] - _path[_activePoint - 1];
					Vector2 v2 = _path[_activePoint + 1] - _path[_activePoint];
					v1.Normalize();
					v2.Normalize();

					targetPos = new Vector3(_path[_activePoint].x + _offset.x, transform.position.y, _path[_activePoint].y + _offset.y);
					turnSpeed = moveSpeed / Vector2.Distance(_path[_activePoint - 1], _path[_activePoint]);

					originalRot = transform.rotation;
				}
			}

			totalTurn = 0;
		} else {
			transform.rotation = Quaternion.Slerp(originalRot, Quaternion.LookRotation(targetPos - transform.position), totalTurn);
			transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);
		}
		totalTurn += turnSpeed * Time.deltaTime;

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

		float x = _life / (float)maxLife;
		transform.FindChild("EnemyModel").gameObject.GetComponent<Renderer>().material.color = new Color(x, x, x);

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

public enum MobType {
	Basic,
	Creepling,
	Tank,
	Speedster
//	Healer,
//	Boss
}