using UnityEngine;
using UnityEngine.InputSystem;
using rayzngames;

[RequireComponent(typeof(BicycleVehicle))]
public class BikeControllerSc : MonoBehaviour
{

    [SerializeField] bool controllingBike;

    BicycleVehicle bicycle;

    InputAction moveInput;
    InputAction brakeInput;

    void Awake()
    {
        bicycle = GetComponent<BicycleVehicle>();
        moveInput = InputSystem.actions.FindAction("Move");
        brakeInput = InputSystem.actions.FindAction("Brake");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 inputVector = moveInput.ReadValue<Vector2>();

        bicycle.verticalInput = inputVector.y;
        bicycle.horizontalInput = inputVector.x;

        bicycle.braking = brakeInput.IsPressed();

        bicycle.InControl(controllingBike);

        if (controllingBike) bicycle.ConstrainRotation(bicycle.OnGround());
        else bicycle.ConstrainRotation(false);

    }
}