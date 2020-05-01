using UnityEngine;

public class DestroyParentOnDestroy : MonoBehaviour
{
    // Hack for bad prefab hierarchy :( 

    private void OnDestroy()
    {
        Destroy(gameObject.transform.parent.gameObject);
    }
}
