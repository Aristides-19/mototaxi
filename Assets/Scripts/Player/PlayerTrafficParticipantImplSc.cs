using ArcadeBP_Pro;
using Gley.TrafficSystem;
using UnityEngine;

[RequireComponent(typeof(ArcadeBikeControllerPro), typeof(Rigidbody))]
public class PlayerTrafficParticipantImplSc : MonoBehaviour, ITrafficParticipant
{
    private Rigidbody rb;
    private Collider[] childColliders;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        childColliders = GetComponentsInChildren<Collider>();
    }

    public bool AlreadyCollidingWith(Collider[] trafficCarColliders)
    {
        for (int i = 0; i < childColliders.Length; i++)
        {
            for (int j = 0; j < trafficCarColliders.Length; j++)
            {
                if (childColliders[i].bounds.Intersects(trafficCarColliders[j].bounds))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public float GetCurrentSpeedMS() => rb != null ? rb.linearVelocity.magnitude : 0f;

    public Vector3 GetHeading() => transform.forward;
}
