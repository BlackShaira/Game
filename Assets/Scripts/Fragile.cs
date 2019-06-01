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
    public float CollisionWaitTime = 4f;

    private float lastCollisionTime;
    private GameState gameState;

    public bool WasNotHitLongTimeByMissile
    {
        get { return Time.fixedTime - lastCollisionTime > CollisionWaitTime; }
    }

    private void Start()
    {
        gameState = FindObjectOfType<GameState>();
        lastCollisionTime = Time.fixedTime;
    }

    private void FixedUpdate()
    {
        lastCollisionTime = Math.Max(gameState.LastShotTime, lastCollisionTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var missile =
            collision.otherCollider.GetComponent<Missile>() ??
            collision.collider.GetComponent<Missile>();

        if (missile)
        {
            lastCollisionTime = Time.fixedTime;
        }

        //Debug.Log("collision relative speed: " + collision.relativeVelocity.magnitude);
        if (collision.relativeVelocity.magnitude > CrackOverVelocity)
        {
            float comboMultiplier = 1f;

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
