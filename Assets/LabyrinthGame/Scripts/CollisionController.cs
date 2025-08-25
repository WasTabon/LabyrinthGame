using UnityEngine;

public class CollisionController : MonoBehaviour
{
    public GameObject target;
    
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.TryGetComponent(out PlayerController _))
        {
            LevelController.Instance._maze0f.SetActive(false);
            LevelController.Instance._maze90f.SetActive(false);
            LevelController.Instance._maze180f.SetActive(false);
            LevelController.Instance._mazeM90f.SetActive(false);
        }
    }
}
