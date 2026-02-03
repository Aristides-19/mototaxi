using rayzngames;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(BicycleVehicle))]
public class SpeedometerSc : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI speedText;
    BicycleVehicle bicycle;

    void Awake()
    {
        bicycle = GetComponent<BicycleVehicle>();
    }

    void Update()
    {
        float speed = bicycle.currentSpeed * 3.6f; // Convert m/s to km/h
        speedText.text = Mathf.RoundToInt(speed).ToString() + " km/h";
    }
}
