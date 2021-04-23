using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TransitionManager : MonoBehaviour
{
    public static Action TransitionDone;

    public Transform ghost;

    public Transform[] Levels;

    public float walkingSpeed;
    public float dashingSpeed;

    private Animator ghostAnimator;

    // Start is called before the first frame update
    void Start() {
        ghostAnimator = ghost.GetComponentInChildren<Animator>();

        StartCoroutine(TransitionAnimation());
    }

    private IEnumerator TransitionAnimation() {

        while (TimeManager.instance == null)
            yield return null;
        yield return new WaitForSecondsRealtime(0.1f);

        int CurrentDay = TimeManager.instance.CurrentDay;

        for (int i = 0; i < Levels.Length; i++) {
            if (i < CurrentDay) Levels[i].gameObject.GetComponent<LevelTileAnimator>().Flip();
        }

        ghost.position = Levels[Mathf.Clamp(CurrentDay - 1, 0, Levels.Length - 1)].position;

        yield return new WaitForSecondsRealtime(1.0f);

        if(UnityEngine.Random.Range(0.0f, 1.0f) < 0.5f) {
            ghostAnimator.SetBool("Walking", true);

            //Walk over to the next level
            bool arrived = false;
            while (!arrived) {
                ghost.position = Vector3.MoveTowards(ghost.position, Levels[CurrentDay].position, walkingSpeed * Time.unscaledDeltaTime);
                if (Vector3.Distance(ghost.position, Levels[CurrentDay].position) < 0.01f) {
                    arrived = true;
                }
                yield return null;
            }

            ghostAnimator.SetBool("Walking", false);
        }
        else {
            ghostAnimator.SetBool("Dashing", true);
            AudioManager.instance.PlaySound(SoundName.Dash);
            //Dash over to the next level
            bool arrived = false;
            while (!arrived) {
                ghost.position = Vector3.MoveTowards(ghost.position, Levels[CurrentDay].position, dashingSpeed * Time.unscaledDeltaTime);
                if (Vector3.Distance(ghost.position, Levels[CurrentDay].position) < 0.01f) {
                    arrived = true;
                }
                yield return null;
            }

            ghostAnimator.SetBool("Dashing", false);
        }

        yield return new WaitForSecondsRealtime(0.2f);

        //Color the level
        Levels[CurrentDay].gameObject.GetComponent<LevelTileAnimator>().Flip();

        yield return new WaitForSecondsRealtime(1.0f);

        if (TransitionDone != null) TransitionDone();
    }
}
