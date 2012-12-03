using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageController : MonoBehaviour {
	private Hashtable bossStages = new Hashtable();
	private WaveController waveController;
	public List<TimeEvent> timeEvents;
	public List<WaveEvent> waveEvents;
	
	public int finalWave = -1;
	
	public Hashtable specialEvents = new Hashtable();
	
	public StageController(int f) {
		finalWave = f;
		//waveController = GameObject.Find(" GameController").GetComponentInChildren<WaveController>();
	}
	

	// Use this for initialization
	void Start () {
		waveController = GameObject.Find(" GameController").GetComponentInChildren<WaveController>();
		timeEvents = new List<TimeEvent>();
		waveEvents = new List<WaveEvent>();
		
		TimeEvent newTimeEvent = new TimeEvent(7f);
		newTimeEvent.addAction(newSkybox, new Hashtable());
		timeEvents.Add(newTimeEvent);
	}
	
	// Update is called once per frame
	public virtual void Update () {
		listenForEvents();
	}
	
	public void newSkybox(Hashtable h) {
		Debug.Log ("should change skybox here");
		Skybox s = GameObject.Find("Main Camera").GetComponent<Skybox>();
		s.material = Resources.Load("Skyboxes/Skybox17", typeof(Material)) as Material;
	}
	
	public bool isVictorious(Hashtable h) {
		Debug.Log("final wave = " + ((int)h[1]).ToString());
		int w = ((WaveController)h[0]).waveNumber;
		Debug.Log("current wavenumber = " + w.ToString());
		return w == (int)h[1];
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
