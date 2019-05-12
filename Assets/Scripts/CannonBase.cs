using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class CannonBase : MonoBehaviour
{
    public GameObject MissilePrefab;
    public float MissileSpeed;

    public abstract void ShootMissiles(GameObject cannonObject, Vector2 aimingDirection);
}
