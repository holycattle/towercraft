using UnityEngine;
using System.Collections;

public class EndPortal : MonoBehaviour {

	private GameController _gameController;

	void Start() {
		_gameController = GameObject.Find(" GameController").GetComponent<GameController>();
	}

	void OnTriggerEnter(Collider other) {
		GameObject go = other.transform.root.gameObject;
		
		if(!go.name.Equals("Player", System.StringComparison.OrdinalIgnoreCase)) {
			// Destroys the entity that enters it.
			Destroy(go);
			// Subtract from Life
			_gameController.SubLife(1);
		}
	}
}
