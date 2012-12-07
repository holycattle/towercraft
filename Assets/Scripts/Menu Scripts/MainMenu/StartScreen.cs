using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour {
	Texture2D bg;
	const int BUTTON_HEIGHT = 128;
	GUIStyle[] buttonStyles;
	Texture2D[][] buttons;
	Rect[] buttonRects;

	void Start() {
		bg = Resources.Load("Textures/GUI/MainMenu2/bg0", typeof(Texture)) as Texture2D;

		string[] buttonNames = {"newgame", "credits", "exit"};
		buttons = new Texture2D[3][];
		buttonRects = new Rect[3];
		buttonStyles = new GUIStyle[3];
		for (int i = 0; i < buttons.Length; i++) {
			buttons[i] = new Texture2D[3];
			for (int o = 0; o < 2; o++) {
				buttons[i][o] = Resources.Load("Textures/GUI/MainMenu2/" + buttonNames[i] + o, typeof(Texture)) as Texture2D;
			}
			buttonStyles[i] = new GUIStyle();
			buttonStyles[i].normal.background = buttons[i][0];
			buttonStyles[i].hover.background = buttons[i][1];
			buttonRects[i] = new Rect(32, Screen.height * 0.4f + (i * BUTTON_HEIGHT),
				(buttons[i][0].width / buttons[i][0].height) * BUTTON_HEIGHT, BUTTON_HEIGHT);
		}
	}

	void OnGUI() {
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), bg);

		Screen.lockCursor = false;
		bool startGame = GUI.Button(buttonRects[0], "", buttonStyles[0]);
		bool credits = GUI.Button(buttonRects[1], "", buttonStyles[1]);
		bool exitGame = GUI.Button(buttonRects[2], "", buttonStyles[2]);

		if (startGame)
			AutoFade.LoadLevel("Main", 3, 1, Color.black);

		if (credits)
			AutoFade.LoadLevel("Credits", 3, 1, Color.black);

		if (exitGame)
			Application.Quit();
	}
}
