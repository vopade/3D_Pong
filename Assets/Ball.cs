using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
//----------------------------------------------------------------------------------------------------------------------#VARIABLES
	// Def class variables for the ball
	private Rigidbody rb;							// Der Rigidbody vom Ball
	private bool firstRound;						// Der erste Sound der Runde
	private Vector3 speedDiff;						// Die Differenz des Speeds
	private float timerSpeedup;						// Die Zeit für den speedup
	private Vector3 randomStart;					// Der Zufällige Startvektor
	private float timeoutInvert;					// Die Zeitsperre invertiert
	private float timerInvisible;					// Die Zeit für Unsichtbarkeit
    private bool ballIsSpeededup;					// Die Aussage ob der Ball schneller ist
    private bool ballIsInvisible;					// Die Aussage ob der Ball sichtbar ist
    private bool invertIsTimeOut;					// Die Aussage ob die Invertierung vorbei ist
	private byte oldBallDirection;					// Die alte Richtung des Balls
	private Vector3 velocitySpeededBall;			// Die Geschwindigkeit des Balls
	private float timerNotificationDurationBall;	// ???
	//private Vector3 ea; // Für die Rotation

	// Def class variables for the ball
	public static bool firstSound;					// Schalter ob es der erste Sound ist
	public enum BallDirection {						// In Richtung Spieler 1 (lila): 0, in Richtung Spieler 2 (türkis): 1
		towardsPlayer1,
		towardsPlayer2
	};

	// Def & init constants of class ball
	private const byte maxSpeed = 5;					// Die maximale Geschwindigkeit
	private const float minSpeed = 1.9f;
	private const byte veloDiffRegulator = 3;			// Die Differenz der Ballgeschwindigkeit
	private const byte slightlyIncreaseFactor = 10;		// Die Schrittweise Erhöhung der Geschwindigkeit. Je größer, desto langsamer die Beschleunigung
	private const float notificationDurationBall = 5f;	// ???

//----------------------------------------------------------------------------------------------------------------------#START
    void Start()
	{
		// Init the class variables
		firstSound = true;
		firstRound = true;
		ballIsSpeededup = false;
		ballIsInvisible = false;
		invertIsTimeOut = false;
		rb = GetComponent<Rigidbody>();
		timerNotificationDurationBall = notificationDurationBall;
		timeoutInvert = ItemHandler.eventSpecifications[(int)ItemHandler.EventSpecifications.timeoutInvert];
		timerSpeedup = ItemHandler.eventSpecifications[(int)ItemHandler.EventSpecifications.durationSpeedup];
		timerInvisible = ItemHandler.eventSpecifications[(int)ItemHandler.EventSpecifications.durationDisappearance];

		// NEVER USE X - Z!!!
		//ea = new Vector3 (0, 100, 0);//transform.eulerAngles;
    }
		
