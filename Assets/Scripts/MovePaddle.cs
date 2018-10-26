using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MovePaddle : MonoBehaviour {
    [SerializeField] private Slider slider;

    [SerializeField] private float positionLimit = 2.0f;
    [SerializeField] private GameObject pauseScreen;
    private GameObject ball;

    private GameManager code;

    // Use this for initialization
    void Start() {
        ball = GameObject.Find("ball");
        code = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update() {
        if (code.inGame&&!pauseScreen.activeSelf) {
            
            float pos = slider.value;
            var position = transform.position;
            position = new Vector3(pos * positionLimit, position.y, position.z);
            transform.position = position;
        }
    }
}