using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeConstructor : MonoBehaviour
{


    public float hallWidth
    {
        get; private set;
    }
    public float hallHeight
    {
        get; private set;
    }

    public int startRow
    {
        get; private set;
    }
    public int startCol
    {
        get; private set;
    }

    public int goalRow
    {
        get; private set;
    }
    public int goalCol
    {
        get; private set;
    }
    
    public bool showDebug;
    // materials for generated models
    [SerializeField] private Material mazeMat1;
    [SerializeField] private Material mazeMat2;
    [SerializeField] private Material startMat;
    [SerializeField] private Material treasureMat;
    
    // to store datagenerator
    private MazeDataGenerator dataGenerator;
    private MazeMeshGenerator meshGenerator;
    
    
    

    // read-only outside this class, cant be modified from outside
    public int[,] data
    {
        get; private set;
    }

    void Awake()
    {

        dataGenerator = new MazeDataGenerator();
        meshGenerator = new MazeMeshGenerator();
        
        // default to walls surrounding a single empty cell
        data = new int[,]
        {
            // 1= wall, 0=empty
            { 1, 1, 1 },
            { 1, 0, 1 },
            { 1, 1, 1 }
        };
    }

    public void GenerateNewMaze(int sizeRows, int sizeCols, TriggerEventHandler startCallback = null, TriggerEventHandler goalCallback = null)
    {
        if (sizeRows % 2 == 0 && sizeCols % 2 == 0)
        {
            Debug.LogError("Odd numbers work better for dungeon size");
        }
        DisposeOldMaze();

        data = dataGenerator.FromDimensions(sizeRows, sizeCols);
        
        FindStartPosition();
        FindGoalPosiiton();
        
        // store values used to generate this mesh
        hallWidth = meshGenerator.width;
        hallHeight = meshGenerator.height;
        
        DisplayMaze();
        
        PlaceStartTrigger(startCallback);
        PlaceGoalTrigger(goalCallback);
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

    private void DisplayMaze()
    {
        GameObject go = new GameObject();
        go.transform.position = Vector3.zero;
        go.name = "Procedural Maze";
        go.tag = "Generated";


        MeshFilter mf = go.AddComponent<MeshFilter>();
        mf.mesh = meshGenerator.FromData(data);

        MeshCollider mc = go.AddComponent<MeshCollider>();
        mc.sharedMesh = mf.mesh;

        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        mr.materials = new Material[2] { mazeMat1, mazeMat2 };
        
    }
    
    // destroy old maze
    public void DisposeOldMaze()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Generated");
        foreach (GameObject go in objects)
        {
            Destroy(go);
        }
    }
    
    // find start position
    private void FindStartPosition()
    {
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);

        for (int i = 0; i <= rMax; i++)
        {
            for (int j = 0; j <= cMax; j++)
            {
                if (maze[i, j] == 0)
                {
                    startRow = i;
                    startCol = j;
                    return;
                    
                }
            }
        }
        
    }
    
    
    // find goal position
    private void FindGoalPosiiton()
    {
        int[,] maze = data;
        int rMax = maze.GetUpperBound(0);
        int cMax = maze.GetUpperBound(1);
        
        // loop  top to buttom, right to left
        for (int i = rMax; i >= 0; i--)
        {
            for (int j = cMax; j >= 0; j--)
            {
                if (maze[i, j] == 0)
                {
                    goalRow = i;
                    goalCol = j;
                    return;
                }
            }
        }
            
    }


    private void PlaceStartTrigger(TriggerEventHandler callback)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = new Vector3(startCol * hallWidth, .5f, startRow * hallWidth);
        go.name = "Start Trigger";
        go.tag = "Generated";

        go.GetComponent<BoxCollider>().isTrigger = true;
        go.GetComponent<MeshRenderer>().sharedMaterial = startMat;

        TriggerEventRouter tc = go.AddComponent<TriggerEventRouter>();
        tc.callback = callback;
    }

    private void PlaceGoalTrigger(TriggerEventHandler callback)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.position = new Vector3(goalCol * hallWidth, .5f, goalRow * hallWidth);
        go.name = "Treasure";
        go.tag = "Generated";
        
        go.GetComponent<BoxCollider>().isTrigger = true;
        go.GetComponent<MeshRenderer>().sharedMaterial = startMat;

        
        // takes a callback function to call when smth enters the trigger volume
        TriggerEventRouter tc = go.AddComponent<TriggerEventRouter>();
        tc.callback = callback;
        
    }
    
}
