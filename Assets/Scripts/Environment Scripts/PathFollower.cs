using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFollower : MonoBehaviour {
	public const int YPOS = 2;

	// Path Following
	public List<Vector2> path;		// List of waypoints to follow
	public int _moveSpeed = 64;
	private int _activePoint;		// Current point in the path

	void Start() {
		transform.position = new Vector3(path[0].x, YPOS, path[0].y);
		_activePoint = 1;
	}
	
	void Update() {
		if (_activePoint == path.Count) {
			// Dont do anything anymore.
			Destroy(gameObject);
			return;
		}

		Vector3 targetPos = new Vector3(path[_activePoint].x, YPOS, path[_activePoint].y);
		float dist = Vector3.Distance(targetPos, transform.position);

		if (dist < _moveSpeed * Time.deltaTime) {
			transform.rotation = Quaternion.LookRotation(targetPos - transform.position, Vector3.up);
			transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime, Space.Self);

			// Move the extra
			_activePoint++;
			if (_activePoint != path.Count) {
				targetPos = new Vector3(path[_activePoint].x, YPOS, path[_activePoint].y);
				transform.rotation = Quaternion.LookRotation(targetPos - transform.position, Vector3.up);
				transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime, Space.Self);
			}
		} else {
			transform.rotation = Quaternion.LookRotation(targetPos - transform.position, Vector3.up);
			transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime, Space.Self);
		}
	}
}
