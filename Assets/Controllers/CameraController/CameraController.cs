using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	private Camera thisCamera;

	void Awake () {
		thisCamera = GetComponent<Camera>();
		Vector3 cam = General.GetCameraSize(thisCamera);
		float camAspectRatio = cam.x / cam.y;

		float newCamSize = 17.5f / (camAspectRatio * 2f);
		thisCamera.orthographicSize = newCamSize;

	}

}
