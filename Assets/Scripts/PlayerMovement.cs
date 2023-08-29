using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // movement parameters
    public float maxMoveSpeed;
    private float currentMaxMoveSpeed;
    public float accelerationForce;
    public float dashForce;
    
    // input parameters
    public string moveLeftKey;
    public string moveRightKey;
    public string dashLeftKey;
    public string dashRightKey;

    // components
    private Rigidbody2D rb;

    // local variables
    private bool isDashing;
    private float atDashSpeed;

    // local vars for slowing down
    private float slowDownTime;
    private bool isSlowed;
    private float slowCounter;
    private GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        currentMaxMoveSpeed = maxMoveSpeed;
        rb = GetComponent<Rigidbody2D>();
        isDashing = false;
        manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!manager.isGameOver)
        {
            HandleSlowed();

            Vector2 movement = Vector2.zero;
            if (isDashing)
            {
                if (rb.velocity.magnitude <= atDashSpeed)
                {
                    isDashing = false;
                }
                return;
            }


            if (Input.GetKeyDown(dashLeftKey))
            {
                isDashing = true;
                atDashSpeed = rb.velocity.magnitude;
                rb.velocity = Vector2.zero;
                movement = Vector2.left * dashForce;
                rb.AddForce(movement, ForceMode2D.Impulse);
            }
            else if (Input.GetKeyDown(dashRightKey))
            {
                isDashing = true;
                atDashSpeed = rb.velocity.magnitude;
                rb.velocity = Vector2.zero;
                movement = Vector2.right * dashForce;
                rb.AddForce(movement, ForceMode2D.Impulse);
            }
            else if (Input.GetKey(moveLeftKey) && rb.velocity.magnitude < currentMaxMoveSpeed)
            {
                movement = Vector2.left * accelerationForce;
                rb.AddForce(movement * Time.deltaTime);
            }
            else if (Input.GetKey(moveRightKey) && rb.velocity.magnitude < currentMaxMoveSpeed)
            {
                movement = Vector2.right * accelerationForce;
                rb.AddForce(movement * Time.deltaTime);
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void HandleSlowed()
    {
        if (isSlowed)
        {
            if (slowCounter < slowDownTime)
            {
                slowCounter += Time.deltaTime;
            }
            else
            {
                slowCounter = 0;
                isSlowed = false;
                currentMaxMoveSpeed = maxMoveSpeed;
            }
        }
    }

    public void SlowSpeedFor(float time, float slownessFactor)
    {
        if (!isSlowed)
        {
            this.slowDownTime = time;
            currentMaxMoveSpeed = maxMoveSpeed / slownessFactor;
            isSlowed = true;
            slowCounter = 0;
        }
    }

    public void ResetMovement()
    {
        Vector3 startPos = transform.position;
        startPos.x = 0;
        transform.position = startPos;

        isSlowed = false;
        isDashing = false;
        rb.velocity = Vector2.zero;
    }
}
