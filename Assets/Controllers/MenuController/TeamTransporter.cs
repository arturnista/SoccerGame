using UnityEngine;
using System.Collections;

public class TeamTransporter : MonoBehaviour {
	
	private Team team1;
	private General.PlayerController team1Controller;
	private Team team2;
	private General.PlayerController team2Controller;

	void Awake () {
		DontDestroyOnLoad(this.gameObject);
	}

	public void SetTeams(Team t1, General.PlayerController t1Controller, Team t2, General.PlayerController t2Controller){
		team1 = new Team();
		team1.name = t1.name;
		team1.sprite = t1.sprite;
		team1Controller = t1Controller;

		team2 = new Team();
		team2.name = t2.name;
		team2.sprite = t2.sprite;
		team2Controller = t2Controller;
	}

	public PlayerData PlayerData1(){
		PlayerData pt = new PlayerData();
		pt.playerController = team1Controller;
		pt.playerGoals = 0;
		pt.playerName = team1.name;
		pt.playerNumber = General.PlayerNumber.Player1;
		pt.playerSprite = team1.sprite;
		return pt;
	}

	public PlayerData PlayerData2(){
		PlayerData pt = new PlayerData();
		pt.playerController = team2Controller;
		pt.playerGoals = 0;
		pt.playerName = team2.name;
		pt.playerNumber = General.PlayerNumber.Player2;
		pt.playerSprite = team2.sprite;
		return pt;
	}

	public void Destroy(){
		Destroy(this.gameObject);
	}

}
