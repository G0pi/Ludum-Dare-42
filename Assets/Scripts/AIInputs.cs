using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIInputs : MonoBehaviour, Inputs {

	private Vector2 movement;
	[SerializeField]
	public GameObject target;
	[SerializeField]
	private MoveMethods moveMethod = MoveMethods.Target;
	[SerializeField]
	private Vector2 sleepRangeWalking = new Vector2(0, 2);
	[SerializeField]
	private Vector2 sleepRangeIdle = new Vector2(1, 3);
	private float nextTime;
	[SerializeField]
	private float followRange = .5f;

	// Use this for initialization
	void Awake () {
		movement = Vector2.zero;
		nextTime = 0;
        if(!target) {
			target = GameObject.FindGameObjectWithTag("Player");
        }
	}

	void FixedUpdate() {
		switch (moveMethod) {
			case MoveMethods.Target:
				TargetWalk ();
				break;
			case MoveMethods.Random:
				RandomWalk();
				break;
			case MoveMethods.Crazy:
				CrazyWalk();
				break;
		}
	}

	void TargetWalk() {
		if (target == null) {
			moveMethod = MoveMethods.Random;
			return;
		}
		Vector2 move = target.transform.position - transform.position;
		if (move.magnitude < followRange) {
			movement = Vector2.zero;
		} else {
			movement = GetRawVector (move);
		}

	}

	void RandomWalk() {
		// If not on cooldown
		if(nextTime - Time.time < 0) {
			// Coinflip on if the character should move at all.
			Vector2 sleepRange;
			if (Random.Range(0, 100) < 50) {
				movement = Vector2.zero;
				sleepRange = sleepRangeIdle;
			} else {
				movement.x = (int)Random.Range(-1, 2);
				movement.y = (int)Random.Range(-1, 2);
				sleepRange = sleepRangeWalking;
			}
			
			nextTime = Time.time + Random.Range(sleepRange.x, sleepRange.y);
		}
	}


	void CrazyWalk() {
		// Coinflip on if the character should stand still or move
		if (Random.Range (0, 100) < 50) {
			return;
		} else {
			movement.x = Random.Range (-1, 2);
			movement.y = Random.Range (-1, 2);
		}
	}

	private Vector2 GetRawVector(Vector2 v) {
		Vector2 res = Vector2.zero;
		if (v.x > .5f)
			res.x = 1;
		else if (v.x < -.5f)
			res.x = -1;
		
		if (v.y > .5f)
			res.y = 1;
		else if (v.y < -.5f)
			res.y = -1;

		return res;
	}

	public Vector2 GetMovement() {
		return movement;
	}

	public void SetMoveMethod(MoveMethods moveMethod) {
		movement = Vector2.zero;
		this.moveMethod = moveMethod;
	}

	public bool GetInteract() {
		return false;
	}

	public bool GetAttack() {
		return false;
	}
}

public enum MoveMethods {
	Idle,
	Target,
	Attack,
	Task,
	Random,
	Crazy,
}