using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GUIManager : MonoBehaviour
{
    [SerializeField] private GameManagerScript gameManager;
    [SerializeField] private ScoreScript scoreManager;

    [SerializeField] private Sprite[] finalResultImages;
    [SerializeField] private GameObject finalResultDisplay;

    // Wordbank Var
    public TMP_Text wordBank;

    // Timer Var
    public TMP_Text timer;
    private float startingTime;
    private float timeRemaining;
    private float timeRemainingOutput;
    private bool isTimerOn = false;

    // Score Field Var
    public TMP_Text ScoreText;

    // End tabs Var
    public GameObject continueTab;
    public GameObject retryTab;

    void Start() {
        timeRemaining = InitializeTime();
        startingTime = timeRemaining;
    }

    void Update() {
        if (isTimerOn) {
            if (timeRemaining > 0) {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
            } else {
                isTimerOn = false;
                Invoke("ShowRetryTab", 2f);
            }
        }  
        ScoreText.text = scoreManager.GetTotalScore().ToString("0");
    }

    // Set up wordbank using the data from GameLogicManager
    public void SetUpWordBank(List<string> wordBankContent) {
        string wordBankText = "";
        for (int i = 0; i < wordBankContent.Count; i++) {
            string temp = wordBankContent[i];
            int randomIndex = Random.Range(i, wordBankContent.Count);
            wordBankContent[i] = wordBankContent[randomIndex];
            wordBankContent[randomIndex] = temp;
        }
        foreach(string word in wordBankContent) {
            wordBankText += word + "\n";
        }
        wordBank.text = wordBankText;
    }

    public void StartTimer() {
        if (!isTimerOn) {
            isTimerOn = true;
        } 
    }

    // To control time based on level
    int InitializeTime() {
        if (gameManager.GetCurrentLevel() == 1) {
            return 150;
        } else if(gameManager.GetCurrentLevel() == 2) {
            return 300;
        } else if(gameManager.GetCurrentLevel() == 3) {
            return 300;
        } else if(gameManager.GetCurrentLevel() == 4) {
            return 300;
        } else if(gameManager.GetCurrentLevel() == 5) {
            return 480;
        } else if(gameManager.GetCurrentLevel() == 6) {
            return 480;
        } else {
            return 0;
        }

    }

    void DisplayTime(float timeToDisplay) {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeToDisplay / 60); 
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeRemainingOutput = minutes * 60 + seconds;
        timer.text = string.Format("{0:00} {1:00}", minutes, seconds);
    }

    public float GetRemainingTime() {
        return timeRemainingOutput;
    }

    public float GetStartingTime() {
        return startingTime;
    }

    public void LevelCleared(int numberOfStages) {
        isTimerOn = false;
        // if the level only has one stage
        if (numberOfStages == 1) {
            ShowContinueTab();
        } else {
            ShowFinalResult();
            Invoke("ShowContinueTab", 2f);
        }
    }

    void ShowFinalResult() {
        finalResultDisplay.SetActive(true);
        finalResultDisplay.GetComponent<Image>().sprite = finalResultImages[gameManager.GetCurrentLevel() - 2];
    }

    void ShowContinueTab() {
        FindObjectOfType<AudioManager>().Play("Page Flip");
        continueTab.SetActive(true);
        TMP_Text finalScore = continueTab.transform.GetChild(3).GetComponent<TMP_Text>();
        finalScore.text = "Score:" + scoreManager.GetTotalScore().ToString("0");
    }

    void ShowRetryTab() {
        PlayerPrefs.SetInt("TotalScore", 0);
        FindObjectOfType<AudioManager>().Play("Timeover");
        retryTab.SetActive(true);
        TMP_Text finalScore = retryTab.transform.GetChild(3).GetComponent<TMP_Text>();
        finalScore.text = "Score:" + scoreManager.GetTotalScore().ToString("0");
    }
}
