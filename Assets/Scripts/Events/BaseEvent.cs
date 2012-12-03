using UnityEngine;
using System.Collections;
using System;

public class BaseEvent {	
	public Action<Hashtable> actionFunction;
	Hashtable actionFunctionArgs = new Hashtable();
	public bool isSatisfied;
	
	public BaseEvent() {
		isSatisfied = false;
	}
	
	public virtual void addAction(Action<Hashtable> actionFunction, Hashtable actArg) {
		this.actionFunction = actionFunction.Clone() as Action<Hashtable>;
		actionFunctionArgs = actArg.Clone() as Hashtable;
		Debug.Log("added function: " + actionFunction.ToString());
	}
	
	public virtual void doAction() {
		Debug.Log("done!");
		
		actionFunction(actionFunctionArgs);
	}
}
