using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour, IPooledObject
{
    [SerializeField]
    protected int health;
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected int gems;

    protected Vector3 curretTarget;
    protected Animator anim;
    protected SpriteRenderer sprite;

    protected bool isDead = false;

    //variable to store the player
    protected Player player;
    protected UIManager uiManager;
    protected WaveSpawner wavespawner;

    public virtual void Init()
    {
        anim = GetComponentInChildren<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
        uiManager = FindObjectOfType<UIManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        wavespawner = FindObjectOfType<WaveSpawner>();
    }

    private void Start()
    {
        Init();
    }

    public virtual void Update()
    {
        if (!uiManager._isGameStarted)
        {
            return;
        }

        if (!isDead)
        {
            Movement();
        }
    }

    public virtual void Movement() //virtual is a method that can be used to share same logic that inherit from enemy class
    {
        //check for distance between player and enemy
        float distance = Vector3.Distance(transform.localPosition, player.transform.localPosition);

        //if greater than 2 units
        if (distance > 2.0f)
        {
            //move towards player
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

            Vector3 direction = player.transform.localPosition - transform.localPosition;

            if (direction.x > 0)
            {
                sprite.flipX = true;
            }
            else if (direction.x < 0)
            {
                sprite.flipX = false;
            }
        }
        else
        {
            if (sprite.flipX == true)
            {
                anim.SetBool("InCombatRight", true);
            }
            else if (sprite.flipX == false)
            {
                anim.SetBool("InCombatLeft", true);
            }
        }
    }

    public virtual void OnObjectSpawn()
    {
        isDead = false;
    }
}
