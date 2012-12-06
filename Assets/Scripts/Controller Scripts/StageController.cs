using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageController : MonoBehaviour {
	const int FIRST_MESSAGE_TIME = 12;
	const int SECOND_MESSAGE_TIME = FIRST_MESSAGE_TIME + LONG_MESSAGE_DURATION + 1;
	const int THIRD_MESSAGE_TIME = SECOND_MESSAGE_TIME + LONG_MESSAGE_DURATION + 1;
	const int FOURTH_MESSAGE_TIME = THIRD_MESSAGE_TIME + SHORT_MESSAGE_DURATION + 1;
	const int FIFTH_MESSAGE_TIME = FOURTH_MESSAGE_TIME + LONG_MESSAGE_DURATION + 1;
	const int SIXTH_MESSAGE_TIME = FIFTH_MESSAGE_TIME + LONG_MESSAGE_DURATION + 1;
	const int SEVENTH_MESSAGE_TIME = SIXTH_MESSAGE_TIME + LONG_MESSAGE_DURATION + 1;
	const int EIGTH_MESSAGE_TIME = SEVENTH_MESSAGE_TIME + LONG_MESSAGE_DURATION + 4;
	const int NINTH_MESSAGE_TIME = EIGTH_MESSAGE_TIME + LONG_MESSAGE_DURATION + 3;
	const int TENTH_MESSAGE_TIME = NINTH_MESSAGE_TIME + LONG_MESSAGE_DURATION + 2;
	
	const int SHORT_MESSAGE_DURATION = 5;
	const int LONG_MESSAGE_DURATION = 10;
	private Hashtable bossStages = new Hashtable();
	private WaveController waveController;
	private GameController _game;
	public List<TimeEvent> timeEvents;
	public List<WaveEvent> waveEvents;
	
	public int finalWave;
	
	//hard-coded crap
	public bool waveHasStarted;
		
	public bool firstMessage;
	public bool secondMessage;
	public bool thirdMessage;
	public bool fourthMessage;
	public bool fifthMessage;
	public bool sixthMessage;
	public bool seventhMessage;
	public bool eigthMessage;
	public bool ninthMessage;
	public bool tenthMessage;
	public bool eleventhMessage;
	
	public StageController(int f) {
		finalWave = f;
		//waveController = GameObject.Find(" GameController").GetComponentInChildren<WaveController>();
	}
	

	// Use this for initialization
	void Start () {
		Time.timeScale = 10f;
		firstMessage = false;
		_game = GameObject.Find(" GameController").GetComponent<GameController>();
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
		messageEvent = new TimeEvent((float)FOURTH_MESSAGE_TIME);
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

		//sixth event
		messageEvent = new TimeEvent((float)SIXTH_MESSAGE_TIME);
		messageEvent.addAction(receiveSixthMessage, args);
		timeEvents.Add(messageEvent); //don't forget to add it to the list!
		
		//seventh event
		messageEvent = new TimeEvent((float)SEVENTH_MESSAGE_TIME);
		messageEvent.addAction(receiveSeventhMessage, args);
		timeEvents.Add(messageEvent); //don't forget to add it to the list!
		
		//eigth event
		messageEvent = new TimeEvent((float)EIGTH_MESSAGE_TIME);
		messageEvent.addAction(receiveEigthMessage, args);
		timeEvents.Add(messageEvent); //don't forget to add it to the list!
		
		//ninth event
		messageEvent = new TimeEvent((float)NINTH_MESSAGE_TIME);
		messageEvent.addAction(receiveNinthMessage, args);
		timeEvents.Add(messageEvent); //don't forget to add it to the list!
		
		//tenth event
		messageEvent = new TimeEvent((float)TENTH_MESSAGE_TIME);
		messageEvent.addAction(receiveTenthMessage, args);
		timeEvents.Add(messageEvent); //don't forget to add it to the list!
		
		//11th event - a WaveEvent
		WaveEvent waveEvent = new WaveEvent(1);
		waveEvent.addAction(waveStarted, args);
		waveEvents.Add(waveEvent);
		
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
		s._game.Messenger.HUDMessage("MULTIPLE UNIDENTIFIED CONTACTS DETECTED.", 5f);
		
	}
	
	public static void waveStarted(Hashtable args) {
		StageController s = args[0] as StageController;
		s.waveHasStarted = true;
		
	}
	
	public static void receiveFourthMessage(Hashtable args) {
		StageController s = args[0] as StageController;
		s.fourthMessage = true;
	}
	
	public static void receiveFifthMessage(Hashtable args) {
		StageController s = args[0] as StageController;
		s.fifthMessage = true;
	}
	
	public static void receiveSixthMessage(Hashtable args) {
		StageController s = args[0] as StageController;
		s.sixthMessage = true;
	}
	
	public static void receiveSeventhMessage(Hashtable args) {
		StageController s = args[0] as StageController;
		s.seventhMessage = true;
	}
	
	public static void receiveEigthMessage(Hashtable args) {
		StageController s = args[0] as StageController;
		s.eigthMessage = true;
	}
	
	public static void receiveNinthMessage(Hashtable args) {
		StageController s = args[0] as StageController;
		s.ninthMessage = true;
	}
	
	public static void receiveTenthMessage(Hashtable args) {
		StageController s = args[0] as StageController;
		s.tenthMessage = true;
	}
	
	public static void receive11thMessage(Hashtable args) {
		StageController s = args[0] as StageController;
		s.eleventhMessage = true;
	}
	
	public void OnGUI() {
		GUI.skin = Resources.Load("Skins/DialogSkin") as GUISkin;
		GUI.skin.box.fontSize = 21;
		const float DIALOG_BOX_WIDTH = 512;
		const float DIALOG_BOX_HEIGHT = 128;
		Rect upperCenterRect = new Rect((Screen.width - DIALOG_BOX_WIDTH)/2, (Screen.height - DIALOG_BOX_HEIGHT)/2 - 25, DIALOG_BOX_WIDTH, DIALOG_BOX_HEIGHT);
		if(firstMessage) {
			GUI.Box(upperCenterRect, "Dr. Pierce:\n" + "Good morning. I see you’re taking well to the new suit." +
				"You’ll notice some changes to the UI since you last stepped into it," +
				"so I’ll just run you down through the basics before we get you up to speed on the situation.");
			if(Time.timeSinceLevelLoad > FIRST_MESSAGE_TIME + LONG_MESSAGE_DURATION) {
				firstMessage = false;
			}
		}
		if(secondMessage) {
			GUI.Box(upperCenterRect, "Dr. Pierce:\nUse WSAD to move forward, backward, and side to side. Move the mouse in any" +
				"direction to look in that direction.");
			if(Time.timeSinceLevelLoad > SECOND_MESSAGE_TIME + LONG_MESSAGE_DURATION) {
				secondMessage = false;
			}
		}
		if(thirdMessage) {
			GUI.Box(upperCenterRect, "Dr. Pierce:\nGood, looks like everything is in order.");
			if(Time.timeSinceLevelLoad > THIRD_MESSAGE_TIME + SHORT_MESSAGE_DURATION) {
				thirdMessage = false;
			}
		}
		if(fourthMessage) {
			GUI.Box(upperCenterRect, "Dr. Pierce:\nBandits. That’s not good. All right, I’ll teach you the interface as we defend." +
				"Sorry about that, I guess timing is everything these days.");
			if(Time.timeSinceLevelLoad > FOURTH_MESSAGE_TIME + LONG_MESSAGE_DURATION - 4) {
				fourthMessage = false;
			}
		}
		if(fifthMessage) {
			GUI.Box(upperCenterRect, "Dr. Pierce:\n Press ‘1’ to switch to the nanolathe. You can assemble" +
				"defensive structures from items in" +
				"your inventory using this. For now, we’ll build your stock turrets because we" +
				"don’t have the time or resources for more advanced defenses.");
			if(Time.timeSinceLevelLoad > FIFTH_MESSAGE_TIME + LONG_MESSAGE_DURATION) {
				fifthMessage = false;
			}
		}
		if(sixthMessage) {
			GUI.Box(upperCenterRect, "Dr. Pierce:\nWhile the nanolathe is selected, you’ll notice a highlighted square on " +
				"the ground where it’s pointing. This is an augmented reality contrivance we had to put in to ensure that " +
				"you point the nanolathe projector in the right direction.");
			if(Time.timeSinceLevelLoad > SIXTH_MESSAGE_TIME + LONG_MESSAGE_DURATION) {
				sixthMessage = false;
			}
		}
		if(seventhMessage) {
			GUI.Box(upperCenterRect, "Dr. Pierce:\n Left-click on a highlighted space in front of you, then" +
				"select a base and a turret type. Right now you don’t have much in the way of choices," +
				"but I doubt that this raid will consist of any more than small fry.");
			if(Time.timeSinceLevelLoad > SEVENTH_MESSAGE_TIME + LONG_MESSAGE_DURATION) {
				seventhMessage = false;
			}
		}
		if(eigthMessage) {
			GUI.Box(upperCenterRect, "Dr. Pierce:\nGood. The turret’s functional. Better than having to type shell " +
				"commands to assemble turrets, right?");
			if(Time.timeSinceLevelLoad > EIGTH_MESSAGE_TIME + SHORT_MESSAGE_DURATION) {
				eigthMessage = false;
			}
		}
		if(ninthMessage) {
			GUI.Box(upperCenterRect, "Dr. Pierce:\nAlternately, you can press C to use our tower warp guidance system to " +
				"for a more convenient overhead view. You can also warp in towers from there. However, you can only use this " +
				"when there are no enemies around.");
			if(Time.timeSinceLevelLoad > NINTH_MESSAGE_TIME + LONG_MESSAGE_DURATION) {
				ninthMessage = false;
			}
		}
		if(tenthMessage) {
			GUI.Box(upperCenterRect, "Dr. Pierce:\nHere they come. Watch the fireworks!");
			if(Time.timeSinceLevelLoad > TENTH_MESSAGE_TIME + SHORT_MESSAGE_DURATION) {
				tenthMessage = false;
			}
		}
		//hard-coded </3
		if(waveHasStarted && waveController.waveNumber == 1 && !waveController._waveActive) {
			if(_game.Lives == GameController.MAX_LIVES)
				GUI.Box(upperCenterRect, "Dr. Pierce:\nGood work. We got them.");
			else GUI.Box(upperCenterRect, "Damn it, he got through. The port to Eden has sustained some damage. Next time be more vigilant.");
			//if(Time.timeSinceLevelLoad > ELEVENTH + SHORT_MESSAGE_DURATION) {
			Invoke("endWaveMessage", 5f);
			
		}

	}
	
	public void endWaveMessage() {
		waveHasStarted = false;
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
