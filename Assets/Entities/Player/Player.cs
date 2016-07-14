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

	// Variables from IA
	private PlayerIA playerIA;

	// Variables from game
	private GameController gameController;
	private SpriteRenderer spriteRenderer;
	private PlayerSelection selection;

	private bool nonPlayable;
	private bool currentPlayer;
	private PlayerData playerData;

	private StartPosition normalStart;
	private StartPosition offensiveStart;

	void Awake () {
		transform.parent = General.GetParent("Players");
		gameController = GameObject.FindObjectOfType<GameController>();
		spriteRenderer = transform.FindChild("Sprite").GetComponent<SpriteRenderer>();
		selection = GetComponentInChildren<PlayerSelection>();
		selection.SetActive(false);

		thisRigidbody = GetComponent<Rigidbody2D>();
		mass = thisRigidbody.mass;

		playerIA = GetComponent<PlayerIA>();

		SetAiming(false);
		// In the start the player is not playable
		// Wait for the start
		SetNonPlayable();
	}

	void Update () {
		thisRigidbody.velocity = SoccerPhysics.ApplyPhysics(thisRigidbody.velocity, mass);

		if(isAiming){
			if(nonPlayable){
				SetAiming(false);
				return;
			}

			Vector3 mousePos = General.GetMousePosition();
			float distance = Vector2.Distance(transform.position, mousePos);
			if(distance > padding){
				selection.SetSelected();
			} else {
				selection.SetCanceled();
			}

			float ang = General.AngleBetweenPosition(transform.position, mousePos);
			transform.eulerAngles = new Vector3(0f, 0f, ang);
		}
	}

	void OnMouseDown(){
		if(nonPlayable){
			return;
		}
		SetAiming(true);
	}
		
	void OnMouseUp(){
		if(nonPlayable){
			return;
		}
		SetAiming(false);
		Vector3 mousePos = General.GetMousePosition();
		float distance = Vector2.Distance(transform.position, mousePos);
		if(distance > padding){
			Vector3 dir = General.GetDirectionBetweenPosition(transform.position, General.GetMousePosition());

			Shoot(dir, distance);
		}
	}

	public void Shoot(Vector3 dir, float distance){
		float force = Mathf.Clamp(distance * THROW_FORCE, 0f, MAXIMUM_FORCE);
		thisRigidbody.velocity = dir * force;

		gameController.SwitchCurrentPlayer();
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

	void SetAiming(bool aiming){
		isAiming = aiming;
		selection.SetActive(aiming);
	}

	public void SetPlayerData(PlayerData plrData){
		playerData = plrData;
		playerIA.enabled = false;
		if(playerData.playerController == General.PlayerController.Computer){
			SetNonPlayable();
			playerIA.enabled = true;
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

	public bool IsNonPlayable(){
		return nonPlayable;
	}

	public bool IsCurrentPlayer(){
		return currentPlayer;
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
		currentPlayer = true;
	}

	public void SetNotCurrentPlayer(){
		SetNonPlayable();
		spriteRenderer.color = Color.grey;
		currentPlayer = false;
	}
}
