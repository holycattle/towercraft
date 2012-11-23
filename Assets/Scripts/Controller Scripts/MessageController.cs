using UnityEngine;
using System.Collections;

public class MessageController : MonoBehaviour {
	private const string PICKUP_MESSAGE = "Picked up ";
	private bool ItemAnnouncement = false;
	private string message = "";
	public GUISkin messageSkin;
	public float TEXT_DURATION; //this is changeable so that it can be modified based on the need/type of text
	private Color alphaContainer;
	private float delta;
	//private bool startDeltaTimer = false;
	
	void Start() {
		delta = 0;
		TEXT_DURATION = 3f;
		//fadingTime = 3f;
		//timeDelta = 0;
		//alphaContainer = new Color(1.0f, 1.0f, 1.0f, 1.0f);
		//fix this later
		//messageSkin = (GUISkin)Resources.Load("Assets/Fonts/AnonymousSkin", typeof(GUISkin));
	}
	
	void Update() {
		if (ItemAnnouncement)
			delta += 1.0f * Time.deltaTime;
	}
	
	void OnGUI() {
		//initialize custom GUISkin settings
		GUI.skin = messageSkin;
		GUI.skin.label.alignment = TextAnchor.UpperCenter;

		if (ItemAnnouncement) {
			AnnounceItem();
		}
	}
	
	public void ItemMessage(string itemName) {
		if(this.enabled == true) {
			if (delta == 0) {
				message += PICKUP_MESSAGE + itemName;
				delta = 0;
			} else {
				message += "\n" + PICKUP_MESSAGE + itemName;
				delta = 0.1f; //just to make sure that it doesn't revert to 0; fixes newline bug
			}
		}
		
		ItemAnnouncement = true;
	}
	
	private void AnnounceItem() {
		int w = 400;
		int h = 300;
		Rect rect = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);
		GUI.Label(rect, message);
		
		//remove text after TEXT_DURATION seconds
		if (delta >= TEXT_DURATION) {
			ItemAnnouncement = false;
			message = "";
			delta = 0;
		}
	}
}