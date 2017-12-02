using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Disable if it's a poolable object
        PoolableObject poolableObject = other.GetComponent<PoolableObject>();

        if (poolableObject != null)
        {
            poolableObject.Deactivate();
            return;
        }

        //Otherwise destroy
        GameObject.Destroy(other.gameObject);
    }
}
