using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MazeSpawner))]
public class MazeEditor : Editor
{
    private MazeSpawner mazeSpawner;

    void OnEnable()
    {
        mazeSpawner = (MazeSpawner)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Maze"))
        {
            mazeSpawner.GenerateMaze();
        }

        if (GUILayout.Button("Delete Maze"))
        {
            mazeSpawner.DeleteMaze();
        }
    }
}
