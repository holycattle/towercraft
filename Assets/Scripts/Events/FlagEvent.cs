using UnityEngine;
using System.Collections;
using System;

public class FlagEvent : BaseEvent {
	public Predicate<Hashtable> conditionFunction;
	private Hashtable conditionFunctionArgs;
	
	public void addFlag(Predicate<Hashtable> cond, Hashtable args) {
		conditionFunction = (Predicate<Hashtable>)cond.Clone();
		conditionFunctionArgs = (Hashtable)args.Clone();
	}
	
	public bool evalCondition() {
		if(!conditionFunction(conditionFunctionArgs)) {
			return false;
		}
		isSatisfied = true;
		return true;
	}
}
