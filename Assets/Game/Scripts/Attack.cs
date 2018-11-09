using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    //variable to determine if the damage function can be called
    private bool _canDamage = true;

    private void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable hit = other.GetComponent<IDamageable>();

        if (hit != null)
        {
            //if can attack 
            if (_canDamage)
            {
                hit.Damage();
                _canDamage = false;
                StartCoroutine(ResetDamageRoutine());
            }
            
            // set that variable to false
        }
    }

    //coroutine to reset variable after 0.5 seconds
    private IEnumerator ResetDamageRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        _canDamage = true;
    }
}
