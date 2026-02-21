using TMPro;
using UnityEngine;

public class CountPasajeros : MonoBehaviour
{

    public TextMeshProUGUI TextoPasajeros;
    private int pasajero = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ActualizarInterfaz();
    }
    public void SumarPasajero() 
    {
        pasajero++;
        ActualizarInterfaz();
    }

    public void ActualizarInterfaz() 
    {
        TextoPasajeros.text = "Pasajeros: " + pasajero;
    }
    // Update is called once per frame
}
