using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
	private LevelController _levelController;
	private GameController _gameController;

	// Towers
	public GameObject tower;
	private BaseTower _ts; // Tower on this grid.
	private MeshRenderer _selectionGrid;
	private bool _hasTower;

	void Awake() {
		_ts = null;
		_levelController = GameObject.Find(" GameController").GetComponent<LevelController>();
		_gameController = GameObject.Find(" GameController").GetComponent<GameController>();

		_selectionGrid = transform.FindChild("GridSelection").GetComponent<MeshRenderer>();
		_hasTower = false;
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

	public bool HasTower() {
		return _ts != null || _hasTower;
	}

	public bool TempTower {
		get { return _hasTower; }
		set { _hasTower = value; }
	}

	// Used for path generation
	public BaseTower Tower {
		get { return _ts; }
		set { _ts = value; }
	}
}
