using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnBehavior : MonoBehaviour
{
    // public vars
    public GameObject[] spawnables;
    public float spawnableSpeed = 1;
    public float clumpSpawnRate = 1;
    public float fragmentSpawnRate = 1;
    public float collectibleSpawnRate = 1;

    // counters
    private float clumpCounter;
    private float fragmentCounter;
    private float collectibleCounter;

    private List<GameObject> clumpables;
    private List<GameObject> fragmentables;
    private List<GameObject> collectibles;

    // represents a column for spawning. The first two values
    // are the range the column represents, the int value
    // is how long ago in intervals it was used.
    private (int, int, int)[] columns;

    // manager
    private GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        this.clumpCounter = 0;
        this.fragmentCounter = 0;
        this.collectibleCounter = 0;

        this.clumpables = new List<GameObject>();
        this.fragmentables = new List<GameObject>();
        this.collectibles = new List<GameObject>();

        foreach (GameObject spawnable in spawnables)
        {
            SpawnableBehavior sb = spawnable.GetComponent<SpawnableBehavior>();
            if (sb.isClumpable)
            {
                this.clumpables.Add(spawnable);
            }
            if (sb.isFragmentable)
            {
                this.fragmentables.Add(spawnable);
            }
            if (sb.isCollectible)
            {
                this.collectibles.Add(spawnable);
            }
        }

        // hard coded to 5 col, 4 wide each
        columns = new (int, int, int)[5];
        int currentLeftIndex = -10;
        for (int i = 0; i < columns.Length; i++)
        {
            (int, int, int) column = (currentLeftIndex, currentLeftIndex + 4, 0);
            columns[i] = column;
            currentLeftIndex += 4;
        }

        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!manager.isGameOver)
        {
            HandleClumpCounter();
            HandleFragmentCounter();
            HandleCollectibleCounter();
        }
    }

    public void HandleClumpCounter()
    {
        if (clumpCounter >= 1 / clumpSpawnRate)
        {
            clumpCounter = Time.deltaTime;
            SpawnClump();
        }
        else
        {
            clumpCounter += Time.deltaTime;
        }
    }

    public void SpawnClump()
    {
        // get a random clumpable
        GameObject spawnable = this.clumpables[Random.Range(0, this.clumpables.Count)];
        // from the list of available columns, get the available columns
        (int, int, int)[] availableColumns = this.columns.Where(columns => columns.Item3 == 0).ToArray();
        // choose one randomly
        (int, int, int) spawnColumn = availableColumns[Random.Range(0, availableColumns.Length)];

        // then, spawn a random clump in it (maybe just test 3 deep)
        for (int i = 0; i < 3; i++)
        {
            List<int> positions = Enumerable.Range(spawnColumn.Item1, spawnColumn.Item2 - spawnColumn.Item1).ToList();
            int posToFill = spawnColumn.Item2 - spawnColumn.Item1 - 1;
            for (int j = 0; j < posToFill; j++)
            {
                int spawnPoint = positions[Random.Range(0, positions.Count)];
                positions.Remove(spawnPoint);
                SpawnObject(spawnable, spawnPoint, transform.position.y - i);
            }
        }

        // then increment the other columns if they were used, and set them to 0 if they reach some
        // value, say 3. Has to be less than the number of columns
        for (int i = 0; i < columns.Length; i++)
        {
            (int, int, int)  column = columns[i];
            if (column.Item3 > 0)
            {
                column.Item3 += 1;
                if (column.Item3 == 3)
                {
                    column.Item3 = 0;
                }
            }
            else if (column.Item1 == spawnColumn.Item1)
            {
                column.Item3 = 1;
            }

            columns[i] = column;
        }
    }

    public void HandleFragmentCounter()
    {
        if (fragmentCounter >= 1 / fragmentSpawnRate)
        {
            fragmentCounter = Time.deltaTime;
            SpawnFragment();
        }
        else
        {
            fragmentCounter += Time.deltaTime;
        }
    }

    public void SpawnFragment()
    {
        GameObject spawnable = this.fragmentables[Random.Range(0, this.fragmentables.Count)];
        (int, int, int)[] availableColumns = this.columns.Where(columns => columns.Item3 == 0).ToArray();
        // choose one randomly
        (int, int, int) spawnColumn = availableColumns[Random.Range(0, availableColumns.Length)];

        List<int> positions = Enumerable.Range(spawnColumn.Item1, spawnColumn.Item2 - spawnColumn.Item1).ToList();
        int spawnPoint = positions[Random.Range(0, positions.Count)];
        SpawnObject(spawnable, spawnPoint);
    }

    public void HandleCollectibleCounter()
    {
        if (collectibleCounter >= 1 / collectibleSpawnRate)
        {
            collectibleCounter = Time.deltaTime;
            SpawnCollectible();
        }
        else
        {
            collectibleCounter += Time.deltaTime;
        }
    }

    public void SpawnCollectible()
    {

    }


    private void SpawnObject(GameObject spawnable, float x, float y = -100)
    {
        if (y == -100)
        {
            y = transform.position.y;
        }
        Vector3 spawnPoint = new Vector3(x, y);
        GameObject spawned = Instantiate(spawnable, spawnPoint, transform.rotation);
        spawned.GetComponent<MoveUpwards>().SetSpeed(this.spawnableSpeed);
    }

    public void ResetSpawner()
    {
        this.clumpCounter = 0;
        this.fragmentCounter = 0;
        this.collectibleCounter = 0;
    }
}
