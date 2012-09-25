using UnityEngine;
using System.Collections;

public class Builder : GameTool {
	public override void WhenEquipped() {
		Debug.Log("Builder Equipped");
		_input.RaycastLayer = LayerMask.NameToLayer("Level");
	}

	public override void MouseClickedOn(GameObject g) {
		if (g != null) {
			g.GetComponent<Grid>().TEMP_toggleTower();
//			_game.Active = false;
		}
	}

	public override void MouseDownOn(GameObject g) {
	}
}
