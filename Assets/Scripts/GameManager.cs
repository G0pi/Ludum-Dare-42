using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	[SerializeField]
	private int maxPop = 20;
	[SerializeField]
	private int penaltyPop = 5;
	private GameObject player;
	private List<GameObject> friendlyPop;
	private List<GameObject> enemyPop;
	[SerializeField]
	private Text popText;
	[SerializeField]
	private RectTransform energyBar;

	// Use this for initialization
	void Start () {
		// Count population at start
		player = GameObject.FindGameObjectWithTag("Player");
		friendlyPop = new List<GameObject> (GameObject.FindGameObjectsWithTag("FriendlyAI"));
		enemyPop = new List<GameObject> (GameObject.FindGameObjectsWithTag("EnemyAI"));
		UpdatePopText();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void LightningStrike(GameObject go) {

	}

	public void Kill(GameObject go) {
		RemovePop(go);
		Destroy(go);
	}

	public void Kill(string tag, int amount = 1) {
		List<GameObject> pop = GetPopulation(tag);
		for(int i = 0; i < amount; i++) {
			GameObject go = RemoveRandomPop(tag);
			Destroy(go);
		}
	}

	public void Birth(GameObject mother, GameObject child) {
		AddToPop(mother.tag, child);
		if(GetTotalPopulation() > maxPop) {
			Kill(mother.tag, penaltyPop);
		} 
	}

	void UpdatePopText() {
		popText.text = GetTotalPopulation() + "/" + maxPop;
	}

	public void AddToPop(string tag, GameObject child) {
		GetPopulation(tag).Add(child);
		UpdatePopText();
	}

	public GameObject RemoveRandomPop(string tag) {
		List<GameObject> pop = GetPopulation(tag);
		int index = Mathf.RoundToInt(Random.Range(0, pop.Count));
		GameObject res = pop [index];
		pop.RemoveAt(index);
		UpdatePopText();
		return res;
	}

	public GameObject RemovePop(GameObject rem) {
		GetPopulation(rem.tag).Remove(rem);
		UpdatePopText();
		return rem;
		//Kill(rem);
	}

	public bool CanBirth() {
		return GetTotalPopulation() < maxPop;
	}

	public int GetFriendlyPop() {
		return 1 + friendlyPop.Count;
	}

	public int GetEnemyPop() {
		return enemyPop.Count;
	}

	public int GetTotalPopulation() {
		return GetFriendlyPop() + GetEnemyPop();
	}

	public void SetEneryBarPercentage(float percentage) {
		Vector2 edgeVals = new Vector2(98, 2);
		float right = edgeVals.y + edgeVals.x * (1 - percentage);
		Vector2 temp = energyBar.offsetMax;
		temp.x = -right;
		energyBar.offsetMax = temp;
	}

	public List<GameObject> GetPopulation(string tag) {
		List<GameObject> pop;
		if (tag.Equals("FriendlyAI")) {
			pop = friendlyPop;
		} else {
			pop = enemyPop;
		}

		return pop;
	}
}
