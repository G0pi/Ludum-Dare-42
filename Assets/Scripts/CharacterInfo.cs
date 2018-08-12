using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo : MonoBehaviour {
	public Gender gender = Gender.Male;
	public string charName;
	public bool adult = true;
	public GameObject mother;
}

public enum Gender {
	Male,
	Female
}