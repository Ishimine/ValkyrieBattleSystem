using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sombra : MonoBehaviour {

    public Collider colParent;
    float boundY;
    public LayerMask mask;

	void Awake ()
    {
        colParent = transform.parent.GetComponent<Collider>();
    }

    private void Update()
    {
        boundY = colParent.bounds.extents.y;
        RaycastHit hit;
        if (Physics.Raycast(transform.parent.position - Vector3.down * boundY, Vector3.down, out hit, mask))
        {
            transform.position = hit.point;
        }
    }
}
