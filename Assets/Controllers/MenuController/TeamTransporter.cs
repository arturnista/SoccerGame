using UnityEngine;
using System.Collections;

public class TeamTransporter : MonoBehaviour {
	
	private Team team1;
	private Team team2;

	void Awake () {
		DontDestroyOnLoad(this.gameObject);
	}

	public void SetTeams(Team t1, Team t2){
		team1 = new Team();
		team1.name = t1.name;
		team1.sprite = t1.sprite;

		team2 = new Team();
		team2.name = t2.name;
		team2.sprite = t2.sprite;
	}

	public PlayerData PlayerData1(){
		PlayerData pt = new PlayerData();
		pt.playerController = General.PlayerController.Human;
		pt.playerGoals = 0;
		pt.playerName = team1.name;
		pt.playerNumber = General.PlayerNumber.Player1;
		pt.playerSprite = team1.sprite;
		return pt;
	}

	public PlayerData PlayerData2(){
		PlayerData pt = new PlayerData();
		pt.playerController = General.PlayerController.Computer;
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
