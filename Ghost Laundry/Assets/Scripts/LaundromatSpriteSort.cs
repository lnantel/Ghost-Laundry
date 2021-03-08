using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

[RequireComponent(typeof(SortingGroup))]
public class LaundromatSpriteSort : MonoBehaviour
{
    public Transform groundContact;
    public bool CanBePlacedOnThings;

    private SortingGroup sortingGroup;
    private SortingGroup surfaceSortingGroup;

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
        else if (surfaceSortingGroup != null) {
            sortingGroup.sortingLayerID = surfaceSortingGroup.sortingLayerID;
            sortingGroup.sortingOrder = surfaceSortingGroup.sortingOrder + 1;
        }
        else
        {
            sortingGroup.sortingLayerID = SortingLayer.NameToID("Entities");
            sortingGroup.sortingOrder = -(int)(groundContact.position.y * 100.0f);
        }
    }

    public void PlacedOn(SortingGroup other) {
        if(CanBePlacedOnThings)
            surfaceSortingGroup = other;
    }

    public void RemovedFromSurface() {
        surfaceSortingGroup = null;
    }
}
