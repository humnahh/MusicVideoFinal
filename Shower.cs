using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shower : MonoBehaviour
{
    // spawn in spheres
    GameObject[] spheres;
    static int numSphere = 20;

    // make all the vector positions
    Vector3[] initPos = new Vector3[numSphere];
    Vector3[] startPosition = new Vector3[numSphere];
    Vector3[] endPosition = new Vector3[numSphere];
    Vector3[] startRandom = new Vector3[numSphere];
    Vector3[] endRandom = new Vector3[numSphere];

    // times, speeds, and misc. values
    float[] lerpTimes = new float[numSphere];
    float elapsedTime = 0f;  // Tracks time since start
    float betterTime = 0f;
    float time = 0f;
    float slowTimer = 0f;

    float lerpFraction; 
    float lerpSpeeds;

    bool spawn = false;

    bool isSlowing = false; 

    bool isSecondAnimation = false;

    float[] lerpCooldowns = new float[numSphere];  // Holds the cooldown time for each sphere
    float lerpDelayMin = 1f;  // Minimum delay in seconds
    float lerpDelayMax = 6f;  // Maximum delay in seconds

    // start method
    void Start() {
        Invoke("Spawn", 38f);
    }

    // spawn method that waits 37 seconds before spawning in the spheres
    void Spawn()
    {
        spheres = new GameObject[numSphere];
        time = 0f;

        // set start and end positions
        for (int i = 0; i < numSphere; i++) {
            // Random off-screen start (top-left)
            float startX = Random.Range(-15f, -5f);
            float startY = Random.Range(8f, 15f);
            float startZ = 0; // Add slight depth variation

            float endX = (startX + 20f);
            float endY = (startY - 20f);
            float endZ = startZ; 

            startPosition[i] = new Vector3(startX, startY, startZ);
            endPosition[i] = new Vector3(endX, endY, endZ);
            startRandom[i] = new Vector3(10f * Random.Range(-1f, 1f), 10f * Random.Range(-1f, 1f),startZ); 
            endRandom[i] = new Vector3(10f * Random.Range(-1f, 1f),10f* Random.Range(-1f, 1f),endZ); 
        }

        for (int i = 0; i < numSphere; i++) {
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            spheres[i].transform.localScale = new Vector3(0.3f, 0.3f, 0.3f); 

            spheres[i].transform.position = startPosition[i];

            // set glowing white
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();

            Color color = Color.HSVToRGB(0f, 0f, 5f);
            sphereRenderer.material.color = color;

            lerpTimes[i] = 0f;
            lerpCooldowns[i] = Random.Range(lerpDelayMin, lerpDelayMax);
        }

        lerpSpeeds = Random.Range(0.8f, 1f); // Lower = slower, Higher = faster
        spawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        betterTime += Time.deltaTime;
        // checks if the spheres were spawned in
        if (spawn == true) {
            // update the time reacting to audio
            time += Time.deltaTime * AudioSpectrum.audioAmp; 
            // update the elapsed time in seconds
            elapsedTime += Time.deltaTime;

            // after 18 seconds, queue the second animation
            if (elapsedTime >= 17f && !isSecondAnimation)
            {
                isSecondAnimation = true;
                isSlowing = false;
            }

            // After 15 seconds, start slowing down
            if (elapsedTime >= 15f && !isSlowing && !isSecondAnimation)
            {
                isSlowing = true;
                slowTimer = 0f;
            }
        
            // iterate through each sphere
            for (int i = 0; i < numSphere; i++) {
                float scale = 1f + (AudioSpectrum.audioAmp * 3);

                if (Time.time >= (lerpCooldowns[i] + 37f))
                {
                    // get in here new colors
                    Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
                    Color color = Color.HSVToRGB(0f, 0f, scale);
                    sphereRenderer.material.color = color;
                
                    time += Time.deltaTime;
                    if (lerpTimes[i] < 1f) {
                        if (isSlowing) {
                            // Gradually slow down over "slowDownDuration" seconds
                            slowTimer += Time.deltaTime;
                            float slowdownFactor = Mathf.Clamp01(1f - (slowTimer / 20f));
                            float currentSpeed = lerpSpeeds * slowdownFactor; // Decrease speed over time

                            lerpTimes[i] += Time.deltaTime * currentSpeed;
                        } else {
                            lerpTimes[i] += Time.deltaTime * lerpSpeeds;
                        }

                        if (isSecondAnimation) {
                            spheres[i].transform.position = Vector3.Lerp(startRandom[i], endRandom[i], lerpTimes[i] * 2);
                            spheres[i].transform.localScale = new Vector3(1f, 1f, 1f); 

                            sphereRenderer = spheres[i].GetComponent<Renderer>();
                            float hue = (float)i / numSphere; // Hue cycles through 0 to 1
                            color = Color.HSVToRGB(Mathf.Abs(hue * Mathf.Sin(time)), Mathf.Cos(time), 2f + Mathf.Cos(time)); // Full saturation and brightness
                            sphereRenderer.material.color = color;
                        } else {
                            spheres[i].transform.position = Vector3.Lerp(startPosition[i], endPosition[i], lerpTimes[i]);
                        }
                    } else {
                        spheres[i].transform.position = startPosition[i];
                        lerpTimes[i] = 0f;
                    } 
                }
            }
        } else if (betterTime > 72f) {
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
