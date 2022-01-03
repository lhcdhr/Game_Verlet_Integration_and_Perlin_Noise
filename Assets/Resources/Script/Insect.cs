using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Haochen Liu 
 * 260917834
 * COMP521 A2 
 * 
 * This class manages the behavior of each Insect, 
 * including drawing insects, handling constraints,
 * collision check, and collision resolution.
 */
public class Insect : MonoBehaviour
{
    // prefabs
    public GameObject point;
    public GameObject line;

    // current positions of points
    public List<Vector3> current = new List<Vector3>();
    // the positions of points in the previous frame
    public List<Vector3> previous = new List<Vector3>();

    // accelerations
    public float xAcc;
    public float yAcc;

    // The insect are divided into 8 parts
    public GameObject antenna1;
    public GameObject antenna2;
    public GameObject wingL1;
    public GameObject wingL2;
    public GameObject wingL3;
    public GameObject wingR1;
    public GameObject wingR2;
    public GameObject wingR3;

    // Temporary acceleration used for collision resolution and wind.
    public float xAccTemp;
    public float yAccTemp;

    // Start is called before the first frame update
    // Setup the basic shape of insect and generate all its parts.
    void Start()
    {
        SetupInsect();
        GenerateInsect();
    }

    // Update is called once per frame
    void Update()
    {
        DestroyOutofBound();
        TerrainCheck();
        LeftWallCheck();
        ApplyWind();
        //CollisionWithCannonball();
        // Insects move randomly.
        xAcc = Random.Range(-0.001f, 0.001f) + xAccTemp;
        yAcc = Random.Range(-0.001f, 0.001f) + yAccTemp;
        UpdateCenter();
        
        // update all coordinates based on the setup
        UpdatePositionsWithConstraint();

        LeftWallCheck();
        DestroyOutofBound();
        TerrainCheck();

        // draw the insect with lines based on the cooridnates
        UpdateLinePositions();

        // reset for next frame
        xAccTemp = 0f;
        yAccTemp = 0f;
    }

    // Setup the basic shape of insect.
    public void SetupInsect() {
        Debug.Log("SetupInsect");
        Vector3 coord = gameObject.transform.position;
        Vector3 center = new Vector3(coord.x, coord.y, 0);
        Vector3 antenna1 = new Vector3(coord.x - 0.5f, coord.y + 1.5f, 0);
        Vector3 antenna2 = new Vector3(coord.x + 0.5f, coord.y + 1.5f, 0);
        Vector3 wingL1 = new Vector3(coord.x - 1f, coord.y + 1f, 0);
        Vector3 wingL2 = new Vector3(coord.x - 1f, coord.y + 0.5f, 0);
        Vector3 wingL3 = new Vector3(coord.x - 1f, coord.y - 0.5f, 0);
        Vector3 wingL4 = new Vector3(coord.x - 1f, coord.y - 1f, 0);
        Vector3 wingR1 = new Vector3(coord.x + 1f, coord.y + 1f, 0);
        Vector3 wingR2 = new Vector3(coord.x + 1f, coord.y + 0.5f, 0);
        Vector3 wingR3 = new Vector3(coord.x + 1f, coord.y - 0.5f, 0);
        Vector3 wingR4 = new Vector3(coord.x + 1f, coord.y - 1f, 0);
        current.Add(center);
        current.Add(antenna1);
        current.Add(antenna2);
        current.Add(wingL1);
        current.Add(wingL2);
        current.Add(wingL3);
        current.Add(wingL4);
        current.Add(wingR1);
        current.Add(wingR2);
        current.Add(wingR3);
        current.Add(wingR4);
        // store positions in fields
        for (int i = 0; i < 11; i++) {
            previous.Add(new Vector3(current[i].x, current[i].y, 0));
        }
    }

    //Generate each component of insect.
    void GenerateInsect() {
        GenerateAntenna1();
        GenerateAntenna2();
        GenerateWingL1();
        GenerateWingL2();
        GenerateWingL3();
        GenerateWingR1();
        GenerateWingR2();
        GenerateWingR3();
    }
    
    // Destroy all compoments of insect.
    // Used when it is time to destroy the isnect.
    void DestroyAllComponent() {
        Destroy(antenna1);
        Destroy(antenna2);
        Destroy(wingL1);
        Destroy(wingL2);
        Destroy(wingL3);
        Destroy(wingR1);
        Destroy(wingR2);
        Destroy(wingR3);
    }
    
