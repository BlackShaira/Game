using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Ballistics
{
    /// <summary>
    /// Returns a parametric trajectory function that can be used to 
    /// estimate movement of a missile in a gravitational field.
    /// The function parameter is time.
    /// </summary>
    public static Func<float, Vector2> Trajectory(
        Vector2 initialPosition,
        Vector2 initialSpeed,
        Vector2 gravity
        )
    {
        return t => initialPosition + initialSpeed * t + 0.5f * gravity * t * t;
    }
}
