using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltInputClient: MonoBehaviour
 {

    float x;
    float y;
    float z;

  
    int frameCount;

	void Start ()
    {
        frameCount = 0;
    }
	
	
	void Update ()
    {
        if (frameCount == 5)
        {

            x = Input.acceleration.x;
            y = Input.acceleration.y;
            z = Input.acceleration.z;
            

            NetworkClientUI.SendTiltInfo(x, y, z);

            

            frameCount = 0;
        }
        frameCount++;
    }
}
