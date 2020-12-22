using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraMovement : MonoBehaviour
{

    [SerializeField] private float sensitivity = 100f;

    [SerializeField] private float yMaximum = 90f;
    [SerializeField] private float yMinimum = -90f;

    private Transform root;

    private float xRotation;

    private bool isInMenu = false;

    private void Start()
    {
        root = gameObject.transform.parent.transform;
    }

    public void SetMenuState(bool _isInMenu)
    {
        isInMenu = _isInMenu;
    }

    private void Rotate()
    {
        float x = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
        float y = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

        xRotation -= y;

        xRotation = Mathf.Clamp(xRotation, yMinimum, yMaximum);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        root.Rotate(Vector3.up * x);

    }

    private void Update()
    {
        if (!GameManager.isInMenu)
            Rotate();
    }

}
