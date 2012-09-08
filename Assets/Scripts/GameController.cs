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
	
	// Private Variables
	private GridController[] _map;
	private List<Vector2> path;
	private int _livesLeft;
	
	void Start () {
		if (mapWidth * mapHeight == 0) {
			mapWidth = 16;
			mapHeight = 16;
		}
		_map = new GridController[mapWidth * mapHeight];
		
		// Instantiate the Tiles
		for (int y = 0; y < mapHeight; y++) {
			for (int x = 0; x < mapWidth; x++) {				
				GameObject t = Instantiate(tilePrefab, new Vector3(startXPosition + x * 4 + 2, 0, startYPosition + y * 4 + 2), Quaternion.identity) as GameObject;
				t.name = "Grid:" + x + "," + y;
				_map[y * mapWidth + x] = t.GetComponent<GridController>();
			}
		}
		
		_livesLeft = MAX_LIVES;
		
		UpdatePath();
	}
	
	void Update () {
		if (path != null) {
			for (int i = 0; i < path.Count - 1; i++) {
				Debug.DrawLine(new Vector3(path[i].x, 1, path[i].y),
					new Vector3(path[i + 1].x, 1,path[i + 1].y));
			}
		}
		
		if (Input.GetKeyDown(KeyCode.Space)) {
			GameObject g = Instantiate(enemyPrefab, new Vector3(startXPosition + 2, 2, startYPosition - 2), Quaternion.identity) as GameObject	;
			MovementController m = g.GetComponent<MovementController>();
			m.Path = path;
		}
	}
	
	public void UpdatePath() {
		// Pathfind!
		path = AStar.PathFind(new Vector2(0, 0), new Vector2(mapWidth - 1, mapHeight - 1), _map, mapWidth, mapHeight);
		
		if (path != null) {
			// Convert Path to real coordinates.
			List<Vector2> act = new List<Vector2>();
			while (path.Count > 0) {
				act.Add(new Vector2(startXPosition + path[0].x * 4 + 2, startYPosition + path[0].y * 4 + 2));
				path.RemoveAt(0);
			}
			path = act;
			
			// Append Coords of the End Portal
			path.Add(new Vector2(startXPosition + (mapWidth - 1) * 4 + 2, startYPosition + mapHeight * 4 + 2));
		}
	}
	
	public GridController GetTile(int x, int y) {
		if (x >= mapWidth) x = mapWidth - 1;
		if (y >= mapHeight) y = mapHeight - 1;
		if (x < 0) x = 0;
		if (y < 0) y = 0;
		
		return _map[y * mapWidth + x];
	}
	
	public int Lives {
		get { return _livesLeft; }
	}
}