using UnityEngine;
using System.Collections;

public class StartScreen : MonoBehaviour {
	GUITexture t = null;
	
	const int BUTTON_WIDTH = 100;
	const int BUTTON_HEIGHT = 50;
	
	void OnGUI() {
		Screen.lockCursor = false;
		Rect startGameRect = new Rect((Screen.width - BUTTON_WIDTH)/2, (Screen.height - BUTTON_HEIGHT)/2 + 50, BUTTON_WIDTH, BUTTON_HEIGHT);
		bool startGame = GUI.Button(startGameRect, "Start Game");
		
		Rect exitGameRect = new Rect((Screen.width - BUTTON_WIDTH)/2, (Screen.height - BUTTON_HEIGHT)/2 + 150, BUTTON_WIDTH, BUTTON_HEIGHT);
		bool exitGame = GUI.Button(exitGameRect, "Exit Game");
//		Debug.Log("test");
		if(startGame)
			AutoFade.LoadLevel("Main" ,3,1,Color.black);
		
		if(exitGame)
			Application.Quit();
	}
}
