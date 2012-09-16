using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	
	public const int MAX_LIVES = 10;
	
	// Map Dimensions
	public int mapWidth;
	public int mapHeight;
	public int startXPosition;
	public int startYPosition;
	
	// Objects
	public GameObject tilePrefab;
	public GameObject enemyPrefab;

	/* PRIVATE */
	// Map Variables
	private Grid[] _map;
	private List<Vector2> _path;

	// Player Variables
	private int _livesLeft;
	private int _money;
	
	void Awake() {
		// Set default width x height
		if (mapWidth * mapHeight == 0) {
			mapWidth = 16;
			mapHeight = 16;
		}
		_map = new Grid[mapWidth * mapHeight];

		
		// Instantiate the Tiles and Path
		GameObject gridParent = new GameObject("Grid Parent");
		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {				
				GameObject t = Instantiate(tilePrefab, new Vector3(startXPosition + x * 4 + 2, 0, startYPosition + y * 4 + 2), Quaternion.identity) as GameObject;
				
				t.name = "Grid:" + x + "," + y;
				t.transform.parent = gridParent.transform;
				_map[y * mapWidth + x] = t.GetComponent<Grid>();
			}
		}
		UpdatePath();

		// Init Player Variables
		_livesLeft = MAX_LIVES;
		_money = 10;
	}
	
	void Update() {
		// Draw the Path
		if (_path != null) {
			for (int i = 0; i < _path.Count - 1; i++) {
				Debug.DrawLine(new Vector3(_path[i].x, 1, _path[i].y),
					new Vector3(_path[i + 1].x, 1, _path[i + 1].y));
			}
		}

		if (Input.GetKeyDown(KeyCode.F4)) {
			// KILL ALL BUTTON
			GameObject[] e = GameObject.FindGameObjectsWithTag("Enemy");
			foreach (GameObject go in e) {
				Destroy(go);
			}
		}

		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
			GameObject g = Instantiate(enemyPrefab,
				new Vector3(startXPosition + 2 + Random.Range(-1f, 1f), 2, startYPosition - 2 + Random.Range(-1f, 1f)),
				Quaternion.identity) as GameObject;
			BaseEnemy m = g.GetComponent<BaseEnemy>();
			m.Path = _path;
		}
	}
	
	public void UpdatePath() {
		// Pathfind!
		_path = AStar.PathFind(new Vector2(0, 0), new Vector2(mapWidth - 1, mapHeight - 1), _map, mapWidth, mapHeight);
		
		if (_path != null) {
			// Convert Path to real coordinates.
			List<Vector2> act = new List<Vector2>();
			while (_path.Count > 0) {
				act.Add(new Vector2(startXPosition + _path[0].x * 4 + 2, startYPosition + _path[0].y * 4 + 2));
				_path.RemoveAt(0);
			}
			_path = act;
			
			// Append Coords of the End Portal
			_path.Add(new Vector2(startXPosition + (mapWidth - 1) * 4 + 2, startYPosition + mapHeight * 4 + 2));
		}
	}
	
	public Grid GetTile(int x, int y) {
		if (x >= mapWidth)
			x = mapWidth - 1;
		if (y >= mapHeight)
			y = mapHeight - 1;
		if (x < 0)
			x = 0;
		if (y < 0)
			y = 0;
		
		return _map[y * mapWidth + x];
	}

	public List<Vector2> Path {
		get { return _path; }
	}

	public int Lives {
		get { return _livesLeft; }
	}

	public int Money {
		get { return _money; }
	}

	public void AddLife(int life) {
		_livesLeft += life;
		if (_livesLeft < 0) {
			// YOU LOSE!
			_livesLeft = 0;
		}
	}

	public void SubLife(int life) {
		_livesLeft -= life;
		if (_livesLeft < 0) {
			// YOU LOSE!
			_livesLeft = 0;
		}
	}

	public bool AddMoney(int money) {
		_money += money;
		if (_money < 0) {
			_money -= money;
			return false;
		}
		return true;
	}

	public bool SubMoney(int money) {
		_money -= money;
		if (_money < 0) {
			_money += money;
			return false;
		}
		return true;
	}
}