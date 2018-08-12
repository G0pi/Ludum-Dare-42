using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputs : MonoBehaviour, Inputs {

	private Vector2 movement;

	// Use this for initialization
	void Start () {
		movement = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update () {
		movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
	}

	public Vector2 GetMovement () {
		return movement;
	}

	public bool GetInteract() {
		return Input.GetMouseButtonDown(1);
	}

	public bool GetAttack() {
		return Input.GetMouseButtonDown(0);
	}
		
}
