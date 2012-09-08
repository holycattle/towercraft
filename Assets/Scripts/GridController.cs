using UnityEngine;
using System.Collections;

public class GridController : MonoBehaviour {
	
	// Tower
	public GameObject towerPrefab;
	
	private GameController _gameController;
	private TowerScript _ts;
	
	void Start () {
		_ts = null;
		_gameController = GameObject.Find(" GameController").GetComponent<GameController>();
	}
	
	void Update () {
	
	}
	
	
	void OnMouseEnter() {
		GetComponent<MeshRenderer>().enabled = true;
		GetComponent<Renderer>().material.color = Color.red;
	}
	
	void OnMouseExit() {
		GetComponent<MeshRenderer>().enabled = false;
		GetComponent<Renderer>().material.color = Color.white;
	}
	
	void OnMouseOver() {
		
	}
	
	void OnLeftClick() {
		if (_ts == null) AddTower();
		else RemoveTower();
	}
	
	void OnRightClick() {
		Debug.Log("Right Clicked");
	}
	
	private void RemoveTower() {
		Destroy(_ts.gameObject);
		_ts = null;
		
		// Update Path
		_gameController.UpdatePath();
	}
	
	private void AddTower() {
		GameObject t = Instantiate(towerPrefab, transform.position, Quaternion.identity) as GameObject;
		t.name = "Tower: " + name;
		_ts = t.GetComponent<TowerScript>();
		
		// Update Path
		_gameController.UpdatePath();
	}
	
	#region Setter/Getter Functions
	public TowerScript Tower {
		get { return _ts; }
		set { _ts = value; }
	}
	#endregion
}
