using UnityEngine;

public class AttachToGameObject : MonoBehaviour
{
    public GameObject target;
    
    void FixedUpdate()
    {
        transform.position = target.transform.position;
    }
}
