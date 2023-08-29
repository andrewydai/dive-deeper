using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool isGameOver;
    public Canvas endGameCanvas;
    public Text gameOverText;
    public Text gameOverDepthText;
    public Button restartButton;

    private PlayerStatus ps;
    private PlayerMovement pm;
    private SpawnBehavior spawner;

    void Start()
    {
        isGameOver = true;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        ps = player.GetComponent<PlayerStatus>();
        pm = player.GetComponent<PlayerMovement>();
        spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<SpawnBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        // reset player status
        ps.ResetStatus();
        // reset spawner status
        pm.ResetMovement();
        // reset player position and velocity
        spawner.ResetSpawner();
        // remove all objects
        foreach (GameObject obs in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Destroy(obs);
        }


        isGameOver = false;
        endGameCanvas.enabled = false;
    }

    public void EndGame()
    {
        isGameOver = true;
        gameOverText.text = "Game Over!";
        float endDepth = ps.depth;
        gameOverDepthText.text = $"You reached a depth of {endDepth.ToString("0")} feet";
        endGameCanvas.enabled = true;
    }
}
