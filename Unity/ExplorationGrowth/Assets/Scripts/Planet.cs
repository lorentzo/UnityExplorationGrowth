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
        rotatePlanetWithKeyboard();
        rotatePlanetWithMouse();
    }

    void rotatePlanetWithMouse()
    {
        if (Input.GetMouseButton(2))
        {
            float RotateX = Input.GetAxis("Mouse X") * rotateSpeed;
            float RotateY = Input.GetAxis("Mouse Y") * rotateSpeed;

            RotateX *= Time.deltaTime;
            RotateY *= Time.deltaTime;

            transform.Rotate(RotateY, 0, -RotateX);
        }
    }

    void rotatePlanetWithKeyboard()
    {
        float RotateZ = Input.GetAxis("Vertical") * rotateSpeed;
        float RotateX = Input.GetAxis("Horizontal") * rotateSpeed;

        RotateX *= Time.deltaTime;
        RotateZ *= Time.deltaTime;

        transform.Rotate(RotateX, 0, RotateZ);
    }
}
