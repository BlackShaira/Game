using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScoreValue : MonoBehaviour
{
    private GameState gameState;
    private TMPro.TextMeshProUGUI label;
    private int lastScore = int.MinValue;

    private void Start()
    {
        gameState = FindObjectOfType<GameState>();
        label = GetComponent<TMPro.TextMeshProUGUI>();
    }

    private void Update()
    {
        if (lastScore != gameState.Score)
        {
            if (label == null)
            {
                Debug.LogErrorFormat(
                    "Score not updated because component of type {0} was missing",
                    typeof(TMPro.TextMeshProUGUI));
            }
            else
            {
                label.text = gameState.Score.ToString();
            }
            lastScore = gameState.Score;
        }
    }
}
