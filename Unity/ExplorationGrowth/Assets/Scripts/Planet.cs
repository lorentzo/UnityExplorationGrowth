using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    private float rotateSpeed = 100.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float RotateZ = Input.GetAxis("Vertical") * rotateSpeed;
        float RotateX = Input.GetAxis("Horizontal") * rotateSpeed;

        RotateX *= Time.deltaTime;
        RotateZ *= Time.deltaTime;

        transform.Rotate(RotateX, 0, RotateZ);
    }
}
