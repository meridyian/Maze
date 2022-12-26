using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeConstructor : MonoBehaviour
{


    public bool showDebug;
    // materials for generated models
    [SerializeField] private Material mazeMat1;
    [SerializeField] private Material mazeMat2;
    [SerializeField] private Material startMat;
    [SerializeField] private Material treasureMat;
    
    // read-only outside this class, cant be modified from outside
    public int[,] data
    {
        get; private set;
    }

    void Awake()
    {
        // default to walls surrounding a single empty cell
        data = new int[,]
        {
            // 1= wall, 0=empty
            { 1, 1, 1 },
            { 1, 0, 1 },
            { 1, 1, 1 }
        };
    }

    public void GenerateNewMaze(int sizeRows, int sizeCols)
    {
        // stub to fill in
    }
    
    // to display

    private void OnGUI()
    {
        //1
        if (!showDebug) ;
        {
            return;
        }   
        
        //2 MAX ROW AND COLUMN TO BUILD UP
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        string msg = "";
        
        //3 check rows ando columns, append "...." or "==" if the value is 0

        for (int i = rMax; i >= 0; i--)
        {
            for (int j = 0; j <= cMax; j++)
            {
                if (maze[i, j] == 0)
                {
                    msg += "....";

                }
                else
                {
                    msg += "==";
                }
            }

            msg += "\n";
        }
        // prints out the built-up string
        GUI.Label(new Rect(20,20,500,500), msg);
    }
}
