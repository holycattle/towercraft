using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
	// TOCHANGE
	public const int MENU_GAME = 0;
	public const int MENU_INVENTORY = 1;
	public const int MENU_WEAPON = 2;
	public const int MAX_LIVES = 10;

	// Game
	private Menu _activeMenu = Menu.Game;
	private bool _captureCursor = true;

	// Game Variables
	private int _livesLeft;
	private int _money;
			
	// Message Controller
	public MessageController Messenger;
	
	void Start() {
		Messenger = GetComponent<MessageController>();
		Messenger.enabled = true;
	}

	void Awake() {
		// Init Player Variables
		_livesLeft = MAX_LIVES;
		_money = 100;

		// Initialize Messenger
		Messenger = GetComponent<MessageController>();
	}

	void Update() {
		/*
		 *	Screen Locking
		 */
		if (_captureCursor) {
			if (Screen.lockCursor) {
				if (ActiveMenu == Menu.Game) {
					Time.timeScale = 1;			// (Locked) AND (Active) = Playable
				} else {
					Screen.lockCursor = false;	// (Locked) AND (Not Active) = Not Playable (unlock cursor)
					Time.timeScale = 0.0f;
				}
			} else {
				// (Not Locked) AND (Active) = To Lock Game.
				// (Not Locked) AND (Not Active) = Unlock Game.
				Screen.lockCursor = ActiveMenu == Menu.Game;
				Time.timeScale = Screen.lockCursor ? 1.0f : 0.0f;
			}
		} else {
			if (Screen.lockCursor) {
				// Unlock the cursor
				Screen.lockCursor = false;
			} else {
				Time.timeScale = ActiveMenu == Menu.Game ? 1.0f : 0.0f;
			}
		}

		/*
		 *	Game Status Check
		 */
		if (_livesLeft <= 0) {
			// You Lose!
			Debug.Log("You Lose the Game!");
		}
	}

	public bool CaptureCursor {
		get { return _captureCursor;}
		set { _captureCursor = value; }
	}

	public Menu ActiveMenu {
		get { return _activeMenu;}
		set { _activeMenu = value;}
	}

	public bool Active {
		get { return _activeMenu == Menu.Game; }
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
}

public enum Menu {
	Game,
	Inventory,
	Builder
}