using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableItem : MonoBehaviour
{ 
    public string interactionName = "INTERACT";
    public string interactSubtitle;

    public bool interactable = true;

    public UnityEvent onInteractEvent;

    public virtual void OnUse()
    {
        SubtitlesManager.Instance.ShowSubtitle(interactSubtitle, 4f); 
        onInteractEvent.Invoke();
    }
}
