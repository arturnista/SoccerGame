using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TeamSelector : MonoBehaviour {

	public GameObject teamTransporterPrefab;

	private Team team1;
	private General.PlayerController team1Controller;
	private int team1Index;

	private Team team2;
	private General.PlayerController team2Controller;
	private int team2Index;

	private TeamDatabase teamDatabase;
	private MenuUIController menuController;

	void Awake () {
		teamDatabase = GameObject.FindObjectOfType<TeamDatabase>();
		menuController = GameObject.FindObjectOfType<MenuUIController>();

		team1Index = 5;
		team2Index = 0;
		UpdateTeam();
	}
	
	public void SelectNextTeam1(){
		team1Index = teamDatabase.NextIndex(team1Index);
		ChangeTeam();
	}

	public void SelectPreviousTeam1(){
		team1Index = teamDatabase.PreviousIndex(team1Index);
		ChangeTeam();
	}

	public void SelectNextTeam2(){
		team2Index = teamDatabase.NextIndex(team2Index);
		ChangeTeam();
	}

	public void SelectPreviousTeam2(){
		team2Index = teamDatabase.PreviousIndex(team2Index);
		ChangeTeam();
	}

	void UpdateTeam(){
		team1 = teamDatabase.teamDatabase[team1Index];
		team2 = teamDatabase.teamDatabase[team2Index];
	}

	void ChangeTeam(){
		UpdateTeam();
		menuController.UpdateTeams();
	}

	public Team GetTeam1(){
		return team1;
	}

	public Team GetTeam2(){
		return team2;
	}

	public void StartGame(){
		GameObject ob = Instantiate(teamTransporterPrefab, Vector3.zero, Quaternion.identity) as GameObject;
		TeamTransporter tTranspo = ob.GetComponent<TeamTransporter>();
		tTranspo.SetTeams(team1, team2);
		SceneManager.LoadScene("Game");
	}
}
