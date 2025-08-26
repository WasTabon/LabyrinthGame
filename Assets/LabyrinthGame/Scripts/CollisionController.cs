using UnityEngine;

public enum MazeType
{
    m0f,
    m90f,
    m180f,
    mN90f
}

public class CollisionController : MonoBehaviour
{
    public MazeType target;

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.TryGetComponent(out PlayerController _))
        {
            LevelController.Instance.ActivateMaze(target);
        }
    }
}