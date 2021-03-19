using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashTubRenderer : MonoBehaviour
{
    private WashTub washTub;
    private SpriteRenderer spriteRenderer;

    public Sprite NoSoapSprite;
    public Sprite SoapSprite;

    private void OnEnable() {
        WashTub.SoapLevelChanged += OnSoapLevelChanged;
    }

    private void OnDisable() {
        WashTub.SoapLevelChanged -= OnSoapLevelChanged;
    }

    // Start is called before the first frame update
    void Start()
    {
        washTub = GetComponentInParent<WashTub>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnSoapLevelChanged() {
        if (washTub.IsSoapy)
            spriteRenderer.sprite = SoapSprite;
        else
            spriteRenderer.sprite = NoSoapSprite;
    }
}
