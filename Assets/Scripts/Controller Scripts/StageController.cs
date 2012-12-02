using UnityEngine;
using System.Collections;

public class StageController : MonoBehaviour {
	private Hashtable bossStages = new Hashtable();
	private WaveController waveController;
	//public GameMessage gameMessages[];
	
	public int finalWave = -1;
	
	public Hashtable specialEvents = new Hashtable();
	
	public StageController(int f) {
		finalWave = f;
		//waveController = GameObject.Find(" GameController").GetComponentInChildren<WaveController>();
	}
	

	// Use this for initialization
	void Start () {
		
		waveController = GameObject.Find(" GameController").GetComponentInChildren<WaveController>();
		
		//hard code special events for now
		//specialEvents[] = 
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if(waveController.waveNumber != -1 && waveController.waveNumber > finalWave) {
			//do victory scene/action
			//Camera.mainCamera.GetComponent<Skybox>().material = Resources.Load("Skyboxes/Skybox17", typeof(Material)) as Material;
			Debug.Log("Victory!");
		}
	}
	
	public int getFinalWave() {
		return finalWave;
	}
}
