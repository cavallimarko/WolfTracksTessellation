using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    public float rotationSpeed = 1;
    public Transform Target, Player;
    float mouseX, mouseY;
    // Start is called before the first frame update
    void Start()
    {
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LateUpdate()
    {
        CameraRotate();
    }
    void CameraRotate() {
        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        mouseY += Input.GetAxis("Mouse Y") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -35, 60);
        transform.LookAt(Target);
        Target.rotation = Quaternion.Euler(mouseY,mouseX,0);
        

    }
}
