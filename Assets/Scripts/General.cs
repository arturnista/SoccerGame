using UnityEngine;
using System.Collections;

public class General : MonoBehaviour {

	public enum PlayerNumber {
		Player1,
		Player2
	}

	public enum PlayerController {
		Human,
		Computer
	}

	public static float AngleBetweenPosition(Vector3 current, Vector3 target){
		Vector3 v3Pos = target;
		v3Pos = v3Pos - current;
		float fAngle = Mathf.Atan2 (v3Pos.y, v3Pos.x) * Mathf.Rad2Deg;
		if (fAngle < 0.0f) {
			fAngle += 360.0f;
		}

		return fAngle - 90;
	}

	public static Vector3 GetMousePosition(){
		return Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	public static Transform GetParent(string parentName){
		GameObject parent = GameObject.Find(parentName);
		if(!parent){
			parent = new GameObject(parentName);
		}
		return parent.transform;
	}

	public static Vector3 GetCameraSize() {
		float quadHeight = Camera.main.orthographicSize * 2f;
		float quadWidth = quadHeight * Screen.width / Screen.height;
		return new Vector3(quadWidth, quadHeight, 1f);
	}

	public static bool PositionIsPassable(Vector2 pos){
		RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
		if(hit){
			GameObject coll = hit.collider.gameObject;
			if(coll.tag == "Obstacle"){
				return false;
			}
		}
		return true;
	}
}
