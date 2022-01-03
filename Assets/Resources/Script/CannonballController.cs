using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Haochen Liu 
 * 260917834
 * COMP521 A2 
 * 
 * This class controls the behavoir of each cannonball created
 * as well as collision check and resolution.
 */

public class CannonballController : MonoBehaviour
{
    // initial speed of cannonball at the muzzle
    public float muzzleSpeed;
    // firing angle
    public float muzzleAngle;
    // speeds
    public float xSpeed;
    public float ySpeed;
    public float xAcc; // "friction"
    public float yAcc; // gravity
    // bound has size 1*1, so radius is 0.5f.
    public float radius = 0.5f;
    // indicating whether the cannonball has passed the invisible wall on the left
    public bool invisibleWallCheck = false;
    // all positions of cannonball
    public static List<Vector3> cannonballsPositions = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        // Initializing speeds, angles, and accelerations.
        muzzleSpeed = CannonController.GetSpeed() / 125f;
        muzzleAngle = CannonController.GetAngle();
        xSpeed = -Mathf.Cos(muzzleAngle / 180f * Mathf.PI) * muzzleSpeed;
        ySpeed = Mathf.Sin(muzzleAngle / 180f * Mathf.PI) * muzzleSpeed;
        xAcc = 0.0002f;
        yAcc = -0.00098f;
        
    }



    // Update is called once per frame
    void Update()
    {
        // destory stationarly cannonballs
        DestroyOnStationary();
        OutOfScreenCheck();

        xSpeed += xAcc;
        ySpeed += yAcc;

        // Perform collision resolution with terrain based on 
        // which area of terrain is collided.
        int terrainHit = TerrainCollideCheck();
        if (terrainHit != -1) {
            TerrainCollideResolution(terrainHit);
        }
        
        // left wall
        LeftWallCheck();

        // After passed the invisible wall on the left, if the cannonball hit the wall again,
        // it will be destoryed.
        Vector3 currentPos = gameObject.transform.position;
        if (currentPos.x < 10f) {
            invisibleWallCheck = true;
        }
        InvisibleWallCheck();
        
        Vector3 nextPos = new Vector3(currentPos.x + xSpeed, currentPos.y + ySpeed, 0);
        gameObject.transform.position = nextPos;
        cannonballsPositions.Add(nextPos);
        
        // when at x-coord-stationnary, do not apply the horizontal friction.
        // And also change the direction of friction based on the direction of horizontal velocity.
        if (xSpeed == 0f)
        {
            xAcc = 0f;
        }
        else if (xSpeed > 0)
        {
            xAcc = -0.0002f;
        }
        else {
            xAcc = 0.0002f;
        }
        CannonballCollisionCheck();
        cannonballsPositions = new List<Vector3>();
    }
    
    // Check which area of terrain does the cannonball hit.
    // xcoord -20 to -8 and 8 to 20 are plain, -8 to 8 is mound.
    // More precisely, point with index 0 to 31 is left plain,
    // index 32 to 113 is left mound, 114 to 197 is right mound, 
    // 197 to 224 is right plain.
    public int TerrainCollideCheck() {
        List<Vector3> terrainPoints = TerrainGenerator.TerrainCoords();
        Vector3 currentPos = gameObject.transform.position;

        for (int i = 0;i< terrainPoints.Count; i++){
            Vector3 terrainCoord = terrainPoints[i];
            float distSquare = Mathf.Pow(terrainCoord.x - currentPos.x, 2) + Mathf.Pow(terrainCoord.y - currentPos.y, 2);
            if (distSquare < 0.25) {
                return i;
            
            }
        }
        return -1;
    }

    // Collision resolution of colliding with terrain
    // based on the area of hit. Speed will be portionally reduced 
    // for each collision.
    public void TerrainCollideResolution(int hitPoint) {
        Vector3 currentPos = gameObject.transform.position;
        // either left or right plain
        if (hitPoint <= 31 || hitPoint >= 197) {
            ySpeed = -ySpeed * 0.5f;
        }
        //left side of the mound
        else if (hitPoint > 31 && hitPoint <= 113) {
            if (xSpeed < 0)
            {
                ySpeed = -ySpeed * 0.5f;
            }
            else {
                
                xSpeed = -xSpeed * 0.75f;
                ySpeed = -ySpeed * 0.5f;
            }
        }
        // right side of the mound.
        else if (hitPoint > 113 && hitPoint <197) {
            if (xSpeed > 0)
            {
                ySpeed = -ySpeed * 0.5f;

            }
            else {
                xSpeed = -xSpeed * 0.75f;
                ySpeed = -ySpeed * 0.5f;
            }
        }
        gameObject.transform.position = new Vector3(currentPos.x, currentPos.y + 0.2f, 0f);
    }

    // Check whether this cannonball hit other cannonballs.
    // If hit, perform it as a complete elastic collision,
    // which means that the velocity of two collided cannonballs
    // will be switched.
    public void CannonballCollisionCheck() {
        foreach (GameObject toCheck in GameObject.FindGameObjectsWithTag("Cannonball")) {
            if (!gameObject.Equals(toCheck)) {
                Vector3 currentPos = gameObject.transform.position;
                Vector3 toCheckPos = toCheck.transform.position;
                float distSquare = Mathf.Pow(toCheckPos.x - currentPos.x, 2) + Mathf.Pow(toCheckPos.y - currentPos.y, 2);
                if (distSquare <= 1) // the radius of cannonball is 0.5(known from editor)
                {
                    // exchange velocity
                    CannonballController collider = toCheck.GetComponent<CannonballController>();
                    float x = collider.xSpeed;
                    float y = collider.ySpeed;
                    collider.xSpeed = xSpeed*1f;
                    collider.ySpeed = ySpeed *1f;
                    xSpeed = x * 1f;
                    ySpeed = y * 1f;
                    
                }
            }
        }
    }
    // Check whether the cannonball reached the invisible wall again.
    // If so, destroy it.
    public void InvisibleWallCheck() {
        Vector3 currentPos = gameObject.transform.position;
        if (invisibleWallCheck && currentPos.x >= 10) {
            Destroy(gameObject);
        }
        
    }

    // Check whether the cannonball collided with the left wall.
    // Perform collision resolution.
    public void LeftWallCheck() {
        Vector3 currentPos = gameObject.transform.position;
        if (currentPos.x <= -19f) {
            gameObject.transform.position = new Vector3(-18.75f, currentPos.y, 0f);
            xSpeed = -xSpeed*0.75f;
        }
    }
    
    // Destroy when the cannonball is out of the bounds of the 40*20 game scene.
    public void OutOfScreenCheck() {
        Vector3 currentPos = gameObject.transform.position;
        if (currentPos.x > 20 || currentPos.x < -21 || currentPos.y > 10 || currentPos.y < -10) {
            Destroy(gameObject);
        }
    }

    // Destroy stationary cannonballs with time delay.
    public void DestroyOnStationary() {
        if (Mathf.Abs(xSpeed) < 0.01f) {
            Destroy(gameObject,3f);
        }
    }


}
