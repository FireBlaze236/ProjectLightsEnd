using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarPickup : BasePickup
{
    public override void Pickup(GameObject who)
    {
        base.Pickup(who);
        if (who.TryGetComponent(out PlayerStats playerStats))
        {
            GameManager.Instance.AddStar();
            this.gameObject.SetActive(false);
        }
    }
}
