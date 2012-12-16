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
		return waveController._timeTillNextWave <= waveController._timeTillNextWave - timeInSeconds && waveFlag == waveController.waveNumber;
	}
}
