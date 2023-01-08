using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [SerializeField]
    private Light light;
    private float startIntensity; 

    [SerializeField]
    public float minIntensity = 0.1f;
    [SerializeField]
    public float maxIntensity = 1f;
    [SerializeField]
    public int smoothing = 100;

    Queue<float> smoothQueue; 
    float lastSum = 0;


    public Renderer renderer;

    private MaterialPropertyBlock propBlock;

    public bool flickering;

    public AudioSource source;
    public AudioClip flickerOut; 

    public bool on
    {
        get
        {
            return light.enabled;
        }
        set
        {
            light.enabled = value;
            light.intensity = startIntensity;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        smoothQueue = new Queue<float>(smoothing);
        propBlock = new MaterialPropertyBlock();

        startIntensity = light.intensity;
    }

    // Update is called once per frame
    void Update()
    {
        if (flickering)
        {
            // pop off an item if too big
            while (smoothQueue.Count >= smoothing)
            {
                lastSum -= smoothQueue.Dequeue();
            }

            // Generate random new item, calculate new average
            float newVal = Random.Range(minIntensity, maxIntensity);
            smoothQueue.Enqueue(newVal);
            lastSum += newVal;

            // Calculate new smoothed average
            light.intensity = lastSum / (float)smoothQueue.Count;
        }

        renderer.GetPropertyBlock(propBlock); 
        // Assign our new value.
        propBlock.SetColor("_EmissionColor", Color.Lerp(Color.white, Color.black, light.enabled ? Mathf.Clamp01(1 - light.intensity) : 1));
        // Apply the edited values to the renderer.
        renderer.SetPropertyBlock(propBlock);
    }

}