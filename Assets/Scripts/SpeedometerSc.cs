// using rayzngames;
using TMPro;
using UnityEngine;

// [RequireComponent(typeof(BicycleVehicle))]
public class SpeedometerSc : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI speedText;
    // BicycleVehicle bicycle;

    void Awake()
    {
        // bicycle = GetComponent<BicycleVehicle>();
    }

    void Update()
    {
        // float speed = bicycle.CurrentSpeed * 3.6f; // Convert unit(m)/s to km/h
        speedText.text = Mathf.RoundToInt(33).ToString() + " km/h";
    }
}
