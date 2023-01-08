using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : MonoBehaviour
{
    public PlayerController playerController;

    public Animator animator;

    public int shotsInBarrel = 2; 
    public int totalAmmo = 20;

    public bool canFire;
    public bool reloading;

    public AudioSource audioSource;
    public AudioClip fireClip;
    public AudioClip reloadClip; 

    void Update()
    { 
        animator.SetBool("IsRunning", playerController.isRunning);

        if (shotsInBarrel <= 0 && !reloading && canFire)
        {
            SetReloadingTrue();
        }
    }

    public void TryFire() 
    { 
        if (gameObject.activeInHierarchy && canFire && shotsInBarrel > 0 && !reloading)
        {
            animator.SetTrigger("Fire");
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(fireClip); 
            shotsInBarrel -= 1; 

            shotsInBarrel = Mathf.Clamp(shotsInBarrel, 0, 2);

            if (TargetDirectionCheck() >= 0.95f)  
            {
                float dist = Vector3.Distance(transform.position, GameManager.Instance.combineHarvester.transform.position);

                float minDistance = 15; 
                float maxDistance = 80;    
                 
                float damage = Mathf.Lerp(5, 0, (dist - minDistance) / (maxDistance - minDistance));

                GameManager.Instance.combineHarvester.DealDamage(damage); 
            }
        }
    }

    public float TargetDirectionCheck()
    {
        Vector3 forward = Camera.main.transform.forward;
        Vector3 targetPosition = GameManager.Instance.combineHarvester.transform.position; 

        Vector3 toOther = targetPosition - transform.position;

        return Vector3.Dot(forward, toOther.normalized);
    }


    public void SetFiringFalse()
    {
        canFire = false;
    }

    public void SetFiringTrue()
    {
        canFire = true;
    }


    public void SetReloadingTrue()
    {
        if (totalAmmo > 0)
        {
            reloading = true;
            animator.SetBool("Reload", true);  
            audioSource.pitch = 1;
            audioSource.PlayOneShot(reloadClip);
        }
    }

    public void SetReloadingFale()
    { 
        animator.SetBool("Reload", false); 
        reloading = false;
        shotsInBarrel = 2;
        totalAmmo -= 2;

        totalAmmo = Mathf.Clamp(totalAmmo, 0, int.MaxValue);
    }
}
