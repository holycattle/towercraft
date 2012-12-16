using UnityEngine;
using System.Collections;

public class BeforeWaveEvent : WaveEvent {
	public float timeInSeconds;
	
	public BeforeWaveEvent (int waveNumber, float t) : base(waveNumber) {
		timeInSeconds = t;
		isSatisfied = false;
	}
	
	public void addCondition(int waveNumber, float t) {
		timeInSeconds = t;
		base.waveFlag = waveNumber;
	}
	
	public override bool eventSatisfied() {
		return timeInSeconds <= Time.timeSinceLevelLoad && waveFlag == waveController.waveNumber;
	}
}
