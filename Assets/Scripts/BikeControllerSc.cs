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
    InputAction wheelieInput;

    void Awake()
    {
        bicycle = GetComponent<BicycleVehicle>();
        moveInput = InputSystem.actions.FindAction("Move");
        brakeInput = InputSystem.actions.FindAction("Brake");
        wheelieInput = InputSystem.actions.FindAction("Wheelie");

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector2 inputVector = moveInput.ReadValue<Vector2>();

        bicycle.VerticalInput = inputVector.y;
        bicycle.HorizontalInput = inputVector.x;

        bicycle.Braking = brakeInput.IsPressed();

        bicycle.Wheelie = wheelieInput.IsPressed();

        bicycle.InControl(controllingBike);

        if (controllingBike) bicycle.ConstrainRotation(bicycle.OnGround());
        else bicycle.ConstrainRotation(false);

    }
}