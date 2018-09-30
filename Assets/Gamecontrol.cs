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
	private short count;
	private float s_fps;
	private Text scoreText1;
	private Text scoreText2;
	private int index;
	private bool inserted;
	private string[] lowScoreBoard;
	private string[] highScoreBoard;
	private const byte fractionalDigitsFPS = 3;
	private Text interactiveField;

	private Ball ball;
	private Ball_Retro ballRetro;
	private ItemSpawning item;


	private RawImage[] itemPlayer1_Light = new RawImage[11];
	private RawImage[] itemPlayer1_Dark = new RawImage[11];
	private RawImage[] itemPlayer2_Light = new RawImage[11];
	private RawImage[] itemPlayer2_Dark = new RawImage[11];

	void Awake() {
		// Init global variables
		Need.scene_index = SceneManager.GetActiveScene ().buildIndex;
		generateRandom ();
	}

	void Start() {
		// Init class variables
		i = 0;
		cnt = 0;
		count = 0;
		inserted = false;
		fps = GameObject.Find ("FPSTextField").GetComponent<Text> ();
		lowScoreBoard = new string[] { "Assets/Resources/lowScoreSingle.txt", "Assets/Resources/lowScoreMulti.txt" };
		highScoreBoard = new string[] { "Assets/Resources/highScoreSingle.txt", "Assets/Resources/highScoreMulti.txt" };
		if (Need.scene_index == 1 || Need.scene_index == 3) {
			scoreText1 = GameObject.Find ("ScorePlayer1").GetComponent <Text> ();
			scoreText2 = GameObject.Find ("ScorePlayer2").GetComponent <Text> ();
			// GameObject is not longer activated in the retro version!
			//interactiveField = GameObject.Find ("InteractiveField").GetComponent<Text> ();
		}
		//Wichtig, darf nicht mit oben rein!
		if (Need.scene_index == 1) {
			ball = GameObject.Find ("Ball").GetComponent<Ball> ();
			item = GameObject.Find ("Item").GetComponent<ItemSpawning> ();
			scoreText1.text = Need.player1_name + ": " + Need.player1_points;
			scoreText2.text = Need.player2_name + ": " + Need.player2_points;
			interactiveField = GameObject.Find ("InteractiveField").GetComponent<Text> ();
			for (int j = 0; j <= 10; j++) {
				itemPlayer1_Dark [j] = GameObject.Find ("IconsPlayer_1").transform.GetChild (j).GetComponent<RawImage> ();
				itemPlayer1_Dark [j].enabled = false;
				itemPlayer1_Light[j] = GameObject.Find ("IconsPlayer_1").transform.GetChild (11+j).GetComponent<RawImage> ();
				itemPlayer1_Light [j].enabled = false;
				itemPlayer2_Dark[j] = GameObject.Find ("IconsPlayer_2").transform.GetChild (j).GetComponent<RawImage> ();
				itemPlayer2_Dark [j].enabled = false;
				itemPlayer2_Light[j] = GameObject.Find ("IconsPlayer_2").transform.GetChild (11+j).GetComponent<RawImage> ();
				itemPlayer2_Light [j].enabled = false;
			}
			//Debug.Log ("GamGAMECONTROL: Icons: "+itemPlayer1_Light [10].enabled);
			//Debug.Log ("GamGAMECONTROL: Icons: "+itemPlayer2_Light [10].enabled);
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

		if (Need.scene_index == 1 || Need.scene_index == 3) {
			// Trigger the Pause scene
			if (Input.GetKeyDown (KeyCode.Escape)) {
				SceneManager.LoadScene (2);
				Need.last_scene = Need.scene_index;
			}
		}

		if (Need.scene_index == 1) {
			showLifes ();

			//Debug.Log (Need.isGame);
			if (Need.player1_lifes > 0 && Need.player2_lifes > 0) {
				Need.isGame = true;
			}

			// If the game is running
			if(Need.isGame) {
				handleIcons ();
				if (Need.skipNew) {
					Need.skipNew = false;
					ball.startNewGame ();
					item.ItemEntry ();
				}

				if (Need.newRound) {
					Need.isPoint = false;
					if (!Need.isSubmited) {
						hitSpace ();
					} else {
						if (!Need.runOnce) {
							Need.isWaiting = true;
						}
						//Maybe this can trigger some problems
						if (Need.isWaiting) {
							if (!Need.runOnce) {
								Need.runOnce = true;
								StartCoroutine (showCountdown ());
							}
						} else {
							Need.runOnce = false;
							Need.newRound = false;
							ball.startNewGame ();
							item.ItemEntry ();
						}
					}
				} else {
					// If the Game is running
					Need.isSubmited = false;
					item.ItemEntry ();
					calcPoints ();
					//handleIcons ();
					// Handle the icons of the items
					if (Need.isItem) {
						showIcon ();
					} else {
						hideIcon ();
					}
					// Handle the lifes of the players an show Points if there is a change, but what is with the init?
					if (Need.isPoint) {
						Need.newRound = true;
						ball.reset ();
						if (Need.lastPoint == Need.player1_name) {
							Need.player2_lifes -= 1;
							Need.player1_points += Need.points;
						} else {
							Need.player1_lifes -= 1;
							Need.player2_points += Need.points;
						}
						Need.points = 0;
						scoreText1.text = Need.player1_name + ": " + Need.player1_points;
						scoreText2.text = Need.player2_name + ": " + Need.player2_points;
					}
					// Handle the lifes of the players
					checkLifes ();
				}
			} else {
				// The winner becomes for each life an multiplier of 0.5x
				if (!Need.isSinglePlayer) {
					if (Need.player1_lifes > 0) {
						// #FLOATINT
						Need.player1_points += Need.player1_points * Need.player1_lifes;
					} else {
						// #FLOATINT
						Need.player2_points += Need.player2_points * Need.player2_lifes;
					}
				}
				// If the game is over and the timer is at zero
				if(!Need.isWaiting) {
					if (!Need.isSinglePlayer) {
						checkScore (Need.player2_name, Need.player2_points);
					}
					checkScore (Need.player1_name, Need.player1_points);
					SceneManager.LoadScene (0);
				}
			}
		}
		if (Need.scene_index == 3) {
			Need.isGame = true;
			if (Need.newRound) {
				Need.newRound = false;
				ballRetro.startNewGame ();
			} else {
				// Show Points if there is a change, but what is with the init?
				if (Need.isPoint) {
					Need.newRound = true;
					ballRetro.reset ();
					if (Need.lastPoint == Need.player1_name) {
						Need.player1_points++;
					} else {
						Need.player2_points++;
					}
					scoreText1.text = "" + Need.player1_points;
					scoreText2.text = "" + Need.player2_points;
				}
			}
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
		if (Need.scene_index == 0 && !Need.isGame) {
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
			}
			if(Need.isPoint) {
				Debug.Log("EVENT: isPoint");
				Need.inRetro = false;
				ItemHandler.eventFlags[(int)ItemHandler.Events.Pong2D] = false; // deaktiviere Event
				if (Need.lastPoint == Need.player1_name) {
					// The looser lose one life
					Need.player2_lifes -= 1;
					// The winner get his points multiplied by 1.5x
					// #FLOATINT
					Need.player1_points *= 2;
				} else {
					Need.player1_lifes -= 1;
					// #FLOATINT
					Need.player2_points *= 2;
				}
				// Go back to the 3D mode
				SceneManager.LoadScene (1);
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
				if (int.Parse (highScore [i].Substring (highScore [i].LastIndexOf ("#~#") + 3)) < score && !inserted) {
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

	// Not a beautyful method, but functional ;)
	private void checkLifes() {
		showLifes ();
        if (Need.lastPoint == Need.player1_name)
        {
            if (Need.player2_lifes == 0)
            {
				Need.isGame = false;
				StartCoroutine(showGameOver(Need.player2_name, Need.player1_name));
            }
        }
        else
        {
            if (Need.player1_lifes == 0)
            {
				Need.isGame = false;
				StartCoroutine(showGameOver(Need.player1_name, Need.player2_name));
            }
        }
	}

	// Only for one item at the same time!!!
    
	private void showIcon() {
		if (ItemHandler.eventFlags [(int)ItemHandler.Events.BallSpeedUp]) {
			if (Need.lastItem == Need.player1_name) {
				itemPlayer1_Dark [0].enabled = true;
				itemPlayer1_Dark [6].enabled = true;
				itemPlayer1_Light [0].enabled = true;
				itemPlayer1_Light [6].enabled = true;
			} else {
				itemPlayer2_Dark [0].enabled = true;
				itemPlayer2_Dark [6].enabled = true;
				itemPlayer2_Light [0].enabled = true;
				itemPlayer2_Light [6].enabled = true;
			}
		}
		if(ItemHandler.eventFlags[(int)ItemHandler.Events.BallRespawn]) {
			if (Need.lastItem == Need.player1_name) {
				itemPlayer1_Dark [0].enabled = true;
				itemPlayer1_Dark [10].enabled = true;
				itemPlayer1_Light [0].enabled = true;
				itemPlayer1_Light [10].enabled = true;
			} else {
				itemPlayer2_Dark [0].enabled = true;
				itemPlayer2_Dark [10].enabled = true;
				itemPlayer2_Light [0].enabled = true;
				itemPlayer2_Light [10].enabled = true;
			}
		}
		if(ItemHandler.eventFlags[(int)ItemHandler.Events.BallDisappear]) {
			if (Need.lastItem == Need.player1_name) {
				itemPlayer1_Dark [0].enabled = true;
				itemPlayer1_Dark [8].enabled = true;
				itemPlayer1_Light [0].enabled = true;
				itemPlayer1_Light [8].enabled = true;
			} else {
				itemPlayer2_Dark [0].enabled = true;
				itemPlayer2_Dark [8].enabled = true;
				itemPlayer2_Light [0].enabled = true;
				itemPlayer2_Light [8].enabled = true;
			}
		}
		if(ItemHandler.eventFlags[(int)ItemHandler.Events.BallDirectionInversion]) {
			if (Need.lastItem == Need.player1_name) {
				itemPlayer1_Dark [0].enabled = true;
				itemPlayer1_Dark [4].enabled = true;
				itemPlayer1_Light [0].enabled = true;
				itemPlayer1_Light [4].enabled = true;
			} else {
				itemPlayer2_Dark [0].enabled = true;
				itemPlayer2_Dark [4].enabled = true;
				itemPlayer2_Light [0].enabled = true;
				itemPlayer2_Light [4].enabled = true;
			}
		}
		if(ItemHandler.eventFlags[(int)ItemHandler.Events.CameraShaking]) {
			if (Need.lastItem == Need.player1_name) {
				itemPlayer1_Dark [2].enabled = true;
				itemPlayer1_Dark [7].enabled = true;
				itemPlayer1_Light [2].enabled = true;
				itemPlayer1_Light [7].enabled = true;
			} else {
				itemPlayer2_Dark [2].enabled = true;
				itemPlayer2_Dark [7].enabled = true;
				itemPlayer2_Light [2].enabled = true;
				itemPlayer2_Light [7].enabled = true;
			}
		}
		if(ItemHandler.eventFlags[(int)ItemHandler.Events.NothingHappend]) {
			if (Need.lastItem == Need.player1_name) {
				itemPlayer1_Dark [3].enabled = true;
				itemPlayer1_Light [3].enabled = true;
			} else {
				itemPlayer2_Dark [3].enabled = true;
				itemPlayer2_Light [3].enabled = true;
			}
		}
		if(ItemHandler.eventFlags[(int)ItemHandler.Events.PanelInversion]) {
			if (Need.lastItem == Need.player1_name) {
				itemPlayer1_Dark [1].enabled = true;
				itemPlayer1_Dark [4].enabled = true;
				itemPlayer1_Light [1].enabled = true;
				itemPlayer1_Light [4].enabled = true;
			} else {
				itemPlayer2_Dark [1].enabled = true;
				itemPlayer2_Dark [4].enabled = true;
				itemPlayer2_Light [1].enabled = true;
				itemPlayer2_Light [4].enabled = true;
			}
		}
		if(ItemHandler.eventFlags[(int)ItemHandler.Events.PanelShrinking]) {
			if (Need.lastItem == Need.player1_name) {
				itemPlayer1_Dark [1].enabled = true;
				itemPlayer1_Dark [5].enabled = true;
				itemPlayer1_Light [1].enabled = true;
				itemPlayer1_Light [5].enabled = true;
			} else {
				itemPlayer2_Dark [1].enabled = true;
				itemPlayer2_Dark [5].enabled = true;
				itemPlayer2_Light [1].enabled = true;
				itemPlayer2_Light [5].enabled = true;
			}
		}
		if(ItemHandler.eventFlags[(int)ItemHandler.Events.PanelSlowdown]) {
			if (Need.lastItem == Need.player1_name) {
				itemPlayer1_Dark [1].enabled = true;
				itemPlayer1_Dark [9].enabled = true;
				itemPlayer1_Light [1].enabled = true;
				itemPlayer1_Light [9].enabled = true;
			} else {
				itemPlayer2_Dark [1].enabled = true;
				itemPlayer2_Dark [9].enabled = true;
				itemPlayer2_Light [1].enabled = true;
				itemPlayer2_Light [9].enabled = true;
			}
		}
	}

	// Is only functional if there is one item at the same time!!!
	private void hideIcon() {
		if (Need.lastItem == Need.player1_name) {
			for (int i = 0; i <= 10; i++) {
				if (itemPlayer1_Dark [i].enabled || itemPlayer1_Light [i].enabled) {
					itemPlayer1_Dark [i].enabled = false;
					itemPlayer1_Light [i].enabled = false;
				}
			}
		} else {
			for (int i = 0; i <= 10; i++) {
				if (itemPlayer2_Dark [i].enabled || itemPlayer2_Light [i].enabled) {
					itemPlayer2_Dark [i].enabled = false;
					itemPlayer2_Light [i].enabled = false;
				}
			}
		}
	}
    

	IEnumerator showGameOver(string loserName, string winnerName) {
		Need.isWaiting = true;
		interactiveField.text = "GameOver :'(";
		yield return new WaitForSeconds(3);
		interactiveField.text = "You loose! " + loserName;
		yield return new WaitForSeconds(3);
		if (!Need.isSinglePlayer) {
			interactiveField.text = "Congratulations " + winnerName + " you win :)";
			yield return new WaitForSeconds (3);
		}
		Need.isWaiting = false;
	}

	IEnumerator showCountdown() {
		//for (int i = 4; i >= 0; i--) {
		//	if (i > 1) {
		//		interactiveField.text = "" + (i-1);
		//	} else if (i == 1) {
		//		interactiveField.text = "GO";
		//	} else {
		//		interactiveField.text = "";
		//		Need.isWaiting = false;
		//	}
		//	if (i >= 1) {
		//		yield return new WaitForSeconds (1);
		//	} else {
		//		yield return new WaitForSeconds (0);
		//	}
		//}
		interactiveField.text = "3";
		yield return new WaitForSeconds (1);
		interactiveField.text = "2";
		yield return new WaitForSeconds (1);
		interactiveField.text = "1";
		yield return new WaitForSeconds (1);
		interactiveField.text = "GO";
		yield return new WaitForSeconds (1);
		interactiveField.text = "";
		Need.isWaiting = false;
	}

	private void hitSpace() {
		interactiveField.text = "Press <space> to start";
		if (Input.GetKeyDown (KeyCode.Space)) {
			Need.isSubmited = true;
		}
	}

	// Points only calculated for the time; alternative for the ball changes...
	private void calcPoints() {
		if(count == 60) {
			if (Need.points < 100) {
				Need.points += 10;
			} else if (Need.points >= 100 && Need.points < 1000) {
				Need.points += 100;
			} else if (Need.points >= 1000 && Need.points < 10000) {
				Need.points += 1000;
			} else if (Need.points >= 10000 && Need.points < 100000) {
				Need.points += 10000;
			} else if (Need.points >= 100000 && Need.points < 1000000) {
				Need.points += 100000;
			} else {
				Need.points += 1000000;
			}
			count = 0;
			//Debug.Log ("GC: Points: "+Need.points);
		}
		count++;
	}

	private void generateRandom() {
		float random = 0;
		//Damit die Wahrscheinlichkeit wirklich gleich ist!!! ein Wert MUSS die Grenze sein
		while (random == 0) {
			random = Random.Range (-0.3f, 0.3f);
		}
		if (random > 0) {
			Need.goesToPlayer1 = true;
		}
		if (random < 0) {
			Need.goesToPlayer1 = false;
		}
	}

	//Need.isWaiting -> bei start der Methode, dann parallel warten und benachrichtigen, wenn fertig -> perfektes Warten
	//Start() {
	//	StartCoroutine(waitFor(5));
	//}
	//
	//IEnumerator waitFor(int sec)
	//{
	//	print(Time.time);
	//	Need.isWaiting = true;
	//	yield return new WaitForSeconds(sec);
	//	print(Time.time);
	//	Need.isWaiting = false;
	//}

	private void handleIcons() {
		if (Need.isSinglePlayer) {
			for (int i = 0; i <= 10; i++) {
				if (itemPlayer1_Light [i].enabled || itemPlayer2_Light [i].enabled) {
					itemPlayer1_Light [i].enabled = false;
					itemPlayer2_Light [i].enabled = false;
				}
			}
		} else {
			if (Need.ball_direction_x) {
				for (int i = 0; i <= 10; i++) {
					if (itemPlayer1_Dark [i].enabled || itemPlayer2_Dark [i].enabled) {
						itemPlayer1_Dark [i].enabled = false;
						itemPlayer1_Light [i].enabled = true;
						itemPlayer2_Dark [i].enabled = false;
						itemPlayer2_Light [i].enabled = true;
					}
				}
			} else {
				for (int i = 0; i <= 10; i++) {
					if (itemPlayer1_Light [i].enabled || itemPlayer2_Light [i].enabled) {
						itemPlayer1_Light [i].enabled = false;
						itemPlayer1_Dark [i].enabled = true;
						itemPlayer2_Light [i].enabled = false;
						itemPlayer2_Dark [i].enabled = true;
					}
				}
			}
		}
	}

	private void showLifes() {
		if (Need.isSinglePlayer) {
			for (int i = 0; i <= 5; i++) {
				GameObject.Find ("Player1_Lifes").transform.GetChild (6 + i).GetComponent<RawImage> ().enabled = false;
				GameObject.Find ("Player2_Lifes").transform.GetChild (6 + i).GetComponent<RawImage> ().enabled = false;
			}
		} else {
			if (Need.ball_direction_x) {
				for (int i = 0; i <= 5; i++) {
					GameObject.Find ("Player1_Lifes").transform.GetChild (i).GetComponent<RawImage> ().enabled = true;
					GameObject.Find ("Player2_Lifes").transform.GetChild (i).GetComponent<RawImage> ().enabled = true;
					GameObject.Find ("Player1_Lifes").transform.GetChild (6 + i).GetComponent<RawImage> ().enabled = false;
					GameObject.Find ("Player2_Lifes").transform.GetChild (6 + i).GetComponent<RawImage> ().enabled = false;
				}
			} else {
				for (int i = 0; i <= 5; i++) {
					GameObject.Find ("Player1_Lifes").transform.GetChild (6 + i).GetComponent<RawImage> ().enabled = true;
					GameObject.Find ("Player2_Lifes").transform.GetChild (6 + i).GetComponent<RawImage> ().enabled = true;
					GameObject.Find ("Player1_Lifes").transform.GetChild (i).GetComponent<RawImage> ().enabled = false;
					GameObject.Find ("Player2_Lifes").transform.GetChild (i).GetComponent<RawImage> ().enabled = false;
				}
			}
		}

		if (Need.player2_lifes == 2)
		{
			if (Need.ball_direction_x) {
				GameObject.Find ("Player2_Lifes").transform.GetChild (0).GetComponent<RawImage> ().enabled = false;
			} else {
				GameObject.Find ("Player2_Lifes").transform.GetChild (6).GetComponent<RawImage> ().enabled = false;
			}
		}
		if (Need.player2_lifes == 1)
		{
			if (Need.ball_direction_x) {
				GameObject.Find ("Player2_Lifes").transform.GetChild (0).GetComponent<RawImage> ().enabled = false;
				GameObject.Find ("Player2_Lifes").transform.GetChild (2).GetComponent<RawImage> ().enabled = false;
			} else {
				GameObject.Find ("Player2_Lifes").transform.GetChild (6).GetComponent<RawImage> ().enabled = false;
				GameObject.Find ("Player2_Lifes").transform.GetChild (8).GetComponent<RawImage> ().enabled = false;
			}
		}
		if (Need.player2_lifes == 0)
		{
			if (Need.ball_direction_x) {
				GameObject.Find ("Player2_Lifes").transform.GetChild (0).GetComponent<RawImage> ().enabled = false;
				GameObject.Find ("Player2_Lifes").transform.GetChild (2).GetComponent<RawImage> ().enabled = false;
				GameObject.Find ("Player2_Lifes").transform.GetChild (4).GetComponent<RawImage> ().enabled = false;
			} else {
				GameObject.Find ("Player2_Lifes").transform.GetChild (6).GetComponent<RawImage> ().enabled = false;
				GameObject.Find ("Player2_Lifes").transform.GetChild (8).GetComponent<RawImage> ().enabled = false;
				GameObject.Find ("Player2_Lifes").transform.GetChild (10).GetComponent<RawImage> ().enabled = false;
			}
		}

		if (Need.player1_lifes == 2)
		{
			if (Need.ball_direction_x) {
				GameObject.Find ("Player1_Lifes").transform.GetChild (0).GetComponent<RawImage> ().enabled = false;
			} else {
				GameObject.Find ("Player1_Lifes").transform.GetChild (6).GetComponent<RawImage> ().enabled = false;
			}
		}
		if (Need.player1_lifes == 1)
		{
			if (Need.ball_direction_x) {
				GameObject.Find ("Player1_Lifes").transform.GetChild (0).GetComponent<RawImage> ().enabled = false;
				GameObject.Find ("Player1_Lifes").transform.GetChild (2).GetComponent<RawImage> ().enabled = false;
			} else {
				GameObject.Find ("Player1_Lifes").transform.GetChild (6).GetComponent<RawImage> ().enabled = false;
				GameObject.Find ("Player1_Lifes").transform.GetChild (8).GetComponent<RawImage> ().enabled = false;
			}
		}
		if (Need.player1_lifes == 0)
		{
			if (Need.ball_direction_x) {
				GameObject.Find ("Player1_Lifes").transform.GetChild (0).GetComponent<RawImage> ().enabled = false;
				GameObject.Find ("Player1_Lifes").transform.GetChild (2).GetComponent<RawImage> ().enabled = false;
				GameObject.Find ("Player1_Lifes").transform.GetChild (4).GetComponent<RawImage> ().enabled = false;
			} else {
				GameObject.Find ("Player1_Lifes").transform.GetChild (6).GetComponent<RawImage> ().enabled = false;
				GameObject.Find ("Player1_Lifes").transform.GetChild (8).GetComponent<RawImage> ().enabled = false;
				GameObject.Find ("Player1_Lifes").transform.GetChild (10).GetComponent<RawImage> ().enabled = false;
			}
		}
	}
}