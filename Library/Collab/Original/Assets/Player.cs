using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
//----------------------------------------------------------------------------------------------------------------------#VARIABLES
	// Def class variables for the player
	private string p1;									// Der Name des ersten GameObjects (NICHT DER DES SPIELERS)
	private string p2;									// Der Name des zweiten GameObjects (NICHT DER DES SPIELERS)
	private Vector3 upP1;								// Der Vektor für Player1 nach oben
	private Vector3 upP2;								// Der Vektor für Player2 nach oben
	private float veloDiff;								// Die Differenz der Geschwindigkeit
	private Vector3 forwardP1;							// Der Vektor für Player1 zur Seite
	private Vector3 forwardP2;							// Der Vektor für Player2 zur Seite
	private bool isPanelSlowed;							// Der Status ob das Panel verlangsamt ist
	private float timerSlowdown;						// Der Zeitgeber für das Verlangsamen
	private bool isPanelInverted;						// Der Status ob die Steuerung invertiert ist
	private bool isPanelShrinked;						// Der Status ob das Panel verkleinert ist
	private float timerPanelInvert;						// Der Zeitgeber für das Invertieren
	private float timerPanelShrink;						// Der Zeitgeber für das Schrumpfen
	private Vector3 defaultPanelSize;					// Die Standardgröße der Pannels
	private float currentVelocityPanel1;				// Die Geschwindigkeit des ersten Pannels
	private float currentVelocityPanel2;				// Die Geschwindigkeit des zweiten Pannels

	// Def class variables for the player
	public Material ownColor;							// Das Material, dass der Ball bei Berührung annehmen soll
	public Material hoverColor;							// Das Material, dass das Pannel bei richtiger Position annehmen soll
	public Material touchColor;							// Das Material, dass das Pannel bei Berührung annehmen soll
	public Material defaultColor;						// Das Material, dass das Pannel standardmäßig annehmen soll

	private GameObject Player1;
	private GameObject Player2;
	private GameObject P1_1;
	private GameObject P1_2;
	private GameObject P1_3;
	private GameObject P1_4;
	private GameObject P1_5;
	private GameObject P2_1;
	private GameObject P2_2;
	private GameObject P2_3;
	private GameObject P2_4;
	private GameObject P2_5;

	// Def & init constants of class player
	//private const float defaultVelocityPanel = 15f;	// Die Standardgeschwindigkeit der Pannels
	private const float defaultVelocityPanel = 0.75f;	// Die Standardgeschwindigkeit der Pannels
	private const float boundaryMovementPanelY = 1.95f;
	private const float boundaryMovementPanelZ = 2.95f;

//----------------------------------------------------------------------------------------------------------------------#START
	void Start() {
		// Init the class variables
		veloDiff = 0.0f;
		p1 = "Player_1";
		p2 = "Player_2";
		isPanelSlowed = false;
		isPanelInverted = false;
		isPanelShrinked = false;
		upP1 = new Vector3(0f, 1f, 0f);
		upP2 = new Vector3(0f, 1f, 0f);
		forwardP1 = new Vector3(0f, 0f, 1.778f);
		forwardP2 = new Vector3(0f, 0f, 1.778f);
		currentVelocityPanel1 = defaultVelocityPanel;
		currentVelocityPanel2 = defaultVelocityPanel;
		timerSlowdown = ItemHandler.eventSpecifications[(int)ItemHandler.EventSpecifications.durationPanelSlowdown];
		timerPanelInvert = ItemHandler.eventSpecifications[(int)ItemHandler.EventSpecifications.durationPanelInvert];
		timerPanelShrink = ItemHandler.eventSpecifications[(int)ItemHandler.EventSpecifications.durationPanelShrink];

		Player1 = GameObject.Find ("Player_1");
		Player2 = GameObject.Find ("Player_2");
		P1_1 = GameObject.Find ("Player_1").transform.GetChild(0).gameObject;
		P1_2 = GameObject.Find ("Player_1").transform.GetChild(1).gameObject;
		P1_3 = GameObject.Find ("Player_1").transform.GetChild(2).gameObject;
		P1_4 = GameObject.Find ("Player_1").transform.GetChild(3).gameObject;
		P1_5 = GameObject.Find ("Player_1").transform.GetChild(4).gameObject;
		P2_1 = GameObject.Find ("Player_2").transform.GetChild(0).gameObject;
		P2_2 = GameObject.Find ("Player_2").transform.GetChild(1).gameObject;
		P2_3 = GameObject.Find ("Player_2").transform.GetChild(2).gameObject;
		P2_4 = GameObject.Find ("Player_2").transform.GetChild(3).gameObject;
		P2_5 = GameObject.Find ("Player_2").transform.GetChild(4).gameObject;
	}

