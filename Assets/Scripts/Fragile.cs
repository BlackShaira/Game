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
            float comboMultiplier = 1f;
            var missile =
                collision.otherCollider.GetComponent<Missile>() ??
                collision.collider.GetComponent<Missile>();

            if (missile)
            {
                comboMultiplier *= missile.PointsMultiplier;
                // next hits will get more points:
                missile.PointsMultiplier *= missile.ComboMultiplier;
            }

            Crash(comboMultiplier);
        }
    }

    private void Crash(float comboMultiplier)
    {
        // planks have no receiver (no triggerBreak method)
        BroadcastMessage("triggerBreak", SendMessageOptions.DontRequireReceiver);
        Destroy(this.gameObject);

        int pointsReceived = (int)(PointsForCracking * comboMultiplier);
        gameState.GainPoints(pointsReceived);
    }
}
