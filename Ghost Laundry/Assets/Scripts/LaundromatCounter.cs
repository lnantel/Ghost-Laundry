using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LaundromatCounter : MonoBehaviour
{
    private SortingGroup sortingGroup;

    private void Start() {
        sortingGroup = GetComponent<SortingGroup>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        LaundromatSpriteSort spriteSort = collision.GetComponent<LaundromatSpriteSort>();
        if (spriteSort != null && spriteSort.CanBePlacedOnThings) {
            spriteSort.PlacedOn(sortingGroup);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        LaundromatSpriteSort spriteSort = collision.GetComponent<LaundromatSpriteSort>();
        if (spriteSort != null && spriteSort.CanBePlacedOnThings) {

        }
    }
}
