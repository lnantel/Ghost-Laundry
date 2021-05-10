// This code is part of the Fungus library (https://github.com/snozbot/fungus)
// It is released for free under the MIT open source license (https://github.com/snozbot/fungus/blob/master/LICENSE)

﻿using UnityEngine;
using UnityEngine.EventSystems;

namespace Fungus
{
    /// <summary>
    /// Supported modes for clicking through a Say Dialog.
    /// </summary>
    public enum ClickMode
    {
        /// <summary> Clicking disabled. </summary>
        Disabled,
        /// <summary> Click anywhere on screen to advance. </summary>
        ClickAnywhere,
        /// <summary> Click anywhere on Say Dialog to advance. </summary>
        ClickOnDialog,
        /// <summary> Click on continue button to advance. </summary>
        ClickOnButton
    }

    /// <summary>
    /// Input handler for say dialogs.
    /// </summary>
    public class DialogInput : MonoBehaviour
    {
        [Tooltip("Click to advance story")]
        [SerializeField] protected ClickMode clickMode;

        [Tooltip("Delay between consecutive clicks. Useful to prevent accidentally clicking through story.")]
        [SerializeField] protected float nextClickDelay = 0f;

        [Tooltip("Allow holding Cancel to fast forward text")]
        [SerializeField] protected bool cancelEnabled = true;

        [Tooltip("Ignore input if a Menu dialog is currently active")]
        [SerializeField] protected bool ignoreMenuClicks = true;

        protected bool dialogClickedFlag;

        protected bool nextLineInputFlag;

        protected float ignoreClickTimer;

        protected bool customButtonClickedFlag;

        protected bool fastForwarding;

        protected bool ignoreClicksFlag;

        protected StandaloneInputModule currentStandaloneInputModule;

        protected Writer writer;

        protected virtual void Awake()
        {
            writer = GetComponent<Writer>();

            CheckEventSystem();
        }

        // There must be an Event System in the scene for Say and Menu input to work.
        // This method will automatically instantiate one if none exists.
        protected virtual void CheckEventSystem()
        {
            EventSystem eventSystem = GameObject.FindObjectOfType<EventSystem>();
            if (eventSystem == null)
            {
                // Auto spawn an Event System from the prefab
                GameObject prefab = Resources.Load<GameObject>("Prefabs/EventSystem");
                if (prefab != null)
                {
                    GameObject go = Instantiate(prefab) as GameObject;
                    go.name = "EventSystem";
                }
            }
        }
            
        protected virtual void Update()
        {
            if (EventSystem.current == null)
            {
                return;
            }

            if (currentStandaloneInputModule == null)
            {
                currentStandaloneInputModule = EventSystem.current.GetComponent<StandaloneInputModule>();
            }

            if (writer != null && writer.IsWriting)
            {
                if (Input.GetButtonDown(currentStandaloneInputModule.submitButton) ||
                    (cancelEnabled && Input.GetButton(currentStandaloneInputModule.cancelButton)) ||
                    fastForwarding)
                {
                    SetNextLineFlag();
                }
            }

            switch (clickMode)
            {
            case ClickMode.Disabled:
                break;
            case ClickMode.ClickAnywhere:
                if (Input.GetMouseButtonDown(0) || Input.GetButtonDown("PickUp") || Input.GetButtonDown("Interact"))
                {
                    SetNextLineFlag();
                }
                break;
            case ClickMode.ClickOnDialog:
                if (dialogClickedFlag)
                {
                    SetNextLineFlag();
                    dialogClickedFlag = false;
                }
                break;
            }

            if (ignoreClickTimer > 0f)
            {
                ignoreClickTimer = Mathf.Max (ignoreClickTimer - Time.deltaTime, 0f);
            }

            if (ignoreMenuClicks)
            {
                // Ignore input events if a Menu is being displayed
                if (MenuDialog.ActiveMenuDialog != null && 
                    MenuDialog.ActiveMenuDialog.IsActive() &&
                    MenuDialog.ActiveMenuDialog.DisplayedOptionsCount > 0)
                {
                    dialogClickedFlag = false;
                    nextLineInputFlag = false;
                }
            }

            if (customButtonClickedFlag) {
                nextLineInputFlag = false;
                customButtonClickedFlag = false;
            }

            if (ignoreClicksFlag) {
                nextLineInputFlag = false;
            }

            // Tell any listeners to move to the next line
            if (nextLineInputFlag)
            {
                var inputListeners = gameObject.GetComponentsInChildren<IDialogInputListener>();
                for (int i = 0; i < inputListeners.Length; i++)
                {
                    var inputListener = inputListeners[i];
                    inputListener.OnNextLineEvent();
                }
                nextLineInputFlag = false;
            }
        }

        #region Public members

        /// <summary>
        /// Trigger next line input event from script.
        /// </summary>
        public virtual void SetNextLineFlag()
        {
            nextLineInputFlag = true;
        }

        /// <summary>
        /// Set the dialog clicked flag (usually from an Event Trigger component in the dialog UI).
        /// </summary>
        public virtual void SetDialogClickedFlag()
        {
            // Ignore repeat clicks for a short time to prevent accidentally clicking through the character dialogue
            if (ignoreClickTimer > 0f)
            {
                return;
            }
            ignoreClickTimer = nextClickDelay;

            // Only applies in Click On Dialog mode
            if (clickMode == ClickMode.ClickOnDialog)
            {
                dialogClickedFlag = true;
            }
        }

        /// <summary>
        /// Sets the button clicked flag.
        /// </summary>
        public virtual void SetButtonClickedFlag()
        {
            // Only applies if clicking is not disabled
            if (clickMode != ClickMode.Disabled)
            {
                SetNextLineFlag();
            }
        }

        /// <summary>
        /// Sets the custom button clicked flag.
        /// Prevents NextLineEvent if the click was on a custom button.
        /// </summary>
        public virtual void SetCustomButtonClickedFlag() {
            customButtonClickedFlag = true;
        }

        /// <summary>
        /// Toggles fast-forwarding.
        /// </summary>
        public virtual void ToggleFastForwarding() {
            if(Input.GetMouseButtonDown(0))
                fastForwarding = !fastForwarding;
        }

        /// <summary>
        /// Resets fast-forwarding flag. Called when a dialog event is completed.
        /// </summary>
        public void ResetFastForwardingFlag() {
            fastForwarding = false;
        }

        /// <summary>
        /// Sets ignore clicks flag. Prevents NextLineEvent until reset.
        /// </summary>
        public void SetIgnoreClicksFlag() {
            ignoreClicksFlag = true;
        }

        /// <summary>
        /// Resets ignore clicks flag. Permits NextLineEvent until set.
        /// </summary>
        public void ResetIgnoreClicksFlag() {
            ignoreClicksFlag = false;
        }

        #endregion
    }
}
