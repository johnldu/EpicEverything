﻿using UnityEngine;
using System.Collections;

public class ElevatePiece : MonoBehaviour {
	public int damage;
	public GameObject rocks;
	public GameObject flash;
	public GameObject sparks;
	public GameObject impact;
	public GameObject dust;
	public GameObject camera;
	public GameObject highlight;
	private float shake_intensity = 0;
	private float shake_decay = 0.1f;
	private bool straightened = false;

	void Start() {
		rocks.particleSystem.emissionRate = 0;
		flash.particleSystem.emissionRate = 0;
		sparks.particleSystem.emissionRate = 0;
		impact.particleSystem.emissionRate = 0;
		dust.particleSystem.emissionRate = 0;
//		highlight.particleSystem.emissionRate = 0;
	}

	void Update()
	{
		if (Input.GetKeyUp (KeyCode.LeftArrow)) Animate(-1);
		if (Input.GetKeyUp (KeyCode.RightArrow)) Animate(1);
		if (Input.GetKeyUp (KeyCode.Q)) Highlight(0);
		if (Input.GetKeyUp (KeyCode.W)) Highlight(1);
		if (Input.GetKeyUp (KeyCode.E)) Highlight(2);
		if (Input.GetKeyUp (KeyCode.R)) Highlight(3);
		if (Input.GetKeyUp (KeyCode.T)) Unhighlight();
		if (shake_intensity > 0) {
			camera.transform.position =  new Vector3(
				Random.Range(-shake_intensity, shake_intensity),
				500,
				Random.Range(-shake_intensity, shake_intensity));
			shake_intensity -= shake_decay;
		}
		if (transform.position.y > 2) {
			straightened = false;
		}
		if (transform.position.y < 2 && !straightened) {
			transform.position = new Vector3(0, 1, 0);
			transform.rotation = Quaternion.Euler(270, 180, 0);
			straightened = true;
		}
	}
	
	protected void Animate(float direction)
	{
		shake_intensity = Mathf.Min(10, damage) / 5;
		rocks.transform.rotation = Quaternion.Euler(-115, -20 * direction, 0);
		sparks.transform.rotation = Quaternion.Euler(-140, -20 * direction, 0);
		rocks.particleSystem.startSize = 5 + Mathf.Min(10, damage) / 2;
		flash.particleSystem.startSize = Mathf.Min(10, damage) * Mathf.Min(10, damage);
		rocks.particleSystem.Emit(Mathf.Min(10, damage) + 15);
		flash.particleSystem.Emit(Mathf.Min(10, damage) * Mathf.Min(10, damage));
		sparks.particleSystem.Emit(Mathf.Min(10, damage) * 10 + 50);
		impact.particleSystem.Emit(Mathf.Min(10, damage) * 100);
		dust.particleSystem.Emit(Mathf.Min(10, damage) * Mathf.Min(10, damage) * 20);
		Vector3 force = new Vector3 (0, Mathf.Min(10, damage) * 200 + 1000, 0);
		Vector3 position = new Vector3 (transform.position.x + direction, 
		                                transform.position.y, 
		                                transform.position.z - 1);
		rigidbody.AddForceAtPosition(force, position);
	}

	/* 0: white, 1: red, 2: green, 3: blue*/
	protected void Highlight(int color)
	{
		if (color == 0) {
			highlight.particleSystem.startColor = new Color(1.0f, 1.0f, 1.0f, 0.75f);
		} else if (color == 1) {
			highlight.particleSystem.startColor = new Color(1.0f, 0.25f, 0.25f, 0.75f);
		} else if (color == 2) {
			highlight.particleSystem.startColor = new Color(0.25f, 1.0f, 0.25f, 0.75f);
		} else {
			highlight.particleSystem.startColor = new Color(0.25f, 0.25f, 1.0f, 0.75f);
		}
		highlight.particleSystem.emissionRate = 1000;
	}

	protected void Unhighlight()
	{
		highlight.particleSystem.emissionRate = 0;
	}
}