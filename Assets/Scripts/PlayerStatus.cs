using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public float maxOxygen = 100;
    public float OxygenDepletionFactor;
    public float divingRate = 1;

    public Text oxygenText;
    public Text depthText;

    private float currentOxygen;
    public float depth;
    private GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        currentOxygen = maxOxygen;
        depth = 0;
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!manager.isGameOver)
        {
            depth += divingRate * Time.deltaTime;
            depthText.text = depth.ToString("0");

            currentOxygen -= OxygenDepletionFactor * Time.deltaTime;
            if (currentOxygen <= 0)
            {
                currentOxygen = 0;
                manager.EndGame();
            }
            oxygenText.text = currentOxygen.ToString("0.00");
        }
    }

    public void AddOxygen(float amount)
    {
        if (this.currentOxygen + amount > maxOxygen)
        {
            this.currentOxygen = maxOxygen;
        }
        else
        {
            this.currentOxygen = this.currentOxygen + amount;
        }
    }

    public void ResetStatus()
    {
        currentOxygen = maxOxygen;
        depth = 0;
    }
}
