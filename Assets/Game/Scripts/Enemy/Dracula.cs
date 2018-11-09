using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dracula : Enemy, IDamageable, IPooledObject //use many different interfaces is allowed by seperating it by comma
{
    public int Health { get; set; }

    public int NextWave { get; set; }

    //use for initialization
    //override is use to take control method from parent class
    //the method will run in this script rather than parent class script
    public override void Init()
    {
        base.Init();
        Health = base.health;
        NextWave = wavespawner.nextWave;
    }

    public override void Movement()
    {
        base.Movement();
    }

    public void Damage()
    {
        //substact 1 from health
        if (player._startPowerUp)
        {
            Health -= 6;
        }

        Health -= 3;
        anim.SetTrigger("Hit");
        player.RegenerateHealth();
        //if health less than 1
        //destroy the object
        if (Health < 1)
        {
            player.PowerUp();
            isDead = true;
            this.gameObject.SetActive(false);
        }
    }
}
