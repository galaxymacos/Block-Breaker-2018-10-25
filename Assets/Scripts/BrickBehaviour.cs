using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class BrickBehaviour : MonoBehaviour {
    [SerializeField] private GameObject[] dropItemList;

    [SerializeField] private int PowerUPPercentage = 20;
    [SerializeField] private int blockHp = 1;
    private GameObject ball;
    private bool isHit;
    private GameManager code;

    // Use this for initialization
    void Start() {
        code = GameObject.Find("GameManager").GetComponent<GameManager>();
        for (int i = 0; i < blockHp; i++) {
            code.AddBlock();
        }

        ball = GameObject.Find("ball");
    }

    // Update is called once per frame
    void Update() {
    }

    private void OnCollisionExit2D(Collision2D other) {
        blockHp--;

        Debug.Log("HP: " + blockHp);
        float destiny = Random.Range(1, 100);
        if (destiny > 0 && destiny <= PowerUPPercentage) {
            GameObject itemReadyToDrop = dropItemList[Random.Range(0, dropItemList.Length)];
            Instantiate(itemReadyToDrop, other.gameObject.transform.position, Quaternion.identity);
        }

        code.AddPoints();
        if (blockHp <= 0) {
            Debug.Log("Destroy a block in collision");
            code.PlayExplosionSound(gameObject.transform.position.x);
            Destroy(gameObject);
        }
        else {
            code.PlayBlockBreakSound(gameObject.transform.position.x);
            GetComponent<SpriteRenderer>().color = new Color(CreateRandomValue(0,255),CreateRandomValue(0,255),CreateRandomValue(0,255));
        }
    }

    private float CreateRandomValue(int min, int max) {
        return Random.Range(min, max + 1) - Mathf.Epsilon;
    }

    private void OnTriggerEnter2D(Collider2D other) // trigger when the ball has fire effect
    {
        if (!code.isFireBall) return;
        if (isHit) return;
        isHit = true;
        code.PlayExplosionSound(gameObject.transform.position.x);

        float destiny = Random.Range(1, 100);
        if (destiny > 0 && destiny <= PowerUPPercentage) {
            GameObject itemReadyToDrop = dropItemList[Random.Range(0, dropItemList.Length)];
            Instantiate(itemReadyToDrop, other.gameObject.transform.position, Quaternion.identity);
        }

        for (int i = 0; i < blockHp; i++) {
            code.AddPoints();
        }
        Destroy(gameObject);
    }
}