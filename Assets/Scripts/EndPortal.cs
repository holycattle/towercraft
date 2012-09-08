using UnityEngine;
using System.Collections;

public class EndPortal : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
		// Destroys the entity that enters it.
		Destroy(other.transform.root.gameObject);
//		Debug.Log("Trigger: Collider Enter!");
	}
}
