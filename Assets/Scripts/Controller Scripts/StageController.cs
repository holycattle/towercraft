using UnityEngine;
using System.Collections;

public class StageController : MonoBehaviour {
	private Hashtable bossStages = new Hashtable();
	private WaveController waveController;
	//public GameMessage gameMessages[];
	public int finalWave = -1;
	
	public StageController(int f) {
		finalWave = f;
		waveController = GameObject.Find(" GameController").GetComponentInChildren<WaveController>();
	}
	

	// Use this for initialization
	void Start () {
		
		waveController = GameObject.Find(" GameController").GetComponentInChildren<WaveController>();
	}
	
	// Update is called once per frame
	public virtual void Update () {
		if(waveController.waveNumber != -1 && waveController.waveNumber > finalWave) {
			//do victory scene/action
			Debug.Log("Victory!");
		}
	}
	
	public int getFinalWave() {
		return finalWave;
	}
}
