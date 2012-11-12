using UnityEngine;
using System.Collections;

public class ItemScript : MonoBehaviour {
	public const float INDICATOR_HEIGHT = 2f;	// World Units

	public Item item;
	private LineRenderer _line;
	private Transform _target;

	void Start() {
		item = new Item();
		rigidbody.velocity = new Vector3(Random.Range(-2f, 2f), 8, Random.Range(-2f, 2f));
		rigidbody.AddTorque(Vector3.right * Random.Range(-5, 5) + Vector3.forward * Random.Range(-5, 5));

		_line = GetComponent<LineRenderer>();
		_line.useWorldSpace = true;

		_target = null;
	}

	void Update() {
		if (_target != null) {
			_line.enabled = false;
			rigidbody.velocity = (_target.position - transform.position).normalized * 16;
		} else {
			_line.SetPosition(0, transform.position);
			_line.SetPosition(1, transform.position + Vector3.up * INDICATOR_HEIGHT);
		}
	}

	public Transform Target {
		set { _target = value; }
	}
}
