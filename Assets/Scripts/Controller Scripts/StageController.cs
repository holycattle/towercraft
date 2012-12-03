using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageController : MonoBehaviour {
	private Hashtable bossStages = new Hashtable();
	private WaveController waveController;
	public List<Event> events;
	
	public int finalWave = -1;
	
	public Hashtable specialEvents = new Hashtable();
	
	public StageController(int f) {
		finalWave = f;
		//waveController = GameObject.Find(" GameController").GetComponentInChildren<WaveController>();
	}
	

	// Use this for initialization
	void Start () {
		waveController = GameObject.Find(" GameController").GetComponentInChildren<WaveController>();
		events = new List<Event>();
		Event e = new Event();
		Hashtable args = new Hashtable();
		//Debug.Log("current wave: " + waveController.waveNumber.ToString());
		args[0] = waveController;
		args[1] = finalWave;
		e.addAction(newSkybox, args);
		e.addCondition(isVictorious, args);
		events.Add(e);
		Debug.Log ("Added " + events.ToString());
	}
	
	// Update is called once per frame
	public virtual void Update () {
		//if(waveController.waveNumber != -1 && waveController.waveNumber > finalWave) {
			//do victory scene/action
			//Camera.mainCamera.GetComponent<Skybox>().material = Resources.Load("Skyboxes/Skybox17", typeof(Material)) as Material;
		//}
		//Event e = new Event(, null);
		listenForEvents();
	}
	
	public void newSkybox(Hashtable h) {
		Debug.Log ("should change skybox here");
		Camera.mainCamera.GetComponent<Skybox>().material = Resources.Load("Skyboxes/Skybox17", typeof(Material)) as Material;
	}
	
	public bool isVictorious(Hashtable h) {
		Debug.Log("final wave = " + ((int)h[1]).ToString());
		int w = ((WaveController)h[0]).waveNumber;
		Debug.Log("current wavenumber = " + w.ToString());
		return w == (int)h[1];
	}
	
	public void listenForEvents() {
		//this needs garbage collection
		foreach(Event eventItr in events) {
			if(!eventItr.isSatisfied) { //minor optimization... I really need to find a way to really optimize this
				eventItr.evalCondition();
			}
		}
	}
}
