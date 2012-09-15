using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
	
	// Tower
	public GameObject towerPrefab;
	private GameController _gameController;
	private BaseTower _ts;
	
	void Start () {
		_ts = null;
		_gameController = GameObject.Find(" GameController").GetComponent<GameController>();
	}
	
	void Update () {
	
	}
	
	void OnMouseEnter () {
		GetComponent<MeshRenderer>().enabled = true;
		GetComponent<Renderer>().material.color = Color.red;
	}
	
	void OnMouseExit () {
		GetComponent<MeshRenderer>().enabled = false;
		GetComponent<Renderer>().material.color = Color.white;
	}
	
	void OnMouseOver () {
		
	}
	
	void OnLeftClick () {
		if (_ts == null)
			AddTower();
		else
			RemoveTower();
	}
	
	void OnRightClick () {
		Debug.Log("Right Clicked");
	}
	
	private void RemoveTower () {
		Destroy(_ts.gameObject);
		_ts = null;
		
		// Update Path
		_gameController.UpdatePath();
	}
	
	private void AddTower () {
		if (!_gameController.SubMoney(2))
			return;

		GameObject t = Instantiate(towerPrefab, transform.position, Quaternion.identity) as GameObject;
		t.name = "Tower: " + name;
		t.transform.parent = transform;
		_ts = t.GetComponent<BaseTower>();
		
		// Update Path
		_gameController.UpdatePath();
	}
	
	#region Setter/Getter Functions
	public BaseTower Tower {
		get { return _ts; }
		set { _ts = value; }
	}
	#endregion
}
