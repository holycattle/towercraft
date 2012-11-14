using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelController : MonoBehaviour {

	public const int TILE_SIZE = 8;
	public const int HTILE_SIZE = TILE_SIZE / 2;
	
	// Map Dimensions
	public int mapWidth;
	public int mapHeight;
	[HideInInspector]
	public int startXPosition;
	[HideInInspector]
	public int startYPosition;
	public Vector3 mobSpawnPoint;

	// Objects
	public GameObject tilePrefab;
	public GameObject enemyPrefab;

	/* PRIVATE */
	// Map Variables
	private Grid[] _map;
	private List<Vector2> _path;
	
	void Awake() {
		// Set default width x height
		if (mapWidth * mapHeight == 0) {
			mapWidth = 16;
			mapHeight = 16;
		}
		_map = new Grid[mapWidth * mapHeight];
		startXPosition = -(mapWidth / 2) * TILE_SIZE;
		startYPosition = -(mapHeight / 2) * TILE_SIZE;

		mobSpawnPoint = new Vector3(startXPosition + HTILE_SIZE, 1, startYPosition - TILE_SIZE);

		// Instantiate the Tiles and Path
		GameObject gridParent = new GameObject("Grid Parent");
		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {
				GameObject t = Instantiate(tilePrefab,
					new Vector3(startXPosition + x * TILE_SIZE + HTILE_SIZE, 0, startYPosition + y * TILE_SIZE + HTILE_SIZE),
					Quaternion.identity) as GameObject;
				
				t.name = "Grid:" + x + "," + y;
				t.transform.parent = gridParent.transform;
				_map[y * mapWidth + x] = t.GetComponent<Grid>();
			}
		}
		UpdatePath();
	}
	
	void Update() {
		// Draw the Path
		if (_path != null) {
			Vector3 p;
			int i = 0;
			for (; i < _path.Count - 1; i++) {
				p = new Vector3(_path[i].x, 1, _path[i].y);

				Debug.DrawLine(p, new Vector3(_path[i + 1].x, 1, _path[i + 1].y));
				Debug.DrawLine(p, p + Vector3.up * 5);
			}
			p = new Vector3(_path[i].x, 1, _path[i].y);
			Debug.DrawLine(p, p + Vector3.up * 5);
		}

		if (Input.GetKeyDown(KeyCode.F4)) {
			// KILL ALL BUTTON
			GameObject[] e = GameObject.FindGameObjectsWithTag("Enemy");
			foreach (GameObject go in e) {
				Destroy(go);
			}
		}

		if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
			GameObject g = Instantiate(enemyPrefab,
				new Vector3(startXPosition + HTILE_SIZE + Random.Range(-1f, 1f), 2, startYPosition - HTILE_SIZE + Random.Range(-1f, 1f)),
				Quaternion.identity) as GameObject;
			BaseEnemy m = g.GetComponent<BaseEnemy>();
			m.MotionPath = _path;
		}
	}
	
	public void UpdatePath() {
		// Pathfind!
		_path = AStar.PathFind(new Vector2(0, 0), new Vector2(mapWidth - 1, mapHeight - 1), _map, mapWidth, mapHeight);

		if (_path != null) {
			// Convert Path to real coordinates.
			List<Vector2> act = new List<Vector2>();
			while (_path.Count > 0) {
				act.Add(new Vector2(startXPosition + _path[0].x * TILE_SIZE, startYPosition + _path[0].y * TILE_SIZE));
				_path.RemoveAt(0);
			}
			_path = act;
		}

		// Update the paths of all Enemies on the board
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		for (int i = 0; i < enemies.Length; i++) {
			enemies[i].GetComponent<BaseEnemy>().PathUpdate();
		}
	}

	public bool HasPath() {
		List <Vector2> testPath = AStar.PathFind(new Vector2(0, 0), new Vector2(mapWidth - 1, mapHeight - 1), _map, mapWidth, mapHeight);
		if (testPath == null) {
			return false;
		}

		return testPath != null;
	}

	public List<Vector2> RecalculatePath(Vector2 start) {
		// Pathfind!
		List<Vector2> newPath = AStar.PathFind(start, new Vector2(mapWidth - 1, mapHeight - 1), _map, mapWidth, mapHeight);

		if (newPath != null) {
			// Convert Path to real coordinates.
			List<Vector2> act = new List<Vector2>();
			while (newPath.Count > 0) {
				act.Add(new Vector2(startXPosition + newPath[0].x * TILE_SIZE, startYPosition + newPath[0].y * TILE_SIZE));
				newPath.RemoveAt(0);
			}
			newPath = act;
		}

		return newPath;
	}

	public Vector2 GridConvert(float x, float y) {
		Vector2 v = new Vector2((int)((x + mapWidth * HTILE_SIZE) / TILE_SIZE), (int)((y + mapHeight * HTILE_SIZE) / TILE_SIZE));
		if (v.x < 0)
			v.Set(0, v.y);
		if (v.x >= mapWidth)
			v.Set(mapWidth - 1, v.y);
		if (v.y < 0)
			v.Set(v.x, 0);
		if (v.y >= mapHeight)
			v.Set(v.x, mapHeight - 1);
		return v;
	}

	public Vector2 GridConvert(Vector3 v) {
		return GridConvert(v.x, v.z);
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

	public List<Vector2> MotionPath {
		get { return _path; }
	}
}