using UnityEngine;
using System.Collections;

/*@implements
	bool eventSatisfied();
	bool evalCondition();
	void addCondition();
*/

public class WaveEvent : BaseEvent, IEvent {
	
	public int waveFlag;
	public int mode = 0; //BEFORE, DURING
	public WaveController waveController;
	
	public float timeInSeconds;
	
	public WaveEvent(int waveNumber) {
		waveController = GameObject.Find(" GameController").GetComponentInChildren<WaveController>();
		waveFlag = waveNumber;
	}
	
	public void addCondition(int waveNumber) {
		waveFlag = waveNumber;
	}
	
	public override bool eventSatisfied() {
		return waveFlag == waveController.waveNumber;
	}
	
	public override bool evalCondition() {
		if(!isSatisfied) {
			isSatisfied = eventSatisfied();
			//Debug.Log(isSatisfied.ToString());
			if(isSatisfied)
				doAction();
		}
			
		return isSatisfied;
	}
}
