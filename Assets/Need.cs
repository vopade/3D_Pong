using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Need : MonoBehaviour {
	public static bool isPaused;
	public static bool isGame;
	public static bool isPoint;
	public static bool inRetro;
	public static bool newRound;
	public static bool isSinglePlayer;
	public static float defaultVelocityPanel;
	public static string player1_name;
	public static Vector3 player1_position;
	public static int player1_points;
	public static int player1_retroPoints;
	public static int player1_lifes;
	public static string player2_name;
	public static Vector3 player2_position;
	public static int player2_points;
	public static int player2_retroPoints;
	public static int player2_lifes;
	public static string lastPoint;
	public static float ball_position_x;
	public static float ball_position_y;
	public static float ball_position_z;
	public static float ball_speed;
	public static int ball_direction;
	public static int scene_index;
	public static int last_scene;
	public static bool newHighscore;
	public static bool newLowscore;
	public static bool godMode;
	public static bool showFPS;
	public static bool skipNew;
	public static bool ball_direction_x;
	public static bool ball_direction_y;
	public static bool ball_direction_z;
	public static int points;
	//public static bool[] items_flags;
	public static string lastItem;
	public static bool isItem;
	public static bool isWaiting;
	public static bool isSubmited;
	public static bool runOnce;
	public static bool isOnPannel;
	public static bool goesToPlayer1;
	public static bool isOnPannel1;
	public static bool isOnPannel2;

	public static Vector3 ball_position = new Vector3(ball_position_x, ball_position_y, ball_position_z);

	//THE IMPORTANTS FUNCTION OF THE GAME
	void Awake() {
		DontDestroyOnLoad (transform.gameObject);
	}

	//Fill the variables with the init values
	void Start() {
		isPaused = Initialisation.isPaused;
		isGame = Initialisation.isGame;
		isPoint = Initialisation.isPoint;
		isSinglePlayer = Initialisation.isSinglePlayer;
		defaultVelocityPanel = Initialisation.defaultVelocityPanel;
		player1_name = Initialisation.player1_name;
		player1_position = Initialisation.player1_position;
		player1_points = Initialisation.player1_points;
		player2_name = Initialisation.player2_name;
		player2_position = Initialisation.player2_position;
		player2_points = Initialisation.player2_points;
		ball_position_x = Initialisation.ball_position_x;
		ball_position_y = Initialisation.ball_position_y;
		ball_position_z = Initialisation.ball_position_z;
		ball_speed = Initialisation.ball_speed;
		ball_direction = Initialisation.ball_direction;
		scene_index = Initialisation.scene_index;
		last_scene = Initialisation.last_scene;
		newHighscore = Initialisation.newHighscore;
		newLowscore = Initialisation.newLowscore;
		godMode = Initialisation.godMode;
		showFPS = Initialisation.showFPS;
		player1_retroPoints = Initialisation.player1_retroPoints;
		player2_retroPoints = Initialisation.player2_retroPoints;
		lastPoint = Initialisation.lastPoint;
		player1_lifes = Initialisation.player1_lifes;
		player2_lifes = Initialisation.player2_lifes;
		inRetro = Initialisation.inRetro;
		newRound = Initialisation.newRound;
		skipNew = Initialisation.skipNew;
		ball_direction_x = Initialisation.ball_direction_x;
		ball_direction_y = Initialisation.ball_direction_y;
		ball_direction_z = Initialisation.ball_direction_z;
		points = Initialisation.points;
		//items_flags = Initialisation.items_flags;
		lastItem = Initialisation.lastItem;
		isItem = Initialisation.isItem;
		isWaiting = Initialisation.isWaiting;
		isSubmited = Initialisation.isSubmited;
		runOnce = Initialisation.runOnce;
		isOnPannel = Initialisation.isOnPannel;
		goesToPlayer1 = Initialisation.goesToPlayer1;
		isOnPannel1 = Initialisation.isOnPannel1;
		isOnPannel2 = Initialisation.isOnPannel2;
	}
}