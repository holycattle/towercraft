using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour {
//	GUITexture t = null;
	const int BUTTON_HEIGHT = 128;
	GUIStyle[] buttonStyles;
	Texture2D[][] buttons;
	Rect[] buttonRects;

	void Start() {
		string[] buttonNames = {"newgame", "credits", "exit"};
		buttons = new Texture2D[3][];
		buttonRects = new Rect[3];
		buttonStyles = new GUIStyle[3];
		for (int i = 0; i < buttons.Length; i++) {
			buttons[i] = new Texture2D[3];
			for (int o =0; o < 3; o++) {
				buttons[i][o] = Resources.Load("Textures/GUI/MainMenu/" + buttonNames[i] + o + "b", typeof(Texture)) as Texture2D;
			}
			buttonStyles[i] = new GUIStyle();
			buttonStyles[i].normal.background = buttons[i][0];
			buttonStyles[i].hover.background = buttons[i][1];
			buttonStyles[i].active.background = buttons[i][2];
			buttonRects[i] = new Rect(32, Screen.height * 0.4f + (i * BUTTON_HEIGHT),
				buttons[i][0].width, buttons[i][0].height);
		}
	}

	void OnGUI() {
		Screen.lockCursor = false;
		bool startGame = GUI.Button(buttonRects[0], "", buttonStyles[0]);
		bool credits = GUI.Button(buttonRects[1], "", buttonStyles[1]);
		bool exitGame = GUI.Button(buttonRects[2], "", buttonStyles[2]);

		if (startGame)
			AutoFade.LoadLevel("Main", 3, 1, Color.black);

//		if (credits)
//			;

		if (exitGame)
			Application.Quit();
	}
}
