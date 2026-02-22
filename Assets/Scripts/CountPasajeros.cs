using TMPro;
using UnityEngine;

public class CountPasajeros : MonoBehaviour
{

    public TextMeshProUGUI TextoDinero;
    public float dineroTotal = 0f;
    public float recompensaMaxima = 100f;
    public float recompensaMinima = 10f;
    public float penalizacionPorSegundo = 2f;

    private bool viajeActivo = false;
    private float tiempoDelViaje = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ActualizarInterfaz();
    }

    public void Update()
    {
        if (viajeActivo) 
        {
            tiempoDelViaje += Time.deltaTime;
        }
    }

    public void IniciarViaje() 
    {
        viajeActivo = true;
        tiempoDelViaje = 0f;
        Debug.Log("Viaje iniciado");
    }

    public void FinalizarViaje() 
    {
        if (viajeActivo) 
        {
            viajeActivo = false;
            float dineroGanado = recompensaMaxima - (tiempoDelViaje * penalizacionPorSegundo);

            dineroGanado = Mathf.Max(dineroGanado, recompensaMinima);

            dineroGanado = Mathf.Round(dineroGanado);

            dineroTotal += dineroGanado;

            Debug.Log("Tiempo tardado: " + tiempoDelViaje + "s. Dinero ganado: $" + dineroGanado);
            ActualizarInterfaz();
        }
    }
    //public void SumarPasajero() 
    //{
    //    pasajero++;
    //    ActualizarInterfaz();
    //}

    public void ActualizarInterfaz() 
    {
        TextoDinero.text = "Dinero: $" + dineroTotal;
    }
    // Update is called once per frame
}
 