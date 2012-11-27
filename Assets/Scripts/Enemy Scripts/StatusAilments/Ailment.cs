using UnityEngine;
using System.Collections;

public class Ailment : MonoBehaviour {

	protected BaseEnemy _enemy;
	protected float interval;

	protected virtual void Start() {
		_enemy = transform.root.GetComponentInChildren<BaseEnemy>();
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
		string[] loadable = {"Burn", "Slow"};
		string s = loadable[Random.Range(0, loadable.Length)];
		GameObject ailment = Resources.Load("Prefabs/StatusAilments/" + s, typeof(GameObject)) as GameObject;
		AddStatusAilment(b, ailment);
	}

	public static void AddStatusAilment(BaseEnemy b, GameObject ailment) {
		GameObject ail = Instantiate(ailment, b.transform.position, Quaternion.identity) as GameObject;
		ail.transform.parent = b.transform.root;
	}
}
