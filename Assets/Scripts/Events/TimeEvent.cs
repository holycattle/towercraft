using UnityEngine;
using System.Collections;

//to-do: implement an Interface perhaps?
public class TimeEvent : BaseEvent {
	public float timeInSeconds;
	
	public TimeEvent (float t) : base() {
		timeInSeconds = t;
		isSatisfied = false;
	}
	
	public void addCondition(float t) {
		timeInSeconds = t;
	}
	
	public bool evalCondition() {
		if (!isSatisfied) {
//			Debug.Log(timeInSeconds.ToString() + " = " + Time.timeSinceLevelLoad);
			isSatisfied = (timeInSeconds <= Time.timeSinceLevelLoad);
//			Debug.Log(isSatisfied.ToString());
			if (isSatisfied)
				doAction();
		}
//		Debug.Log("curr time: " + Time.timeSinceLevelLoad.ToString());
//		Debug.Log("waiting time: " + timeInSeconds.ToString());
		return isSatisfied;
	}
}
