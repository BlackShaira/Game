using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class WinLoseCondition : MonoBehaviour
{
    public float EvaluteEveryNSeconds = 1f;
    public float DelayAfterGameFinishInSeconds = 2f;
    public GameObject WinGui;
    public GameObject LoseGui;

    private GameState gameState;
    private float lastEvaluationTime = float.MinValue;

    private void Start()
    {
        gameState = FindObjectOfType<GameState>();
    }

    private void Update()
    {
        float evalutionTime = Time.time;
        float timeSinceLastEvaluation = evalutionTime - lastEvaluationTime;
        if (timeSinceLastEvaluation >= EvaluteEveryNSeconds)
        {
            lastEvaluationTime = evalutionTime;
            Evaluate();
        }
    }

    private void Evaluate()
    {
        if (gameState.Phase == GamePhase.Playing)
        {
            Fragile[] fragiles = FindObjectsOfType<Fragile>();
            if (fragiles.Length == 0)
            {
                StartCoroutine(WinRoutine());
            }

            Missile[] missiles = FindObjectsOfType<Missile>();
            int activeMissiles = CountActiveBodies(missiles);
            int activeFragiles = CountActiveBodies(fragiles);
            if (gameState.MissilesRemaining == 0
                && fragiles.Length > 0
                && activeMissiles == 0
                && activeFragiles == 0)
            {
                StartCoroutine(LoseRoutine());
            }
        }
    }

    private int CountActiveBodies(Component[] actors)
    {
        return actors.Count(actor => !actor.GetComponent<Rigidbody2D>().IsSleeping());
    }

    IEnumerator WinRoutine()
    {
        Debug.Log("Win!");
        gameState.Phase = GamePhase.Won;
        yield return new WaitForSeconds(DelayAfterGameFinishInSeconds);

        EnableObject(WinGui);
    }

    IEnumerator LoseRoutine()
    {
        Debug.Log("Loss!");
        gameState.Phase = GamePhase.Lost;
        yield return new WaitForSeconds(DelayAfterGameFinishInSeconds);

        EnableObject(LoseGui);
    }

    private void EnableObject(GameObject gameObject)
    {
        if (!gameObject)
        {
            Debug.LogError("Make sure the GUI to enable is set!");
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
