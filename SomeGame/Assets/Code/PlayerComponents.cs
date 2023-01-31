using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal abstract class PlayerComponents : MonoBehaviour
{
    #region Unity Components
    internal Rigidbody2D rigidBody;
    internal Collider2D collider2D;
    internal Animator animator;
    //protected PlayerMovement playerMovement;
    //protected PlayerHealth playerHealth;
    internal SpriteRenderer playerSprite;
    #endregion

    internal LayerMask groundLayer;
    internal PlayerState state = PlayerState.idle;

    // Start is called before the first frame update
    internal virtual void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        collider2D = GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();
        //playerMovement = GetComponent<PlayerMovement>();
        groundLayer = LayerMask.GetMask("GroundLayer");
        playerSprite = GetComponent<SpriteRenderer>();
        //playerHealth = GetComponent<PlayerHealth>();
    }
}
