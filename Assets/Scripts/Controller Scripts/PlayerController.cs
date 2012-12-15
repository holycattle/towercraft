using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	public const int LIVES_WIDTH = 128;
	public const int LIVES_HEIGHT = 30;
	public const int TEXT_HEIGHT = 30;
	private Vector3 DISABLETRANSFORM = new Vector3(0, 100, 0);
	const float DEATH_TIME = 15f; //set this to how long before character respawns again (in seconds)

	// Game Controllers
	private GameController _game;
	private WaveController _wave;
	private WeaponController _weapon;
	private CharacterMotor _motor;

	// Player Variables
	public const int MAX_LIFE = 100;
	private int _life;

	// GUI
	public Texture2D menu;
	public Texture2D item;

	// Damage bars
	private MyTexture2D leftDamage;
	private MyTexture2D rightDamage;
	private float timeLeft;
	private float timeDamage = 0.25f;
	public float respawnCountdown = 0f;
	CameraController _overviewCamera;
	public bool isDead = false;

	// Audio
	private AudioClip[] footsteps;
	private float footstepInterval = 0.30f;
	private float footstepTimer;

	void Start() {
		_game = GameObject.Find(" GameController").GetComponent<GameController>();
		_wave = GameObject.Find(" GameController").GetComponent<WaveController>();
		_weapon = GetComponentInChildren<WeaponController>();
		_overviewCamera = GameObject.Find("Minimap Camera").GetComponent<CameraController>();

		Screen.showCursor = true;

		Texture2D left = Resources.Load("Textures/Weapon/TakeDamage0", typeof(Texture2D)) as Texture2D;
		Texture2D right = Resources.Load("Textures/Weapon/TakeDamage1", typeof(Texture2D)) as Texture2D;
		Color c = new Color(0.8f, 0, 0, 1f);
		leftDamage = new MyTexture2D(left,
			new Rect((Screen.width / 2) - left.width, (Screen.height - left.height) / 2, left.width, left.height), c);
		rightDamage = new MyTexture2D(right,
			 new Rect(Screen.width / 2, (Screen.height - right.height) / 2, right.width, right.height), c);

		// Player Variables
		_life = MAX_LIFE;

		_motor = GetComponent<CharacterMotor>();
		footsteps = new AudioClip[3];
		footsteps[0] = Resources.Load("Sound/Player/Footstep_Gravel_1") as AudioClip;
		footsteps[1] = Resources.Load("Sound/Player/Footstep_Gravel_3") as AudioClip;
		footsteps[2] = Resources.Load("Sound/Player/Footstep_Gravel_4") as AudioClip;
	}

	void Update() {
		if (timeLeft > 0) {
			timeLeft -= Time.deltaTime;
			leftDamage.Alpha = timeLeft / timeDamage;
			rightDamage.Alpha = timeLeft / timeDamage;
		}

		if (_life <= 0) {
			if (respawnCountdown == 0) {
				Die();
			} else if (respawnCountdown > 0) {
				respawnCountdown -= Time.deltaTime;
			} else if (respawnCountdown <= 0) {
				Respawn();
			}
		}

//		Debug.Log("Velocity: " + _motor.movement.velocity.magnitude);
		Debug.Log("Velocity: " + _motor.movement.velocity.x + ", " + _motor.movement.velocity.y + ", " + _motor.movement.velocity.z);

		if (_motor.grounded && (_motor.movement.velocity.x != 0 || _motor.movement.velocity.z != 0)) {
			if (footstepTimer <= 0) {
				audio.clip = footsteps[Random.Range(0, footsteps.Length)];
				audio.Play();
				footstepTimer = footstepInterval;
			} else {
				footstepTimer -= Time.deltaTime;
			}
		} else {
			footstepTimer = 0;
		}
	}
	
	void Die() {
		respawnCountdown = DEATH_TIME;
		//transform.rotation = Quaternion.LookRotation(Vector3.up);
		//move to minimap camera mode
		_overviewCamera.SetOverviewCamera(true);
		isDead = true;
	}
	
	void Respawn() {
		respawnCountdown = 0;
		_life = MAX_LIFE;
		_overviewCamera.SetOverviewCamera(false);
		transform.position = new Vector3(0, 25, 0);
		isDead = false;
	}
	
	void OnGUI() {
		GUI.backgroundColor = Color.grey;

		if (timeLeft > 0) {
			leftDamage.DrawMe();
			rightDamage.DrawMe();
//			GUI.DrawTexture(leftDmgBox, leftDamage);
//			GUI.DrawTexture(rightDmgBox, rightDamage);
		}
	}

	public void SetEnabled(bool b) {
		GetComponent<CharacterMotor>().enabled = b;
		MouseLook[] mLook = GetComponentsInChildren<MouseLook>();
		foreach (MouseLook m in mLook) {
			m.enabled = b;
		}

		transform.position += DISABLETRANSFORM * (b ? 1 : -1);
		_weapon.drawCrosshair = b;
	}

	public void AddLife(int amt) {
		_life += amt;

		if (amt < 0 && timeLeft <= 0) {
			timeLeft = timeDamage;
		}

		if (_life >= MAX_LIFE) {
			_life = MAX_LIFE;
		}
//		if (_life <= 0)
//			Lose();
	}

	public int Life {
		get { return _life; }
	}
}
