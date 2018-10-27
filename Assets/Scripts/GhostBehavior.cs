using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.CompareTag("ball")) {
			gameObject.transform.position+=new Vector3(Random.Range(-0.5f,0.5f),0);
			Debug.Log("change position");
		}
	}
}
