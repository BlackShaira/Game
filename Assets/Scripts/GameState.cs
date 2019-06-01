using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    private static int highscore;

    public CannonBase CurrentCannon;
    public int Score;
    public int MissilesRemaining;
    public GamePhase Phase = GamePhase.Playing;
    public float LastShotTime;

    private int initialMissileCount;

    public int Highscore
    {
        get { return GameState.highscore; }
        set { GameState.highscore = value; }
    }

    private void Awake()
    {
        initialMissileCount = MissilesRemaining;
    }

    public void Restart()
    {
        MissilesRemaining = initialMissileCount;
        Score = 0;
        Phase = GamePhase.Playing;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SetScore(int score)
    {
        Score = score;
        Highscore = Math.Max(score, Highscore);
    }

    public void GainPoints(int points)
    {
        SetScore(Score + points);
    }
}
