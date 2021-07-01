using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private Collider2D coll;
    [SerializeField] private LayerMask ground;
    private enum State {idle, running, jumping, crouching, hit, falling, climbing}
    private State state = State.idle;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
    }
    private void Update()
    {

        float hDirection = Input.GetAxis("Horizontal");
        float vDirection = Input.GetAxis("Jump");

        if (hDirection < 0)
        {
            rb.velocity = new Vector2(-5, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
            
        }

        else if (hDirection > 0)
        {
            rb.velocity = new Vector2(5, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
            
        }

        else
        {
            state = State.idle;
        }

        if (vDirection > 0 && coll.IsTouchingLayers(ground))
        {
            rb.velocity = new Vector2(rb.velocity.x, 30f);
            state = State.jumping;
        }

        else if (vDirection < 0 && coll.IsTouchingLayers(ground))
        {
            state = State.crouching;
        }

        /*
        else if (vDirection == 0 && coll.IsTouchingLayers(ground))
        {
            state = State.idle;
        }
        */
        
        else
        {

        }

        VelocityState();
        anim.SetInteger("state", (int)state);
    }

    private void VelocityState()
    {
        if(state == State.jumping)
        {
            if(rb.velocity.y < 0.1f)
            {
                state = State.falling;
            }
        }
        
        else if(state == State.falling)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        
        else if(Mathf.Abs(rb.velocity.x) > 2f)
        {
            //Moving
            state = State.running;
        }

        else
        {
            state = State.idle;
        }

    }
}
