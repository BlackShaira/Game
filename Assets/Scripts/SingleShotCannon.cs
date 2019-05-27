using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class SingleShotCannon : CannonBase
{
    private GameState gameState;

    private void Start()
    {
        gameState = FindObjectOfType<GameState>();
    }

    public override void ShootMissiles(GameObject cannonObject, Vector2 aimingDirection)
    {
        if (MissilePrefab == null)
        {
            Debug.LogError("Missile prefab is not set. Can't shoot!");
            return;
        }

        GameObject missile = GameObject.Instantiate(
            MissilePrefab,
            cannonObject.transform.position,
            Quaternion.identity);
        SetSpeed(missile, aimingDirection.normalized * MissileSpeed);

        if (gameState == null)
        {
            gameState = FindObjectOfType<GameState>();
        }
        gameState.MissilesRemaining -= 1;
    }

    private void SetSpeed(GameObject missile, Vector2 velocity)
    {
        Rigidbody2D body = missile.GetComponent<Rigidbody2D>();
        if (body == null)
        {
            Debug.LogError(
                "The missile prefab seems to not have a rigid body. " +
                "It's required so that velocity can be set for new missiles.");
            return;
        }

        body.velocity = velocity;
    }
}