    // Generating the components of insect.
    void GenerateAntenna1() {
        GameObject tline = Instantiate(line);
        antenna1 = tline;
        LineRenderer lineRenderer = tline.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, current[0]);
        lineRenderer.SetPosition(1, current[1]);
    }
    void GenerateAntenna2()
    {
        GameObject tline = Instantiate(line);
        antenna2 = tline;
        LineRenderer lineRenderer = tline.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, current[0]);
        lineRenderer.SetPosition(1, current[2]);
    }
    void GenerateWingL1() {
        GameObject tline = Instantiate(line);
        wingL1 = tline;
        LineRenderer lineRenderer = tline.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 6;
        Vector3[] wL1 = { current[0], current[3], current[4], current[5], current[6], current[0] };
        lineRenderer.SetPositions(wL1);
    }
    void GenerateWingL2() {
        GameObject tline = Instantiate(line);
        wingL2 = tline;
        LineRenderer lineRenderer = tline.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, current[0]);
        lineRenderer.SetPosition(1, current[4]);
    }
    void GenerateWingL3()
    {
        GameObject tline = Instantiate(line);
        wingL3 = tline;
        LineRenderer lineRenderer = tline.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, current[0]);
        lineRenderer.SetPosition(1, current[5]);
    }
    void GenerateWingR1()
    {
        GameObject tline = Instantiate(line);
        wingR1 = tline;
        LineRenderer lineRenderer = tline.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 6;
        Vector3[] wR1 = { current[0], current[7], current[8], current[9], current[10], current[0] };
        lineRenderer.SetPositions(wR1);
    }
    void GenerateWingR2()
    {
        GameObject tline = Instantiate(line);
        wingR2 = tline;
        LineRenderer lineRenderer = tline.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, current[0]);
        lineRenderer.SetPosition(1, current[8]);
    }
    void GenerateWingR3()
    {
        GameObject tline = Instantiate(line);
        wingR3 = tline;
        LineRenderer lineRenderer = tline.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, current[0]);
        lineRenderer.SetPosition(1, current[9]);
    }

    // Calculate the  distance between 2 points.
    float Distance(Vector3 p1, Vector3 p2){
        return Mathf.Sqrt((Mathf.Pow(p1.x - p2.x, 2) + Mathf.Pow(p1.y - p2.y,2)));

    }


    // Update the position of center point based on Verlet Integration.
    void UpdateCenter() {
        Vector3 current_dash = new Vector3(current[0].x, current[0].y, 0);
        current[0] = new Vector3(2 * current[0].x - previous[0].x + xAcc, 2 * current[0].y - previous[0].y + yAcc, 0);
        previous[0] = new Vector3(current_dash.x, current_dash.y, 0);
    }
    // Update the position of other points of insect based on the constraint of Verlet Integration.
    // The constraint is based on the distance between this point and the center point.
    void UpdatePositionsWithConstraint() {

        // flexible constraints of ranges for different lines
        float aCons_max = 1.05f * Mathf.Sqrt(10);
        float aCons_min = 0.95f * Mathf.Sqrt(10);

        float wOutCons_max = 1.1f * Mathf.Sqrt(8);
        float wOutCons_min = 0.9f * Mathf.Sqrt(8);

        float wInCons_max = 1.1f * Mathf.Sqrt(5);
        float wInCons_min = 0.9f * Mathf.Sqrt(5);


        // antenna constraint
        // calculate the distance between center
        float dist_a1 = Distance(current[0], current[1]);
        previous[1] = new Vector3(current[1].x,current[1].y,0);
        // if it is out of the range of constraint, reset it.
        if (dist_a1 > aCons_max || dist_a1 < aCons_min) {
            current[1] = new Vector3(current[0].x-1f, current[0].y+3f,0);
        }
        // similar to above.
        float dist_a2 = Distance(current[0], current[2]);
        previous[2] = new Vector3(current[2].x, current[2].y, 0);
        if (dist_a2 > aCons_max || dist_a2 < aCons_min) {
            current[2] = new Vector3(current[0].x+1f, current[0].y+3f,0);
        
        }
        // left wing constraint
        float dist_wl1 = Distance(current[0], current[3]);
        previous[3] = new Vector3(current[3].x, current[3].y, 0);
        if (dist_wl1 > wOutCons_max || dist_wl1 < wOutCons_min) {
            current[3] = new Vector3(current[0].x - 2f, current[0].y + 2f, 0);
        }

        float dist_wl2 = Distance(current[0], current[4]);
        previous[4] = new Vector3(current[4].x, current[4].y, 0);
        if (dist_wl2 > wInCons_max || dist_wl2 < wInCons_min)
        {
            current[4] = new Vector3(current[0].x - 2f, current[0].y + 1f, 0);
        }

        float dist_wl3 = Distance(current[0], current[5]);
        previous[5] = new Vector3(current[5].x, current[5].y, 0);
        if (dist_wl3 > wInCons_max || dist_wl3 < wInCons_min)
        {
            current[5] = new Vector3(current[0].x - 2f, current[0].y - 1f, 0);
        }

        float dist_wl4 = Distance(current[0], current[6]);
        previous[6] = new Vector3(current[6].x, current[6].y, 0);
        if (dist_wl4 > wOutCons_max || dist_wl4 < wOutCons_min)
        {
            current[6] = new Vector3(current[0].x - 2f, current[0].y - 2f, 0);
        }
        // right wing constraint
        float dist_wr1 = Distance(current[0], current[7]);
        previous[7] = new Vector3(current[7].x, current[7].y, 0);
        if (dist_wr1 > wOutCons_max || dist_wr1 < wOutCons_min)
        {
            current[7] = new Vector3(current[0].x + 2f, current[0].y + 2f, 0);
        }
        float dist_wr2 = Distance(current[0], current[8]);
        previous[8] = new Vector3(current[8].x, current[8].y, 0);
        if (dist_wr2 > wInCons_max || dist_wr2 < wInCons_min)
        {
            current[8] = new Vector3(current[0].x + 2f, current[0].y + 1f, 0);
        }

        float dist_wr3 = Distance(current[0], current[9]);
        previous[9] = new Vector3(current[9].x, current[9].y, 0);
        if (dist_wr3 > wInCons_max || dist_wr3 < wInCons_min)
        {
            current[9] = new Vector3(current[0].x + 2f, current[0].y - 1f, 0);
        }

        float dist_wr4 = Distance(current[0], current[10]);
        previous[10] = new Vector3(current[10].x, current[10].y, 0);
        if (dist_wr4 > wOutCons_max || dist_wr4 < wOutCons_min)
        {
            current[10] = new Vector3(current[0].x + 2f, current[0].y - 2f, 0);
        }

    }

    // Update the position of lines based on the current positions.
    void UpdateLinePositions() {
        LineRenderer a1 = antenna1.GetComponent<LineRenderer>();
        a1.SetPosition(0, current[0]);
        a1.SetPosition(1, current[1]);
        LineRenderer a2 = antenna2.GetComponent<LineRenderer>();
        a2.SetPosition(0, current[0]);
        a2.SetPosition(1, current[2]);
        
        LineRenderer wl1 = wingL1.GetComponent<LineRenderer>();
        Vector3[] wl1Pos = {current[0], current[3], current[4], current[5], current[6], current[0]};
        wl1.SetPositions(wl1Pos);
        LineRenderer wl2 = wingL2.GetComponent<LineRenderer>();
        wl2.SetPosition(0, current[0]);
        wl2.SetPosition(1, current[4]);
        LineRenderer wl3 = wingL3.GetComponent<LineRenderer>();
        wl3.SetPosition(0, current[0]);
        wl3.SetPosition(1, current[5]);

        LineRenderer wr1 = wingR1.GetComponent<LineRenderer>();
        Vector3[] wr1Pos = { current[0], current[7], current[8], current[9], current[10], current[0] };
        wr1.SetPositions(wr1Pos);
        LineRenderer wr2 = wingR2.GetComponent<LineRenderer>();
        wr2.SetPosition(0, current[0]);
        wr2.SetPosition(1, current[8]);
        LineRenderer wr3 = wingR3.GetComponent<LineRenderer>();
        wr3.SetPosition(0, current[0]);
        wr3.SetPosition(1, current[9]);
    }

    // Collision detection and resolution between insects and cannonballs.
    // This implementation will affect the collision with terrain for some reason that
    // I cannot explain, so commented out this part in order to demonstrate the 
    // the collision detection and resolution with terrain.
    /*
    public bool CollisionWithCannonball() {
        bool collided = false;
        if (GameObject.FindGameObjectsWithTag("Cannonball").Length != 0)
        {
            
            foreach (GameObject toCheck in GameObject.FindGameObjectsWithTag("Cannonball"))
            {
                
                for(int i=1; i<current.Count;i++)
                {
                    float distance = Distance(current[i], toCheck.transform.position);
                    if (distance < 2f)
                    {
                        collided = true;
                        
                        Debug.Log(distance);
                        Debug.Log("Collided with insect");
                        //ShrinkToCenter();
                        
                        bool vertical = false;
                        bool horizontal = false;
                        // true if cannonball is on the left
                        if (current[0].x > toCheck.transform.position.x) {
                            horizontal = true;
                        }
                        // true if cannonball is below
                        if (current[0].x > toCheck.transform.position.y)
                        {
                            vertical = true;
                        }
                        if (horizontal && vertical)
                        {
                            //current[0] = new Vector3(current[0].x + 0.001f, current[0].y + 0.001f, 0);
                            xAccTemp += 0.001f;
                            yAccTemp += 0.001f;
                        }
                        else if ((!horizontal) && vertical)
                        {
                            //current[0] = new Vector3(current[0].x - 0.001f, current[0].y + 0.001f, 0);
                            xAccTemp -= 0.001f;
                            yAccTemp += 0.001f;
                        }
                        else if (horizontal && (!vertical))
                        {
                            //current[0] = new Vector3(current[0].x + 0.001f, current[0].y - 0.001f, 0);
                            xAccTemp += 0.001f;
                            yAccTemp -= 0.001f;
                        }
                        else if((!horizontal)&&(!vertical)){
                            //current[0] = new Vector3(current[0].x - 0.001f, current[0].y - 0.001f, 0);
                            xAccTemp -= 0.001f;
                            yAccTemp -= 0.001f;
                        }

                    }
                }
            }
        }
        return collided;
    }
    
    public void ShrinkToCenter() {
        for (int i = 1; i < current.Count; i++) {
            Vector3 temp = new Vector3(current[i].x,current[i].y,0);
            previous[i] = new Vector3(temp.x, temp.y, 0);
            float distX = current[i].x - current[0].x;
            float distY = current[i].y - current[0].y;
            current[i] = new Vector3(current[0].x + 0.2f*distX, current[0].y + 0.2f*distY,0);
        }
    }
    */

    // Check whether the insect reached the wall on the left.
    // It should not cross this wall.
    public bool LeftWallCheck() {
        bool collided = false;
        for (int i = 0; i < current.Count; i++) {

            if (current[i].x <= -19f) {
                current[i] = new Vector3(-19f,current[i].y,0);
                collided = true;
            }

        }
        if (collided) {
            xAccTemp += 0.001f;
        }
        return collided;
    }

    // Check whether the insect collides with the terrain.
    // It should not cross the terrain.
    public void TerrainCheck() {
        List<Vector3> points = TerrainGenerator.TerrainCoords();
        for (int i = 0; i < points.Count; i++) {
            foreach (Vector3 insectPoint in current) {
                if (Distance(insectPoint, points[i]) < 0.39f) {
                    if (i < 32 || i > 193)
                    {
                        yAccTemp += 0.0005f;
                    }
                    else if (i >= 32 && i <= 113)
                    {
                        xAccTemp -= 0.0005f;
                        yAccTemp += 0.0005f;
                    }
                    else {
                        xAccTemp += 0.0005f;
                        yAccTemp += 0.0005f;
                    }
                }
            }
        }
    }
    // If the insect reached the upper bound or the invisible wall
    // on the left, then it should be destoryed.
    public void DestroyOutofBound() {
        foreach (Vector3 coord in current) {
            if (coord.y > 11 || coord.x > 9) {
                // destroy all compoments
                DestroyAllComponent();
                // destroy Insect
                Destroy(gameObject);            
            }
        }
    }

    // Apply wind of the insect with center point in the windzone.
    // 
    public void ApplyWind() {
        if (current[0].x > -15f && current[0].x < -14f) {
            yAccTemp += InsectManager.wind1;
        }
        if (current[0].x > -13f && current[0].x < -12f)
        {
            yAccTemp += InsectManager.wind2;
        }
        if (current[0].x > -9f && current[0].x < -8f)
        {
            yAccTemp += InsectManager.wind3;
        }
    }

}
