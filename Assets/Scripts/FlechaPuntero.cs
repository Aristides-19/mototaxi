using UnityEngine;

public class FlechaPuntero : MonoBehaviour
{
    [Header("¿A dónde tengo que mirar?")]
    public Transform objetivo; // El punto de destino

    [Header("Ajustes de movimiento")]
    public float velocidadRotacion = 10f;
    public float velocidadFlotacion = 5f;
    public float alturaFlotacion = 0.2f;

    private Vector3 posicionInicial;

    void Start()
    {
        // Guardamos su altura original para que sepa desde dónde empezar a flotar
        posicionInicial = transform.localPosition;
    }

    void Update()
    {
        // Si tiene un destino asignado, entonces apunta y flota
        if (objetivo != null)
        {
            Apuntar();
            Flotar();
        }
    }

    void Apuntar()
    {
        // Calculamos la distancia y dirección hacia el destino
        Vector3 direccion = objetivo.position - transform.position;

        // Magia negra: le decimos que ignore la altura (Y) para que no se incline hacia el piso
        direccion.y = 0;

        if (direccion != Vector3.zero)
        {
            // Giramos la flecha suavemente hacia esa dirección
            Quaternion rotacionDeseada = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, Time.deltaTime * velocidadRotacion);
        }
    }

    void Flotar()
    {
        // Efecto de sube y baja usando matemáticas (seno)
        float nuevaY = posicionInicial.y + (Mathf.Sin(Time.time * velocidadFlotacion) * alturaFlotacion);
        transform.localPosition = new Vector3(posicionInicial.x, nuevaY, posicionInicial.z);
    }

    // Añade esto al final de FlechaPuntero.cs
    public void ActivarGPS(Transform nuevoDestino)
    {
        objetivo = nuevoDestino;
        gameObject.SetActive(true); // Enciende el objeto visualmente
    }

    public void ApagarGPS()
    {
        objetivo = null;
        gameObject.SetActive(false); // Apaga el objeto visualmente
    }
}
