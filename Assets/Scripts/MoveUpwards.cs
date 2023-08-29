using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpwards : MonoBehaviour
{
    public float maxHeight;
    private float speed = 0;
    private Vector2 destination;
    private GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        this.destination = new Vector2(transform.position.x, maxHeight);
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!manager.isGameOver)
        {
            if (transform.position.y < maxHeight)
            {
                float step = this.speed * Time.deltaTime;
                transform.position = Vector2.MoveTowards(transform.position, this.destination, step);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
}
