using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedDownBehavior : MonoBehaviour {

	[SerializeField] private float duration = 5f;
	private GameManager code;
	
	private bool countDown;
	// Use this for initialization
	void Start () {
		code = GameObject.Find("GameManager").GetComponent<GameManager>();

	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(0, -2 * Time.deltaTime, 0);
		
	}

	private bool hasCollided;
	
	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag("paddle")&&!hasCollided) {
			hasCollided = true;
			gameObject.transform.position = new Vector3(1000,0,0);
			StartCoroutine(CountDown());	
		}
		
	}

	IEnumerator CountDown() {
		code.AddDragonBall();
		Time.timeScale += 1f;
		yield return new WaitForSeconds(duration*Time.timeScale);	// default: 5s
		Time.timeScale -= 1f;
		Destroy(gameObject);

	}
}
