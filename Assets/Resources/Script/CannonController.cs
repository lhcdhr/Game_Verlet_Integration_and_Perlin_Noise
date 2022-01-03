using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*
 * Haochen Liu 
 * 260917834
 * COMP521 A2 
 * 
 * This class manages the controlling of the cannon.
 * Cannonballs are fired with a reloading time of 0.5 seconds.
 */
public class CannonController : MonoBehaviour
{
    // assets and prefabs
    public GameObject cannon;
    public GameObject cannonball;
    // where cannonball is instantiated
    public GameObject muzzle;
    public static float angle = 0f;
    public static float speed;
    // displaying the muzzle speed
    public Text muzzleText;
    // 
    public bool holdFire = false;
    public float lastFire;

    // Change the angle of cannon based on the keyboard input.
    public void UpdateAngle() 
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && angle < 90f) {// 
            Debug.Log("up");
            angle = angle + 5f;
            cannon.transform.Rotate(0, 0, -5f);   
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)&& angle > 0f) { //
            Debug.Log("down");
            angle = angle - 5f;
            cannon.transform.Rotate(0, 0, 5f);
        }
    }

    // Change the muzzle speed based on the keyboard input/
    public void UpdateSpeed() {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && speed < 50) {
            Debug.Log("left");
            speed = speed + 5f;
            muzzleText.text = "Muzzle Velocity: " + speed;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && speed > 5)
        {
            Debug.Log("right");
            speed = speed - 5f;
            muzzleText.text = "Muzzle Velocity: " + speed;
        }

    }

    // Instantaite the cannonball at muzzle with the current muzzle speed and angle.
    public void Fire() {

        GameObject bullet = Instantiate(cannonball, muzzle.transform.position,Quaternion.identity);

    }

    public static float GetAngle()
    {
        return angle;
    }

    public static float GetSpeed()
    {
        return speed;
    }

    // Start is called before the first frame update
    void Start()
    {
        // initial speed = 10.
        speed = 10;
        muzzleText.text = "Muzzle Velocity: " + speed;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAngle();
        // next shot should be fired lt least 0.5s later.
        if (Input.GetKeyDown(KeyCode.Space) && holdFire == false) {
            Fire();
            holdFire = true;
            lastFire = Time.time;
        }
        UpdateSpeed();
        if (Time.time - lastFire >= 0.5f) {
            holdFire = false;
        }
    }
}
