using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;

[RequireComponent(typeof(SortingGroup))]
public class LaundromatSpriteSort : MonoBehaviour
{
    private SortingGroup sortingGroup;

    // Start is called before the first frame update
    void Start()
    {
        sortingGroup = GetComponent<SortingGroup>();
        sortingGroup.sortingLayerID = SortingLayer.NameToID("Entities");
        sortingGroup.sortingOrder = -(int)(transform.position.y * 100.0f);
    }

    // Update is called once per frame
    void Update()
    {
        sortingGroup.sortingOrder = -(int)(transform.position.y * 100.0f);
    }
}
