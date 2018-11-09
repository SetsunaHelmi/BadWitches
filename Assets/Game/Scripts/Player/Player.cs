using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IDamageable
{
    //get handle to rigidbody
    private Rigidbody2D _rigid;

    private bool _grounded = false;
    public bool _startWave = false;
    public bool _startPowerUp = false;

    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField]
    private int health;

    //PlayerAnimation handler
    private PlayerAnimation _playerAnim;
    private SpriteRenderer _playerSprite;

    public int Health { get; set; }

    public float CurrentHealth { get; set; }
    public float MaxHealth { get; set; }

    public float CurrentPower { get; set; }
    public float MinPower { get; set; }

    public Slider healthBar;
    public Slider PowerBar;

    [SerializeField]
    private Transform pointA;

    private UIManager uiManager;

    private void Start()
    {
        //assign handle rigidbody
        _rigid = GetComponent<Rigidbody2D>();
        //assign PlayerAnimation handler
        _playerAnim = GetComponent<PlayerAnimation>();
        _playerSprite = GetComponentInChildren<SpriteRenderer>();
        uiManager = FindObjectOfType<UIManager>();

        MaxHealth = 100f;
        CurrentHealth = MaxHealth;

        MinPower = 0f;
        CurrentPower = MinPower;

        healthBar.value = CalculateHealth();
        PowerBar.value = CalculatePower();
    }

    private void Update()
    {
        if (!uiManager._isGameStarted)
        {
            return;
        }

        if (!_startWave)
        {
            float distance = Vector3.Distance(transform.localPosition, pointA.transform.localPosition);

            if (distance > 2f)
            {
                transform.position = Vector3.MoveTowards(transform.position, pointA.transform.position, _speed * Time.deltaTime);
            }

            else if (distance < 2f)
            {
                _startWave = true;
                _playerAnim.Idle();
            }

            return;
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                _playerSprite.flipX = false;
                _playerAnim.AttackRight();
                FindObjectOfType<AudioManager>().PlayOneShot("GirlAttack");
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                _playerSprite.flipX = true;
                _playerAnim.AttackLeft();
                FindObjectOfType<AudioManager>().PlayOneShot("GirlAttack");
            }
        }
    }

    private void Movement()
    {
        _grounded = isGrounded();

        //current velocity = new velocity (horizontal input, current velocity.y)
        //_rigid.velocity = new Vector2(move * _speed, _rigid.velocity.y);

        //Run animation
        //_playerAnim.Move(move);
    }

    bool isGrounded()
    {
        RaycastHit2D hitInfo =  Physics2D.Raycast(transform.position, Vector2.down, 1f, 1 << 8);
        Debug.DrawRay(transform.position, Vector2.down, Color.green);

        return false;
    }

    public void Damage()
    {
        if (_startPowerUp)
        {
            return;
        }

        CurrentHealth -= 2;

        healthBar.value = CalculateHealth();

        if (CurrentHealth >= 120)
        {
            CurrentHealth = 120f;
        }

        else if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
            uiManager.Death();
        }
    }

    private IEnumerator PowerUpCouroutine()
    {
        yield return new WaitForSeconds(5f);
        _startPowerUp = false;
        transform.GetChild(0).localScale = new Vector3(5f, 5f, 5f);
        transform.GetChild(0).GetChild(0).localScale = new Vector3(0.2f, 0.2f, 0.2f);
    }

    public void PowerUp()
    {
        CurrentPower += 10;

        PowerBar.value = CalculatePower();

        if (CurrentPower >= 99)
        {
            CurrentPower = 0;

            _startPowerUp = true;

            FindObjectOfType<AudioManager>().PlayOneShot("Evolve");

            CurrentHealth += 30;
            healthBar.value = CalculateHealth();

            int random = Random.Range(0, 2);

            if (_startPowerUp)
            {
                StartCoroutine(PowerUpCouroutine());

                switch (random)
                {
                    case 0:
                        transform.GetChild(0).localScale *= Random.Range(2, 5);
                        transform.GetChild(0).GetChild(0).localScale *= 2.5f;
                        break;
                    case 1:
                        transform.GetChild(0).localScale /= Random.Range(2, 5);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    public void RegenerateHealth()
    {
        CurrentHealth += 1;

        healthBar.value = CalculateHealth();
    }

    public float CalculateHealth()
    {
        return CurrentHealth / MaxHealth;
    }

    public float CalculatePower()
    {
        return CurrentPower / 100f;
    }
}