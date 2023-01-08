using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunInteractableItem : InteractableItem
{
    public AudioSource source;
    public AudioClip pickupClip;

    public override void OnUse()
    {
        base.OnUse();
        source.PlayOneShot(pickupClip);  
         
        GameManager.Instance.playerController.PickupShotgun();

        gameObject.SetActive(false);
    }
}
