using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CircularOrbit : MonoBehaviour
{
    public Transform center;
    public float radius = 1.0f;
    public float speed = 1.0f;
    public float angle = 0.0f;

    void Update()
    {
        angle += speed * Time.deltaTime;
        if (center != null)
        {
            transform.position = center.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
        }
    }
}
