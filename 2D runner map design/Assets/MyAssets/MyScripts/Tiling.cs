using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour {

	public int offsetX = 2; //An offset to give the sprites spawning doesn't give any bad clipping errors

	//These are used for checking if we need to instantiate objects
	public bool hasARightBuddy = false;
	public bool hasALeftBuddy = false;

	public bool reverseScale = false; // used if obj is not tilable

	private float spriteWidth = 0f; // the width of the element
	private Camera cam;
	private Transform myTransform;

	void Awake () {
		cam = Camera.main;
		myTransform = transform;
	}

	// Use this for initialization
	void Start () {
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer> ();
		spriteWidth = sRenderer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () {
		//Does it still need buddies? If it doesn't, do nothing
		if (hasALeftBuddy == false || hasARightBuddy == false) {
			//Calculate the cameras extend, half the width, of what the camera is seeing within the world coordinates
			float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

			//calculate the x position where the camera can see the edge of the sprite (element0
			float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth/2) - camHorizontalExtend;
			float edgeVisiblePositionLeft = (myTransform.position.x + spriteWidth/2) + camHorizontalExtend;

			// Checking if we can see the edge og the element and then calls "MakeNewBuddy" if it can
			if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBuddy == false) 
			{
				MakeNewBuddy (1);
				hasARightBuddy = true;
			} 
			else if (cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftBuddy == false) 
			{
				MakeNewBuddy (-1);
				hasALeftBuddy = true;
			}

		}
	
	}
	void MakeNewBuddy (int rightOrLeft) {
		//calculating the new position for the new buddy (next sprite)
		Vector3 newPosition = new Vector3 (myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
		//Instantating our new body and storing him in a variable
		Transform newBuddy = Instantiate (myTransform, newPosition, myTransform.rotation) as Transform;

		//if not tilable, it reverses the x size of the object to get rid of mismatched seams.
		if (reverseScale == true) {
			newBuddy.localScale = new Vector3 (newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);
		}

		newBuddy.parent = myTransform;
		if (rightOrLeft > 0) {
			newBuddy.GetComponent<Tiling> ().hasALeftBuddy = true;
		} else {
			newBuddy.GetComponent<Tiling> ().hasARightBuddy = true;
		}
	}
}