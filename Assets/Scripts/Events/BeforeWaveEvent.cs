using UnityEngine;
using System.Collections;

/*@implements
	bool eventSatisfied();
	bool evalCondition();
	void addCondition();
*/

public class BeforeWaveEvent : WaveEvent, IEvent {
	
	public BeforeWaveEvent (int waveNumber, float t) : base(waveNumber) {
		timeInSeconds = t;
		isSatisfied = false;
	}
	
	public void addCondition(float t) {
		timeInSeconds = t;
	}
	
	public bool eventSatisfied() {
		return timeInSeconds <= Time.timeSinceLevelLoad && waveFlag == waveController.waveNumber;
	}
	
	public bool evalCondition() {
		if (!isSatisfied) {
//			Debug.Log(timeInSeconds.ToString() + " = " + Time.timeSinceLevelLoad);
			isSatisfied = eventSatisfied();
//			Debug.Log(isSatisfied.ToString());
			if (isSatisfied)
				doAction();
		}
//		Debug.Log("curr time: " + Time.timeSinceLevelLoad.ToString());
//		Debug.Log("waiting time: " + timeInSeconds.ToString());
		return isSatisfied;
	}
}
