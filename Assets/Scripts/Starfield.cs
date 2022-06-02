using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// Follows around a camera.
/// </summary>

public class Starfield : MonoBehaviour
{
    public new Transform camera;

    private void Start()
    {
        var particleSystemRenderer = GetComponent<ParticleSystemRenderer>();

        if(particleSystemRenderer != null)
        {
            particleSystemRenderer.sharedMaterial.renderQueue = (int) RenderQueue.Background;
        }

    }

    private void LateUpdate()
    {
        if (camera != null)
        {
            transform.position = camera.position;
        }
    }
}
