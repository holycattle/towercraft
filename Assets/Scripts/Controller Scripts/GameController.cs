using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
//	public const float NORMAL_TIMESCALE = 1f;
	public float NORMAL_TIMESCALE = 4.0f;

	// TOCHANGE
	public const int MENU_GAME = 0;
	public const int MENU_INVENTORY = 1;
	public const int MENU_WEAPON = 2;
	public const int MAX_LIVES = 30;

	// Game
	private Menu _activeMenu = Menu.Game;
	private bool _captureCursor = true;

	// Game Variables
	private int _livesLeft;
	public PlayerController _player;

	// Message Controller
	[HideInInspector]
	public MessageController Messenger;

	void Start() {
		Messenger = GetComponent<MessageController>();
		Messenger.enabled = true;
		_player = GameObject.Find("Player").GetComponent<PlayerController>();
	}

	void Awake() {
		// Init Player Variables
		_livesLeft = MAX_LIVES;

		// Initialize Messenger
		Messenger = GetComponent<MessageController>();
		Screen.fullScreen = true; //force fullscreen
	}

	void Update() {
		/*
		 *	Screen Locking
		 */
		if (_captureCursor) {
			if (Screen.lockCursor) {
				if (ActiveMenu == Menu.Game) {
					Time.timeScale = NORMAL_TIMESCALE;			// (Locked) AND (Active) = Playable
				} else {
					Screen.lockCursor = false;	// (Locked) AND (Not Active) = Not Playable (unlock cursor)
					Time.timeScale = 0.0f;
				}
			} else {
				// (Not Locked) AND (Active) = To Lock Game.
				// (Not Locked) AND (Not Active) = Unlock Game.
				Screen.lockCursor = ActiveMenu == Menu.Game;
				Time.timeScale = Screen.lockCursor ? NORMAL_TIMESCALE : 0.0f;
			}
		} else {
			if (Screen.lockCursor) {
				// Unlock the cursor
				Screen.lockCursor = false;
			} else {
				Time.timeScale = ActiveMenu == Menu.Game ? NORMAL_TIMESCALE : 0.0f;
			}
		}

		/*
		 *	Game Status Check
		 */
		if (_livesLeft <= 0) {
			// You Lose!
			AutoFade.LoadLevel("Start", 3, 1, Color.black);
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
}

public enum Menu {
	Game,
	Inventory,
	Builder
}