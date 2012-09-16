/* SAUCE: http://blog.gfx47.com/2011/04/04/detect-right-click-on-game-objects-in-unity3d/ */
using UnityEngine;
using System.Collections;
 
public class InputHandler : MonoBehaviour {
	public bool HandleLeftClick = true;
	public bool HandleRightClick = true;
	public bool HandleMiddleClick = false;
	public string OnLeftClickMethodName = "OnLeftClick";
	public string OnRightClickMethodName = "OnRightClick";
	public string OnMiddleClickMethodName = "OnMiddleClick";
	public LayerMask layerMask;

	// Mouse Locking
	public static bool lockCursor = true;

	void Update() {
//		if (Screen.lockCursor != lockCursor) {
//			if (lockCursor && Input.GetMouseButton(0)) {
//				Screen.lockCursor = true;
//			} else if (!lockCursor) {
//				Screen.lockCursor = false;
//			}
//		}

		/* Mouse Clicks */
		GameObject clickedGmObj = null;
		bool clickedGmObjAcquired = false;

		// Left click
		if (HandleLeftClick && Input.GetMouseButtonDown(0)) {
			/*if (!clickedGmObjAcquired)
            {*/
			clickedGmObj = GetClickedGameObject();
			clickedGmObjAcquired = true;
			/*}*/
 
			if (clickedGmObj != null)
				clickedGmObj.SendMessage(OnLeftClickMethodName, null, SendMessageOptions.DontRequireReceiver);
		}
 
		// Right click
		if (HandleRightClick && Input.GetMouseButtonDown(1)) {
			if (!clickedGmObjAcquired) {
				clickedGmObj = GetClickedGameObject();
				clickedGmObjAcquired = true;
			}
 
			if (clickedGmObj != null)
				clickedGmObj.SendMessage(OnRightClickMethodName, null, SendMessageOptions.DontRequireReceiver);
		}
 
		// Middle click
		if (HandleMiddleClick && Input.GetMouseButtonDown(2)) {
			if (!clickedGmObjAcquired) {
				clickedGmObj = GetClickedGameObject();
				clickedGmObjAcquired = true;
			}
 
			if (clickedGmObj != null)
				clickedGmObj.SendMessage(OnMiddleClickMethodName, null, SendMessageOptions.DontRequireReceiver);
		}
	}
 
	GameObject GetClickedGameObject() {
		// Builds a ray from camera point of view to the mouse position
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
 
		// Casts the ray and get the first game object hit
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
			return hit.transform.gameObject;
		else
			return null;
	}
}
