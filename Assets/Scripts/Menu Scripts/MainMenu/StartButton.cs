using UnityEngine;
using System.Collections;

public class StartButton : MonoBehaviour {
	GUITexture t = null;

	void OnMouseDown() {
		t = GameObject.Find("StartGame").guiTexture;
		t.texture = (Texture)Resources.Load("Textures/Web_Button_Gray_Hover");
	}
	
	void OnMouseUp() {
		t.texture = (Texture)Resources.Load("Textures/Web_Button_Gray");
		Application.LoadLevel("Main");
	}
}
