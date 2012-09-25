using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	private GameTool[] _weapons;
	private int _activeTool;

	void Start() {
		_weapons = new GameTool[2];
		_weapons[0] = new Builder();
		_weapons[1] = new Weapon();
	}

	// Update is called once per frame
	void Update() {
		_weapons[_activeTool].Update();
	}

	void OnGUI() {
		GUI.backgroundColor = Color.grey;
		GUI.Box(new Rect(0, 120, 128, 30), "Bullets: " + _weapons[_activeTool].bullets);
	}

	public void SetActiveWeapon(int i) {
		if (i < 0 || i >= 2 || i == _activeTool)
			return;

		_activeTool = i;
		_weapons[_activeTool].WhenEquipped();
	}

	public GameTool ActiveTool {
		get { return _weapons[_activeTool]; }
	}
}
