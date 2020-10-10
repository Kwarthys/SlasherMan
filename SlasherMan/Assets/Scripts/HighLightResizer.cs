using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighLightResizer : MonoBehaviour
{
    //Raw size of the imported png
    private Vector3 currentSize = new Vector3(4.92f, .82f, 4.79f);

    public void resize(Vector3 size)
    {
        Vector3 newSize = new Vector3();
        newSize.x = size.x / currentSize.x;
        newSize.y = size.y / currentSize.y;
        newSize.z = size.z / currentSize.z;

        transform.localScale = newSize;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, currentSize);
    }
}
