using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public Sprite pl1icn;
	public Sprite pl2icn;

	private PlayerData player1Data;
	private PlayerData player2Data;

	private float gameTime;
	private float initTime;

	private float playerTime = 10f;
	private float initPlayerTime;

	public GameObject playerPrefab;
	public GameObject ballPrefab;

	private Ball ball;
	private List<Player> players;
	private List<Player> p1Players;
	private List<Player> p2Players;
	private General.PlayerNumber currentPlayer;

	private UIController uiController;

	void Awake(){
		uiController = GameObject.FindObjectOfType<UIController>();

		GameObject ballOb = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		ball = ballOb.GetComponent<Ball>();

		player1Data = new PlayerData();
		player1Data.playerName = "PLAYER 1";
		player1Data.playerGoals = 0;
		player1Data.playerController = General.PlayerController.Human;
		player1Data.playerNumber = General.PlayerNumber.Player1;
		player1Data.playerSprite = pl1icn;

		player2Data = new PlayerData();
		player2Data.playerName = "PLAYER 2";
		player2Data.playerGoals = 0;
		player2Data.playerController = General.PlayerController.Human;
		player2Data.playerNumber = General.PlayerNumber.Player2;
		player2Data.playerSprite = pl2icn;
	}

	void Start () {
		players = new List<Player>();
		p1Players = new List<Player>();
		p2Players = new List<Player>();
		NewGame();
	}

	void Update(){
		if(GetRemainingTime() <= 0f){
			FinishGame();
		}

		if(GetRemainingPlayerTime() <= 0f){
			SwitchCurrentPlayer();
		}
	}

	void FinishGame(){
		foreach(Player pl in players){
			pl.Lock();
		}
		ball.Lock();

		uiController.EnableFinishGameText();
	}

	public PlayerData GetPlayer1Data(){
		return player1Data;
	}
	public PlayerData GetPlayer2Data(){
		return player2Data;
	}
	
	public void Player1Goal(){
		player1Data.playerGoals++;
		uiController.EnableGoalText(player1Data);
		currentPlayer = General.PlayerNumber.Player1;
		Goal();
	}

	public void Player2Goal(){
		player2Data.playerGoals++;
		uiController.EnableGoalText(player2Data);
		currentPlayer = General.PlayerNumber.Player2;
		Goal();
	}

	void Goal(){
		ball.Lock();
		foreach(Player pl in players){
			pl.SetNonPlayable();
		}
		Invoke("StartGame", 2f);
	}

	public void NewGame(){
		uiController.DisableFinishGameText();
		foreach(Player pl in players){
			Destroy(pl.gameObject);
		}
		players.Clear();
		p1Players.Clear();
		p2Players.Clear();

		gameTime = 120f;
		initTime = Time.time;

		StartPosition[] startPositions = GameObject.FindObjectsOfType<StartPosition>();
		List<StartPosition> p1Normal = new List<StartPosition>();
		List<StartPosition> p1Offensive = new List<StartPosition>();
		List<StartPosition> p2Normal = new List<StartPosition>();
		List<StartPosition> p2Offensive = new List<StartPosition>();

		foreach(StartPosition stPos in startPositions){
			if(stPos.player == General.PlayerNumber.Player1){
				if(stPos.startType == StartPosition.StartType.Normal){
					p1Normal.Add(stPos);
				} else {
					p1Offensive.Add(stPos);
				}
			} else {
				if(stPos.startType == StartPosition.StartType.Normal){
					p2Normal.Add(stPos);
				} else {
					p2Offensive.Add(stPos);
				}
			}
		}

		for (int i = 0; i < p1Normal.Count; i++) {
			GameObject playerGB = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			Player pl = playerGB.GetComponent<Player>();
			pl.SetPlayerData(player1Data);
			players.Add(pl);
			p1Players.Add(pl);
			if(p1Offensive.Count >= i+1){
				pl.SetStartPositions(p1Normal[i], p1Offensive[i]);
			} else {
				pl.SetStartPositions(p1Normal[i]);
			}
		}

		for (int i = 0; i < p2Normal.Count; i++) {
			GameObject playerGB = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			Player pl = playerGB.GetComponent<Player>();
			pl.SetPlayerData(player2Data);
			players.Add(pl);
			p2Players.Add(pl);
			if(p2Offensive.Count >= i+1){
				pl.SetStartPositions(p2Normal[i], p2Offensive[i]);
			} else {
				pl.SetStartPositions(p2Normal[i]);
			}
		}

		currentPlayer = General.PlayerNumber.Player1;
		StartGame();
	}

	void StartGame(){
		ball.Free();
		uiController.DisableGoalText();
		ball.transform.position = Vector3.zero;
		foreach(Player pl in players){
			if(pl.GetPlayerNumber() == currentPlayer){
				pl.Restart(false);
				pl.SetCurrentPlayer();
			} else {
				pl.Restart(true);
				pl.SetNotCurrentPlayer();
			}
		}
		initPlayerTime = Time.time;
	}

	public void SwitchCurrentPlayer(){
		foreach(Player pl in players){
			if(pl.GetPlayerNumber() == currentPlayer){
				pl.SetNotCurrentPlayer();
			} else {
				pl.SetCurrentPlayer();
			}
		}
		if(currentPlayer == General.PlayerNumber.Player1){
			currentPlayer = General.PlayerNumber.Player2;
		} else {
			currentPlayer = General.PlayerNumber.Player1;
		}

		initPlayerTime = Time.time;
	}

	public float GetRemainingTime(){
		return Mathf.Clamp(gameTime - (Time.time - initTime), 0f, gameTime);
	}

	public float GetRemainingPlayerTime(){
		return Mathf.Clamp(playerTime - (Time.time - initPlayerTime), 0f, playerTime);
	}

	public float GetRemainingPlayerTimeProp(){
		return Mathf.Clamp(playerTime - (Time.time - initPlayerTime), 0f, playerTime) / playerTime;
	}
}
