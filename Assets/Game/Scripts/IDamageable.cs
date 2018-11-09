using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable //polymorphism
{
    int Health { get; set; } //{get:set} is getter and setter which it is a property where it can be get and set
    void Damage();
}
