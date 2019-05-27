using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameState : MonoBehaviour
{
    public CannonBase CurrentCannon;
    public int Score;
    public int Highscore;
    public int MissilesRemaining;
    public GamePhase Phase = GamePhase.Playing;

    private int initialMissileCount;

    private void Awake()
    {
        initialMissileCount = MissilesRemaining;
    }

    public void Restart()
    {
        MissilesRemaining = initialMissileCount;
        Score = 0;
        Phase = GamePhase.Playing;
    }

    public void SetScore(int score)
    {
        Score = score;
        Highscore = Math.Max(score, Highscore);
    }
}
