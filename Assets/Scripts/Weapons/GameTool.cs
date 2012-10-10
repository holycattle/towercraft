using UnityEngine;
using System.Collections;

public class GameTool : MonoBehaviour {
	protected GameController _game;
	public WeaponController _input;
	public int bullets;

	protected virtual void Awake() {
		_input = GameObject.Find("Player").GetComponentInChildren<WeaponController>();
		_game = GameObject.Find(" GameController").GetComponent<GameController>();

		bullets = 0;
	}

	protected void Init() {
	}

	public virtual void WhenEquipped() {
	}

	public virtual void WhenUnequipped() {
	}

	public virtual void MouseClickedOn(GameObject g) {
	}

	public virtual void MouseDownOn(GameObject g) {
	}

	public virtual void MouseUpOn(GameObject g) {
	}
}
