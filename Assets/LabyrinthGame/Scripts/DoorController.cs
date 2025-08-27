using UnityEngine;

public class DoorController : MonoBehaviour
{
    public int needKey;

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.TryGetComponent(out PlayerController _))
        {
            InventoryController.Instance.SetNeededKeyDoor(needKey, gameObject);
        }
    }
}
