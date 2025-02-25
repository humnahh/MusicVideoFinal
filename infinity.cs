using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfinityShape : MonoBehaviour
{
    GameObject[] spheres;
    static int numSphere = 50;
    float baseScale = 5f; 
    float baseSpeed = 2f; 
    float time = 0f;
    bool spawned = false;
    float elapsedTime = 0f;

    float[] offsets;

    void Start() {
        Invoke("Spawn", 30f);
    }

    void Spawn()
    {
        float time = 0f;
        spheres = new GameObject[numSphere];
        offsets = new float[numSphere];

        for (int i = 0; i < numSphere; i++)
        {
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            
            offsets[i] = i * (Mathf.PI * 2f / numSphere);

            spheres[i].transform.position = Vector3.zero;

            // Assign colors
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            Color color = Color.HSVToRGB(0.98f, 1f, 10f); 
            sphereRenderer.material.color = color;
        }
        spawned = true;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        time += Time.deltaTime * AudioSpectrum.audioAmp; 

        if (elapsedTime <= 39) {
            if (spawned == true) {
                for (int i = 0; i < numSphere; i++)
                {
                    float t = time - offsets[i]; 

                    float x = Mathf.Sin(t) * 6f;
                    float y = Mathf.Sin(t) * Mathf.Cos(t) * 6f;
                    float z = 5f;

                    spheres[i].transform.position = new Vector3(x, y, z);

                    // Adjust sphere size based on audio
                    float scale = 0.1f + AudioSpectrum.audioAmp;
                    spheres[i].transform.localScale = new Vector3(scale, scale, scale);
                }
            }
        } else if (elapsedTime > 39 && elapsedTime <= 40) {
            for (int i =0; i < numSphere; i++) {
                Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();

                float hue = 0f; // Keep hue at red
                float brightness = Mathf.Abs(Mathf.Cos(time)) * 0.5f + 0.5f; // Cycles between 0.5 and 1

                Color color = Color.HSVToRGB(hue, 20f, 20f);
                sphereRenderer.material.color = color;
            }
        } else {
            Debug.Log("Destroying all spheres!");
    
            for (int i = 0; i < numSphere; i++)
            {
                if (spheres[i] != null) // Check if the object exists
                {
                    Destroy(spheres[i]);
                }
            }
        }
    }
}