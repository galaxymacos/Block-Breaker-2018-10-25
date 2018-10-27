using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankingScreen : MonoBehaviour {
//    [SerializeField] private InputField playerName;
    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private TextMeshProUGUI first;
    [SerializeField] private TextMeshProUGUI second;
    [SerializeField] private TextMeshProUGUI third;
    [SerializeField] private GameObject gameOverScreen;

    private GameManager code;

    private bool isConfirmedQuit;

    // Use this for initialization
    void Start() {
//        PlayerPrefs.DeleteAll();    // TODO delete
        code = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update() {

    }

    IEnumerator GoToEndScreen() {
        yield return new WaitForSeconds(5);
        gameOverScreen.SetActive(true);
        gameObject.SetActive(false);
        SaveScoreToFile();
    }

    public void UploadPlayerScore() {
        var playerScore = new GameManager.PlayerInfo {Name = playerName.text, Score = code.point};
        int index = code.playersInfo.Length - 1;
        if (code.playersInfo[index].Score > code.point) {
            return;
        }

        code.playersInfo[index] = playerScore;
        while (index - 1 >= 0 && code.playersInfo[index - 1].Score < code.playersInfo[index].Score) {
            var temp = code.playersInfo[index - 1];
            code.playersInfo[index - 1] = code.playersInfo[index];
            code.playersInfo[index] = temp;
            index--;
        }

        first.text = code.playersInfo[0].DisplayMessage();
        second.text = code.playersInfo[1].DisplayMessage();
        third.text = code.playersInfo[2].DisplayMessage();
        
        StartCoroutine("GoToEndScreen");     
    }

    private void SaveScoreToFile() {
        string[] _playersInfo = new string[3];
        for (int i = 0; i < _playersInfo.Length; i++) {
            _playersInfo[i] = code.playersInfo[i].ToString();
        }

        var playerLongString = string.Join(";", _playersInfo);

        PlayerPrefs.SetString("RankingList", playerLongString);
    }
}