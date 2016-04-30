using UnityEngine;
using System.Collections;

public class StartPosition : MonoBehaviour {

	public enum StartType {
		Normal,
		Offensive
	}

	public General.PlayerNumber player;
	public StartType startType;
	[HideInInspector]
	public Vector3 position;

	void Awake(){
		transform.parent = General.GetParent("StartPositions");
		position = transform.position;
	}

}