//----------------------------------------------------------------------------------------------------------------------#FIXEDUPDATE
    void FixedUpdate()
    {
		//Debug.Log ("BALL: rb.velocity.x: "+rb.velocity.x);
		//Debug.Log ("BALL: maxSpeed: "+maxSpeed);
		//Debug.Log ("BALL: rb.velocity.x<maxSpeed: "+(rb.velocity.x<maxSpeed));

		handleNeeded ();
		// Rotation (2)
		//Quaternion deltaRotation = Quaternion.Euler(ea * Time.deltaTime);
		//rb.MoveRotation (rb.rotation * deltaRotation);

        //Debug.Log("BALL: Speed" + rb.velocity);
        /* Lösung per Semaphore
         * 
         * speedupEventIsTriggered wird in ItemHandler.cs 
         * ballIsSpeededup verhindert die gleichzeitige Ausführung des gleichen Events
         * In speedDiff wird der Wert gespeichert, um den die Geschwindigkeit des Balls erhöht wird. 
         * Falls sich die Richtung des Balls in der Zeit der Ausführung des Events geändert hat ( --> min. 1 Wert im
         * Vektor geändert), so muss der Wert der entsprechenden Achse invertiert werden, damit der Ball nicht
         * dauerhaft um den doppelten Wert beschleunigt wird (Minus und Minus ergibt Plus ;) )
         * Es gibt Ungenaugkeiten bei Richtungsänderungen (< 5%), vermutlich wg. float oder Unity-Engine
         * Da mehrere Kollisionen pro sichtbarer Kollision erkannt werden (vermutlich hat Body der Kiste
         * mehrere Collider) muss bei manchen Events eine SPerre mit eingebaut werden, damit es nicht 
         * mehrmals ausgeführt wird
         */
        if (ItemHandler.eventFlags[(int)ItemHandler.Events.BallSpeedUp])
        {
            if (!ballIsSpeededup) // Einmalig bei Auslösung, wenn Ball nicht beschleunigt
            {
                ItemHandler.userReadableNameCurrentActiveEvent = "WOW! That is faaaaaaaast!";
                Debug.Log("EVENT: Speedup getriggered: Velocity: " + rb.velocity);
                speedDiff = rb.velocity;
                rb.velocity += speedDiff;
                velocitySpeededBall = rb.velocity;
                ballIsSpeededup = true;
                Debug.Log("EVENT: Speedup ausgeführt: Velocity: " + rb.velocity);
            }

            timerSpeedup -= Time.deltaTime;
            if (timerSpeedup <= 0.0f) // wenn Eventzeit abgelaufen, dann ...
            {
				Need.isItem = false;
                // Invertierung des Balls
                if (rb.velocity.x != velocitySpeededBall.x)
                    speedDiff.x = -speedDiff.x;
                if (rb.velocity.y != velocitySpeededBall.y)
                    speedDiff.y = -speedDiff.y;
                if (rb.velocity.z != velocitySpeededBall.z)
                    speedDiff.z = -speedDiff.z;

                //rb.velocity -= speedDiff;
                ItemHandler.eventFlags[(int)ItemHandler.Events.BallSpeedUp] = false;
                ballIsSpeededup = false;
                timerSpeedup = ItemHandler.eventSpecifications[(int)ItemHandler.EventSpecifications.durationSpeedup];
                ItemHandler.userReadableNameCurrentActiveEvent = "";
                Debug.Log("EVENT: Speedup beendet: Velocity: " + rb.velocity);
            }
        }

        if (ItemHandler.eventFlags[(int)ItemHandler.Events.BallDisappear])
        {
            if (!ballIsInvisible)
            {
                Debug.Log("EVENT: Ball ist unsichtbar");
                ItemHandler.userReadableNameCurrentActiveEvent = "Where did the ball go?";
				this.hideTrail ();
                GetComponent<MeshRenderer>().enabled = false;
                ballIsInvisible = true;
            }

            timerInvisible -= Time.deltaTime;
            if (timerInvisible <= 0.0f)
			{
				Need.isItem = false;
                GetComponent<MeshRenderer>().enabled = true;
                ItemHandler.eventFlags[(int)ItemHandler.Events.BallDisappear] = false;
                ballIsInvisible = false;
                timerInvisible = ItemHandler.eventSpecifications[(int)ItemHandler.EventSpecifications.durationDisappearance];
                ItemHandler.userReadableNameCurrentActiveEvent = "";
                Debug.Log("EVENT: Ball ist wieder sichtbar");
				this.showTrail ();
            }
        }

        if (ItemHandler.eventFlags[(int)ItemHandler.Events.BallDirectionInversion])
        {
            if (!invertIsTimeOut) // Einmalig
            {
                ItemHandler.userReadableNameCurrentActiveEvent = "OMG! Look at the Direction!";
                Debug.Log("EVENT: Invert triggered: Velocity: " + rb.velocity);
                rb.velocity = -rb.velocity;
                Debug.Log("EVENT: Invert beendet: Velocity: " + rb.velocity);
                invertIsTimeOut = true;
            }
            timeoutInvert -= Time.deltaTime;
            if (timeoutInvert <= 0.0f) // falls timeout vorbei
			{
				Need.isItem = false;
                ItemHandler.userReadableNameCurrentActiveEvent = "";
                invertIsTimeOut = false; // Timeout vorbei
                timeoutInvert = ItemHandler.eventSpecifications[(int)ItemHandler.EventSpecifications.timeoutInvert];
                ItemHandler.eventFlags[(int)ItemHandler.Events.BallDirectionInversion] = false;
            }
        }

        if (ItemHandler.eventFlags[(int)ItemHandler.Events.BallRespawn])
        {
			// #Geht zu schnell weg!
			Need.isItem = false;
            ItemHandler.userReadableNameCurrentActiveEvent = "$h!t. Where's the Ball?";
            Debug.Log("EVENT: Respawn is triggered");
			this.hideTrail ();
            transform.position = generateRandomPositionInMidOfField();
            transform.TransformDirection(generateBallDirection());
			this.showTrail ();
            ItemHandler.eventFlags[(int)ItemHandler.Events.BallRespawn] = false;
        }

		// #Was ist das?!
        timerNotificationDurationBall -= Time.deltaTime;
        if (timerNotificationDurationBall <= 0.0f)
        {
            ItemHandler.userReadableNameCurrentActiveEvent = "";
        }
        
        increaseVelocitySlightlyContinous();
        panelHit();

		// Schweif Farbe veränern
		// Keine Ahnung warum das passiert, sieht aber gut aus
		// #Fehlerprävention (gameobject not found null reference...) -> nur wenn keine neue Runde am Initialisieren ist
		if (!Need.newRound) {
			changeTrail ();
		}
	}

