using UnityEngine;
using System.Collections;
using System;

public class Event : MonoBehaviour {
	public Predicate<Hashtable> conditionFunction;
	public Predicate<Hashtable> actionFunction;
	Hashtable conditionFunctionArgs = new Hashtable();
	Hashtable actionFunctionArgs = new Hashtable();
	public bool isSatisfied;
	
	public Event() {}

	// Use this for initialization
	void Start () {
		Debug.Log("Starting!");
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log("running!");
		if(evalCondition()) {
			Debug.Log("oh yeah!");
			Destroy(this);
		}
	}
	
	//T and T2 are ideally an array or list, but it can be anything
	public virtual void addCondition(Predicate<Hashtable> conditionFunction, Hashtable condArg) {
		this.conditionFunction = conditionFunction.Clone() as Predicate<Hashtable>;
		conditionFunctionArgs = condArg.Clone() as Hashtable;
	}
	
	public virtual void addAction(Action<Hashtable> actionFunction, Hashtable actArg) {
		this.actionFunction = actionFunction.Clone() as Predicate<Hashtable>;
		actionFunctionArgs = actArg.Clone() as Hashtable;
	}
	
	public virtual bool evalCondition() {		
		isSatisfied = conditionFunction(conditionFunctionArgs);
		if(isSatisfied)
			doAction();
		
		return isSatisfied;
	}
	
	public virtual void doAction() {
		actionFunction(actionFunctionArgs);
	}
}
