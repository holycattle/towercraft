using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageController : MonoBehaviour {
	// Types of Messages
	public const int MSG_PICKUP = 0;
	public const int MSG_WARNING = 1;
	public const int MSG_HUD = 2;
	public const int MSG_DIALOG = 3;

	// Single Message Dimensions
	public const float PICKUP_STARTPOSX = 0.025f;	// [0, 1] How far down in the screen you start drawing
	public const float PICKUP_STARTPOSY = 0.2f;
	public const float HUD_STARTPOSY = 0.2f;
	public const float DIALOG_BOX_WIDTH = 512;
	public const float DIALOG_BOX_HEIGHT = 128;
	public const int BOX_WIDTH = 512;
	public const int BOX_HEIGHT = 40;

	// Colour Lerping Constants
	protected const float STARTCOLORCHANGE = 0.9f;	// How long during the interval until you start fading out
	protected const int COLORCHANGES = 10;			// # of colour changes

	// Message Constants
	public const int MAX_NUM_MSG = 12;
	public const float DEFAULT_TEXT_DURATION = 2.5f;

	//
	public GUISkin messageSkin;
	public GUISkin dialogSkin;
	private List<Message> messages;		// Houses the list of ALL messages.

	// User Defined
	public bool pickupMessagesEnabled = true;

	void Start() {
		messageSkin = Resources.Load("Fonts/AnonymousSkin", typeof(GUISkin)) as GUISkin;
		dialogSkin = Resources.Load("Fonts/DialogSkin") as GUISkin;

		messages = new List<Message>();
	}
	
	void Update() {
		for (int i = 0; i < messages.Count; i++) {
			messages[i].Update();
			if (messages[i].destroyAfterUpdate) {
				messages.RemoveAt(i);
				i--;
			}
		}
	}

	void OnGUI() {
		// Set the Skin
		GUI.skin = messageSkin;

		// Draw each message to the screen
		foreach (Message m in messages)
			m.DrawToGUI();

		// Nullify the Skin for the next GUI function to use
		GUI.skin = null;
	}

	public void AddMessage(string s, int type) {
		AddMessage(s, type, DEFAULT_TEXT_DURATION);
	}

	public void AddMessage(string s, int type, float duration) {
		if (this.enabled == true)
			AddMessage(new Message(this, s, type, duration));
	}

	private void AddMessage(Message m) {
		// Add the message to the top of the list
		messages.Insert(0, m);

		int numMessagesOfType = 1;
		for (int i = 0; i < messages.Count; i++) {
			Message tm = messages[i];
			if (tm.msgType == m.msgType) {
				numMessagesOfType++;

				if (numMessagesOfType > MAX_NUM_MSG) {
					messages.Remove(tm);
					i--;
					continue;
				}

				// Move it down
				tm.MoveRectangle(1);
			}
		}
	}

	public void ItemMessage(string itemName) {
		if (pickupMessagesEnabled)
			AddMessage("Picked up " + itemName, MSG_PICKUP);
	}

	public void WarningMessage(string warning) {
		AddMessage(warning, MSG_WARNING);
	}

	public void HUDMessage(string msg, float duration) {
		AddMessage(msg, MSG_HUD, duration);
	}

	public void DialogMessage(string msg, float duration) {
		AddMessage(msg, MSG_DIALOG, duration);
	}

	public class Message {
		public int msgType;
		public string msg;

		// Should destroy at the end of the update loop?
		public bool destroyAfterUpdate = false; 		// Should destroy at the end of update loop?

		// Colour Lerping
		protected float interval;
		protected float startTime;
		protected int move;

		// Drawing Info
		protected GUISkin skin;
		protected TextAnchor anchor;
		protected Rect r;
		protected Color textColor;
		protected bool drawOutline = true;
		protected Color outlineColor = new Color(0, 0, 0);

		public Message (MessageController m, string mess, int type, float duration) {
			msg = mess;
			msgType = type;

			float posX = 0;
			float posY = 0;
			float width = BOX_WIDTH;
			float height = BOX_HEIGHT;

			skin = m.messageSkin;

			// Set Message Specific Variables
			switch (msgType) {
				case MSG_PICKUP:
					anchor = TextAnchor.MiddleLeft;
					textColor = new Color(0f, 0.8f, 0f, 1f);	// Light Green
					height = BOX_HEIGHT / 2;
					posX = Screen.width * PICKUP_STARTPOSX;
					posY = Screen.height * PICKUP_STARTPOSY;
					break;
				case MSG_WARNING:
					anchor = TextAnchor.MiddleCenter;
					textColor = new Color(1f, 0f, 0f, 1f);		// Red
					posX = (Screen.width - width) / 2;
					posY = 0;
					break;
				case MSG_HUD:
					anchor = TextAnchor.MiddleCenter;
					textColor = new Color(1f, 0f, 0f, 1f);		// Red
					height = BOX_HEIGHT / 2;
					posX = (Screen.width - width) / 2;
					posY = Screen.height * HUD_STARTPOSY;
					break;
				case MSG_DIALOG:
					anchor = TextAnchor.MiddleLeft;
					skin = m.dialogSkin;
					skin.box.fontSize = 21;
					drawOutline = false;
					textColor = new Color(1f, 1f, 1f, 1f);		// White

					width = DIALOG_BOX_WIDTH;
					height = DIALOG_BOX_HEIGHT;
					posX = (Screen.width - width) / 2;
					posY = (Screen.height - height) / 2 - 50;
					break;
			}
			move = 0;
			r = new Rect(posX, posY, width, height);

			startTime = Time.time;
			interval = duration;
		}

		public void Update() {
			if (Time.time >= startTime + STARTCOLORCHANGE * interval + ((1 - STARTCOLORCHANGE) * interval / COLORCHANGES) * move) {
				move++;
				textColor.a = textColor.a - (1.0f / COLORCHANGES) * move;
				if (drawOutline)
					outlineColor.a = outlineColor.a - (1.0f / COLORCHANGES) * move;
			}

			if (Time.time >= startTime + interval) {
				destroyAfterUpdate = true;
			}
		}

		public void MoveRectangle(int x) {
			MoveRectangle(0, x * r.height);
		}

		public void MoveRectangle(float x, float y) {
			MoveRectangle(new Vector2(x, y));
		}

		public void MoveRectangle(Vector2 v) {
			r.center += v;
		}

		public void DrawToGUI() {
			GUI.skin = skin;
			GUI.skin.label.alignment = anchor;
			if (drawOutline) {
				GUI.color = outlineColor;
				r.x--;
				GUI.Box(r, msg);
				r.x += 2;
				GUI.Box(r, msg);
				r.x--;
				r.y--;
				GUI.Box(r, msg);
				r.y += 2;
				GUI.Box(r, msg);
				r.y--;
			}

			GUI.color = textColor;
			GUI.Box(r, msg);
		}
	}
}
