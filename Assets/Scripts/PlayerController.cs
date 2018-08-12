using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Inputs))]
public class PlayerController : MonoBehaviour {

	[SerializeField]
	private float speed = 3;
	[SerializeField]
	private float energy = 100;
	private float startingEnergy;
	private Inputs input;
	private GameManager gm;


	// sprites[0] = Looking down, sprites[1] = Looking up, sprites[2] = Looking side
	[SerializeField]
	private Sprite[] sprites = new Sprite[3];
	private GameObject graphics;
	public Animator gAnimator { get; private set; }
	private SpriteRenderer sr;

	// Gameplay stuff


	// Use this for initialization
	void Start () {
		startingEnergy = energy;
		input = GetComponent<Inputs> ();
		gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

		foreach (Transform t in gameObject.GetComponentInChildren<Transform>()) {
			if (t.name.Equals ("Graphics")) {
				graphics = t.gameObject;
				break;
			}
		}
		if (graphics == null)
			Debug.LogError ("No graphics object");
		gAnimator = graphics.GetComponent<Animator> ();
		sr = graphics.GetComponent<SpriteRenderer> ();
	}
	

	void LateUpdate() {
		Anim();
	}

	void FixedUpdate () {
		// Movement
		transform.Translate (input.GetMovement().normalized * Time.fixedDeltaTime * speed);

		// Interact
		if(input.GetInteract()) {
			//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			int layer = 1 << 9;
			RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero, 10f, layer);
			foreach(RaycastHit2D hit in hits) {
				Interactable interactable = hit.transform.gameObject.GetComponent<Interactable>();
				if (interactable != null) {
					if(energy >= interactable.InteractCost()) {
						interactable.Interact();
						energy -= interactable.InteractCost();
						gm.SetEneryBarPercentage(energy / startingEnergy);
					}
					break;
				}
			}
		}
	}

	void Anim() {
		// Animator
		if (input.GetMovement() != Vector2.zero)
			gAnimator.SetBool ("Walking", true);
		else
			gAnimator.SetBool ("Walking", false);

		if (input.GetMovement() != Vector2.zero) {
			// Change sprite on walking
			if (input.GetMovement().y > 0) {
				sr.sprite = sprites [1];
                if(graphics.transform.localScale.x != 1)
				    SetGraphicsXScale (1);
			} else if (input.GetMovement().y < 0) {
				sr.sprite = sprites [0];
                if (graphics.transform.localScale.x != 1)
                    SetGraphicsXScale(1);
            } else if (input.GetMovement().x != 0) {
				sr.sprite = sprites [2];

				// Change orientation
				if (input.GetMovement().x > 0 && graphics.transform.localScale.x != 1) {
					SetGraphicsXScale (1);
				} else if (input.GetMovement().x < 0 && graphics.transform.localScale.x != -1) {
					SetGraphicsXScale (-1);
				}
			}
		}

		// SortingLayer fix
		sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100f);
	}

	private void SetGraphicsXScale(float x) {
		Debug.Log(x);
		Vector3 newScale = graphics.transform.localScale;
		newScale.x = x;
		graphics.transform.localScale = newScale;
	}
}
	