using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace ChromaShift {
    [RequireComponent(typeof(Volume))]
    public class ColourManager : MonoBehaviour {
        
        public static ColourManager Instance { get; private set; }

        public static event EventHandler<ColourChangeEventArgs> OnColourChangeEvent;
        public class ColourChangeEventArgs : EventArgs {
            public ColourChangeEventArgs(Colour colour) {
                newColour = colour;
            }
            public Colour newColour;
        }
        
        [SerializeField] private Colour visibleColour = Colour.White;
        
        [SerializeField] public bool canSeeWhite = true;
        public Colour VisibleColour {
            get => visibleColour;
            set {
                visibleColour = value;

                colorAdjustments.colorFilter.overrideState = true;
                colorAdjustments.colorFilter.value = value switch {
                    Colour.Red => redColour,
                    Colour.Green => greenColour,
                    Colour.Blue => blueColour,
                    _ => whiteColour,
                };

                colorAdjustments.active = true;
                Debug.Log($"set color to {visibleColour}");
                OnColourChangeEvent?.Invoke(this, new ColourChangeEventArgs(value));
            }
        }
        public enum Colour {
            Red,
            Green,
            Blue,
            White
        }

        private Volume postProcessVolume;
        private ColorAdjustments colorAdjustments;

        [SerializeField] private Color redColour;
        [SerializeField] private Color greenColour;
        [SerializeField] private Color blueColour;
        [SerializeField] private Color whiteColour;
        
        private InputAction showWhiteAction;
        private InputAction resetLevelAction;

        private CharacterController2D playerController;

        private bool isSetup = false;
        
        void Awake() {
            Instance = this;
            postProcessVolume = GetComponent<Volume>();
            showWhiteAction = InputSystem.actions.FindAction("ShowWhite");
            resetLevelAction = InputSystem.actions.FindAction("ResetLevel");
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController2D>();
            if (!postProcessVolume.profile.TryGet(out colorAdjustments)) {
                colorAdjustments = postProcessVolume.profile.Add<ColorAdjustments>();
            }
            isSetup = true;
        }

        void Start() {
            VisibleColour = visibleColour;
        }

        // Update is called once per frame
        void Update() {
            if (resetLevelAction.WasPressedThisFrame()) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            if (!canSeeWhite) return;
            if (showWhiteAction.WasPressedThisFrame()) {
                postProcessVolume.enabled = false;
                playerController.enabled = false;
                playerController.GetComponent<Animator>().enabled = false;
            } else if (showWhiteAction.WasReleasedThisFrame()) {
                postProcessVolume.enabled = true;
                playerController.enabled = true;
                playerController.GetComponent<Animator>().enabled = true;

            }
        }

        private void OnValidate() {
            if (isSetup) {
                VisibleColour = visibleColour;
            }
        }

    }
}
