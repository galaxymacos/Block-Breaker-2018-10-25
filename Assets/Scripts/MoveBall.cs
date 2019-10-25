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


//	[SerializeField] private float paddleDirBlindSpot = 1.5f;
    [SerializeField] private float deflectionFactor = 1.5f;

    [SerializeField] private float vForceMinX = 0.3f;
    [SerializeField] private float vForceMinY = 0.6f;

    [SerializeField] private float vForceMultiplier = 2f;

    public bool isDupBall;
    public int dupBallId;

    private float forceMagnitude;

    // Use this for initialization
    void Start() {
        code = GameObject.Find("GameManager").GetComponent<GameManager>();
        ball = GetComponent<Rigidbody2D>();
        ball.simulated = false;
        if (code.initPos == Vector3.zero) {
            code.initPos = transform.localPosition;
        }
        myParent = transform.parent;
    }

    public void Init() {
        transform.SetParent(myParent);

        transform.localPosition = code.initPos;
        ball.simulated = false;
        ball.velocity = new Vector2(0, 0);
        onlyOnce = false;
        isDupBall = false;    // BUG don't know if there is a bug or not
    }

    private Vector2 velocityCacheEachHit;

    // Update is called once per frame
    void Update() {
        if ((Input.GetButtonUp("Jump") || isDupBall) && !onlyOnce && code.inGame) {
            onlyOnce = true;
            ball.simulated = true;
            var position = ball.transform.position;
            ball.transform.SetParent(null);
            ball.transform.position = position;
            if(!isDupBall)
            ball.AddForce(new Vector2(dir, dir));
            else {
                if (dupBallId == 1) {
                    ball.velocity = code.allBalls[0].GetComponent<Rigidbody2D>().velocity.Rotate(30);
                }
                else if (dupBallId == 2) {
                    ball.velocity = code.allBalls[0].GetComponent<Rigidbody2D>().velocity.Rotate(-30);
                }
            }
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
                Debug.Log("copied ball death");
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
            ball.AddForce(new Vector2(xForce / sqrt * forceMagnitude, dir / sqrt * forceMagnitude));
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

public static class Vector2Extension {
    public static Vector2 Rotate(this Vector2 v, float degrees) {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
}