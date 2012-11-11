using UnityEngine;
using System.Collections;

public class ItemScript : MonoBehaviour {
	public const float INDICATOR_HEIGHT = 2f;	// World Units

	public Item item;
	private LineRenderer _line;

	void Start() {
		item = new Item();
		rigidbody.velocity = new Vector3(Random.Range(0, 0.5f), 8, Random.Range(0, 0.5f));
		_line = GetComponent<LineRenderer>();
	}

	void Update() {
		_line.SetPosition(0, transform.position);
		_line.SetPosition(1, transform.position + Vector3.up * 2);
	}
}
