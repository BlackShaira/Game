using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MissilesValue : MonoBehaviour
{
    private GameState gameState;
    private TextMeshProUGUI label;
    private int lastAmount = int.MinValue;

    private void Start()
    {
        gameState = FindObjectOfType<GameState>();
        label = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (lastAmount != gameState.Score)
        {
            if (label == null)
            {
                Debug.LogErrorFormat(
                    "Score not updated because component of type {0} was missing",
                    typeof(TextMeshProUGUI));
            }
            else
            {
                label.text = gameState.MissilesRemaining.ToString();
            }
            lastAmount = gameState.MissilesRemaining;
        }
    }
}
