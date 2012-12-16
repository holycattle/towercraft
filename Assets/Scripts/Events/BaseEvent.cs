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
	
	public virtual bool eventSatisfied() {
		return false;
	}
	
	public virtual bool evalCondition() {
		if(!isSatisfied) {
			isSatisfied = eventSatisfied();
			
			if(isSatisfied)
				doAction();
		}
			
		return isSatisfied;
	}
	
	public virtual void addAction(Action<Hashtable> actionFunction, Hashtable actArg) {
		this.actionFunction = actionFunction.Clone() as Action<Hashtable>;
		actionFunctionArgs = actArg.Clone() as Hashtable;
	}
	
	public virtual void doAction() {
		actionFunction(actionFunctionArgs);
	}
}
