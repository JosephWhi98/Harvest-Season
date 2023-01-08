using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunAmmoInteractableItem : InteractableItem
{
    public AudioSource source;
    public AudioClip pickupClip;
    public int ammoAmount = 4; 
    public override void OnUse()
    {
        base.OnUse(); 
        source.PlayOneShot(pickupClip);

        GameManager.Instance.playerController.gun.totalAmmo += ammoAmount;

        gameObject.SetActive(false);
    }

    public void OnValidate()
    {
        Vector3 euler = transform.eulerAngles;
        euler.y = Random.Range(0f, 360f);
        transform.eulerAngles = euler; 
    }
}