//----------------------------------------------------------------------------------------------------------------------#HANDLENEEDED
	void handleNeeded() {
		Need.ball_position_x = this.transform.localPosition.x;
		Need.ball_position_y = this.transform.localPosition.y;
		Need.ball_position_z = this.transform.localPosition.z;
		// Flugrichtung des Balls ermitteln
		Need.ball_direction = (rb.velocity.x > 0) ? 0 : 1;
		identifyBallDirectionAxis ();
	}

//----------------------------------------------------------------------------------------------------------------------#FUNCTIONS_PUBLIC
	/// <summary>
	/// Setze den Ball zurück in den Ursprung und nehme den Startinpuls ebenfalls weg
	/// </summary> 
	public void reset () {
		// Damit der Trail sofort verschwindet
		hideTrail ();
		rb.velocity = new Vector3 (0f, 0f, 0f);
		this.transform.position = new Vector3 (0f, 0f, 0f);
		Debug.Log ("BALL: Setze Ball zurück");
	}

	/// <summary>
	/// Beginne den Ball in eine zufällige Richtung mit einer zufälligen Kraft zu bewegen
	/// </summary> 
	public void startNewGame()
	{
		// Damit der Trail auch wieder kommt
		showTrail ();
		if (!firstRound) // in der ersten Runde überspringen
		{
			rb.velocity = new Vector3 (0f, 0f, 0f);
		}
		//rb.AddForce(-(randomStart * Need.ball_speed), ForceMode.Force); // um Force zu neutralisieren, funktioniert nicht, weil Ball immer schenller wird

		firstRound = false;
		randomStart = generateBallDirection();

		if (randomStart.x < 0) // Geschwindigkeit in x-Richtung erhöhen, bis untere Grenze überschritten, da sonst keine kollision stattfindet
		{
			while(randomStart.x >= -0.62f)
			randomStart.x -= 0.1f;
			//Get the random value for x from the gamecontrol -> value is there needed for the init camera position
			if (Need.goesToPlayer1) {
				randomStart.x *= -1;
			}
		}
		else
		{
			while(randomStart.x <= 0.62f)
			randomStart.x += 0.1f;

			if (!Need.goesToPlayer1) {
				randomStart.x *= -1;
			}
		}
		// Ball muss min. 2.0f schnell sein
		//Need.ball_speed = 1f; // temporär zum testen. Wenn Einstellung über GUI --> diese Zeile löschen
		//randomStart = new Vector3(-0.62f,0f, 0f); // Nur zu Testzwecken, ist Startvektor
		//setVectorDirection(randomStart);
		rb.velocity = (randomStart * Need.ball_speed/2); // dies einkommentieren wenn kein test
		rb.AddForce((randomStart * Need.ball_speed), ForceMode.Impulse);
		Debug.Log("AUDIO: Gamestartsound");
		Audio.audioFlags[(int)Audio.AudioFiles.GameStart] = true;
		Debug.Log("BALL: Initial ball speed is " + Need.ball_speed);
	}

	//----------------------------------------------------------------------------------------------------------------------#FUNCTIONS_PRIVATE
	/// <summary>
	/// Bestimmen, wohin der Ball allgemein fliegt
	/// </summary>
	void identifyBallDirectionAxis()
	{
		Need.ball_direction_x = (rb.velocity.x > 0) ? true : false;
		Need.ball_direction_y = (rb.velocity.y > 0) ? true : false;
		Need.ball_direction_z = (rb.velocity.z > 0) ? true : false;
	}

    /// <summary>
    /// Flugrichtung des Balls wird innerhalb definerter Grenzen (neu) bestimmt
    /// </summary>
    Vector3 generateBallDirection()
    {
        float newBallDirectionBoundary = 0.3f;
        return new Vector3(Random.Range(-newBallDirectionBoundary, newBallDirectionBoundary),
                            Random.Range(-newBallDirectionBoundary, newBallDirectionBoundary),
                            Random.Range(-newBallDirectionBoundary, newBallDirectionBoundary));

    }

    /// <summary>
    /// Position des Balls wird innerhalb definierter Grenzen (neu) bestimmt
    /// </summary>
    Vector3 generateRandomPositionInMidOfField()
    {
        sbyte innerCubeBoundary = 2;
        return new Vector3(Random.Range(-innerCubeBoundary, innerCubeBoundary),
                            Random.Range(-innerCubeBoundary, innerCubeBoundary),
                            Random.Range(-innerCubeBoundary, innerCubeBoundary));
    }

    /// <summary>
    /// Ball wird dauerhaft auf Basis der Dauer des letzten Frames geringfügig beschleunigt.
    /// </summary>
    void increaseVelocitySlightlyContinous()
    {
        /* Framerate: 1/Time.deltaTime
         * Passender Wert zur kontinuerlichen Beschleuinigung: Time.deltaTime/slightlyIncreaseFactor
         * Debug.Log("GAME: FPS: " + 1/Time.deltaTime);
         * Die Beschleunigung wird auf der Grundlage der Dauer des letzten Frames getroffen. Dabei können gerinfügige Abweichungen entstehen. Diese sind irrelevant, 
         * da die kontinuiriche geringe Beschleunigung nur den Zweck erfüllt, dass jede Runde irgendwann ein Ende findet.
         * Flugrichtung des Balls muss beachtet werden
         * 
         * Bsp.:
         * Time.deltaTime = 0.02
         * slightlyIncreaseFactor = 8
         * 1 / (0.02 / 8) = 400 ==> Jedes 400. Frame ein Speedup von 1.0 
         *                      ==> Jedes 40.  Frame ein Speedup von 0.1 
         * veloDiffRegulator verhindert eine zu starke Zunahme des Geschwindigkeit
         */
        //Debug.Log("BALL: Alte Geschw.: " + rb.velocity);
		if (Mathf.Abs (rb.velocity.x) < maxSpeed) {
			if (Need.ball_direction == (int)BallDirection.towardsPlayer1) {
				rb.velocity += new Vector3 (((Time.deltaTime / slightlyIncreaseFactor) * rb.velocity.x) / veloDiffRegulator, 0, 0);
			}
			if (Need.ball_direction == (int)BallDirection.towardsPlayer2) {
				rb.velocity -= new Vector3 (-((Time.deltaTime / slightlyIncreaseFactor) * rb.velocity.x) / veloDiffRegulator, 0, 0);
			}
		} 
		// Untere Grenze der ballgeschwindigkeit, da ansonsten kein Abprallen erkannt wird
		/*if (Mathf.Abs (rb.velocity.x) < minSpeed) 
		{
			if (Need.ball_direction == (int)BallDirection.towardsPlayer1) 
			{
				rb.velocity = new Vector3 (1.9f, rb.velocity.y, rb.velocity.z);
			} else 
			{
				rb.velocity = new Vector3 (-1.9f, rb.velocity.y, rb.velocity.z);
			}
				
		}*/
        //Debug.Log("BALL: Neu Geschw.: " + rb.velocity);
    }

	/// <summary>
	/// Stelle fest, ob das Pannel getroffen wurde
	/// #Anmerkung: sollte evtl. nicht hier stehen!
	/// </summary> 
    void panelHit()
    {
        // Wenn Startvektor -x hat, dann falscher Sound
		if (oldBallDirection != Need.ball_direction)
        {
			if (!firstSound)
			{
				Audio.audioFlags [(int)Audio.AudioFiles.BallBounce] = true;
			}
            firstSound = false;
        }
		oldBallDirection = (byte) Need.ball_direction;
    }

	void showTrail() {
		GameObject trail = GameObject.FindGameObjectWithTag ("Trail");
		trail.GetComponent<TrailRenderer> ().enabled = true;
	}

	void hideTrail() {
		GameObject trail = GameObject.FindGameObjectWithTag ("Trail");
		trail.GetComponent<TrailRenderer> ().enabled = false;
	}

	void changeTrail() {
		GameObject trail = GameObject.FindGameObjectWithTag ("Trail");
		trail.GetComponent<TrailRenderer> ().material.color = Color.blue;
	}
}
