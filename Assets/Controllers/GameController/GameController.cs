using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	const float playerTime = 7f;

	private bool gameStarted;

	private PlayerData player1Data;
	private PlayerData player2Data;

	private float gameTime;
	private float initTime;
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

		TeamTransporter tTransp = GameObject.FindObjectOfType<TeamTransporter>();
		player1Data = tTransp.PlayerData1();
		player2Data = tTransp.PlayerData2();
		tTransp.Destroy();

		gameStarted = false;
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
		gameStarted = false;

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

			gameStarted = true;
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

	public bool IsGameStarted(){
		return gameStarted;
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

	public void CloseGame(){
		SceneManager.LoadScene("Menu");
	}
}
