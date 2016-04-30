using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {

	public General.PlayerNumber playerGoal;
	private GameController gameController;

	void Awake(){
		gameController = GameObject.FindObjectOfType<GameController>();
	}

	void OnTriggerEnter2D(Collider2D coll){
		Ball ball = coll.GetComponent<Ball>();
		if(ball){
			if(playerGoal == General.PlayerNumber.Player2){
				gameController.Player1Goal();
			} else {
				gameController.Player2Goal();
			}
		}
	}
}
