using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EllipticalOrbit : ScriptableObject
{
    private float semiMajorAxis = 1f;

    public float SemiMajorAxis
    {
        set { semiMajorAxis = value; RecalculateMinorAxis(); }
        get => semiMajorAxis;
    }

    private float eccentricity;

    public float Eccentricity
    {
        set { eccentricity = value; RecalculateMinorAxis(); }
        get => eccentricity;
    }

    private float semiMinorAxis;

    public EllipticalOrbit(float semiMajorAxis, float eccentricity)
    {
        this.semiMajorAxis = semiMajorAxis;
        this.eccentricity = eccentricity;
        RecalculateMinorAxis();
    }

    public Vector2 GetPosition(float time)
    {
        float angle = 2 * Mathf.PI * time;
        float x = semiMajorAxis * Mathf.Sin(angle);
        float y = semiMinorAxis * Mathf.Cos(angle);
        return new Vector2(x, y);
    }

    void RecalculateMinorAxis()
    {
        semiMinorAxis = semiMajorAxis * Mathf.Sqrt(1 - Mathf.Pow(eccentricity, 2));
    }
}
