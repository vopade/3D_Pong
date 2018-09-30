using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialisation : MonoBehaviour {
	//SelfInit values
	//Global booleans for checking some stats
	public static bool isPaused = false;
	public static bool isGame = false;
	public static bool isPoint = false;
	public static bool inRetro = false;
	public static bool newRound = true;
	public static bool skipNew = false;
	public static bool isWaiting = false;
	public static bool isSubmited = false;
	public static bool runOnce = false;
	public static bool isOnPannel = false;
	public static bool isOnPannel1 = false;
	public static bool isOnPannel2 = false;

	//Global bool for checking gameMode
	public static bool isSinglePlayer = true;

	//Global player values
	public static float defaultVelocityPanel = 15.0f;
	public static int points = 0;

	//Global Player1 values
	public static string player1_name = "Player_1";
	public static Vector3 player1_position = new Vector3(49.5f, 0f, 0f);
	public static int player1_points = 0;
	public static int player1_retroPoints = 0;
	public static int player1_lifes = 3;

	//Global Player1 values
	public static string player2_name = "Player_2";
	public static Vector3 player2_position = new Vector3(49.5f, 0f, 0f);
	public static int player2_points = 0;
	public static int player2_retroPoints = 0;
	public static int player2_lifes = 3;

	//Global ball values
	public static float ball_position_x = 0f;
	public static float ball_position_y = 0f;
	public static float ball_position_z = 0f;
	public static float ball_speed = 2.0f;
	//public static string[] axis_direction_ball = new string[] { "x+", "x-", "y+", "y-", "z+", "z-" };

	//Global item values
	public static bool isItem = false;


	//NonSelfInit values
	//Ball start direction
	public static bool goesToPlayer1;

	//The direction of the ball
	public static int ball_direction;
	public static bool ball_direction_x;
	public static bool ball_direction_y;
	public static bool ball_direction_z;

	//The flags of all events
	//public static bool[] items_flags;
	public static string lastItem;

	//Name of the player of the last point
	public static string lastPoint;

	//Scene vlaues to make a switch easy
	public static int scene_index;
	public static int last_scene;

	//Bool for triggering some text
	public static bool newHighscore;
	public static bool newLowscore;

	//Some debugging output
	public static bool showFPS;

	//Activate the GODMODE if you can :P
	public static bool godMode;
}