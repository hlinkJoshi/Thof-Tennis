using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickActions : MonoBehaviour
{
    public void OnClickPlayAgain()
    {
        ResetAllGame(ref TennisPlayer.instance.scoreManagement, GameManager.instance.gameOverScore.playerScoreFields);
        ResetAllGame(ref AiPlayer.instance.scoreManagement, GameManager.instance.gameOverScore.aiScoreFields);
        GameManager.instance.ResetGame();
    }

    private void ResetAllGame(ref ScoreManagement scoreManagement, GameManager.ScoreFields scoreFields)
    {

        scoreManagement.score =
            scoreManagement.round =
            scoreManagement.totalWin =
            scoreManagement.aces =
            scoreManagement.breakPointsWon =
            scoreManagement.receivingPointsWon =
            scoreManagement.winners =
            scoreManagement.unforcesErrors =
            scoreManagement.totalPointsWon =
            scoreManagement.fastestServe = 0;

        scoreFields.aces.text = "0";
        scoreFields.breakPointsWon.text = "0";
        scoreFields.receivingPointsWon.text = "0";
        scoreFields.Winners.text = "0";
        scoreFields.UnforcedErrors.text = "0";
        scoreFields.totalPointsWon.text = "0";
        scoreFields.fastestServe.text = "0";
    }
    public void MuteAudio()
    {
        GameManager.instance.MuteAudio();
        Debug.Log("Calling *********", transform.gameObject);
    }

    public void ShowHelp()
    {
        Time.timeScale = 0;
        GameManager.instance.howToPlayScreenObj.SetActive(true);
    }
    public void HideHelp()
    {
        Time.timeScale = 1;
        GameManager.instance.howToPlayScreenObj.SetActive(false);
    }
}
