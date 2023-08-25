using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // movement parameters
    public float maxMoveSpeed;
    public float accelerationForce;
    public float dashSpeed;
    public float dashForce;
    public float dashDistance;
    
    // input parameters
    public string moveLeftKey;
    public string moveRightKey;
    public string dashLeftKey;
    public string dashRightKey;

    // components
    private Rigidbody2D rb;

    // local variables
    private Vector2 movement;
    private Vector3 dashDest;
    private bool isDashing;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isDashing = false;
    }

    // Update is called once per frame
    void Update()
    {
        /*
         The dashing function is a little janky right now, because it relies on the force to be sufficient to reach teh
        destination given the current drag. If it doesn't, it never ends. Looks fine though for a first pass.
         */

        movement = Vector2.zero;
        if (isDashing)
        {

            if (rb.velocity.magnitude < 0.5f)
            {
                Debug.Log("End dashing");
                isDashing = false;
            }
            return;
        }
        

        if (Input.GetKeyDown(dashLeftKey))
        {
            isDashing=true;
            rb.velocity = Vector2.zero;
            dashDest = transform.position + (Vector3.left * dashDistance);
            movement = Vector2.left * dashForce;
            rb.AddForce(movement, ForceMode2D.Impulse);
        }
        else if (Input.GetKeyDown(dashRightKey))
        {
            isDashing = true;
            rb.velocity = Vector2.zero;
            dashDest = transform.position + (Vector3.right * dashDistance);
            movement = Vector2.right * dashForce;
            rb.AddForce(movement, ForceMode2D.Impulse);
        }
        else if (Input.GetKey(moveLeftKey) && rb.velocity.magnitude < maxMoveSpeed)
        {
            movement = Vector2.left * accelerationForce;
            rb.AddForce(movement * Time.deltaTime);
        }
        else if (Input.GetKey(moveRightKey) && rb.velocity.magnitude < maxMoveSpeed)
        {
            movement = Vector2.right * accelerationForce;
            rb.AddForce(movement * Time.deltaTime);
        }
    }
}
