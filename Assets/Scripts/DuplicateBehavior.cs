using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class DuplicateBehavior : MonoBehaviour
{
    private GameManager code;
    private GameObject ball;
    [SerializeField] private Transform copiedBall;
    private bool hasCollided;    // prevent it from being triggered more than once

    // Use this for initialization
    void Start()
    {
        code = GameObject.Find("GameManager").GetComponent<GameManager>();
        ball = GameObject.Find("ball");
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, -2 * Time.deltaTime, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject.Find("GameManager").GetComponent<AudioSource>().Play();
        code.allBalls = GameObject.FindGameObjectsWithTag("ball");
        if (other.gameObject.CompareTag("paddle")&&!hasCollided)
        {
            hasCollided = true;
            var extraBall = Instantiate(ball, ball.transform,true);
            MoveBall moveBall = extraBall.GetComponent<MoveBall>();
            moveBall.transform.parent = GameObject.FindGameObjectWithTag("paddle").transform;
            moveBall.isDupBall = true;
            moveBall.dupBallId = 1;
            var extraBall2 = Instantiate(ball, ball.transform,true);
            MoveBall moveBall2 = extraBall2.GetComponent<MoveBall>();
            moveBall2.transform.parent = GameObject.FindGameObjectWithTag("paddle").transform;
            moveBall2.isDupBall = true;
            moveBall2.dupBallId = 2;
            Destroy(gameObject);
        }

        if (other.gameObject.CompareTag("death"))
        {
            Destroy(gameObject);
        }
    }
}