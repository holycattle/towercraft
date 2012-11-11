using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
	private LevelController _levelController;
	private GameController _gameController;

	// Towers
	public GameObject tower;
	private BaseTower _ts; // Tower on this grid.
	private MeshRenderer _selectionGrid;

	void Awake() {
		_ts = null;
		_levelController = GameObject.Find(" GameController").GetComponent<LevelController>();
		_gameController = GameObject.Find(" GameController").GetComponent<GameController>();

		_selectionGrid = transform.FindChild("GridSelection").GetComponent<MeshRenderer>();
		SetSelected(false);
	}

	void InputMouseEnter() {
//		GetComponent<MeshRenderer>().enabled = true;
//		GetComponent<Renderer>().material.color = Color.red;
		SetSelected(true);
	}

	void InputMouseExit() {
//		GetComponent<MeshRenderer>().enabled = false;
//		GetComponent<Renderer>().material.color = Color.white;
		SetSelected(false);
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

	public BaseTower GenerateBaseTower() {
		// Create the tower
		GameObject t = Instantiate(tower, transform.position, Quaternion.identity) as GameObject;
		t.name = "Tower: " + name;
		t.transform.parent = transform;
		_ts = t.GetComponent<BaseTower>();

		// Update Path
		_levelController.UpdatePath();

		return t.GetComponent<BaseTower>();
	}

	private void SetSelected(bool sel) {
		_selectionGrid.enabled = sel;
	}

	// Used for path generation
	public BaseTower Tower {
		get { return _ts; }
		set { _ts = value; }
	}
}
