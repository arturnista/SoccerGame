using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour {

	private const string PLAYER_GOAL_TEXT = "name goal!";

	private Text player1Text;
	private Text player2Text;
	private Text timeText;
	private Slider playerTimeSlider;

	public GameObject goalPanel;
	private Text goalText;
	private Image playerIcon;

	public GameObject finishPanel;
	private Text finishPlayer1Name;
	private Text finishPlayer1Score;
	private Image finishPlayer1Icon;
	private Text finishPlayer2Name;
	private Text finishPlayer2Score;
	private Image finishPlayer2Icon;

	private PlayerData player1Data;
	private PlayerData player2Data;

	private GameController gameController;

	void Awake () {
		goalText = goalPanel.transform.FindChild("GoalText").GetComponent<Text>();
		playerIcon = goalPanel.transform.FindChild("PlayerIcon").GetComponent<Image>();
		DisableGoalText();

		finishPlayer1Name = finishPanel.transform.FindChild("Player1Name").GetComponent<Text>();
		finishPlayer1Score = finishPanel.transform.FindChild("Player1Score").GetComponent<Text>();
		finishPlayer1Icon = finishPanel.transform.FindChild("Player1Icon").GetComponent<Image>();
		finishPlayer2Name = finishPanel.transform.FindChild("Player2Name").GetComponent<Text>();
		finishPlayer2Score = finishPanel.transform.FindChild("Player2Score").GetComponent<Text>();
		finishPlayer2Icon = finishPanel.transform.FindChild("Player2Icon").GetComponent<Image>();
		DisableFinishGameText();

		player1Text = GameObject.Find("Player1Panel").GetComponentInChildren<Text>();
		player2Text = GameObject.Find("Player2Panel").GetComponentInChildren<Text>();

		GameObject p = GameObject.Find("TimePanel");
		timeText = p.GetComponentInChildren<Text>();
		playerTimeSlider = p.GetComponentInChildren<Slider>();
		gameController = GameObject.FindObjectOfType<GameController>();
	}

	void Start() {
		player1Data = gameController.GetPlayer1Data();
		player2Data = gameController.GetPlayer2Data();

		UpdateGoalsText();
	}

	void Update(){
		float time = gameController.GetRemainingTime();
		if(time <= 0){
			return;
		}
		int min = (int) (time / 60);
		int sec = (int) (time % 60);
		timeText.text = min + ":" + sec.ToString("00");

		playerTimeSlider.value = gameController.GetRemainingPlayerTimeProp();
	}

	public void EnableGoalText(PlayerData plData){
		string goalStr = PLAYER_GOAL_TEXT;
		goalStr = goalStr.Replace("name", plData.playerName);
		playerIcon.sprite = plData.playerSprite;
		goalText.text = goalStr;
		goalPanel.SetActive(true);

		UpdateGoalsText();
	}

	public void DisableGoalText(){
		goalPanel.SetActive(false);
	}

	public void EnableFinishGameText(){
		finishPlayer1Name.text = player1Data.playerName;
		finishPlayer1Score.text = player1Data.playerGoals.ToString();
		finishPlayer1Icon.sprite = player1Data.playerSprite;

		finishPlayer2Name.text = player2Data.playerName;
		finishPlayer2Score.text = player2Data.playerGoals.ToString();
		finishPlayer2Icon.sprite = player2Data.playerSprite;

		finishPanel.SetActive(true);
	}

	public void DisableFinishGameText(){
		finishPanel.SetActive(false);
	}

	void UpdateGoalsText(){
		player1Text.text = player1Data.playerGoals.ToString();
		player2Text.text = player2Data.playerGoals.ToString();
	}
}
