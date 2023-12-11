using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupItem : MonoBehaviour
{
    [SerializeField] private bool destroyOnPickup = true;   //픽업했을 때 어떻게 할 거냐.
    [SerializeField] private LayerMask canBePickupBy;   //내가 먹을 수 있는
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canBePickupBy.value == (canBePickupBy.value | (1 << other.gameObject.layer)))
        {
            OnPickedUp(other.gameObject);
            if (pickupSound)
                SoundManager.PlayClip(pickupSound);

            if (destroyOnPickup)
            {
                Destroy(gameObject);
            }
        }
    }

    protected abstract void OnPickedUp(GameObject receiver);
}
