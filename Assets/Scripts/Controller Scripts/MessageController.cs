using UnityEngine;
using System.Collections;

public class MessageController : MonoBehaviour {
	private bool ItemAnnouncement = false;
	private string message;
	public GUISkin messageSkin;
	
	void Start() {
		//fix this later
		//messageSkin = (GUISkin)Resources.Load("Fonts/AnonymousSkin");
	}
	
	void OnGUI() {
		GUI.skin = messageSkin;
		if(ItemAnnouncement) {
			AnnounceItem(message);
		}
	}
	
	public void ItemMessage(string itemName) {
		message = itemName;
		ItemAnnouncement = true;
	}
	
	private static void AnnounceItem(string message) {
		int w = 250;
		int h = 300;
		Rect rect = new Rect((Screen.width - w)/2, (Screen.height-h)/2, w, h);
		GUI.Label(rect, message);
		//GUI.TextArea(rect, message);	
	}
}