using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public const int MAX_LIVES = 10;

	// Game Active
	private bool _gameActive;

	// Game Variables
	private int _livesLeft;
	private int _money;

	void Awake() {
		// Init Player Variables
		_livesLeft = MAX_LIVES;
		_money = 100;
		_gameActive = true;
	}

	#region Setter Getters
	public bool Active {
		get { return _gameActive; }
		set { _gameActive = value; }
	}

	public int Lives {
		get { return _livesLeft; }
	}

	public int Money {
		get { return _money; }
	}

	public void AddLife(int life) {
		_livesLeft += life;
		if (_livesLeft < 0) {
			// YOU LOSE!
			_livesLeft = 0;
		}
	}

	public void SubLife(int life) {
		_livesLeft -= life;
		if (_livesLeft < 0) {
			// YOU LOSE!
			_livesLeft = 0;
		}
	}

	public bool AddMoney(int money) {
		_money += money;
		if (_money < 0) {
			_money -= money;
			return false;
		}
		return true;
	}

	public bool SubMoney(int money) {
		_money -= money;
		if (_money < 0) {
			_money += money;
			return false;
		}
		return true;
	}
	#endregion
}
