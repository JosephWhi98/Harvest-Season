using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class CameraInteractions : MonoBehaviour
{
    public bool lockCursor = true; 

    public LayerMask interactableMask;

    private InteractableItem cachedInteractable;
    public TMP_Text InteractText; 

    void Update()
    { 
        if (lockCursor && GameManager.Instance.AllowInput && !GameManager.Instance.GameOver)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true;
        }

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 6f, interactableMask))
        {
            cachedInteractable = hit.transform.GetComponent<InteractableItem>();

            if (cachedInteractable.interactable == false)
                cachedInteractable = null; 
        }
        else
        {
            cachedInteractable = null; 
        }

        if(cachedInteractable)
            InteractText.text = "['E' TO " +  cachedInteractable.interactionName + "]";

        InteractText.gameObject.SetActive(cachedInteractable != null);
    }

    public void TryInteract()
    {
        if (cachedInteractable)
        {
            cachedInteractable.OnUse();
        }
    }
}
