using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{

    public string text;
    public float duration = 5f;
    public bool ran;

    public void OnTriggerEnter(Collider other)
    {
        if (ran)
            return; 

        PlayerController controller = other.GetComponent<PlayerController>();

        if (controller) 
        {
            SubtitlesManager.Instance.ShowSubtitle(text, duration);
            ran = true;
        }
    }
}
