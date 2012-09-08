using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovementController : MonoBehaviour {
	
	public const int MOVESPEED = 8;
	
	public int maxLife;
	
	private List<Vector2> _path;
	private int _activePoint;
	private int _life;
	
	void Start () {
		_activePoint = 0;
		_life = maxLife;
	}
	
	void Update () {
		// MOVEMENT
		if (_activePoint == _path.Count) {
			// Dont do anything anymore.
			return;
		}
			
		float forwardMoveAmount = 0;
		float zDiff = _path[_activePoint].y - transform.position.z;
		float sideMoveAmount = 0;
		float xDiff = _path[_activePoint].x - transform.position.x;
		
		// Determine how much the entity moves in either direction.
		forwardMoveAmount = CalculateDirection(zDiff) * MOVESPEED * Time.deltaTime;
		sideMoveAmount = CalculateDirection(xDiff) * MOVESPEED * Time.deltaTime;
		
		if (forwardMoveAmount > Mathf.Abs(zDiff)) {
			forwardMoveAmount = zDiff;
			_activePoint++;
			
			if (_activePoint != _path.Count) {
				zDiff = _path[_activePoint].y - (transform.position.z + forwardMoveAmount);
				xDiff = _path[_activePoint].x - (transform.position.x + sideMoveAmount);
				forwardMoveAmount += CalculateDirection(zDiff) * MOVESPEED * Time.deltaTime;
				sideMoveAmount += CalculateDirection(xDiff) * MOVESPEED * Time.deltaTime;
			}
		} else if (sideMoveAmount > Mathf.Abs(xDiff)) {
			sideMoveAmount = xDiff;
			_activePoint++;
			
			if (_activePoint != _path.Count) {
				zDiff = _path[_activePoint].y - (transform.position.z + forwardMoveAmount);
				xDiff = _path[_activePoint].x - (transform.position.x + sideMoveAmount);
				forwardMoveAmount += CalculateDirection(zDiff) * MOVESPEED * Time.deltaTime;
				sideMoveAmount += CalculateDirection(xDiff) * MOVESPEED * Time.deltaTime;
			}
		}
		
		transform.Translate(sideMoveAmount, 0, forwardMoveAmount, Space.World);
		
		// LIVING
		if (_life <= 0) {
			Destroy(this.transform.root.gameObject);
		}
	}
	
	public void AddLife(int amt) {
		_life += amt;
	}
	
	private int CalculateDirection(float diff) {
		if (diff > 0) return 1;
		else if (diff < 0) return -1;
		return 0;
	}
	
	public List<Vector2> Path {
		set { _path = value; }
	}
	
	public int Life {
		get { return _life; }
	}
}
