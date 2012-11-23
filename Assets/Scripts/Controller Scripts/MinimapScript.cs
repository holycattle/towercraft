using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MinimapScript : MonoBehaviour {

	// Update Counter
	private const int UPDATELIMIT = (int)(60.0f / 15);	// = 60 / #ofUpdatesPerSec
	private int updateCounter;

	// Images
	public GUIStyle miniMap;
	public GUIStyle playerIcon;
	public GUIStyle towerIcon;
	public GUIStyle mobIcon;

	// Dimensions
//	private int gridWidth;
//	private int gridHeight;
	private int cornerSpacing = 8;
	private int mapWidth = 192;
	private int mapHeight = 192;
	private int terrainWidth = 192;
	private int terrainHeight = 192;
	private int towerIconSize = 8;
	private int playerIconSize = 16;
	private int _playerRotation;
	private int mobIconSize = 8;
	private Rect mapGroup;


	// Transforms of Objects
	private Rect[] movables;
	private Rect[] towers;

	void Start() {
		mapGroup = new Rect(Screen.width - cornerSpacing - mapWidth, Screen.height - cornerSpacing - mapHeight, mapWidth, mapHeight);

		UpdateTowerList();
		UpdateMobList();
	}

	void FixedUpdate() {
		updateCounter++;
		if (updateCounter == 5) {	// Updates 12 times a second
			UpdateMobList();
			updateCounter = 0;
		}
	}

	void OnGUI() {
		GUI.BeginGroup(mapGroup, miniMap);
		for (int i= 0; i < towers.Length; i++) {
			GUI.Box(towers[i], "", towerIcon);
		}

		GUIUtility.RotateAroundPivot(_playerRotation, movables[0].center);
		GUI.Box(movables[0], "", playerIcon);
		GUIUtility.RotateAroundPivot(-_playerRotation, movables[0].center);

		for (int i = 1; i < movables.Length; i++) {
			GUI.Box(movables[i], "", mobIcon);
		}
		GUI.EndGroup();
	}

	public void UpdateMobList() {
		GameObject player = GameObject.FindGameObjectWithTag("Player");
		GameObject[] mobs = GameObject.FindGameObjectsWithTag("Enemy");
		movables = new Rect[mobs.Length + 1];

		movables[0] = new Rect(GetMapPosition(player.transform.position.x, terrainWidth) - playerIconSize / 2,
				mapHeight - GetMapPosition(player.transform.position.z, terrainHeight) - playerIconSize / 2,
				playerIconSize, playerIconSize);
		_playerRotation = (int)player.transform.rotation.eulerAngles.y;
//		Debug.Log("Length: " + mobs.Length);
		for (int i = 1; i < mobs.Length + 1; i++) {
//			Debug.Log("Movables: " + i + " > " + mobs[i].name);
			movables[i] =
				new Rect(GetMapPosition(mobs[i - 1].transform.position.x, terrainWidth) - mobIconSize / 2,
				mapHeight - GetMapPosition(mobs[i - 1].transform.position.z, terrainHeight) - mobIconSize / 2,
				mobIconSize, mobIconSize);
		}
	}

	public void UpdateTowerList() {
		GameObject[] temp = GameObject.FindGameObjectsWithTag("Tower");
		towers = new Rect[temp.Length];
		for (int i = 0; i < temp.Length; i++) {
			towers[i] = new Rect(GetMapPosition(temp[i].transform.position.x, terrainWidth) - towerIconSize / 2,
				mapHeight - GetMapPosition(temp[i].transform.position.z, terrainHeight) - towerIconSize / 2,
				towerIconSize, towerIconSize);
		}
	}

	private int GetMapPosition(float x, int terrainSize) {
		return (int)(x + terrainSize / 2);
	}
}
