using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class ScoreBoard : MonoBehaviour
{
//----------------------------------------------------------------------------------------------------------------------#VARIABLES

	// Def class variables for the scoreboard
	private Text[] textName = new Text[10];
	private Text[] textScore = new Text[10];
	private string[] currentName = new string[10];
	private string[] currentScore = new string[10];

	// Def class variables for the scoreboard
	public enum Board {
		LowscoreSingle = 0,
		LowscoreMulti,
		HighscoreSingle,
		HighscoreMulti
	};
	public Board selectBoard;

	// Def & init constants of class scoreboard
	private string[] board = new string[] {"Assets/Resources/lowScoreSingle.txt", "Assets/Resources/lowScoreMulti.txt", "Assets/Resources/highScoreSingle.txt", "Assets/Resources/highScoreMulti.txt"};

//----------------------------------------------------------------------------------------------------------------------#START
	// Use this for initialization
	void Start () {
		for (int i = 0; i < 10; i++) {
			//textName [i] = transform.Find(selectBoard.ToString()+"/Entry ("+(i+1)+")/Name ("+(i+1)+")").GetComponent<Text> ();
			//textScore [i] = transform.Find(selectBoard.ToString()+"/Entry ("+(i+1)+")/Score ("+(i+1)+")").GetComponent<Text> ();
			textName [i] = gameObject.transform.GetChild(i+1).GetChild(1).GetComponent<Text>();
			textScore [i] = gameObject.transform.GetChild(i+1).GetChild(2).GetComponent<Text>();
		}
		readScore ();
		for (int i = 0; i < 10; i++) {
			textName [i].text = currentName [i];
			textScore [i].text = currentScore [i];
		}
	}

//----------------------------------------------------------------------------------------------------------------------#FUNCTIONS
	void readScore() {
		// Ich Idiot hatte ernsthaft die String-Arrays hier unten nochmal drinnen -_-
		StreamReader reader = new StreamReader(board[(int) selectBoard]); 
		string entry;
		for (int i = 0; i < currentScore.Length; i++) {
			entry = reader.ReadLine ();
			currentName[i] = entry.Substring(0, entry.IndexOf("#~#"));
			currentScore[i] = entry.Substring(entry.IndexOf("#~#")+3);
		}
		reader.Close();
	}
}