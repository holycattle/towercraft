using UnityEngine;
using System.Collections;

public class Item {
	public string name;
	public Texture texture;

	public Item () {
		name = "" + Random.Range(0, 20);
	}
}