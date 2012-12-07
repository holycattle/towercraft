using UnityEngine;
using System.Collections;

public class FinishedGameScreen : MonoBehaviour {

	float time;

	void Start() {
		time = 5;
		Time.timeScale = 1;
	}

	void Update() {
		time -= Time.deltaTime;
		if (time <= 0) {
			LoadCredits();
		}
	}

	void LoadCredits() {
		AutoFade.LoadLevel("Credits", 3, 1, Color.black);
	}

	// Update is called once per frame
	void OnGUI() {
		const int WIDTH = 256;
		const int HEIGHT = 128;

		Rect midRect = new Rect((Screen.width - WIDTH) / 2, (Screen.height - HEIGHT) / 2, WIDTH, HEIGHT);
		GUI.skin = Resources.Load("Skins/DialogSkin") as GUISkin;
		GUI.skin.label.fontSize = 30;
		GUI.Label(midRect, "To be continued...");
	}
}
