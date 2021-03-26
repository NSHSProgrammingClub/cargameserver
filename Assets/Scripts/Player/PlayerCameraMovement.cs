using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraMovement : MonoBehaviour
{

    [SerializeField] private float sensitivity = 100f;

    [SerializeField] private float yMaximum = 90f;
    [SerializeField] private float yMinimum = -90f;

    private Transform playerObjectTransform;

    private float yRotation;

    private void Start()
    {
        playerObjectTransform = gameObject.transform.parent.transform;

    }


    private void Rotate()
    {
        float x = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
        float y = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;
        yRotation -= y;

        yRotation = Mathf.Clamp(yRotation, yMinimum, yMaximum);


        transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        playerObjectTransform.Rotate(Vector3.up * x);
    }

    private void Update()
    {

        Rotate();
    }

}
