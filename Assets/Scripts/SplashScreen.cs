using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine("GoToMainMenu");
	}

	IEnumerator GoToMainMenu() {
		yield return new WaitForSeconds(5);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
	}
}
