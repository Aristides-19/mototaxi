using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeteccionSimple : MonoBehaviour
{
    public float radioDeDeteccion = 2f;
    public LayerMask capaTraffic;
    public int puntosTotales = 0;

    // Aquí arrastramos "Cash" desde el Inspector
    public TextMeshProUGUI textoDinero;

    private HashSet<Collider> carrosContados = new HashSet<Collider>();

    void Update()
    {
        Collider[] vehiculosCercanos = Physics.OverlapSphere(transform.position, radioDeDeteccion, capaTraffic);

        foreach (Collider vehiculo in vehiculosCercanos)
        {
            if (!carrosContados.Contains(vehiculo))
            {
                puntosTotales += 10;
                Debug.Log("¡Vehículo detectado! Puntos: " + puntosTotales);

                // Aquí le decimos que junte los Bs con lo acumulado
                if (textoDinero != null)
                {
                    textoDinero.text = "Bs" + puntosTotales.ToString();

                }

                carrosContados.Add(vehiculo);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radioDeDeteccion);
    }
}