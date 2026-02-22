using UnityEngine;

public class Get_off_girl : MonoBehaviour
{
    [Header("Configuración")]
    public string nombreManiqui = "Rose_Mannequin";
    public string nombreAsiento = "Copilot_Seat";

    [Tooltip("Punto exacto donde aparecerá la chica al bajar")]
    public Transform getOffDestination;

    // CAMBIO 1: Reemplazamos CountPasajeros por el nuevo HUDManager
    public CountPasajeros hudManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Transform asiento = BuscarHijoRecursivo(other.transform, nombreAsiento);

            if (asiento != null)
            {
                Transform chica = asiento.Find(nombreManiqui);

                if (chica != null)
                {
                    BajarChica(chica);
                }
                else
                {
                    Debug.Log("No hay nadie con el nombre " + nombreManiqui + " en el asiento.");
                }
            }
        }
    }

    void BajarChica(Transform chica)
    {
        chica.SetParent(null);

        chica.position = getOffDestination.position;
        chica.rotation = getOffDestination.rotation;

        if (chica.TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = false;
        }

        if (chica.TryGetComponent<Animator>(out Animator anim))
        {
            anim.SetTrigger("GetOff");
        }

        Debug.Log("La chica ha bajado de la moto.");

        // CAMBIO 2: Calculamos el dinero basado en el tiempo que tardaste
        if (hudManager != null)
        {
            hudManager.FinalizarViaje();
        }
        else
        {
            Debug.LogWarning("Falta asignar el HUDManager en el inspector del Get_off_girl");
        }
    }

    private Transform BuscarHijoRecursivo(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name) return child;
            Transform result = BuscarHijoRecursivo(child, name);
            if (result != null) return result;
        }
        return null;
    }
}