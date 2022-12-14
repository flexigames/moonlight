using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Unity.AI.Navigation;
using System.Collections.Generic;

//<summary>
//Game object, that creates maze and instantiates it in scene
//</summary>
public class MazeSpawner : MonoBehaviour
{
    public enum MazeGenerationAlgorithm
    {
        PureRecursive,
        RecursiveTree,
        RandomTree,
        OldestTree,
        RecursiveDivision,
    }

    public MazeGenerationAlgorithm Algorithm = MazeGenerationAlgorithm.PureRecursive;
    public bool FullRandom = false;
    public int RandomSeed = 12345;
    public GameObject Floor = null;
    public GameObject Wall = null;
    public GameObject Pillar = null;
    public int Rows = 5;
    public int Columns = 5;
    public float CellWidth = 5;
    public float CellHeight = 5;
    public GameObject GoalPrefab = null;
    public GameObject enemyPrefab = null;
    public int enemyCount = 5;

    private BasicMazeGenerator mMazeGenerator = null;

    void Start()
    {
        if (!FullRandom)
        {
            Random.seed = RandomSeed;
        }
        switch (Algorithm)
        {
            case MazeGenerationAlgorithm.PureRecursive:
                mMazeGenerator = new RecursiveMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveTree:
                mMazeGenerator = new RecursiveTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RandomTree:
                mMazeGenerator = new RandomTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.OldestTree:
                mMazeGenerator = new OldestTreeMazeGenerator(Rows, Columns);
                break;
            case MazeGenerationAlgorithm.RecursiveDivision:
                mMazeGenerator = new DivisionMazeGenerator(Rows, Columns);
                break;
        }
        mMazeGenerator.GenerateMaze();

        List<MazeCell> goals = new List<MazeCell>();

        for (int row = 1; row < Rows; row++)
        {
            for (int column = 1; column < Columns; column++)
            {
                MazeCell cell = mMazeGenerator.GetMazeCell(row, column);
                if (cell.IsGoal)
                {
                    goals.Add(cell);
                }
            }
        }

        // shuffle array
        for (int i = 0; i < goals.Count; i++)
        {
            MazeCell temp = goals[i];
            int randomIndex = Random.Range(i, goals.Count);
            goals[i] = goals[randomIndex];
            goals[randomIndex] = temp;
        }

        var maxNumberOfGoals = 5;

        goals = goals.GetRange(0, Mathf.Min(goals.Count, maxNumberOfGoals));



        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                float x = column * CellWidth;
                float z = row * CellHeight;
                MazeCell cell = mMazeGenerator.GetMazeCell(row, column);
                GameObject tmp;
                tmp = Instantiate(Floor, new Vector3(x, 0, z), Quaternion.Euler(0, 0, 0)) as GameObject;
                tmp.transform.parent = transform;
                if (cell.WallRight)
                {
                    tmp = Instantiate(Wall, new Vector3(x + CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 90, 0)) as GameObject;// right
                    tmp.transform.parent = transform;
                }
                if (cell.WallFront)
                {
                    tmp = Instantiate(Wall, new Vector3(x, 0, z + CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 0, 0)) as GameObject;// front
                    tmp.transform.parent = transform;
                }
                if (cell.WallLeft)
                {
                    tmp = Instantiate(Wall, new Vector3(x - CellWidth / 2, 0, z) + Wall.transform.position, Quaternion.Euler(0, 270, 0)) as GameObject;// left
                    tmp.transform.parent = transform;
                }
                if (cell.WallBack)
                {
                    tmp = Instantiate(Wall, new Vector3(x, 0, z - CellHeight / 2) + Wall.transform.position, Quaternion.Euler(0, 180, 0)) as GameObject;// back
                    tmp.transform.parent = transform;
                }
                if (cell.IsGoal && goals.Contains(cell) && GoalPrefab != null && row != 0 && column != 0)
                {
                    var rotation = 0;
                    if (!cell.WallRight)
                    {
                        rotation = 90;
                    }
                    else if (!cell.WallBack)
                    {
                        rotation = 180;
                    }
                    else if (!cell.WallLeft)
                    {
                        rotation = 270;
                    }
                    tmp = Instantiate(GoalPrefab, new Vector3(x, 0, z), Quaternion.Euler(0, rotation, 0)) as GameObject;
                    tmp.transform.parent = transform;
                    Game.AddCoinToTotal();
                }
            }
        }
        if (Pillar != null)
        {
            for (int row = 0; row < Rows + 1; row++)
            {
                for (int column = 0; column < Columns + 1; column++)
                {
                    float x = column * CellWidth;
                    float z = row * CellHeight;
                    GameObject tmp = Instantiate(Pillar, new Vector3(x - CellWidth / 2, 0, z - CellHeight / 2), Quaternion.identity) as GameObject;
                    tmp.transform.parent = transform;
                }
            }
        }

        // Build surface mesh
        var surface = GetComponent<NavMeshSurface>();
        surface.BuildNavMesh();

        // Spawn enemies
        for (int count = 0; count < enemyCount; count++)
        {
            var row = Random.Range(0, Rows);
            var column = Random.Range(0, Columns);
            Instantiate(enemyPrefab, new Vector3(row * CellWidth, 0, column * CellHeight), Quaternion.identity);
        }

        Game.OnMazeDone();
    }
}
