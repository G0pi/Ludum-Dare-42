using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIInputs))]
public class FemaleAI : MonoBehaviour, Interactable {

	[SerializeField]
	private GameObject icon;
	[SerializeField]
	private GameObject babyPrefab;
	[SerializeField]
	private GameObject smokeScreen;
	[SerializeField]
	private int interactCost;

	private AIInputs aii;

	// Use this for initialization
	void Start () {
		aii = GetComponent<AIInputs> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Birth() {
		Instantiate (smokeScreen, transform.position, Quaternion.identity);
		GameObject baby = Instantiate (babyPrefab, transform.position, Quaternion.identity) as GameObject;
		baby.GetComponent<CharacterInfo> ().mother = gameObject;
		GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>().Birth(gameObject, baby);
	}

	void OnMouseOver() {
		icon.SetActive(true);
	}

	void OnMouseExit() {
		icon.SetActive(false);	
	}

	public void Interact() {
		Birth();
	}

	public float InteractCost() {
		return interactCost;
	}
}
