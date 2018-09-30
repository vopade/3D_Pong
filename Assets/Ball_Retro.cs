using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Retro : MonoBehaviour
{
//----------------------------------------------------------------------------------------------------------------------#VARIABLES
	// Def Class varibales of retroball
	private Rigidbody rb;
	private Vector3 speedDiff;
	private Vector3 randomStart;
	private Vector3 velocitySpeededBall;

	// Def Class varibales of retroball
	public enum BallDirection {
		towardsPlayer1,
		towardsPlayer2
	};

	// Def & init constants of class retroball
	private const byte maxSpeed = 50;
	private const byte veloDiffRegulator = 4;
	private const byte slightlyIncreaseFactor = 8;

//----------------------------------------------------------------------------------------------------------------------#START
	void Start()
	{
		// Init class variables
		rb = GetComponent<Rigidbody>();
		//startNewGame();
	}

//----------------------------------------------------------------------------------------------------------------------#FIXEDUPDATE
	void FixedUpdate() {
		handleNeeded ();
		increaseVelocitySlightlyContinous ();
		//Debug.Log ("Ball Position: " + this.transform.position);
		//Debug.Log ("Ball Position: " + this.transform.position.x);
		//Debug.Log ("Ball Position: " + this.transform.position.y);
	}

//----------------------------------------------------------------------------------------------------------------------#HANDLENEEDED
	void handleNeeded() {
		Need.ball_position_x = this.transform.localPosition.x;
		Need.ball_position_y = this.transform.localPosition.y;
	}

//----------------------------------------------------------------------------------------------------------------------#FUNCTIONS_PUBLIC
	public void reset () {
		rb.velocity = new Vector3(0f, 0f, 0f);
		this.transform.position = new Vector3(0f, 0f, 0f);
	}

	public void startNewGame()
	{
		randomStart = generateBallDirection();
		if (randomStart.x < 0) {
			randomStart.x -= 20;
		} else {
			randomStart.x += 20;
		}
		rb.velocity = randomStart * Need.ball_speed / 2f;
	}


//----------------------------------------------------------------------------------------------------------------------#FUNCTIONS_PRIVATE
	Vector3 generateBallDirection()
	{
		sbyte newBallDirectionBoundary = 20;
		return new Vector3(Random.Range(-newBallDirectionBoundary, newBallDirectionBoundary),
			Random.Range(-newBallDirectionBoundary, newBallDirectionBoundary),
			0f);

	}

	Vector3 generateRandomPositionInMidOfField()
	{
		sbyte innerCubeBoundary = 20;
		return new Vector3( Random.Range(-innerCubeBoundary, innerCubeBoundary),
			Random.Range(-innerCubeBoundary, innerCubeBoundary),
			0f);
	}

	void increaseVelocitySlightlyContinous()
	{
		if (Mathf.Abs(rb.velocity.x) < maxSpeed)
		{
			if (Need.ball_direction == (int)BallDirection.towardsPlayer1) {
				rb.velocity += new Vector3 (((Time.deltaTime / slightlyIncreaseFactor) * rb.velocity.x) / veloDiffRegulator, 0, 0);
			}
			if (Need.ball_direction == (int)BallDirection.towardsPlayer2) {
				rb.velocity -= new Vector3 (-((Time.deltaTime / slightlyIncreaseFactor) * rb.velocity.x) / veloDiffRegulator, 0, 0);
			}
		}
	}

	int identifyBallDirection()
	{
		return ((rb.velocity.x > 0) ? 0 : 1);
	}
}
