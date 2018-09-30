using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_Retro : MonoBehaviour
{
	// Class varibales
	private Rigidbody rb;
	private Vector3 speedDiff;
	private Vector3 randomStart;
	private Vector3 velocitySpeededBall;

	private const byte maxSpeed = 50;
	private const byte veloDiffRegulator = 3;
	private const byte slightlyIncreaseFactor = 8;
	public enum BallDirection { towardsPlayer1, towardsPlayer2 };

	void Start()
	{
		rb = GetComponent<Rigidbody>();
		startNewGame();
	}

	public void startNewGame()
	{
		rb.AddForce(20f, 0, 0, ForceMode.Impulse);
	}

	void FixedUpdate() {
		//Need.ball_position_x = this.transform.localPosition.x;
		//Need.ball_position_y = this.transform.localPosition.y;
	}

//	Vector3 generateBallDirection()
//	{
//		sbyte newBallDirectionBoundary = 20;
//		return new Vector3(Random.Range(-newBallDirectionBoundary, newBallDirectionBoundary),
//			Random.Range(-newBallDirectionBoundary, newBallDirectionBoundary),
//			Random.Range(-newBallDirectionBoundary, newBallDirectionBoundary));
//
//	}
//
//	Vector3 generateRandomPositionInMidOfField()
//	{
//		sbyte innerCubeBoundary = 20;
//		return new Vector3( Random.Range(-innerCubeBoundary, innerCubeBoundary),
//			Random.Range(-innerCubeBoundary, innerCubeBoundary),
//			Random.Range(-innerCubeBoundary, innerCubeBoundary));
//	}
//
//	void increaseVelocitySlightlyContinous()
//	{
//		if (rb.velocity.x < maxSpeed)
//		{
//			if (Need.ball_direction == (int)BallDirection.towardsPlayer1)
//				rb.velocity += new Vector3(((Time.deltaTime / slightlyIncreaseFactor) * rb.velocity.x) / veloDiffRegulator, 0, 0);
//			else if (Need.ball_direction == (int)BallDirection.towardsPlayer2)
//				rb.velocity -= new Vector3(-((Time.deltaTime / slightlyIncreaseFactor) * rb.velocity.x) / veloDiffRegulator, 0, 0);
//		}
//	}
//
//	int identifyBallDirection()
//	{
//		return ((rb.velocity.x > 0) ? 0 : 1);
//	}
}