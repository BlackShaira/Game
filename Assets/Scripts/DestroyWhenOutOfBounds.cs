using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

public class DestroyWhenOutOfBounds : MonoBehaviour
{
    private Collider mapArea;
    private Collider2D[] currentColliders;
    private bool isMarkedForDestruction = false;

    private void Start()
    {
        //This must be a 3D collider because 2D colliders are flat and intersection
        //doesn't work.
        mapArea = GameObject.Find("MapBounds").GetComponent<Collider>();
        currentColliders = GetComponents<Collider2D>();
    }

    private void Update()
    {
        if (isMarkedForDestruction)
        {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (!IsInMapBounds())
        {
            isMarkedForDestruction = true;
        }
    }

    private bool IsInMapBounds()
    {
        return currentColliders.Any(collider => mapArea.bounds.Intersects(collider.bounds));
    }
}
