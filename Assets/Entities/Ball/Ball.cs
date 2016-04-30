using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	// Variables from physics
	private Rigidbody2D thisRigidbody;
	private float mass;

	void Awake () {
		thisRigidbody = GetComponent<Rigidbody2D>();
		mass = thisRigidbody.mass;
	}

	void Update () {
		thisRigidbody.velocity = SoccerPhysics.ApplyPhysics(thisRigidbody.velocity, mass);
	}

	public void Lock(){
		thisRigidbody.velocity = Vector2.zero;
		thisRigidbody.angularVelocity = 0f;

		thisRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
	}

	public void Free(){
		thisRigidbody.constraints = RigidbodyConstraints2D.None;
	}
}
