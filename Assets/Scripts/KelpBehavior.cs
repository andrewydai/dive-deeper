using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KelpBehavior : MonoBehaviour
{
    public float slowDownTime;
    public float slowDownFactor;
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
            player.GetComponent<PlayerMovement>().SlowSpeedFor(slowDownTime, slowDownFactor);
            Destroy(gameObject);
        }
    }
}
