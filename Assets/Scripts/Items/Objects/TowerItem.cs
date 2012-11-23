using UnityEngine;
using System.Collections;

public class TowerItem : Item {
	protected TowerComponent _tc;

	/*
	 *	Constructors
	 */
	public TowerItem () : base(ITEM_TOWER) {
		_tc = ComponentGenerator.Get().GenerateComponent(Random.Range(0, BaseTower.TOWER_COMPLETE), 0);
	}

	public TowerItem (int i, int cost) : base(ITEM_TOWER) {
		_tc = ComponentGenerator.Get().GenerateComponent(i, cost);
	}

	public TowerItem (TowerComponent t): base(ITEM_TOWER) {
		_tc = t;
	}

	public override bool isLessThan(Item t) {
		return GetTowerComponent().componentType < ((TowerItem)t).GetTowerComponent().componentType;
	}

	/*
	 *	Getters
	 */
	public TowerComponent GetTowerComponent() {
		return _tc;
	}

	public override string GetTooltip() {
		return _tc.GetTooltipString();
	}

	public override string GetName() {
		return _tc.componentName;
	}
}
