using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DryerLintTrap : LaundryObject
{
    public Sprite openCleanSprite;
    public Sprite openDirtySprite;
    public Sprite closedSprite;

    public GameObject OpenLintTrap;
    public GameObject ClosedLintTrap;

    public LaundryButton startButton;

    public SpriteRenderer lintTrapSpriteRenderer;

    private Dryer dryer;
    private bool open;

    private void Start() {
        dryer = GetComponentInParent<Dryer>();
    }

    public override void OnInteract() {
        if (dryer.state != DryerState.Running) {
            if (!open) {
                Open();
            }
            else if (open && !dryer.lintTrapClean) {
                Clean();
            }
            else if (open && dryer.lintTrapClean) {
                Close();
            }
        }
    }

    private void Open() {
        ClosedLintTrap.SetActive(false);
        OpenLintTrap.SetActive(true);
        if (dryer.lintTrapClean) {
            lintTrapSpriteRenderer.sprite = openCleanSprite;
        }
        else {
            lintTrapSpriteRenderer.sprite = openDirtySprite;
        }
        open = true;
        startButton.locked = true;
        AudioManager.instance.PlaySound(SoundName.OpenLintTrap);
    }

    private void Clean() {
        dryer.CleanLintTrap();
        lintTrapSpriteRenderer.sprite = openCleanSprite;
        AudioManager.instance.PlaySound(SoundName.CleanLintTrap);
    }

    private void Close() {
        //lintTrapSpriteRenderer.sprite = closedSprite;
        OpenLintTrap.SetActive(false);
        ClosedLintTrap.SetActive(true);
        open = false;
        startButton.locked = false;
        AudioManager.instance.PlaySound(SoundName.CloseLintTrap);

    }

    public override InteractionType GetInteractionType() {
        if (!open)
            return InteractionType.Open;

        if (open && !dryer.lintTrapClean)
            return InteractionType.Clean;

        if (open && dryer.lintTrapClean)
            return InteractionType.Close;

        return InteractionType.None;
    }
}
