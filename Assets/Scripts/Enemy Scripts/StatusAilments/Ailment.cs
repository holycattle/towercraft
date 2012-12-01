using UnityEngine;
using System.Collections;

public class Ailment : MonoBehaviour {

	public const int BURN = 0;
	public const int SLOW = 1;
	public const int STUN = 2;
	protected BaseEnemy _enemy;
	protected float interval;

	protected virtual void Start() {
		//_enemy = transform.root.GetComponentInChildren<BaseEnemy>();
		_enemy = transform.parent.GetComponentInChildren<BaseEnemy>();
		//transform.parent = _enemy.transform;
		//Debug.Log(transform.parent.gameObject);
		BeginStatus();
	}

	protected virtual void Update() {
		interval -= Time.deltaTime;
		if (interval <= 0) {
			EndStatus();
		}
	}

	protected virtual void FixedUpdate() {
	}

	protected virtual void BeginStatus() {
	}

	protected virtual void EndStatus() {
		Destroy(gameObject);
	}

	public float Interval {
		set { interval = value; }
	}

	public static void AddRandomStatusAilment(BaseEnemy b) {
		string[] loadable = {"Burn", "Slow", "Stun"};
		string s = loadable[Random.Range(0, loadable.Length)];
		
		GameObject ailment = Resources.Load("Prefabs/StatusAilments/" + s, typeof(GameObject)) as GameObject;
		AddStatusAilment(b, ailment);
	}

	public static void AddStatusAilment(BaseEnemy b, GameObject ailment) {
		GameObject ail = Instantiate(ailment, b.transform.position, Quaternion.identity) as GameObject;
		ail.transform.parent = b.transform;
	}
}
