using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class Gamecontrol : MonoBehaviour {

	// Class varibales
	private int i;
	private Text fps;
	private byte cnt;
	private int second;
	private float s_fps;
	private Text scoreText1;
	private Text scoreText2;
	private int index;
	private bool inserted;
	private bool wasGameOver;
	private string[] lowScoreBoard;
	private string[] highScoreBoard;
	private const byte fractionalDigitsFPS = 3;
	private Text interactiveField;

	private Ball ball;
	private Ball_Retro ballRetro;
	private ItemSpawning item;

	void Awake() {
		// Init global variables
		Need.scene_index = SceneManager.GetActiveScene ().buildIndex;
		StartCoroutine (waitfor(0));
	}

	void Start() {
		// Init class variables
		i = 0;
		cnt = 0;
		inserted = false;
		wasGameOver = false;
		fps = GameObject.Find ("FPSTextField").GetComponent<Text> ();
		lowScoreBoard = new string[] { "Assets/Resources/lowScoreSingle.txt", "Assets/Resources/lowScoreMulti.txt" };
		highScoreBoard = new string[] { "Assets/Resources/highScoreSingle.txt", "Assets/Resources/highScoreMulti.txt" };
		if (Need.scene_index == 1 || Need.scene_index == 3) {
			scoreText1 = GameObject.Find ("ScorePlayer1").GetComponent <Text> ();
			scoreText2 = GameObject.Find ("ScorePlayer2").GetComponent <Text> ();
			interactiveField = GameObject.Find ("InteractiveField").GetComponent<Text> ();
		}
		//Wichtig, darf nicht mit oben rein!
		if (Need.scene_index == 1) {
			ball = GameObject.Find ("Ball").GetComponent<Ball> ();
			item = GameObject.Find ("Item").GetComponent<ItemSpawning> ();
			scoreText1.text = Need.player1_name + ": " + Need.player1_points;
			scoreText2.text = Need.player2_name + ": " + Need.player2_points;
		}
		if (Need.scene_index == 3){
			ballRetro = GameObject.Find ("Ball_Retro").GetComponent<Ball_Retro> ();
		}
	}

	void Update() {
		//Debug.Log (((Mathf.Round ((1 / Time.deltaTime) * Mathf.Pow (10, fractionalDigitsFPS))) / Mathf.Pow (10, fractionalDigitsFPS)).ToString ());
		if (Need.showFPS) {
			// ca. jede Sechstel-Sekunde FPS Anzeige aktualisieren
			if (cnt >= 10) {
				// FPS ermitteln und runden
				fps.text = "FPS: "+((Mathf.Round ((1 / Time.deltaTime) * Mathf.Pow (10, fractionalDigitsFPS))) / Mathf.Pow (10, fractionalDigitsFPS)).ToString ();
				cnt = 0;
			}
			cnt++;
		} else {
			fps.text = "";
		}
	}

	// Use this for initialization
	void FixedUpdate() {
		Need.scene_index = SceneManager.GetActiveScene ().buildIndex;
		if (Need.skipNew) {
			if (Need.scene_index == 1) {
				ball.startNewGame ();
				item.ItemEntry ();
				Need.skipNew = false;
			}
		}
		if (Need.scene_index == 1 || Need.scene_index == 3) {
			if (Need.newRound) {
				Need.isPoint = false;
				if (Need.scene_index == 1) {
					if (hitSpace ()) {
						Need.newRound = false;
						showCountdown ();

						ball.startNewGame ();
						item.ItemEntry();
					}
				}
				if (Need.scene_index == 3) {
					ballRetro.startNewGame ();
				}
			}
		}

		// If the Game is running
		if (Need.scene_index == 1) {
			Need.isGame = true;
			item.ItemEntry ();
			// Show Points if there is a change, but what is with the init?
			if (Need.isPoint) {
				Need.newRound = true;
				scoreText1.text = Need.player1_name + ": " + Need.player1_points;
				scoreText2.text = Need.player2_name + ": " + Need.player2_points;
			}
			//Trigger the Pause scene
			if (Input.GetKeyDown (KeyCode.Escape)) {
				SceneManager.LoadScene (2);
				Need.last_scene = Need.scene_index;
			}
		}
		if(Need.scene_index == 3) {
			Need.isGame = true;
			// Show Points if there is a change, but what is with the init?
			if (Need.isPoint) {
				Need.newRound = true;
				scoreText1.text = "" + Need.player1_points;
				scoreText2.text = "" + Need.player2_points;
			}
			//Trigger the Pause scene
			if (Input.GetKeyDown (KeyCode.Escape)) {
				SceneManager.LoadScene (2);
				Need.last_scene = Need.scene_index;
			}
		}
		if(Need.isGame && Need.isPoint) {
			Debug.Log ("Last point: "+Need.lastPoint);
			Debug.Log ("Checklifes P1: "+Need.player1_lifes);
			Debug.Log ("Checklifes P2: "+Need.player2_lifes);
			if (Need.lastPoint == Need.player1_name) {
				Need.player2_lifes -= 1;
			} else {
				Need.player1_lifes -= 1;
			}
			if (Need.scene_index == 1) {
				ball.reset ();
			}
			if (Need.scene_index == 3) {
				ballRetro.reset ();
			}
		}
		checkLifes ();
		if(!Need.isGame && wasGameOver) {
			if (!Need.isSinglePlayer) {
				checkScore (Need.player2_name, Need.player2_points);
			}
			checkScore (Need.player1_name, Need.player1_points);

		}

		//Menu triggers
		if (Need.scene_index == 2 && Need.isGame) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				Need.newRound = true;
				Need.isPaused = !Need.isPaused;
				SceneManager.LoadScene (Need.last_scene);
				Need.last_scene = Need.scene_index;
			}
		}
		if(Need.scene_index == 0 && !Need.isGame) {
			if (Input.GetKeyDown (KeyCode.Escape)) {
				Debug.Log("GAME: Quit...");
				Application.Quit();
			}
		}

		//EventFlag for Retro
		if (ItemHandler.eventFlags[(int)ItemHandler.Events.Pong2D])
		{
			// TODO: Single/Multiplayer
			if (!Need.inRetro) {
				Need.inRetro = true;
				Debug.Log ("EVENT: Starte Retro");
				Need.newRound = true;
				SceneManager.LoadScene (3); // 3d-Spiel wird druch Szenenwechsel pausiert
			} else {
				if (Need.newRound) {
					ballRetro.startNewGame ();
					Need.newRound = false;
				}
			}
			if(Need.isPoint) {
				Debug.Log("EVENT: isPoint");
				Need.inRetro = false;
				ItemHandler.eventFlags[(int)ItemHandler.Events.Pong2D] = false; // deaktiviere Event
				SceneManager.LoadScene(1);
			}  
		}

		ENTER_GOD_MODE(i);
	}

	// For all scenes
	void ENTER_GOD_MODE(int i) {
		// Why does this not work?!
		if (Input.GetKeyDown (KeyCode.G) && i == 0) {
			i++;
		}
		if (Input.GetKeyDown (KeyCode.O) && i == 1) {
			i++;
		}
		if (Input.GetKeyDown (KeyCode.D) && i == 2) {
			i++;
		}
		if (Input.GetKeyDown (KeyCode.M) && i == 3) {
			i++;
		}
		if (Input.GetKeyDown (KeyCode.O) && i == 4) {
			i++;
		}
		if (Input.GetKeyDown (KeyCode.D) && i == 5) {
			i++;
		}
		if (Input.GetKeyDown (KeyCode.E) && i == 6) {
			Need.godMode = true;
			SceneManager.LoadScene (3);
			Need.last_scene = Need.scene_index;
		}
	}

	// Additional functions
	private void checkScore(string player, int score) {
		// Look at the lowScore and HighScore
		if (!inserted) {
			string temp_prev = "";
			string temp_next = "";
			// If we are in the singlePlayerMode
			if (Need.isSinglePlayer) {
				index = 0;
			} else {
				index = 1;
			}
			string[] lowScore = readScore (lowScoreBoard [index]);
			// Catch all entrys and look at the store
			for (int i = 0; i < lowScore.Length; i++) {
				// If there is an new record write it
				if (int.Parse (lowScore [i].Substring (lowScore [i].LastIndexOf ("#~#") + 3)) > score && !inserted) {
					temp_prev = lowScore [i];
					lowScore [i] = player + "#~#" + score;
					inserted = true;
				}
				// If the record is at the position shift the following
				if (inserted && i < lowScore.Length -1) {
					temp_next = lowScore [i +1];
					lowScore [i +1] = temp_prev;
					temp_prev = temp_next;
				}
			}
			if (inserted) {
				writeScore (lowScoreBoard [index], lowScore);
				SceneManager.LoadScene (0);
			}
		}
		if (!inserted) {
			string temp_prev = "";
			string temp_next = "";
			// If we are in the singlePlayerMode
			if (Need.isSinglePlayer) {
				index = 0;
			} else {
				index = 1;
			}
			string[] highScore = readScore (highScoreBoard [index]);
			// Catch all entrys and look at the store
			for (int i = 0; i < highScore.Length; i++) {
				// If there is an new record write it
				if (int.Parse (highScore [i].Substring (highScore [i].LastIndexOf ("#~#") + 3)) > score && !inserted) {
					temp_prev = highScore [i];
					highScore [i] = player + "#~#" + score;
					inserted = true;
				}
				// If the record is at the position shift the folhighing
				if (inserted && i < highScore.Length -1) {
					temp_next = highScore [i +1];
					highScore [i +1] = temp_prev;
					temp_prev = temp_next;
				}
			}
			if (inserted) {
				writeScore (highScoreBoard [index], highScore);
				SceneManager.LoadScene (0);
			}
		}
	}

	private string[] readScore(string path) {
		//Read the score from the correct file
		string[] currentScore = new string [10];
		StreamReader reader = new StreamReader(path); 
		for (int i = 0; i < currentScore.Length; i++) {
			currentScore[i] = reader.ReadLine().ToString();
		}
		reader.Close();
		return currentScore;
	}

	private void writeScore(string path, string[] newScore) {
		//Write the score into the correct file
		StreamWriter writer = new StreamWriter(path, false);
		for (int i = 0; i < newScore.Length; i++) {
			writer.WriteLine(newScore[i]);
		}
		writer.Close();
	}

	private void checkLifes() {
        if (Need.lastPoint == Need.player1_name)
        {
            if (Need.player2_lifes == 2)
            {
                GameObject.Find("Player2_Lifes").transform.GetChild(4).gameObject.SetActive(false);
            }
            if (Need.player2_lifes == 1)
            {
                GameObject.Find("Player2_Lifes").transform.GetChild(4).gameObject.SetActive(false);
                GameObject.Find("Player2_Lifes").transform.GetChild(2).gameObject.SetActive(false);
            }
            if (Need.player2_lifes == 0)
            {
                GameObject.Find("Player2_Lifes").transform.GetChild(4).gameObject.SetActive(false);
                GameObject.Find("Player2_Lifes").transform.GetChild(2).gameObject.SetActive(false);
                GameObject.Find("Player2_Lifes").transform.GetChild(0).gameObject.SetActive(false);
            }
            if (Need.player2_lifes == 0)
            {
                Need.isGame = false;
                showGameOver(Need.player2_name);
                wasGameOver = true;
            }
        }
        else
        {
            if (Need.player1_lifes == 2)
            {
                GameObject.Find("Player1_Lifes").transform.GetChild(4).gameObject.SetActive(false);
            }
            if (Need.player1_lifes == 1)
            {
                GameObject.Find("Player1_Lifes").transform.GetChild(4).gameObject.SetActive(false);
                GameObject.Find("Player1_Lifes").transform.GetChild(2).gameObject.SetActive(false);
            }
            if (Need.player1_lifes == 0)
            {
                GameObject.Find("Player1_Lifes").transform.GetChild(4).gameObject.SetActive(false);
                GameObject.Find("Player1_Lifes").transform.GetChild(2).gameObject.SetActive(false);
                GameObject.Find("Player1_Lifes").transform.GetChild(0).gameObject.SetActive(false);
            }
            if (Need.player1_lifes == 0)
            {
                Need.isGame = false;
                showGameOver(Need.player1_name);
                wasGameOver = true;
            }
        }
	}

	private void showGameOver(string playerName) {
		interactiveField.text = "GameOver :'(";
		waitfor (3);
		interactiveField.text = "You loose! " + playerName;
	}

	private void showCountdown() {
		for (int i = 3; i >= 0; i--) {
			if (i != 0) {
				interactiveField.text = "" + i;
			} else {
				interactiveField.text = "GO";
			}
			waitfor (1);
		}
		interactiveField.text = "";
	}

	private bool hitSpace() {
		interactiveField.text = "Press <space> to start";
		if (Input.GetKeyDown (KeyCode.Space)) {
			Need.isPaused = !Need.newRound;
			return true;
		}
		return false;
	}

	private IEnumerator waitfor(int i) {
		yield return i;
	}
}