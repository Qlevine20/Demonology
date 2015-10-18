﻿using UnityEngine;
using System.Collections;

public class CharacterMove : MonoBehaviour {
    //character movement speed
    public int speed = 5;

    //is the player touching the ground
    private bool isGrounded = false;

    //player rigidbody
    private Rigidbody2D rb;

    //player collider2D
    private BoxCollider2D bc;

    //player heights
    private float crouchHeight = 1;
    private float standHeight = 2;
    private float heightChange;

    //is the character currently crouched
    private bool isCrouched = false;


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<Collider2D>() as BoxCollider2D;
        heightChange = (crouchHeight / standHeight) * bc.size.y;
    }
	
	// Update is called once per frame
	void Update () {

        //character movement with wasd
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector2.right * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }

        if (Input.GetKeyUp(KeyCode.W) && isGrounded)
        {

            //Force added for up direction
            rb.AddForce(new Vector2(0, speed), ForceMode2D.Impulse);
            isGrounded = false;
        }

        if (Input.GetKeyDown(KeyCode.S) && !isCrouched)
        {
            //change the size and offset of the collider2D
            bc.size = new Vector2(bc.size.x, heightChange);
            bc.offset = new Vector2(bc.offset.x, bc.offset.y - (heightChange/2));
            isCrouched = true;

        }
        if (Input.GetKeyUp(KeyCode.S) && isCrouched)
        {
            bc.size = new Vector2(bc.size.x, (standHeight / crouchHeight) * bc.size.y);
            bc.offset = new Vector2(bc.offset.x, bc.offset.y + (heightChange/2));
            isCrouched = false;

        }

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "floor")
        {
            //Check to see if touching the floor
            isGrounded = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "floor")
        {
            isGrounded = false;
        }
    }
}
