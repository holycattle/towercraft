using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageController : MonoBehaviour {
	const int FIRST_MESSAGE_TIME = 14;
	const int SECOND_MESSAGE_TIME = FIRST_MESSAGE_TIME + MESSAGE_DURATION + 3;
	const int THIRD_MESSAGE_TIME = SECOND_MESSAGE_TIME + MESSAGE_DURATION + 3;
	const int FOURTH_MESSAGE_TIME = SECOND_MESSAGE_TIME + MESSAGE_DURATION + 7;
	const int FIFTH_MESSAGE_TIME = SECOND_MESSAGE_TIME + MESSAGE_DURATION + 3;
	const int MESSAGE_DURATION = 9;
	private Hashtable bossStages = new Hashtable();
	private WaveController waveController;
	private MessageController _messenger;
	public List<TimeEvent> timeEvents;
	public List<WaveEvent> waveEvents;
	
	public int finalWave;
		
	public bool firstMessage;
	public bool secondMessage;
	public bool thirdMessage;
	public bool fourthMessage;
	public bool fifthMessage;
	
	public StageController(int f) {
		finalWave = f;
		//waveController = GameObject.Find(" GameController").GetComponentInChildren<WaveController>();
	}
	

	// Use this for initialization
	void Start () {
		Time.timeScale = 10f;
		firstMessage = false;
		_messenger = GameObject.Find(" GameController").GetComponent<GameController>().Messenger;
		waveController = GameObject.Find(" GameController").GetComponentInChildren<WaveController>();
		timeEvents = new List<TimeEvent>();
		waveEvents = new List<WaveEvent>();
		
		//initialize events here
		TimeEvent messageEvent = new TimeEvent((float)FIRST_MESSAGE_TIME);
		Hashtable args = new Hashtable();
		args[0] = this;
		messageEvent.addAction(receiveFirstMessage, args);
		timeEvents.Add(messageEvent); //don't forget to add it to the list!
		
		//second event
		messageEvent = new TimeEvent((float)SECOND_MESSAGE_TIME);
		messageEvent.addAction(receiveSecondMessage, args);
		timeEvents.Add(messageEvent); //don't forget to add it to the list!
		
		//third event
		messageEvent = new TimeEvent((float)THIRD_MESSAGE_TIME);
		messageEvent.addAction(receiveThirdMessage, args);
		timeEvents.Add(messageEvent); //don't forget to add it to the list!
		
		//fourth-a event
		messageEvent = new TimeEvent((float)THIRD_MESSAGE_TIME);
		messageEvent.addAction(incomingEnemyMessage, args);
		timeEvents.Add(messageEvent); //don't forget to add it to the list!
		
		//fourth-b event
		messageEvent = new TimeEvent((float)FOURTH_MESSAGE_TIME);
		messageEvent.addAction(receiveFourthMessage, args);
		timeEvents.Add(messageEvent); //don't forget to add it to the list!
		
		//fifth event
		messageEvent = new TimeEvent((float)FIFTH_MESSAGE_TIME);
		messageEvent.addAction(receiveFifthMessage, args);
		timeEvents.Add(messageEvent); //don't forget to add it to the list!
		
	}
	
	// Update is called once per frame
	public virtual void Update () {
		listenForEvents();
	}
	
	/*public void newSkybox(Hashtable h) {
		Debug.Log ("should change skybox here");
		Skybox s = GameObject.Find("Main Camera").GetComponent<Skybox>();
		s.material = Resources.Load("Skyboxes/Skybox17", typeof(Material)) as Material;
	}*/
	
	public static void receiveFirstMessage(Hashtable args) {
		StageController s = args[0] as StageController;
		s.firstMessage = true;
	}

	public static void receiveSecondMessage(Hashtable args) {
		StageController s = args[0] as StageController;
		s.secondMessage = true;
	}
	
	public static void receiveThirdMessage(Hashtable args) {
		StageController s = args[0] as StageController;
		s.thirdMessage = true;
	}
	
	public static void incomingEnemyMessage(Hashtable args) {
		StageController s = args[0] as StageController;
		s._messenger.HUDMessage("MULTIPLE UNIDENTIFIED CONTACTS DETECTED.", 4f);
		
	}
	
	public static void receiveFourthMessage(Hashtable args) {
		StageController s = args[0] as StageController;
		s.fourthMessage = true;
	}
	
	public static void receiveFifthMessage(Hashtable args) {
		StageController s = args[0] as StageController;
		s.fifthMessage = true;
	}
	
	public void OnGUI() {
		GUI.skin = Resources.Load("Skins/DialogSkin") as GUISkin;
		GUI.skin.box.fontSize = 21;
		const float DIALOG_BOX_WIDTH = 512;
		const float DIALOG_BOX_HEIGHT = 128;
		Rect upperCenterRect = new Rect((Screen.width - DIALOG_BOX_WIDTH)/2, (Screen.height - DIALOG_BOX_HEIGHT)/2, DIALOG_BOX_WIDTH, DIALOG_BOX_HEIGHT);
		if(firstMessage) {
			GUI.Box(upperCenterRect, "Dr. Pierce:\n" + "Good morning. I see you’re taking well to the new suit." +
				"You’ll notice some changes to the UI since you last stepped into it," +
				"so I’ll just run you down through the basics before we get you up to speed on the situation.");
			if(Time.timeSinceLevelLoad > FIRST_MESSAGE_TIME + MESSAGE_DURATION) {
				firstMessage = false;
			}
		}
		if(secondMessage) {
			GUI.Box(upperCenterRect, "Dr. Pierce:\nUse WSAD to move forward, backward, and side to side. Move the mouse in any" +
				"direction to look in that direction.");
			if(Time.timeSinceLevelLoad > SECOND_MESSAGE_TIME + MESSAGE_DURATION) {
				secondMessage = false;
			}
		}
		if(thirdMessage) {
			GUI.Box(upperCenterRect, "Dr. Pierce:\nGood, looks like everything is in order.");
			if(Time.timeSinceLevelLoad > THIRD_MESSAGE_TIME + MESSAGE_DURATION - 9) {
				thirdMessage = false;
			}
		}
		if(fourthMessage) {
			GUI.Box(upperCenterRect, "Dr. Pierce:\nBandits. That’s not good. All right, I’ll teach you the interface as we defend." +
				"Sorry about that, I guess timing is everything these days.");
			if(Time.timeSinceLevelLoad > FOURTH_MESSAGE_TIME + MESSAGE_DURATION - 4) {
				fourthMessage = false;
			}
		}
		if(fifthMessage) {
			GUI.Box(upperCenterRect, "Dr. Pierce:\n Press ‘1’ to switch to the nanolathe. You can assemble" +
				"defensive structures from items in" +
				"your inventory using this. For now, we’ll build your stock turrets because we" +
				"don’t have the time or resources for more advanced defenses.");
			if(Time.timeSinceLevelLoad > FIFTH_MESSAGE_TIME + MESSAGE_DURATION) {
				fifthMessage = false;
			}
		}

	}
	
	public void listenForEvents() {
		//this needs garbage collection
		foreach(TimeEvent t in timeEvents) {
			if(!t.isSatisfied) { //minor optimization... I really need to find a way to really optimize this
				t.evalCondition();
			}
		}
		foreach(WaveEvent w in waveEvents) {
			if(!w.isSatisfied) { //minor optimization... I really need to find a way to really optimize this
				w.evalCondition();
			}
		}
	}
}
