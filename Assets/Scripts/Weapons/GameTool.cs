using UnityEngine;
using System.Collections;

public class GameTool : MonoBehaviour {
	protected GameController _game;
	protected WeaponController _weapon;
	protected LevelController _level;
	public int bullets;

	protected virtual void Awake() {
		_weapon = GameObject.Find("Player").GetComponentInChildren<WeaponController>();
		_game = GameObject.Find(" GameController").GetComponent<GameController>();
		_level = GameObject.Find(" GameController").GetComponent<LevelController>();

		bullets = 0;
	}

	protected virtual void Start() {
	}

	protected virtual void OnGUI() {
	}

	public virtual void WhenEquipped() {
	}

	public virtual void WhenUnequipped() {
	}

	public virtual void MouseClickOn(GameObject g) {
	}

	public virtual void MouseDownOn(GameObject g) {
	}

	public virtual void MouseUpOn(GameObject g) {
	}
}
