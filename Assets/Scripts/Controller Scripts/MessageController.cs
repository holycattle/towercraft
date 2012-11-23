using UnityEngine;
using System.Collections;

public class MessageController : MonoBehaviour {
	private bool ItemAnnouncement = false;
	private string message;
	
	void OnGUI() {
		GUISkin AnonymousSkin = (GUISkin)Resources.Load("AnonymousSkin");
		GUI.skin = AnonymousSkin;
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