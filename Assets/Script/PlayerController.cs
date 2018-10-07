﻿using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Definition des Inputs
    [HideInInspector]
     public string Horizontal;
    [HideInInspector] public string Vertical;
    [HideInInspector] public KeyCode JumpKey;
    [HideInInspector] public KeyCode UseKey;
    [HideInInspector] public KeyCode TakeKey;
    public bool setuped = false;

    //variable pour le movement processing
    [Space]
    Rigidbody2D rb2;
    [SerializeField]
    float sensiX = 1;
    [SerializeField]
    float sensiY = 1;
    private float vertical;
    [SerializeField]
    float DownDetector = 1;
    [SerializeField]
    float echo_act_3 = 10;
    int layerMask = 1 << 9;
    private bool IsGrounded = true;
    RaycastHit2D hit;
    SpriteRenderer rend;

    //variables de mort

    [SerializeField]
    AudioClip DeathSound;
    GameObject GameManager;


    void Start()
    {
        rb2 = gameObject.GetComponent<Rigidbody2D>();
        layerMask = ~layerMask;
        rend = gameObject.GetComponent<SpriteRenderer>();
        GameManager = GameObject.FindGameObjectWithTag("GameManager");
    }


    void Update()
    {
        ShortInput();
    }
    #region
    void OnCollisionStay2D(Collision2D other)
    {
        if (other.transform != null && hit.transform != null)
        {
            if (other.transform.name == hit.transform.name)
            {
                IsGrounded = true;
            }
        }
    }

    void ShortInput()
    {
        // Si il faut rajouter du code pour différencer les manettes c'est ici
        float horizontal = Input.GetAxisRaw(Horizontal) * sensiX;
        float rawtical = Input.GetAxisRaw(Vertical) * sensiY;

        DectectInput(horizontal, rawtical);
    }

    void DectectInput(float horizontal, float rawtical)
    {
        //Ici on preprocess les inputs

        

        if (Input.GetKeyDown(JumpKey))
        {
            vertical =  sensiY;
        }
        if (Input.GetKeyUp(JumpKey))
        {
            rb2.velocity = new Vector2(rb2.velocity.x, 1 / rb2.velocity.y);
        }

       hit = Physics2D.Raycast(transform.position, Vector2.down, DownDetector, layerMask);

        Move(horizontal, vertical);

        if (IsGrounded == false)
        {
            rb2.gravityScale = echo_act_3;
            vertical = 0;
            IsGrounded = false;
        }
        else
        {
            rb2.gravityScale = 1;
        }
    }

    private void Move(float horizontal, float vertical)
    {
        if (horizontal == 0)
        {
            rb2.velocity = new Vector2(0, rb2.velocity.y);
        }


        if (horizontal > 0)
        {
            rend.flipX = true;
        }
        else if (horizontal < 0)
        {
            rend.flipX = false;
        }

        if (vertical > 0)
        {
            rend.flipY = true;
        }
        else
        {
            rend.flipY = false;
        }

        rb2.AddForce(new Vector2(horizontal, vertical) * Time.deltaTime, ForceMode2D.Impulse);
    }
    #endregion 
    public void Die()
    {
        GameManager.GetComponent<AudioSource>().clip = DeathSound;
        GameManager.GetComponent<AudioSource>().Play();
        GameManager.GetComponent<GameManager>().Respawn();
        Debug.Log("Les vivants morts: ceux qui tolèrent l'injustice");
        Destroy(gameObject);
    }
}
