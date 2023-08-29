using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenBehavior : MonoBehaviour
{
    public float oxygenAmount = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            GameObject player = collider.gameObject;
            player.GetComponent<PlayerStatus>().AddOxygen(oxygenAmount);
            Destroy(gameObject);
        }
    }
}
