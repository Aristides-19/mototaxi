using UnityEngine;

public class Pick_up_girl : MonoBehaviour
{
    [Header("Referencias")]
    public CountPasajeros hud;
    public GameObject dummyRosa;

    private void OnTriggerEnter( Collider other )
    {
        if (other.CompareTag("Player"))
        {
            Transform asientoCopiloto = other.transform.Find("Copilot_Seat");
            if (asientoCopiloto != null)
            {
                MontarCopiloto(asientoCopiloto);
                hud.IniciarViaje();
            }
            else
            {
                Debug.LogWarning("No se encontró 'Copilot_seat' dentro de " + other.name);
            }
        }
    }

    void MontarCopiloto(Transform puntoAsiento)
    {
        dummyRosa.transform.SetParent(puntoAsiento);

        dummyRosa.transform.localPosition = Vector3.zero;
        dummyRosa.transform.localRotation = Quaternion.identity;

        if (dummyRosa.GetComponent<Rigidbody>())
            dummyRosa.GetComponent<Rigidbody>().isKinematic = true;
    }
}
