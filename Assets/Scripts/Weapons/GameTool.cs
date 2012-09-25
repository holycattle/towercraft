using UnityEngine;
using System.Collections;

public class GameTool {
	protected GameController _game;
	protected GameObject _particles;
	protected InputHandler _input;
	public int bullets;

	public GameTool () {
		_input = GameObject.Find("Player").GetComponent<InputHandler>();
		_particles = GameObject.Find("Player").transform.FindChild("Main Camera/GunshotParticle").gameObject;
		_game = GameObject.Find(" GameController").GetComponent<GameController>();

		bullets = 0;
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

	public virtual void Update() {
	}
}
