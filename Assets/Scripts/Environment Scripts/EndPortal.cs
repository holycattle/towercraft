using UnityEngine;
using System.Collections;

public class EndPortal : MonoBehaviour {

	private GameController _gameController;

	void Start () {
		_gameController = GameObject.Find(" GameController").GetComponent<GameController>();
	}

	void OnTriggerEnter (Collider other) {
		// Destroys the entity that enters it.
		Destroy(other.transform.root.gameObject);

		// Subtract from Life
		_gameController.SubLife(1);
	}
}