//----------------------------------------------------------------------------------------------------------------------#ONCOLLISIONENTER
	void OnCollisionEnter(Collision col) {
		// FIX: the playarea take the material from the panels
		if (col.gameObject.name == "Ball") {
			if (gameObject.name == p1) {
				col.gameObject.GetComponent<MeshRenderer> ().material = ownColor;
				handleIndication (touchColor);
			} else {
				col.gameObject.GetComponent<MeshRenderer> ().material = ownColor;
				handleIndication (touchColor);
			}
		}
	}

//----------------------------------------------------------------------------------------------------------------------#FIXEDUPDATE
    void FixedUpdate()
    {
        if (ItemHandler.eventFlags[(int)ItemHandler.Events.PanelSlowdown])
        {
            if (!isPanelSlowed) // wenn Panel normalschnell
            {
                ItemHandler.userReadableNameCurrentActiveEvent = "C'mon MOOOVE you lazy Donkey!";
				if (Need.ball_direction == (int)Ball.BallDirection.towardsPlayer1) // Spieler 1 wird benachteiligt 
                {
                    veloDiff = currentVelocityPanel1 / ItemHandler.eventSpecifications[(int)ItemHandler.EventSpecifications.panelSlowdownFactor];
                    currentVelocityPanel1 -= veloDiff;
                    Debug.Log("EVENT: Panel1 Slowdown triggered, Velocity: " + currentVelocityPanel1);
                }
				else if(Need.ball_direction == (int)Ball.BallDirection.towardsPlayer2) // Spieler 2 wird benachteiligt
                {
                    veloDiff = currentVelocityPanel2 / ItemHandler.eventSpecifications[(int)ItemHandler.EventSpecifications.panelSlowdownFactor];
                    currentVelocityPanel2 -= veloDiff;
                    Debug.Log("EVENT: Panel1 Slowdown triggered, Velocity: " + currentVelocityPanel2);
                }
                isPanelSlowed = true;
                //ItemHandler.eventFlags[(int)ItemHandler.Events.PanelSlowdown] = false;
            }

            timerSlowdown -= Time.deltaTime;
            if (timerSlowdown <= 0.0f)
			{
				Need.isItem = false;
                currentVelocityPanel1 = defaultVelocityPanel;
                currentVelocityPanel2 = defaultVelocityPanel;
                timerSlowdown = ItemHandler.eventSpecifications[(int)ItemHandler.EventSpecifications.durationPanelSlowdown];
                isPanelSlowed = false;
                ItemHandler.eventFlags[(int)ItemHandler.Events.PanelSlowdown] = false;
                ItemHandler.userReadableNameCurrentActiveEvent = "";
                Debug.Log("EVENT: Panel wieder normal");
            }
        }

        if (ItemHandler.eventFlags[(int)ItemHandler.Events.PanelInversion]) 
        {
            if (!isPanelInverted)
            {
                ItemHandler.userReadableNameCurrentActiveEvent = "HELP! My Panel is broken!";
                // steuerung invertieren
				if (Need.ball_direction == (int)Ball.BallDirection.towardsPlayer1) // Spieler 1 wird benachteiligt 
                {
                    Debug.Log("EVENT: Panel1 invertiert");
                    forwardP1.z = -1f;
                    upP1.y = -1f;
                }
				else if (Need.ball_direction == (int)Ball.BallDirection.towardsPlayer2) // Spieler 2 wird benachteiligt
                {
                    Debug.Log("EVENT: Panel2 invertiert");
                    forwardP2.z = -1f;
                    upP2.y = -1f;
                }
                isPanelInverted = true;
            }

            timerPanelInvert -= Time.deltaTime;
            if (timerPanelInvert <= 0.0f)
			{
				Need.isItem = false;
                // steuerung wieder normal setzen
                forwardP1.z = 1f;
                upP1.y = 1f;
                forwardP2.z = 1f;
                upP2.y = 1f;
                ItemHandler.userReadableNameCurrentActiveEvent = "";
                Debug.Log("EVENT: Panel wieder normal");
                timerPanelInvert = ItemHandler.eventSpecifications[(int)ItemHandler.EventSpecifications.durationPanelInvert];
                isPanelInverted = false;
                ItemHandler.eventFlags[(int)ItemHandler.Events.PanelInversion] = false; 
            }
        }

        // #### Shrink
        // PANELSHRINKING ÜBERARBEOTEN

        if (ItemHandler.eventFlags[(int)ItemHandler.Events.PanelShrinking])
        {
            if (!isPanelShrinked)
            {
                ItemHandler.userReadableNameCurrentActiveEvent = "ALARM! ALARM!";
                defaultPanelSize = transform.localScale;
                transform.localScale = new Vector3(
                    transform.localScale.x,
                    transform.localScale.y / ItemHandler.eventSpecifications[(int)ItemHandler.EventSpecifications.panelShrinkingFactor],
                    transform.localScale.z / ItemHandler.eventSpecifications[(int)ItemHandler.EventSpecifications.panelShrinkingFactor]);
                Debug.Log("EVENT:" + this.name + " shrinked");
                isPanelShrinked = true;
            }

            timerPanelShrink -= Time.deltaTime;
            if (timerPanelShrink <= 0.0f)
			{
				Need.isItem = false;
                Debug.Log("EVENT: Panels wieder normal");
                ItemHandler.userReadableNameCurrentActiveEvent = "";
                //gameObject.GetComponent<Player>().transform.localScale = defaultPanelSize;
                transform.localScale = defaultPanelSize;
				foreach (GameObject ele in GameObject.FindGameObjectsWithTag ("Panel")) // schwarz, p2
					ele.transform.localScale = defaultPanelSize;
                timerPanelInvert = ItemHandler.eventSpecifications[(int)ItemHandler.EventSpecifications.durationPanelShrink];
                isPanelShrinked = false;
                ItemHandler.eventFlags[(int)ItemHandler.Events.PanelShrinking] = false;

            }
        }
        
        // Steuerung unter Berücksichtugung von Geschwindigkeit und Invertierung
        if (gameObject.name == p1)
        {
            if (Input.GetButton("Y_" + p1))
            {
				// oben/unten
            	transform.localPosition += Input.GetAxisRaw("Y_" + p1) * upP1 * currentVelocityPanel1 * Time.deltaTime;
				if (transform.localPosition.y >= boundaryMovementPanelY)
					transform.localPosition = new Vector3 (transform.localPosition.x, boundaryMovementPanelY, transform.localPosition.z);
				else if(transform.localPosition.y <= -boundaryMovementPanelY)
					transform.localPosition = new Vector3 (transform.localPosition.x, -boundaryMovementPanelY, transform.localPosition.z);
            }
			if (Input.GetButton ("Z_" + p1))
            {
				// rechts/links
				transform.localPosition += Input.GetAxisRaw("Z_" + p1) * forwardP1 * currentVelocityPanel1 * Time.deltaTime;
				if (transform.localPosition.z >= boundaryMovementPanelZ)
					transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, boundaryMovementPanelZ);
				else if(transform.localPosition.z <= -boundaryMovementPanelZ)
					transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, -boundaryMovementPanelZ);
            }
        }
		else {
			if (!Need.isSinglePlayer) {
	            if (Input.GetButton("Y_" + p2))
	            {
                	transform.localPosition += Input.GetAxisRaw("Y_" + p2) * upP2 * currentVelocityPanel2 * Time.deltaTime;
					if (transform.localPosition.y >= boundaryMovementPanelY)
						transform.localPosition = new Vector3 (transform.localPosition.x, boundaryMovementPanelY, transform.localPosition.z);
					else if(transform.localPosition.y <= -boundaryMovementPanelY)
						transform.localPosition = new Vector3 (transform.localPosition.x, -boundaryMovementPanelY, transform.localPosition.z);
	            }
	            //Attention -> - because the view of the secons player is inverse...
	            if (Input.GetButton("Z_" + p2))
	            {
                	transform.localPosition -= Input.GetAxisRaw("Z_" + p2) * forwardP2 * currentVelocityPanel2 * Time.deltaTime;
					if (transform.localPosition.z >= boundaryMovementPanelZ)
						transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, boundaryMovementPanelZ);
					else if(transform.localPosition.z <= -boundaryMovementPanelZ)
						transform.localPosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, -boundaryMovementPanelZ);
	            }
			}
			else {
				//				if (Need.ball_direction == 1) {
				//					//+/-10 is some buffer of the player hight
				//					if (transform.localPosition.y > Need.ball_position_y +5f && !down) {
				//						down = true;
				//						oldPos = transform.localPosition.y;
				//						newPos = transform.localPosition.y -12.5f;
				//					}
				//					if (down) {
				//						transform.localPosition -= forwardP1 * defaultVelocity * Time.deltaTime;
				//						if (transform.localPosition.y <= newPos || transform.localPosition.y == -37.5f) {
				//							down = false;
				//						}
				//					}
				//					if (transform.localPosition.y < Need.ball_position_y -5f && !up) {
				//						up = true;
				//						oldPos = transform.localPosition.y;
				//						newPos = transform.localPosition.y +12.5f;
				//					}
				//					if (up) {
				//						transform.localPosition += forwardP1 * defaultVelocity * Time.deltaTime;
				//						if (transform.localPosition.y >= newPos || transform.localPosition.y == +37.5f) {
				//							up = false;
				//						}
				//					}
				//				}

				// Direct following could be not smooth -> you can't win :P
				// Two ifs, because the one axis can negatifly influce the other and so we set only in an 3D system one
				// variable and two fixed values (one allways fixed and one adaptive fixed value)
				//if(Need.ball_position_y < 37.5f && Need.ball_position_y > -37.5f) {
				//	transform.localPosition = new Vector3(-49.5f, Need.ball_position_y, transform.localPosition.z);
				//}
				//if(Need.ball_position_z < 66.667f && Need.ball_position_z > -66.667f) {
				//	transform.localPosition = new Vector3(-49.5f, transform.localPosition.y, Need.ball_position_z);
				//}
				//if(Need.ball_position_y < 37.5f && Need.ball_position_y > -37.5f) {

				// KI-Steuerung

				if(Need.ball_position_y < 2f && Need.ball_position_y > -2f) {
					transform.localPosition = new Vector3(transform.localPosition.x, Need.ball_position_y, transform.localPosition.z);
				}
				//if(Need.ball_position_z < 66.667f && Need.ball_position_z > -66.667f) {
				if(Need.ball_position_z < 3f && Need.ball_position_z > -3f) {
					transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, Need.ball_position_z);
				}
			}
		}
		//#WIESO Need.isGame?!
		if (!Need.newRound) {
			handleIndication ();
		}
		//Debug.Log ("PLAYER: isOnPannel: "+Need.isOnPannel);
    }

	private void handleIndication() {
		if (Need.ball_direction_x) {
			//CAUTION KEEP DISTANCE FROM THIS HUGE MURDER IF STATEMENTS XD
			//Habe im Moment keine schönere Lösung...
			if ((Player1.transform.localPosition.y + 0.625f) >= Need.ball_position_y && (Player1.transform.localPosition.y - 0.625f) <= Need.ball_position_y && (Player1.transform.localPosition.z + 1.111f) >= Need.ball_position_z && (Player1.transform.localPosition.z - 1.111f) <= Need.ball_position_z) {
				P1_5.GetComponent<MeshRenderer> ().material = hoverColor;
			} else {
				P1_5.GetComponent<MeshRenderer> ().material = defaultColor;
			}

			if ((Player1.transform.localPosition.y + 1.25f) >= Need.ball_position_y && Player1.transform.localPosition.y <= Need.ball_position_y && (Player1.transform.localPosition.z + 2.222f) >= Need.ball_position_z && Player1.transform.localPosition.z <= Need.ball_position_z && !((Player1.transform.localPosition.y + 0.625f) >= Need.ball_position_y && (Player1.transform.localPosition.y - 0.625f) <= Need.ball_position_y && (Player1.transform.localPosition.z + 1.111f) >= Need.ball_position_z && (Player1.transform.localPosition.z - 1.111f) <= Need.ball_position_z)) {
				P1_1.transform.GetChild (0).GetComponent<MeshRenderer> ().material = hoverColor;
				P1_1.transform.GetChild (1).GetComponent<MeshRenderer> ().material = hoverColor;
				P1_1.transform.GetChild (2).GetComponent<MeshRenderer> ().material = hoverColor;
			} else {
				P1_1.transform.GetChild (0).GetComponent<MeshRenderer> ().material = defaultColor;
				P1_1.transform.GetChild (1).GetComponent<MeshRenderer> ().material = defaultColor;
				P1_1.transform.GetChild (2).GetComponent<MeshRenderer> ().material = defaultColor;
			}
			if ((Player1.transform.localPosition.y + 1.25f) >= Need.ball_position_y && Player1.transform.localPosition.y <= Need.ball_position_y && Player1.transform.localPosition.z >= Need.ball_position_z && (Player1.transform.localPosition.z - 2.222f) <= Need.ball_position_z && !((Player1.transform.localPosition.y + 0.625f) >= Need.ball_position_y && (Player1.transform.localPosition.y - 0.625f) <= Need.ball_position_y && (Player1.transform.localPosition.z + 1.111f) >= Need.ball_position_z && (Player1.transform.localPosition.z - 1.111f) <= Need.ball_position_z)) {
				P1_2.transform.GetChild (0).GetComponent<MeshRenderer> ().material = hoverColor;
				P1_2.transform.GetChild (1).GetComponent<MeshRenderer> ().material = hoverColor;
				P1_2.transform.GetChild (2).GetComponent<MeshRenderer> ().material = hoverColor;
			} else {
				P1_2.transform.GetChild (0).GetComponent<MeshRenderer> ().material = defaultColor;
				P1_2.transform.GetChild (1).GetComponent<MeshRenderer> ().material = defaultColor;
				P1_2.transform.GetChild (2).GetComponent<MeshRenderer> ().material = defaultColor;
			}
			if (Player1.transform.localPosition.y >= Need.ball_position_y && (Player1.transform.localPosition.y - 1.25f) <= Need.ball_position_y && Player1.transform.localPosition.z >= Need.ball_position_z && (Player1.transform.localPosition.z - 2.222f) <= Need.ball_position_z && !((Player1.transform.localPosition.y + 0.625f) >= Need.ball_position_y && (Player1.transform.localPosition.y - 0.625f) <= Need.ball_position_y && (Player1.transform.localPosition.z + 1.111f) >= Need.ball_position_z && (Player1.transform.localPosition.z - 1.111f) <= Need.ball_position_z)) {
				P1_3.transform.GetChild (0).GetComponent<MeshRenderer> ().material = hoverColor;
				P1_3.transform.GetChild (1).GetComponent<MeshRenderer> ().material = hoverColor;
				P1_3.transform.GetChild (2).GetComponent<MeshRenderer> ().material = hoverColor;
			} else {
				P1_3.transform.GetChild (0).GetComponent<MeshRenderer> ().material = defaultColor;
				P1_3.transform.GetChild (1).GetComponent<MeshRenderer> ().material = defaultColor;
				P1_3.transform.GetChild (2).GetComponent<MeshRenderer> ().material = defaultColor;

			}
			if (Player1.transform.localPosition.y >= Need.ball_position_y && (Player1.transform.localPosition.y - 1.25f) <= Need.ball_position_y && (Player1.transform.localPosition.z + 2.222f) >= Need.ball_position_z && Player1.transform.localPosition.z <= Need.ball_position_z && !((Player1.transform.localPosition.y + 0.625f) >= Need.ball_position_y && (Player1.transform.localPosition.y - 0.625f) <= Need.ball_position_y && (Player1.transform.localPosition.z + 1.111f) >= Need.ball_position_z && (Player1.transform.localPosition.z - 1.111f) <= Need.ball_position_z)) {
				P1_4.transform.GetChild (0).GetComponent<MeshRenderer> ().material = hoverColor;
				P1_4.transform.GetChild (1).GetComponent<MeshRenderer> ().material = hoverColor;
				P1_4.transform.GetChild (2).GetComponent<MeshRenderer> ().material = hoverColor;
			} else {
				P1_4.transform.GetChild (0).GetComponent<MeshRenderer> ().material = defaultColor;
				P1_4.transform.GetChild (1).GetComponent<MeshRenderer> ().material = defaultColor;
				P1_4.transform.GetChild (2).GetComponent<MeshRenderer> ().material = defaultColor;
			}

			if ((Player1.transform.localPosition.y + 1.25f) < Need.ball_position_y && (Player1.transform.localPosition.y - 1.25f) > Need.ball_position_y && (Player1.transform.localPosition.y + 2.222f) > Need.ball_position_y && (Player1.transform.localPosition.y - 2.222f) < Need.ball_position_z) {
				Need.isOnPannel = false;
				P1_1.transform.GetChild (0).GetComponent<MeshRenderer> ().material = defaultColor;
				P1_1.transform.GetChild (1).GetComponent<MeshRenderer> ().material = defaultColor;
				P1_1.transform.GetChild (2).GetComponent<MeshRenderer> ().material = defaultColor;
				P1_2.transform.GetChild (0).GetComponent<MeshRenderer> ().material = defaultColor;
				P1_2.transform.GetChild (1).GetComponent<MeshRenderer> ().material = defaultColor;
				P1_2.transform.GetChild (2).GetComponent<MeshRenderer> ().material = defaultColor;
				P1_3.transform.GetChild (0).GetComponent<MeshRenderer> ().material = defaultColor;
				P1_3.transform.GetChild (1).GetComponent<MeshRenderer> ().material = defaultColor;
				P1_3.transform.GetChild (2).GetComponent<MeshRenderer> ().material = defaultColor;
				P1_4.transform.GetChild (0).GetComponent<MeshRenderer> ().material = defaultColor;
				P1_4.transform.GetChild (1).GetComponent<MeshRenderer> ().material = defaultColor;
				P1_4.transform.GetChild (2).GetComponent<MeshRenderer> ().material = defaultColor;
				P1_5.GetComponent<MeshRenderer> ().material = defaultColor;
			} else {
				Need.isOnPannel = true;
			}

			//May only indicate the active Pannel...
		} else {
			if ((Player2.transform.localPosition.y + 0.625f) >= Need.ball_position_y && (Player2.transform.localPosition.y - 0.625f) <= Need.ball_position_y && (Player2.transform.localPosition.z + 1.111f) >= Need.ball_position_z && (Player2.transform.localPosition.z - 1.111f) <= Need.ball_position_z) {
				P2_5.GetComponent<MeshRenderer> ().material = hoverColor;
			} else {
				P2_5.GetComponent<MeshRenderer> ().material = defaultColor;
			}

			if ((Player2.transform.localPosition.y + 1.25f) >= Need.ball_position_y && Player2.transform.localPosition.y <= Need.ball_position_y && (Player2.transform.localPosition.z + 2.222f) >= Need.ball_position_z && Player2.transform.localPosition.z <= Need.ball_position_z && !((Player2.transform.localPosition.y + 0.625f) >= Need.ball_position_y && (Player2.transform.localPosition.y - 0.625f) <= Need.ball_position_y && (Player2.transform.localPosition.z + 1.111f) >= Need.ball_position_z && (Player2.transform.localPosition.z - 1.111f) <= Need.ball_position_z)) {
				P2_1.transform.GetChild (0).GetComponent<MeshRenderer> ().material = hoverColor;
				P2_1.transform.GetChild (1).GetComponent<MeshRenderer> ().material = hoverColor;
				P2_1.transform.GetChild (2).GetComponent<MeshRenderer> ().material = hoverColor;
			} else {
				P2_1.transform.GetChild (0).GetComponent<MeshRenderer> ().material = defaultColor;
				P2_1.transform.GetChild (1).GetComponent<MeshRenderer> ().material = defaultColor;
				P2_1.transform.GetChild (2).GetComponent<MeshRenderer> ().material = defaultColor;
			}
			if ((Player2.transform.localPosition.y + 1.25f) >= Need.ball_position_y && Player2.transform.localPosition.y <= Need.ball_position_y && Player2.transform.localPosition.z >= Need.ball_position_z && (Player2.transform.localPosition.z - 2.222f) <= Need.ball_position_z && !((Player2.transform.localPosition.y + 0.625f) >= Need.ball_position_y && (Player2.transform.localPosition.y - 0.625f) <= Need.ball_position_y && (Player2.transform.localPosition.z + 1.111f) >= Need.ball_position_z && (Player2.transform.localPosition.z - 1.111f) <= Need.ball_position_z)) {
				P2_2.transform.GetChild (0).GetComponent<MeshRenderer> ().material = hoverColor;
				P2_2.transform.GetChild (1).GetComponent<MeshRenderer> ().material = hoverColor;
				P2_2.transform.GetChild (2).GetComponent<MeshRenderer> ().material = hoverColor;
			} else {
				P2_2.transform.GetChild (0).GetComponent<MeshRenderer> ().material = defaultColor;
				P2_2.transform.GetChild (1).GetComponent<MeshRenderer> ().material = defaultColor;
				P2_2.transform.GetChild (2).GetComponent<MeshRenderer> ().material = defaultColor;
			}
			if (Player2.transform.localPosition.y >= Need.ball_position_y && (Player2.transform.localPosition.y - 1.25f) <= Need.ball_position_y && Player2.transform.localPosition.z >= Need.ball_position_z && (Player2.transform.localPosition.z - 2.222f) <= Need.ball_position_z && !((Player2.transform.localPosition.y + 0.625f) >= Need.ball_position_y && (Player2.transform.localPosition.y - 0.625f) <= Need.ball_position_y && (Player2.transform.localPosition.z + 1.111f) >= Need.ball_position_z && (Player2.transform.localPosition.z - 1.111f) <= Need.ball_position_z)) {
				P2_3.transform.GetChild (0).GetComponent<MeshRenderer> ().material = hoverColor;
				P2_3.transform.GetChild (1).GetComponent<MeshRenderer> ().material = hoverColor;
				P2_3.transform.GetChild (2).GetComponent<MeshRenderer> ().material = hoverColor;
			} else {
				P2_3.transform.GetChild (0).GetComponent<MeshRenderer> ().material = defaultColor;
				P2_3.transform.GetChild (1).GetComponent<MeshRenderer> ().material = defaultColor;
				P2_3.transform.GetChild (2).GetComponent<MeshRenderer> ().material = defaultColor;

			}
			if (Player2.transform.localPosition.y >= Need.ball_position_y && (Player2.transform.localPosition.y - 1.25f) <= Need.ball_position_y && (Player2.transform.localPosition.z + 2.222f) >= Need.ball_position_z && Player2.transform.localPosition.z <= Need.ball_position_z && !((Player2.transform.localPosition.y + 0.625f) >= Need.ball_position_y && (Player2.transform.localPosition.y - 0.625f) <= Need.ball_position_y && (Player2.transform.localPosition.z + 1.111f) >= Need.ball_position_z && (Player2.transform.localPosition.z - 1.111f) <= Need.ball_position_z)) {
				P2_4.transform.GetChild (0).GetComponent<MeshRenderer> ().material = hoverColor;
				P2_4.transform.GetChild (1).GetComponent<MeshRenderer> ().material = hoverColor;
				P2_4.transform.GetChild (2).GetComponent<MeshRenderer> ().material = hoverColor;
			} else {
				P2_4.transform.GetChild (0).GetComponent<MeshRenderer> ().material = defaultColor;
				P2_4.transform.GetChild (1).GetComponent<MeshRenderer> ().material = defaultColor;
				P2_4.transform.GetChild (2).GetComponent<MeshRenderer> ().material = defaultColor;
			}

			if ((Player2.transform.localPosition.y + 1.25f) < Need.ball_position_y && (Player2.transform.localPosition.y - 1.25f) > Need.ball_position_y && (Player2.transform.localPosition.y + 2.222f) > Need.ball_position_y && (Player2.transform.localPosition.y - 2.222f) < Need.ball_position_z) {
				Need.isOnPannel = false;
				P2_1.transform.GetChild (0).GetComponent<MeshRenderer> ().material = defaultColor;
				P2_1.transform.GetChild (1).GetComponent<MeshRenderer> ().material = defaultColor;
				P2_1.transform.GetChild (2).GetComponent<MeshRenderer> ().material = defaultColor;
				P2_2.transform.GetChild (0).GetComponent<MeshRenderer> ().material = defaultColor;
				P2_2.transform.GetChild (1).GetComponent<MeshRenderer> ().material = defaultColor;
				P2_2.transform.GetChild (2).GetComponent<MeshRenderer> ().material = defaultColor;
				P2_3.transform.GetChild (0).GetComponent<MeshRenderer> ().material = defaultColor;
				P2_3.transform.GetChild (1).GetComponent<MeshRenderer> ().material = defaultColor;
				P2_3.transform.GetChild (2).GetComponent<MeshRenderer> ().material = defaultColor;
				P2_4.transform.GetChild (0).GetComponent<MeshRenderer> ().material = defaultColor;
				P2_4.transform.GetChild (1).GetComponent<MeshRenderer> ().material = defaultColor;
				P2_4.transform.GetChild (2).GetComponent<MeshRenderer> ().material = defaultColor;
				P2_5.GetComponent<MeshRenderer> ().material = defaultColor;
			} else {
				Need.isOnPannel = true;
			}

			//May only indicate the active Pannel...
		}
	}
}
