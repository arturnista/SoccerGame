using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	// Variables from physics
	private Rigidbody2D thisRigidbody;
	private float mass;

	// Variables from throwing
	private const float THROW_FORCE = 2f;
	private const float MAXIMUM_FORCE = 20f;
	private bool isAiming;
	private const float padding = .5f;

	// Variables from game
	private GameController gameController;
	private SpriteRenderer spriteRenderer;

	private bool nonPlayable;
	private PlayerData playerData;

	private StartPosition normalStart;
	private StartPosition offensiveStart;

	void Awake () {
		transform.parent = General.GetParent("Players");
		gameController = GameObject.FindObjectOfType<GameController>();
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();

		thisRigidbody = GetComponent<Rigidbody2D>();
		mass = thisRigidbody.mass;

		isAiming = false;
		// In the start the player is not playable
		// Wait for the start
		SetNonPlayable();
	}

	void Update () {
		thisRigidbody.velocity = SoccerPhysics.ApplyPhysics(thisRigidbody.velocity, mass);

		if(isAiming && !nonPlayable){
			float ang = General.AngleBetweenPosition(transform.position, General.GetMousePosition());
			transform.eulerAngles = new Vector3(0f, 0f, ang);
		}
	}

	void OnMouseDown(){
		if(nonPlayable){
			return;
		}
		isAiming = true;
	}
		
	void OnMouseUp(){
		if(nonPlayable){
			return;
		}
		isAiming = false;
		Vector3 mousePos = General.GetMousePosition();
		float distance = Vector2.Distance(transform.position, mousePos);
		if(distance > padding){
			float force = Mathf.Clamp(distance * THROW_FORCE, 0f, MAXIMUM_FORCE);

			float ang = General.AngleBetweenPosition(transform.position, General.GetMousePosition()) + 90f;
			float angRad = ang * Mathf.Deg2Rad;

			Vector3 dir = new Vector3(Mathf.Cos(angRad), Mathf.Sin(angRad));
			thisRigidbody.velocity = dir * force;

			gameController.SwitchCurrentPlayer();
		}
	}

	public void Restart(bool isNormal){
		thisRigidbody.velocity = Vector2.zero;
		if(isNormal){
			transform.position = normalStart.position;
		} else {
			transform.position = offensiveStart.position;
		}
		if(playerData.playerController == General.PlayerController.Human){
			SetPlayable();
		}
	}

	public void Lock(){
		thisRigidbody.velocity = Vector2.zero;
		thisRigidbody.angularVelocity = 0f;

		thisRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
		SetNonPlayable();
	}

	public General.PlayerNumber GetPlayerNumber(){
		return playerData.playerNumber;
	}

	public void SetPlayerData(PlayerData plrData){
		playerData = plrData;
		if(playerData.playerController == General.PlayerController.Computer){
			SetNonPlayable();
			GetComponentInChildren<SpriteRenderer>().color = Color.red;
		}
		GetComponentInChildren<SpriteRenderer>().sprite = playerData.playerSprite;
	}

	public void SetStartPositions(StartPosition normal){
		SetStartPositions(normal, normal);
	}

	public void SetStartPositions(StartPosition normal, StartPosition offensive){
		normalStart = normal;
		offensiveStart = offensive;
	}

	public void SetNonPlayable(){
		nonPlayable = true;
	}

	public void SetPlayable(){
		if(playerData.playerController == General.PlayerController.Human){
			nonPlayable = false;
		}
	}

	public void SetCurrentPlayer(){
		SetPlayable();
		spriteRenderer.color = Color.white;
	}

	public void SetNotCurrentPlayer(){
		SetNonPlayable();
		spriteRenderer.color = Color.grey;
	}
}
