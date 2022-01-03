using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Haochen Liu 
 * 260917834
 * COMP521 A2 
 * 
 * 
 * This class is used for generating the overall terrain in game.
 */
public class TerrainGenerator : MonoBehaviour
{
    //Gameobject prefabs used for terrain generation
    public GameObject point;
    public GameObject line;
    //Points of terrain
    public static List<Vector3> landscapePoints = new List<Vector3>();

    // the height of plain will generally be -8
    public int plain_height = -8;

    // The generation of 1d Perlin noise is based on the pseudo code from
    // http://www.arendpeter.com/Perlin_Noise.html

    // Octave number = 3 (sin(x),sin(x-1),sin(x+1))
    // Generate fundamental noise and smoothen it.
    float BasicSmoothNoise(float x) 
    { 
        float basic1 = Mathf.Sin(x - 1f) + Mathf.Sin(x) + Mathf.Sin(x + 1f);
        float basic2 = Mathf.Sin(x - 2f) + Mathf.Sin(x-1) + Mathf.Sin(x);
        float basic3 = Mathf.Sin(x) + Mathf.Sin(x+1) + Mathf.Sin(x + 2);
        return basic1 / 2f + (basic2 + basic3) / 4f;
    }

    // Use cosine function to interpolate the smoothened noise.
    float CosInterpolatedNoise(float x)
    {
        int integer_X = (int)x;
        float fractional_X = x - integer_X;

        float v1 = BasicSmoothNoise(integer_X);
        float v2 = BasicSmoothNoise(integer_X + 1);

        float f = (1 - Mathf.Cos(x * Mathf.PI)) * 0.5f;


        return v1 * (1 - f) + v2 * f;

    }
    // Generate 1d Perlin noise based on the input.
    public float PerlinNoise_1D(float x)
    {
        int p = 10;
        int n = 2;

        float total = 0;
        for (int i = 0; i < n; i++)
        {
            int frequency = (int)Mathf.Pow(2,i);
            int amplitude = (int)Mathf.Pow(p,i);
            total = total + CosInterpolatedNoise(x * frequency) * amplitude;
        }
        return total/35;


    }

    // I set the mound to be a parabola that would have value c between 6 and 12,
    // and for all possible c, f(-8) = f(8) = 8.
    float mound(float c, float x) {        
        return -0.015625f * c * Mathf.Pow(x, 2)+ c;
    }

    // Generate the terrain in game with points and lines.
    // All points will be stored in field landscapePoints.
    // Plains are between x coordinate -20 to -8 and -8 to 20,
    // The mound is between x coordinate -8 to 8.
    void GetTerrain()
    {
        //left side plain
        System.Random random = new System.Random();
        Debug.Log("start GetTerrain");
        // generate the coordinates for plain on the left side of the mound
        for (float i = -24f; i < -8f ; i += 0.5f) {
            Debug.Log(i);
            float noise = PerlinNoise_1D(random.Next(500) + i);
            // add noise to the height
            Vector3 position = new Vector3(i, plain_height + noise, 0);
            landscapePoints.Add(position);

        }

        // random height of mound
        float c = 6f + (float)random.NextDouble() * 6f;
        // generate the coordinates mound
        for (float i = -8f; i < 8f; i += 0.1f)
        {
            float noise = PerlinNoise_1D(random.Next(1000) + i);
            // add noise to the height
            Vector3 position = new Vector3(i, mound(c,i)-8 + noise, 0);
            landscapePoints.Add(position);
        }
        // generate the coordinates for right side plain
        for (float i = 8f; i < 24f; i += 0.5f)
        {
            float noise = PerlinNoise_1D(random.Next(1000) + i);
            // add noise to the height
            Vector3 position = new Vector3(i, plain_height + noise, 0);
            landscapePoints.Add(position);
        }

        // Draw a line between each two neighbor points on landscapePoints
        for (int i = 1; i < landscapePoints.Count; i++)
        {
            Vector3 start_pos = landscapePoints[i - 1];
            Vector3 end_pos = landscapePoints[i];
            Instantiate(point, start_pos, Quaternion.identity);
            Instantiate(point, end_pos, Quaternion.identity);
            GameObject tLine = Instantiate(line);
            LineRenderer lineRenderer = tLine.GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, start_pos);
            lineRenderer.SetPosition(1, end_pos);
        }
        // fininished generating the terrain
        Debug.Log("done GetTerrain");
    }
    // Get all landscapePoints for collision check in other classes.
    public static List<Vector3> TerrainCoords() {

        return landscapePoints;
    }

    // Start is called before the first frame update
    void Start()
    {
        GetTerrain();
        Debug.Log("start terraingenerator");
    }

}
