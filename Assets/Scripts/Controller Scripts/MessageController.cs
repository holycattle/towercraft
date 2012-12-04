using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageController : MonoBehaviour {
	// Constants
	public const int MSG_PICKUP = 0;
	public const int MSG_WARNING = 1;
	public const int BOX_WIDTH = 512;
	public const int BOX_HEIGHT = 40;
	public const int MAX_NUM_MSG = 8;
	public const float DEFAULT_TEXT_DURATION = 3f;
	public const float STARTPOS = 0.1f;	// [0, 1] How far down in the screen you start drawing
	private const string PICKUP_MESSAGE = "Picked up ";

	//
//	private bool ItemAnnouncement = false;
//	private string message = "";
	public GUISkin messageSkin;
	private int messageCounter;
	private List<Message> messages;
	
	

	void Start() {
		//fadingTime = 3f;
		//timeDelta = 0;
		//alphaContainer = new Color(1.0f, 1.0f, 1.0f, 1.0f);

		messageSkin = Resources.Load("Fonts/AnonymousSkin", typeof(GUISkin)) as GUISkin;
		messages = new List<Message>();
	}
	
	void Update() {
//		if (ItemAnnouncement)
//			delta += 1.0f * Time.deltaTime;

		for (int i = 0; i < messages.Count; i++) {
			messages[i].Update();
			if (messages[i].destroy) {
				messages.RemoveAt(i);
				i--;
			}
		}

		if (messages.Count == 0) {
			messageCounter = 0;
		}
	}

	void OnGUI() {
		//initialize custom GUISkin settings
		GUI.skin = messageSkin;
		foreach (Message m in messages) {
			switch(m.msgType) {
				case MSG_PICKUP:
					GUI.skin.label.alignment = TextAnchor.MiddleLeft;				
					break;
				case MSG_WARNING:
					GUI.skin.label.alignment = TextAnchor.MiddleCenter;
					break;
			}
			m.DrawToGUI();
		}
//		if (ItemAnnouncement) {
//			GUI.skin.label.normal.textColor = new Color(0f, 0.8f, 0f, 0.96875f);
//			AnnounceItem();
//		} else {
//			//warning message with font color red
//			GUI.skin.label.normal.textColor = new Color(1f, 0f, 0f, 0.96875f);
//			AnnounceWarning();
//		}

		GUI.skin = null;
	}

	public void ItemMessage(string itemName) {
		if (this.enabled == true) {
			string msg = PICKUP_MESSAGE + itemName;
			messages.Add(new Message(msg, MSG_PICKUP, DEFAULT_TEXT_DURATION, messageCounter));
			messageCounter = (messageCounter + 1) % MAX_NUM_MSG;
		}
	}

	public void WarningMessage(string warning) {
		if (this.enabled == true) {
			messages.Add(new Message(warning, MSG_WARNING, DEFAULT_TEXT_DURATION, messageCounter));
			messageCounter = (messageCounter + 1) % MAX_NUM_MSG;
		}
	}

//	private void AnnounceItem() {
//		int w = 400;
//		int h = 300;
//		Rect rect = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);
//		GUI.Label(rect, message);
//
//		//remove text after TEXT_DURATION seconds
//		if (delta >= TEXT_DURATION) {
//			ItemAnnouncement = false;
//			message = "";
//			delta = 0;
//		}
//	}

//	private void AnnounceWarning() {
//		int w = 400;
//		int h = 300;
//		Rect rect = new Rect((Screen.width - w) / 2, (Screen.height - h) / 2, w, h);
//		GUI.Label(rect, message);
//
//		//remove text after TEXT_DURATION seconds
//		if (delta >= TEXT_DURATION) {
//			ItemAnnouncement = false;
//			message = "";
//			delta = 0;
//		}
//	}

	public class Message {
		private const float STARTCOLORCHANGE = 0.9f;	// How long during the interval until you start fading out
		private const int COLORCHANGES = 10;			// # of colour changes
		protected string msg;
		public int msgType;
		public bool destroy; 		// Should destroy at the end of update loop?

		// Colour Lerping
		protected float interval;
		protected float startTime;
		protected int move;

		// Drawing Info
		protected Rect r;
		protected Color c;
		
		int posX;
		int posY;
		
		public int type;

		public Message (string mess, int type, float duration, int pos) {
			msg = mess;
			msgType = type;
			//int sy = BOX_HEIGHT * pos;
			// Set Color
			switch (msgType) {
				case MSG_PICKUP:
					c = new Color(0f, 0.8f, 0f, 1f);	// Light Green
					posX = 0;
					posY = (((Screen.height - BOX_WIDTH)) / 2) + (BOX_HEIGHT/2 * pos);
					break;
				case MSG_WARNING:
					c = new Color(1f, 0f, 0f, 1f);	// Red
					posX = (Screen.width - BOX_WIDTH) / 2;
					posY = BOX_HEIGHT/2 * pos;
					break;
			}
			move = 0;
//			int sy = (int)(Screen.height * BOX_HEIGHT) + BOX_HEIGHT * pos;
			r = new Rect(posX, posY, BOX_WIDTH, BOX_HEIGHT);
			

			startTime = Time.time;
			interval = duration;
			destroy = false;
		}

		public void Update() {
			if (Time.time >= startTime + STARTCOLORCHANGE * interval + ((1 - STARTCOLORCHANGE) * interval / COLORCHANGES) * move) {
				move++;
				c.a = c.a - (1.0f / COLORCHANGES) * move;
			}

			if (Time.time >= startTime + interval) {
				destroy = true;
			}
		}

		public void DrawToGUI() {
			GUI.color = c;
			GUI.Label(r, msg);
		}
	}
}
