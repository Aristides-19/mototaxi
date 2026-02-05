using ArcadeBP_Pro;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(ArcadeBikeControllerPro))]
public class SpeedometerSc : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI speedText;
    ArcadeBikeControllerPro bikeController;

    void Awake()
    {
        bikeController = GetComponent<ArcadeBikeControllerPro>();
    }

    void Update()
    {
        float speed = bikeController.localBikeVelocity.magnitude * 3.6f; // Convert unit(m)/s to km/h
        speedText.text = Mathf.RoundToInt(speed).ToString() + " km/h";
    }
}
