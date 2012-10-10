using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
	private LevelController _levelController;
	private GameController _gameController;
	private BaseTower _ts;

	void Start() {
		_ts = null;
		_levelController = GameObject.Find(" GameController").GetComponent<LevelController>();
		_gameController = GameObject.Find(" GameController").GetComponent<GameController>();
	}

	void InputMouseEnter() {
		GetComponent<MeshRenderer>().enabled = true;
		GetComponent<Renderer>().material.color = Color.red;
	}

	void InputMouseExit() {
		GetComponent<MeshRenderer>().enabled = false;
		GetComponent<Renderer>().material.color = Color.white;
	}

//	public void TEMP_toggleTower() {
//		if (_ts == null)
//			AddTower();
//		else
//			RemoveTower();
//	}

	void OnRightClick() {
		Debug.Log("Right Clicked");
	}
	
	public void RemoveTower() {
		Destroy(_ts.gameObject);
		_ts = null;

		// Update Path
		_levelController.UpdatePath();
	}

	public void AddTower(GameObject tower) {
		if (!_gameController.SubMoney(2))
			return;

		GameObject t = Instantiate(tower, transform.position, Quaternion.identity) as GameObject;
		t.name = "Tower: " + name;
		t.transform.parent = transform;
		_ts = t.GetComponent<BaseTower>();

		// Update Path
		_levelController.UpdatePath();
	}

	// Used for path generation
	public BaseTower Tower {
		get { return _ts; }
		set { _ts = value; }
	}
}
