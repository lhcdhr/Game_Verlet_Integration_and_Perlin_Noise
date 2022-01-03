using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * Haochen Liu 
 * 260917834
 * COMP521 A2 
 * 
 * 
 * This class manages the spawn of Insect and the generation of 3 windzones.
 */
public class InsectManager : MonoBehaviour
{
    // prefab 
    public GameObject insect;

    // displaying windspeeds
    public Text display1;
    public Text display2;
    public Text display3;

    // time counter for wind
    float time = 0f;

    // windspeeds
    public static float wind1;
    public static float wind2;
    public static float wind3;
    // Start is called before the first frame update
    // Instantiate 3 insects and random positions above the left side plain.
    void Start()
    {
        
        Instantiate(insect, new Vector3(Random.Range(-16, -8), Random.Range(-4, 7), 0), Quaternion.identity);
        Instantiate(insect, new Vector3(Random.Range(-16, -8), Random.Range(-4, 7), 0), Quaternion.identity);
        Instantiate(insect, new Vector3(Random.Range(-16, -8), Random.Range(-4, 7), 0), Quaternion.identity);
    }

    // Update is called once per frame
    // Update windspeed every 2 seconds.
    // Make sure there will always be 3 insects in the game.
    void Update()
    {

        // count time
        time += Time.deltaTime;
        if (time >= 2f) {
            wind();
            time = 0f;
        }
        // check and update the insects(whether to spawn new ones)
        CheckInsectNumber();
        display1.text = "wind1 speed =" + wind1 * 1000;
        display2.text = "wind2 speed =" + wind2 * 1000;
        display3.text = "wind3 speed =" + wind3 * 1000;
    }

    // Count how many insects are currently in the scene.
    // If less than 3, then spawn until there are 3 in the game.
    void CheckInsectNumber() {
        int insectsNum = GameObject.FindGameObjectsWithTag("Insect").Length;
        if (insectsNum < 3) {
            for (int i=0;i<3-insectsNum;i++) {
                Instantiate(insect, new Vector3(Random.Range(-16, -6), Random.Range(-4, 7), 0), Quaternion.identity);
            }
        }


    }

    // Randomly generate windspeed.
    // There are 50% chance that the windspeed being 0,
    // and 50% chance that windspeed > 0 base on the random number.
    void wind() {
        float rd1 = Random.Range(0, 100);
        float rd2 = Random.Range(0, 100);
        float rd3 = Random.Range(0, 100);

        if (rd1 >= 50f)
        {
            wind1 = 0f;
        }
        else {
            wind1 = 2f * rd1 * 0.00001f;
        }

        if (rd2 >= 50f)
        {
            wind2 = 0f;
        }
        else
        {
            wind2 = 2f * rd2 * 0.00001f;
        }

        if (rd3 >= 50f)
        {
            wind3 = 0f;
        }
        else
        {
            wind3 = 2f * rd3 * 0.00001f;
        }
    }
}
