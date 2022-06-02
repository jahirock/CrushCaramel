using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public static GUIManager sharedInstance;

    private int moveCounter;
    public Text movesText, scoreText;

    [SerializeField] GameObject gameOverScreen;
    [SerializeField] Text finalScore;

    [SerializeField] int maxMoves;

    private int score;

    public int Score {
        get { return score; }
        set {
            score = value;
            scoreText.text = "Score: " + score;
        }
    }

    public int MoveCounter
    {
        get { return moveCounter; }
        set
        {
            moveCounter = value;
            movesText.text = "Moves: " + moveCounter;

            if(moveCounter <= 0)
            {
                moveCounter = 0;
                StartCoroutine(GameOver());
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        score = 0;
        moveCounter = maxMoves;
        movesText.text = "Moves: " + moveCounter;
        scoreText.text = "Score: " + score;
    }

    IEnumerator GameOver()
    {
        yield return new WaitUntil(() => !BoardManager.sharedInstace.isShifting);
        yield return new WaitForSeconds(0.1F);

        gameOverScreen.SetActive(true);
        finalScore.text = "Score: " + score;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("Game");
    }
}
