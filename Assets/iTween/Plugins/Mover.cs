using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

	void Start() {
		iTween.MoveTo(gameObject, iTween.Hash("y", 4, "easeType", "easeInOutQuint"));
	}
}
