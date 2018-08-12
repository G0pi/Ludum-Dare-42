using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterInfo), typeof(PlayerController))]
public class BabyAI : MonoBehaviour {

	[SerializeField]
	private float growUpTime = 5f;
	[SerializeField]
	private float growAnimationTime = 4f;
	[SerializeField]
	private GameObject smokeScreen;
	[SerializeField]
	private GameObject[] adults = new GameObject[2];

	private CharacterInfo ci;
	private PlayerController pc;
	private AIInputs ai;

	// Use this for initialization
	void Start () {
		ci = GetComponent<CharacterInfo> ();
		pc = GetComponent<PlayerController> ();
		ai = GetComponent<AIInputs>();
		ai.target = ci.mother;
		ai.SetMoveMethod(MoveMethods.Target);

		// Choose gender
		ci.gender = Gender.Male;
		if (Random.Range(0, 100) < 50)
			ci.gender = Gender.Female;
		
		StartCoroutine (Growing ());
	}
	
	IEnumerator Growing() {
		
		yield return new WaitForSeconds (growUpTime);
		GetComponentInChildren<Animator>().SetTrigger("Growing");
		ai.SetMoveMethod(MoveMethods.Idle);
		yield return new WaitForSeconds (growAnimationTime);
		GrowUp ();
	}

	void GrowUp() {
		GameManager gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
		GameObject adultPrefab = adults[0];
		if (ci.gender == Gender.Female)
			adultPrefab = adults [1];
		Instantiate (smokeScreen, transform.position, Quaternion.identity);
		gm.AddToPop(gameObject.tag, Instantiate(adultPrefab, transform.position, Quaternion.identity) as GameObject);
		gm.RemovePop(gameObject);
		Destroy(gameObject);
	}
}
