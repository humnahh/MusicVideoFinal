using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorShower : MonoBehaviour
{
    GameObject[] spheres;
    static int numSphere = 500; 
    float time = 0f;
    Vector3[] initPos;
    Vector3[] startPositions = new Vector3[numSphere];
    Vector3[] endPositions = new Vector3[numSphere];
    float[] lerpTimes = new float[numSphere]; // Track individual lerp progress
    float t;

    // Start is called before the first frame update
    void Start()
    {
        // Assign proper types and sizes to the variables.
        spheres = new GameObject[numSphere];
        initPos = new Vector3[numSphere]; // Start positions
        
        // Define target positions. Start = random, End = heart 
        for (int i =0; i < numSphere; i++){
            // Random off-screen start (top-left)
            float startX = Random.Range(-15f, -5f);
            float startY = Random.Range(8f, 15f);
            float startZ = Random.Range(-2f, 2f); // Add slight depth variation

            // Random target position (bottom-right)
            float endX = Random.Range(5f, 15f);
            float endY = Random.Range(-8f, -15f);
            float endZ = Random.Range(-2f, 2f); // Keep depth consistent

            startPositions[i] = new Vector3(startX, startY, startZ);
            endPositions[i] = new Vector3(endX, endY, endZ);
        }
        // Let there be spheres..
        for (int i =0; i < numSphere; i++){
            float r = 10f; // radius of the circle
            // Draw primitive elements:
            // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/GameObject.CreatePrimitive.html
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere); 

            // Position
            initPos[i] = startPositions[i];
            spheres[i].transform.position = initPos[i];

            // Color
            // Get the renderer of the spheres and assign colors.
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            // HSV color space: https://en.wikipedia.org/wiki/HSL_and_HSV
            float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(hue, 4f, 4f); // Full saturation and brightness
            sphereRenderer.material.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Measure Time 
        time += Time.deltaTime; // Time.deltaTime = The interval in seconds from the last frame to the current one
        // what to update over time?
        for (int i =0; i < numSphere; i++) {
            lerpTimes[i] = Mathf.Sin(time) * 0.5f + 0.5f;

            // Lerp logic. Update position       
            t = i* 2 * Mathf.PI / numSphere;
            spheres[i].transform.position = Vector3.Lerp(startPositions[i], endPositions[i], lerpTimes[i]);
            // For now, start positions and end positions are fixed. But what if you change it over time?
            // startPosition[i]; endPosition[i];

            if (lerpTimes[i] >= 1f)
            {
                Destroy(spheres[i]);
            }

            // Color Update over time
            Renderer sphereRenderer = spheres[i].GetComponent<Renderer>();
            float hue = (float)i / numSphere; // Hue cycles through 0 to 1
            Color color = Color.HSVToRGB(Mathf.Abs(hue * Mathf.Sin(time)), Mathf.Cos(time), 2f + Mathf.Cos(time)); // Full saturation and brightness
            sphereRenderer.material.color = color;
        }
    }
}
