using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
	// Controllers
	private LevelController _levelController;

	// Grid Info
	private Vector2 _gridValue;
	private GameObject actualRotator;

	// Tower Info
	public GameObject tower;
	public GameObject rotatorPrefab;
	private BaseTower _ts; // Tower on this grid.
	private MeshRenderer _selectionGrid;
	private bool _hasTower;

	void Awake() {
		_ts = null;
		_levelController = GameObject.Find(" GameController").GetComponent<LevelController>();

		rotatorPrefab = Resources.Load("Prefabs/Tools/RangeView", typeof(GameObject)) as GameObject;

		_selectionGrid = transform.FindChild("GridSelection").GetComponent<MeshRenderer>();
		_hasTower = false;
		SetSelected(false);
	}

	void LateUpdate() {
		_hasTower = false;
	}

	void InputMouseEnter() {
		SetSelected(true);
	}

	void InputMouseExit() {
		SetSelected(false);
	}

	public void RemoveTower() {
		Destroy(_ts.gameObject);
		_ts = null;

		// Update Path
		_levelController.UpdatePath();
	}

	public TowerComponent BreakTopComponent() {
		if (_ts != null) {
			TowerComponent t = _ts.BreakTopComponent();
			if (t.componentType == BaseTower.TOWER_BASE) {
				Destroy(_ts.gameObject);
				_ts = null;
			}

			return t;
		}
		
		// Update Path
		_levelController.UpdatePath();

		return null;
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

		if (sel) {
			if (Tower != null) {
				float range = Tower.GetRange();
				if (range > 0) {
					actualRotator = Instantiate(rotatorPrefab) as GameObject;
					RangeView v = actualRotator.GetComponent<RangeView>();
					v.Range = range;
					v.Center = transform.position;
				}
			}
		} else {
			if (actualRotator != null) {
				Destroy(actualRotator);
			}
		}
	}

	public bool HasTower() {
		return _ts != null || _hasTower;
	}

	public Vector2 GridValue {
		get { return _gridValue; }
		set { _gridValue = value; }
	}

	public bool TempTower {
		get { return _hasTower; }
		set { _hasTower = value; }
	}

	public BaseTower Tower {
		get { return _ts; }
		set { _ts = value; }
	}
}
