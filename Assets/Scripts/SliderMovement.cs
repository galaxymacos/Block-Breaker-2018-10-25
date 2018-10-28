using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderMovement : MonoBehaviour {
	private Slider slider;

	[SerializeField] private float moveSpeed = 0.5f;
	// Use this for initialization
	void Start () {
		slider = GetComponent<Slider>();
	}
	
	// Update is called once per frame
	void Update () {
		slider.value += Input.GetAxis("Mouse X") / Time.timeScale * moveSpeed;
//		var userInput = Input.GetAxis("Horizontal");
//		userInput = userInput / Time.timeScale;
//		slider.value += userInput*moveSpeed;
	}
}
