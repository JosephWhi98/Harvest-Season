using UnityEngine;

public class ScaleParticle : MonoBehaviour
{
    public float maxScale = 1.0f; // maximum scale of the object
    public float minScale = 0.0f; // minimum scale of the object
    public float maxDistance = 10.0f; // maximum distance at which the object will be at its maximum scale
    public float offsetDistance = 5.0f; // offset distance at which the object will be at its minimum scale

    void Update() 
    {
        // Calculate the distance between the object and the player
        float distance = Vector3.Distance(transform.position, Camera.main.transform.position);
         
        // Calculate the scale of the object based on the distance 
        float scale = Mathf.Lerp(minScale, maxScale, (distance - offsetDistance) / (maxDistance - offsetDistance));

        // Set the object's scale
        transform.localScale = new Vector3(scale, scale, scale);
    }
}
