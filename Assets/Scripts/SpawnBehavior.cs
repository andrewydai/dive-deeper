using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBehavior : MonoBehaviour
{
    public GameObject[] spawnables; // various spawnables later. maybe in an object 
    public float spawnableSpeed = 1;
    public float spawnRate = 1;
    public int minX = -5;
    public int maxX = 5;
    private float counter;
    private GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        this.counter = 0;
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!manager.isGameOver)
        {
            if (this.counter >= 1 / spawnRate)
            {
                SpawnObjects();
                this.counter = Time.deltaTime;
            }
            else
            {
                this.counter += Time.deltaTime;
            }
        }
    }

    void SpawnObjects()
    {
        /*
         every object will have a spawn density, which reprsents how many of them should be spawned on a line
         We have about 20 positions available to place these objects.
         */
        List<float> spawnPointsX = new List<float>();
        for(int j = this.minX; j < this.maxX + 1; j++)
        {
            spawnPointsX.Add(j);
        }

        foreach (GameObject spawnable in spawnables)
        {
            for(int i=0; i < spawnable.GetComponent<SpawnableBehavior>().spawnDensity; i++)
            {
                int idxToRemove = Random.Range(0, spawnPointsX.Count);
                float spawnPointX = spawnPointsX[idxToRemove];
                spawnPointsX.RemoveAt(idxToRemove);
                Vector3 spawnPoint = new Vector3(spawnPointX, transform.position.y);
                GameObject spawned = Instantiate(spawnable, spawnPoint, transform.rotation);
                spawned.GetComponent<MoveUpwards>().SetSpeed(this.spawnableSpeed);
            }
        }
    }

    public void ResetSpawner()
    {
        counter = 0;
    }
}
