using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuUIController : MonoBehaviour {

	private Text team1Name;
	private Dropdown team1Controller;
	private Image team1Sprite;

	private Text team2Name;
	private Dropdown team2Controller;
	private Image team2Sprite;

	private TeamSelector teamSelector;
	private Team team1;
	private Team team2;

	void Awake () {
		GameObject t1Panel = GameObject.Find("Team1Panel");
		team1Name = t1Panel.transform.FindChild("TeamName").GetComponent<Text>();
		team1Sprite = t1Panel.transform.FindChild("TeamSprite").GetComponent<Image>();

		GameObject t2Panel = GameObject.Find("Team2Panel");
		team2Name = t2Panel.transform.FindChild("TeamName").GetComponent<Text>();
		team2Sprite = t2Panel.transform.FindChild("TeamSprite").GetComponent<Image>();

		teamSelector = GameObject.FindObjectOfType<TeamSelector>();	
	}

	void Start(){
		UpdateTeams();
	}

	public void UpdateTeams(){
		team1 = teamSelector.GetTeam1();
		team2 = teamSelector.GetTeam2();

		team1Name.text = team1.name;
		team1Sprite.sprite = team1.sprite;
		team2Name.text = team2.name;
		team2Sprite.sprite = team2.sprite;
	}
}
