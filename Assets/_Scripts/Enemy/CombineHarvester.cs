using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using UnityEngine.AI;

public class CombineHarvester : MonoBehaviour
{ 
    [Range(0,1)]public float enginePower;
    [Range(0, 1)] public float bladePower;

    public GameObject lights;

    public AudioSource oneShotSource; 
    public AudioClip engineStartupClip;
    public AudioClip bladeStartupClip; 
    public AudioClip lightsOnClip; 
    public AudioClip harvesterDestroyClip;  

    public AudioClip enigneClip;
    public AudioSource engineSource;
    public Transform bodyTransform; 

    public AudioClip bladesClip;
    public AudioSource bladeSource;
    public Transform bladeTransform; 
    public float bladeSpeed;


    [Header("AI")]
    public NavMeshAgent navAgent; 
    public Transform target;
    public float moveSpeed = 0f;

    public Collider killTrigger;

    public float lastCornDestroyTime = 0f;
    public ParticleSystem cornParticleSystem;

    public float health = 100;

    public AudioClip[] damageClips;

    public ParticleSystem sparks;
    public ParticleSystem lightSmoke;
    public ParticleSystem damageFire;
    public ParticleSystem fullFire;
    public AudioSource burningSource;
    public AudioClip lowExplosion;

    public float timeOfDeath; 

    public void Awake()
    {
        bladeSource.volume = 0f;
        bladeSource.clip = bladesClip;
        bladeSource.loop = true; 
        bladeSource.Play();
         
        engineSource.volume = 0f;
        engineSource.clip = enigneClip;
        engineSource.loop = true; 
        engineSource.Play();
    }

    public void Update()
    { 
        lights.SetActive(enginePower > 0);

        engineSource.volume = enginePower;
        bladeSource.volume = bladePower;  

        bladeTransform.Rotate(-Vector3.forward, bladePower * bladeSpeed * Time.deltaTime);
         
        Vector3 bodyPosition = bodyTransform.localPosition;
        bodyPosition.y = Mathf.PingPong(Time.time * 5f, 1f) * 0.02f * enginePower;
        bodyTransform.localPosition = bodyPosition;

        if (target != null)
        {
            navAgent.SetDestination(target.position);

            float direction = TargetDirectionCheck();

            Debug.Log(direction);

            if (direction < 0.9f && enginePower > 0f)
            {
                navAgent.speed = 0.1f;
            }
            else
            {
                navAgent.speed = enginePower * moveSpeed;
            }

            if (Time.time > lastCornDestroyTime)
            {
                if (cornParticleSystem.isPlaying)
                    cornParticleSystem.Stop();
            } 
            else if (!cornParticleSystem.isPlaying)
            { 
                cornParticleSystem.Play();
            }
        }
    }

    [Button]
    public void DebugStartParticle()
    {
        PlayClipOneShot(harvesterDestroyClip, 0.7f);
        lastCornDestroyTime = Time.time + 3f;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Destructable") && enginePower > 0 && bladePower > 0)
        {
            //TEMP  
            other.gameObject.SetActive(false);
            PlayClipOneShot(harvesterDestroyClip, 0.7f);
            lastCornDestroyTime = Time.time + 3f;
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Player") && enginePower > 0 && bladePower > 0)
        {
            if (!GameManager.Instance.GameOver) 
            { 
                GameManager.Instance.KillPlayer();
                moveSpeed = 0f;
            }
        }
    }

    public void PlayClipOneShot(AudioClip clip, float volume)
    {
        oneShotSource.volume = volume; 
        oneShotSource.PlayOneShot(clip);
    }

    [Button] 
    public void StartEngine()
    {
        StartCoroutine(StartEngineRoutine());
    }

    [Button]
    public void StartBlades()
    {
        StartCoroutine(StartBladesRoutine());
    }

    public IEnumerator StartEngineRoutine()
    {
        PlayClipOneShot(engineStartupClip, 0.7f);

        yield return new WaitForSeconds(2f);

        float t = 0;

        PlayClipOneShot(lightsOnClip, 0.5f);

        while (t < 1.5f)
        {
            t += Time.deltaTime;
            enginePower = Mathf.Lerp(0, 0.8f, t/1.5f); 
            yield return null;
        }

        StartBlades();
    }

    public IEnumerator StartBladesRoutine()
    {
        PlayClipOneShot(bladeStartupClip, 0.7f);

        yield return new WaitForSeconds(0.5f);

        float t = 0;
        while (t < 4f)
        {
            t += Time.deltaTime; 
            bladePower = Mathf.Lerp(0, 1f, t /4f);
            yield return null;
        }
    }

    public void PowerDown()
    {
        StartCoroutine(PowerDownRoutine());
    }

    public IEnumerator PowerDownRoutine()
    {
        float startEnginePower = enginePower;
        float startBladePower = bladePower;

        float t = 0;
        while (t < 2f) 
        { 
            t += Time.deltaTime; 
            bladePower = Mathf.Lerp(startBladePower, 0, t / 2f);
            enginePower = Mathf.Lerp(startEnginePower, 0, t / 2f); 
            yield return null;
        }
    }

    public void DealDamage(float damage)
    {
        if (health > 0)
        {
            if (damage > 2f)
            {
                health -= damage;

                sparks.Play();
                PlayClipOneShot(damageClips[Random.Range(0, damageClips.Length)], 1f);
            }

            //TODO - hit feedback.

            health = Mathf.Clamp(health, 0, int.MaxValue);

            if (health <= 0)
            {
                KillHarvester();
            }
            else if (health < 30)
            {
                damageFire.Play();
                burningSource.volume = 0.3f;
            }
            else if (health < 60) 
            {
                lightSmoke.Play();
            } 
        }
    }
     
    public void KillHarvester() 
    { 
        PowerDown();
        PlayClipOneShot(lowExplosion, 0.7f);
        burningSource.volume = 0.6f; 
        lightSmoke.Stop();
        damageFire.Stop();
        fullFire.Play();
        timeOfDeath = Time.time;
        //TODO - full kill routine.
    }

    public float TargetDirectionCheck() 
    { 
        if (target == null)
            return 0;
         

        Vector3 forward = transform.forward;

        Vector3 targetPosition = navAgent.path.corners[1];  

        targetPosition.y = transform.position.y;

        Vector3 toOther = targetPosition - transform.position;

        Debug.Log(toOther);

        return Vector3.Dot(forward, toOther.normalized);  
    }

    Vector3 FindPointAlongPath(Vector3[] path, float distanceToTravel)
    {
        if (distanceToTravel < 0)
        {
            return path[0];
        }

        //Loop Through Each Corner in Path
        for (int i = 0; i < path.Length - 1; i++)
        {
            //If the distance between the next to points is less than the distance you have left to travel
            if (distanceToTravel <= Vector3.Distance(path[i], path[i + 1]))
            {
                //Calculate the point that is the correct distance between the two points and return it
                Vector3 directionToTravel = path[i + 1] - path[i];
                directionToTravel.Normalize();
                return (path[i] + (directionToTravel * distanceToTravel));
            }
            else
            {
                //otherwise subtract the distance between those 2 points from the distance left to travel
                distanceToTravel -= Vector3.Distance(path[i], path[i + 1]);
            }
        }

        //if the distance to travel is greater than the distance of the path, return the final point
        return path[path.Length - 1];
    }

#if UNITY_EDITOR

    public void OnDrawGizmos() 
    {
        if (navAgent.path != null && navAgent.path.corners.Length > 1)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(navAgent.path.corners[1], 1f);
        }
    }

#endif
}
