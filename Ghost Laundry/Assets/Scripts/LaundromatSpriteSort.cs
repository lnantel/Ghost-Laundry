using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

[RequireComponent(typeof(SortingGroup))]
public class LaundromatSpriteSort : MonoBehaviour
{
    public Transform groundContact;

    private SortingGroup sortingGroup;

    public bool forceToFront;

    // Start is called before the first frame update
    void Start()
    {
        sortingGroup = GetComponent<SortingGroup>();
        sortingGroup.sortingLayerID = SortingLayer.NameToID("Entities");
        if (groundContact == null) groundContact = transform;
        sortingGroup.sortingOrder = -(int)(groundContact.position.y * 100.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (forceToFront) {
            sortingGroup.sortingLayerID = SortingLayer.NameToID("ForceToFront");
            sortingGroup.sortingOrder = 0;
        }
        else {
            sortingGroup.sortingLayerID = SortingLayer.NameToID("Entities");
            sortingGroup.sortingOrder = -(int)(groundContact.position.y * 100.0f);
        }
    }
}
