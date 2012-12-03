using UnityEngine;
using System.Collections;

public class WaveEvent : BaseEvent {
	public int waveFlag;
	private WaveController waveController;
	
	public WaveEvent(int waveNumber) {
		waveController = GameObject.Find(" GameController").GetComponentInChildren<WaveController>();
		waveFlag = waveNumber;
	}
	
	public void addCondition(int waveNumber) {
		waveFlag = waveNumber;
	}
	
	public bool waveSatisfied() {
		isSatisfied = true;
		return waveFlag == waveController.waveNumber;
	}
	
	public bool evalCondition() {
		if(!isSatisfied) {
			isSatisfied = waveSatisfied();
			Debug.Log(isSatisfied.ToString());
			if(isSatisfied)
				doAction();
		}
			
		return isSatisfied;
	}


}
