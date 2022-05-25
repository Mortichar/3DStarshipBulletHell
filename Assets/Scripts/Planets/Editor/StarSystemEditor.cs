using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StarSystem))]
public class StarSystemEditor : Editor
{
    private StarSystem starSystem;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Star System"))
        {
            starSystem.Generate();
        }
    }

    private void OnEnable()
    {
        starSystem = (StarSystem)target;
    }
}
