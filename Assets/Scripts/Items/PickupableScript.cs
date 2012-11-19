using UnityEngine;
using System.Collections;

public class PickupableScript : MonoBehaviour {
	public const float INDICATOR_HEIGHT = 2f;	// World Units
	public const float SUCK_DISTANCE = 0.5f;

	// GameObject Behaviour
	protected LineRenderer _line;
	protected Transform _target;

	protected virtual void Start() {
		// Set initial upward movement
		rigidbody.velocity = new Vector3(Random.Range(-2f, 2f), 8, Random.Range(-2f, 2f));
		rigidbody.AddTorque(Vector3.right * Random.Range(-5, 5) + Vector3.forward * Random.Range(-5, 5));

		// Set up Line Renderer
		_line = GetComponent<LineRenderer>();
		_line.useWorldSpace = true;
	}
	
	protected virtual void Update() {
		if (_target != null) {
			_line.enabled = false;
			rigidbody.velocity = (_target.position - transform.position).normalized * 16;
		} else {
			_line.SetPosition(0, transform.position);
			_line.SetPosition(1, transform.position + Vector3.up * INDICATOR_HEIGHT);
		}
	}

	public virtual void Pickup(ItemCollector ic) {
	}

	public virtual bool CanPickup(ItemCollector ic) {
		return false;
	}

	public Transform Target {
		set { _target = value; }
	}
}
