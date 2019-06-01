using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class Fragile : MonoBehaviour
{
    public float CrackOverVelocity = 1f;
    public int PointsForCracking;
    public bool MustCrackToWin = false;

    private GameState gameState;

    private void Start()
    {
        gameState = FindObjectOfType<GameState>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("collision relative speed: " + collision.relativeVelocity.magnitude);
        if (collision.relativeVelocity.magnitude > CrackOverVelocity)
        {
            Crash();
        }
    }

    private void Crash()
    {
        // planks have no receiver
        BroadcastMessage("triggerBreak", SendMessageOptions.DontRequireReceiver);
        Destroy(this.gameObject);
        gameState.GainPoints(PointsForCracking);
    }
}
