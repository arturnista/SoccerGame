using UnityEngine;
using System.Collections;

public class SoccerPhysics : MonoBehaviour {

	public const float FRICTION_COEF = 0.3f;

	public static Vector2 ApplyPhysics(Vector2 velocity, float mass){
		if(velocity == Vector2.zero){
			return velocity;
		}
		Vector2 direction = velocity.normalized;

		// Compute the force
		float force = 0f;
		force += velocity.x * direction.x;
		force += velocity.y * direction.y;

		float mult = mass * FRICTION_COEF;
		force = Mathf.Clamp(force -= mult, 0f, force);

		Vector2 nVelocity = direction * force;
		return nVelocity;
	}
}
