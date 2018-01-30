using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {

	public Transform[] backgrounds;				//This is and array of all the background and foregrounds to be parallaxed
	private float[] parallaxScales;				//The proportion of how the camera move with the background.
	public float smoothing = 1f;				//How smooth the paralax is gonna be

	private Transform cam;						//reference to the main cameras transform
	private Vector3 previousCamPos;				//the position of the camera in the previous frame

	void Awake () {
		// set uo camera the reference
		cam = Camera.main.transform;
	}


	// Use this for initialization
	void Start () {
		//The previous frame had the current frame's camera position
		previousCamPos = cam.position;

		//assigning coresponding paralaxingScales
		parallaxScales = new float[backgrounds.Length];

		for (int i = 0; i < backgrounds.Length; i++) {
			parallaxScales [i] = backgrounds [i].position.z * -1;
		}
	}
	
	// Update is called once per frame
	void Update () {

		//for each background
		for (int i = 0; i < backgrounds.Length; i++) {
			// The Parallax is the opposite of the camera movement because the previous frame multilpied by the scale
			float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

			//set a target x position which is the current position plus the parallax
			float backgroundTargetPosX = backgrounds[i].position.x + parallax;

			// create a target position which is the background's current positions with it's target x position
			Vector3 backgroundTargetPos = new Vector3 (backgroundTargetPosX,  backgrounds[i].position.y, backgrounds[i].position.z);

			// Fade between current position and the target position using
			backgrounds[i].position = Vector3.Lerp (backgrounds[i].position, backgroundTargetPos, smoothing * Time.deltaTime);
		}

		//set the previousCamPos to the camera's position at the end of the frame
		previousCamPos = cam.position;
	}
}
