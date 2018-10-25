using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Xml;
using UnityEditor;
using UnityEngine;

public class MoveBall : MonoBehaviour {
    private GameManager code;
    [SerializeField] private float dir = 150.0f;

    private Rigidbody2D ball;

    private bool onlyOnce;

    private Transform myParent;

    private Vector3 initPos;

//	[SerializeField] private float paddleDirBlindSpot = 1.5f;
    [SerializeField] private float deflectionFactor = 1.5f;

    [SerializeField] private float vForceMinX = 0.3f;
    [SerializeField] private float vForceMinY = 0.6f;

    [SerializeField] private float vForceMultiplier = 2f;

    public bool isDupBall;

    private float forceMagnitude;

    // Use this for initialization
    void Start() {
        code = GameObject.Find("GameManager").GetComponent<GameManager>();
        ball = GetComponent<Rigidbody2D>();
        ball.simulated = false;
        initPos = transform.localPosition; // retrieve the coordinate relative to parent
        myParent = transform.parent;
    }

    public void Init() {
        transform.parent = myParent;
        transform.localPosition = initPos;
        ball.simulated = false;
        ball.velocity = new Vector2(0, 0);
        onlyOnce = false;
    }

    private Vector2 velocityCacheEachHit;

    // Update is called once per frame
    void Update() {
        if ((Input.GetButtonUp("Jump") || isDupBall) && !onlyOnce && code.inGame) {
            onlyOnce = true;
            ball.simulated = true;
            ball.transform.parent = null;
            ball.AddForce(new Vector2(dir, dir));
            forceMagnitude = Mathf.Sqrt(dir * dir + dir * dir);
            velocityCacheEachHit = ball.velocity;
        }

        // Bug the bug, but performance is so bad
        velocityCacheEachHit = ball.velocity;
    }


    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("death")) {
            if (GameObject.FindGameObjectsWithTag("ball").Length > 1) {
                Destroy(gameObject);
            }
            else {
                code.Death();
            }
        }

        else if (other.gameObject.CompareTag("paddle")) {
            float paddleSize = other.gameObject.GetComponent<BoxCollider2D>().size.x;
            if (code.isFireBall) {
                code.ConsumeFireBall();
            }

            Time.timeScale *= 1.005f; // increase difficulty
            // TODO slow down paddle speed
            float diffX = transform.position.x - other.transform.position.x;

            ball.velocity = new Vector2(0, 0);
            float xForce = deflectionFactor * diffX / (paddleSize / 2) * dir;
            float sqrt = Mathf.Sqrt(xForce * xForce + dir * dir);
            ball.AddForce(new Vector2(xForce/sqrt*forceMagnitude, dir/sqrt*forceMagnitude));
        }
        else if (other.gameObject.CompareTag("wall")) {
            if (Mathf.Abs(ball.velocity.x) < vForceMinX) {
                ball.velocity = new Vector2(-velocityCacheEachHit.x, velocityCacheEachHit.y);
            }
        }
        else if (other.gameObject.CompareTag("ceiling")) {
            if (Mathf.Abs(ball.velocity.y) < vForceMinY) {
                ball.velocity = new Vector2(velocityCacheEachHit.x, -velocityCacheEachHit.y);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (Mathf.Abs(ball.velocity.y) < vForceMinY) {
            float velX = ball.velocity.x;
            if (ball.velocity.y < 0) {
                ball.velocity = new Vector2(velX, -vForceMinY * vForceMultiplier);
            }
            else {
                ball.velocity = new Vector2(velX, vForceMinY * vForceMultiplier);
            }
        }
    }
}