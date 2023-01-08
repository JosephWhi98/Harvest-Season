using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class EventManager : Singleton<EventManager>
{
    public PlayerController playerController;
    public CombineHarvester combineHarvester; 
     
    public GameObject shouldCheckBarnTrigger;
    public Collider forceStartChaseTrigger;

    private bool hasTriggeredEnding;
    private int endingTriggered = -1; 

    public void Start()
    { 
        StartCoroutine(RunRoutine());
    }


    public IEnumerator RunRoutine()
    {
        yield return null;


        while (!playerController.unlockedShotgun && !forceStartChaseTrigger.bounds.Contains(playerController.transform.position))
        {
            yield return null;  
        }

        shouldCheckBarnTrigger.gameObject.SetActive(false);
        forceStartChaseTrigger.gameObject.SetActive(false);


        combineHarvester.StartEngine();

        while (!GameManager.Instance.GameOver && combineHarvester.health > 0)
        {
            yield return null;
        }

        if (combineHarvester.health <= 0 && !GameManager.Instance.GameOver)
        {
            while (Time.time < combineHarvester.timeOfDeath + 7f)
            {
                yield return null; 
            }

            EndingThreeDestroy();
        } 
    }


    [Button]
    public void EndingOneNotToday() //Leave the farm before starting. 
    {
        if (!GameManager.Instance.GameOver)
        {
            endingTriggered = 1;
            hasTriggeredEnding = true;
            GameManager.Instance.TriggerEnding(1);
        } 
    }

    [Button]
    public void EndingTwoEscape() //Leave the farm at the maze exit 
    {
        if (!GameManager.Instance.GameOver)
        {
            endingTriggered = 1;
            hasTriggeredEnding = true;
            GameManager.Instance.TriggerEnding(2);
        }
    }

    [Button] 
    public void EndingThreeDestroy() //Kill the harvester
    {
        if (!GameManager.Instance.GameOver) 
        {
            endingTriggered = 1;
            hasTriggeredEnding = true;
            GameManager.Instance.TriggerEnding(3);
        }
    }

}
