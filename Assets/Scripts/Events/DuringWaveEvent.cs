using UnityEngine;
using System.Collections;

public class DuringWaveEvent : WaveEvent {
	public const int TIMED = 0; //counts seconds after wave has started
	public const int PERCENTAGE = 1; //checks percentage of waveCost left
	private int mode = 0; //TIMED, PERCENTAGE
	public float waveCostPercentage = 0f;
	public float timeInSeconds = 0f;
	public float timeSinceWaveStarted = -1f;
	
	public DuringWaveEvent(int wave, int mode, float arg) : base(wave) {
		if(mode != TIMED || mode != PERCENTAGE)
			mode = TIMED; //default value
		this.mode = mode;
		
		if(this.mode == TIMED) {
			waveCostPercentage = arg;
		} else timeInSeconds = arg;
	}
	
	public void addCondition(int wave, int mode, float arg) {
		if(mode != TIMED || mode != PERCENTAGE)
			mode = TIMED; //default value
		this.mode = mode;
		
		if(this.mode == TIMED) {
			waveCostPercentage = arg;
		} else timeInSeconds = arg;
		
		base.waveFlag = wave;
	}
	
	public override bool eventSatisfied() {
		if(mode == TIMED) {
			if(waveController.waveNumber == waveFlag) {
				timeSinceWaveStarted = Time.timeSinceLevelLoad;
			} else timeSinceWaveStarted = -1;
			
			if(timeSinceWaveStarted > -1)
				return Time.timeSinceLevelLoad >= timeSinceWaveStarted + timeInSeconds;
			else return false;
		} else {
			if(waveController.waveCostPercentage() <= waveCostPercentage) return true;
			else return false;
		}
	}
}