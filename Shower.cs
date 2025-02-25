using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shower : MonoBehaviour
{
    GameObject[] spheres;
    static int numSphere = 20;
    Vector3[] initPos = new Vector3[numSphere];
    Vector3[] startPosition = new Vector3[numSphere];
    Vector3[] endPosition = new Vector3[numSphere];
    Vector3[] startRandom = new Vector3[numSphere];
    Vector3[] endRandom = new Vector3[numSphere];
    float[] lerpTimes = new float[numSphere];
    float lerpSpeeds;
    float time = 0f;
    float lerpFraction; 
    bool spawn = false;

    float elapsedTime = 0f;  // Tracks time since start
    bool isSlowing = false;  // Toggles animation state
    float slowTimer = 0f;

    bool isSecondAnimation = false;

    float[] lerpCooldowns = new float[numSphere];  // Holds the cooldown time for each sphere
    float lerpDelayMin = 1f;  // Minimum delay in seconds
    float lerpDelayMax = 10f;  // Maximum delay in seconds
    float myDelta = 0f;

    void Start() {
        Invoke("Spawn", 37f);
    }

    // Start is called before the first frame update
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
            startRandom[i] = new Vector3(10f * Random.Range(-1f, 1f), 10f * Random.Range(-1f, 1f),10f* Random.Range(-1f, 1f)); 
            endRandom[i] = new Vector3(10f * Random.Range(-1f, 1f),10f* Random.Range(-1f, 1f),10f* Random.Range(-1f, 1f)); 
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

        lerpSpeeds = Random.Range(0.5f, 0.8f); // Lower = slower, Higher = faster\
        spawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawn == true) {
        time += Time.deltaTime * AudioSpectrum.audioAmp; 
        elapsedTime += Time.deltaTime;

         // After 30 seconds, start slowing down
        if (elapsedTime >= 15f && !isSecondAnimation)
        {
            isSecondAnimation = true;
            isSlowing = false;
        }

        if (elapsedTime >= 10f && !isSlowing && !isSecondAnimation)
        {
            isSlowing = true;
            slowTimer = 0f;
        }
        
        for (int i = 0; i < numSphere; i++) {
            float scale = 2f + AudioSpectrum.audioAmp;

            if (Time.time >= lerpCooldowns[i])
            {

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
                } else {
                    spheres[i].transform.position = Vector3.Lerp(startPosition[i], endPosition[i], lerpTimes[i]);
                }
            } else {
                    spheres[i].transform.position = startPosition[i];
                    lerpTimes[i] = 0f;
            } 
            //blah

            }
        }
        }
    }
}
