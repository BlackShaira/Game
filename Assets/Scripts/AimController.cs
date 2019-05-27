using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// For how to activate drag handlers see 
/// <see cref="https://forum.unity.com/threads/ibegindraghandler-idraghandler-ienddraghandler-script-attached-to-sprite-object.294168/"/>.
/// </summary>
public class AimController : MonoBehaviour
{
    private GameState gameState;
    private Camera cachedCamera;
    private LineRenderer line;

    public float SniperLineSentivity = 1f;
    public float MaxTrajectoryLength = 10f;
    public float TrajectoryEstimationTimeStep = 0.1f;
    public Transform AimOriginTransform;
    public Transform MovableCannonTransform;

    private Vector3 AimOriginWorldCoordinates
    {
        get
        {
            return (AimOriginTransform ?? this.transform).position;
        }
    }

    private CannonBase Cannon
    {
        get
        {
            return gameState.CurrentCannon;
        }
    }

    private void Start()
    {
        gameState = FindObjectOfType<GameState>();
        cachedCamera = Camera.main;
        line = GetComponent<LineRenderer>();
    }

    public void OnBeginDrag(BaseEventData eventData)
    {
        ShowSniperLine();
        SetSniperLineOrigin();
    }

    public void OnDrag(BaseEventData eventData)
    {
        PointerEventData pointer = (PointerEventData)eventData;

        SetSniperLineTarget(pointer);
        RotateCannon(pointer);
    }

    private void RotateCannon(PointerEventData pointer)
    {
        if (MovableCannonTransform != null)
        {
            Vector2 aimingDirection = GetAimingDirection(pointer);
            float degrees = Vector2.SignedAngle(Vector2.right, aimingDirection);

            MovableCannonTransform.localRotation = Quaternion.Euler(-degrees, 0, 0);
        }
    }

    public void OnEndDrag(BaseEventData eventData)
    {
        PointerEventData pointer = (PointerEventData)eventData;
        Debug.Log(pointer);

        Vector2 aimingDirection = GetAimingDirection(pointer);
        Cannon.ShootMissiles(AimOriginTransform.gameObject, aimingDirection);
        HideSniperLine();
    }

    private void SetSniperLineOrigin()
    {
        line.SetPosition(0, AimOriginWorldCoordinates);
    }

    private void SetSniperLineTarget(PointerEventData pointer)
    {
        Vector3 aimingDirection = GetAimingDirection(pointer);

        var trajectory = Ballistics.Trajectory(
            AimOriginWorldCoordinates,
            aimingDirection.normalized * Cannon.MissileSpeed,
            Physics2D.gravity // does not account for settings on missile
            );

        float trajectoryLength = Math.Min(
            MaxTrajectoryLength,
            aimingDirection.magnitude);

        trajectoryLength = aimingDirection.magnitude;

        var linePoints = new LinkedList<Vector3>();
        Vector3 previousPoint = line.GetPosition(0);
        float totalLineLength = 0f;
        float simulationTime = 0;
        while (totalLineLength < trajectoryLength)
        {
            Vector3 currentPoint = trajectory(simulationTime);
            totalLineLength += (currentPoint - previousPoint).magnitude;
            simulationTime += Time.fixedDeltaTime;

            //simulationTime += TrajectoryEstimationTimeStep;
            previousPoint = currentPoint;

            linePoints.AddLast(currentPoint);
        }

        line.positionCount = linePoints.Count;
        line.SetPositions(linePoints.ToArray());
    }

    private Vector3 GetAimingDirection(PointerEventData pointer)
    {
        Vector3 initialTarget = GetWorldCoordinates(pointer.pressPosition);
        Vector3 currentTarget = GetWorldCoordinates(pointer.position);

        Vector3 aimingDirection = (initialTarget - currentTarget) * SniperLineSentivity;
        return aimingDirection;
    }

    private Vector3 GetWorldCoordinates(Vector3 screenPosition)
    {
        Vector3 targetPosition = cachedCamera.ScreenToWorldPoint(screenPosition);
        targetPosition.z = 0;
        return targetPosition;
    }

    private void ShowSniperLine()
    {
        line.enabled = true;
    }

    private void HideSniperLine()
    {
        line.enabled = false;
    }
}
