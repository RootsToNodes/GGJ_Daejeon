using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : NodeObject
{
    public override void OnDamage(float amount)
    {
        hp -= amount;
        // hp -= Mathf.Max(amount - node.currentStatus.defense, 0);

        if (hp < 0)
        {
            gameObject.SetActive(false);
        }
    }

    public override void OnHealing(float amount)
    {
        gameObject.SetActive(true);

        hp = amount;
    }
}
