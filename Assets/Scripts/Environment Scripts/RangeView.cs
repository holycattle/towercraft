using UnityEngine;
using System.Collections;

public class RangeView : MonoBehaviour {
	Transform[] marchingAnts;

	void Awake() {
		marchingAnts = new Transform[transform.childCount];
		for (int i = 0; i < transform.childCount; i++) {
			marchingAnts[i] = transform.GetChild(i);
		}

		Range = 2;
	}

	public Vector3 Center {
		set {
			transform.position = value;
		}
	}

	public float Range {
		set {
			float trailDuration = 0.25f;

			foreach (Transform t in marchingAnts) {
				Vector3 v = new Vector3(t.localPosition.x, 0, t.localPosition.z);
				v.Normalize();
				v *= value * LevelController.TILE_SIZE;
				t.localPosition = new Vector3(v.x, 2, v.z);

				t.gameObject.GetComponent<TrailRenderer>().time = trailDuration;
			}
		}
	}
}
