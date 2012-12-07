using UnityEngine;
using System.Collections;

public class CreditScreen : MonoBehaviour {
	Texture2D bg;
//	const int SX;
	const int SY = 32;
	const int BUTTON_WIDTH = 256;
	const int BUTTON_HEIGHT = 32;
	GUIStyle[] buttonStyles;
	Texture2D[][] buttons;
	Rect[] buttonRects;
	GUISkin menuSkin;
	string[] names;
	public GUIStyle menuStyle;

	void Start() {
		string[] names2 = {
			"sudo make games",
			"",
			"Programmers:",
			"Joshua Castaneda",
			"Joniel Ibasco",
			"",
			"Artist / 3D Modeller",
			"Jane Raga",
			"",
			"Storyboard / Design",
			"Anton Chua",
			"",
			"Special Thanks to...",
			"Rhem Soneja",
			"Johnel Bacani",
			"Joshua Balagapo",
			"Matthew Chua",
			"warby",
			"azure-scorch"};
		names = names2;

		menuSkin = Resources.Load("Fonts/UISkin", typeof(GUISkin)) as GUISkin;
	}

	void OnGUI() {
		GUI.skin = menuSkin;
		float sx = Screen.width * 0.38f;
		for (int i= 0; i < names.Length; i++) {
			GUI.Label(new Rect(sx - BUTTON_WIDTH / 2, SY + i * BUTTON_HEIGHT, BUTTON_WIDTH, BUTTON_HEIGHT), names[i], menuStyle);
		}

		if (GUI.Button(new Rect(Screen.width * 0.75f, Screen.height * 0.75f, Screen.width * 0.2f, Screen.height * 0.2f), "Return to Title Screen"))
			AutoFade.LoadLevel("Start", 3, 1, Color.black);
	}
}
