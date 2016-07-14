using UnityEngine;
using System.Collections;

public class PlayerIA : MonoBehaviour {

	public LayerMask goalLayer;

	private bool iaConfigurated;

	private GameController gameController;
	private Player player;

	private Ball ball;

	void Awake () {
		iaConfigurated = false;
		gameController = GameObject.FindObjectOfType<GameController>();
		player = GameObject.FindObjectOfType<Player>();
	}

	void Update () {
		if(!iaConfigurated && gameController.IsGameStarted()){
			Configure();
		}
		if(!player.IsCurrentPlayer()){
			return;
		}
		Vector3 ballDir = ball.GetDirection();
		RaycastHit2D hit = Physics2D.Raycast(ball.transform.position, ballDir, 20f, goalLayer);
		if(hit){
			Goal gl = hit.collider.GetComponent<Goal>();
			if(gl && gl.playerGoal == player.GetPlayerNumber()){
				ShootAtBall();
			}
		}

		// If the time is running out OR the ball stop/will stop soon, shoot the ball
		//float ballVelocity = Mathf.Abs(ball.GetComponent<Rigidbody2D>().velocity.magnitude);
		float remTime = gameController.GetRemainingPlayerTime();
		if(remTime <= 1f){
			ShootAtBall();
		}
	}

	void ShootAtBall(){
		float distance = 8f;
		Vector3 dir = General.GetDirectionBetweenPosition(transform.position, ball.transform.position);

		player.Shoot(dir, distance);
	}

	void Configure(){
		ball = GameObject.FindObjectOfType<Ball>();

		iaConfigurated = true;
	}
}
