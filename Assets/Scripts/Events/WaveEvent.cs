using UnityEngine;
using System.Collections;

public class WaveEvent : BaseEvent {
	public int waveFlag;
	public WaveController waveController;
	
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
}
