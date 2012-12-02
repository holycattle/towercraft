using UnityEngine;
using System.Collections;

public class MyTexture2D {
	Rect gRect;
	Texture2D tex;
	Color col;

	public MyTexture2D (Texture2D theTex, Rect theRect, Color theColor) {
		gRect = theRect;
		col = theColor;
		tex = theTex;
	}

	public void DrawMe() {
		Color tempCol = GUI.color;
		GUI.color = col;
		GUI.Label(gRect, tex);
		GUI.color = tempCol;
	}

	public Color color {
		get { return col; }
		set { col = value; }
	}

	public float Alpha {
		get { return col.a; }
		set { col.a = value; }
	}
}