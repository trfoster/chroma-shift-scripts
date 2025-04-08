using UnityEngine;

namespace ChromaShift {
    [RequireComponent(typeof(Collider2D))]
    public class ColourCollider : ColourToggleableMonoBehaviour {

        private Collider2D collider;

        void Awake() {
            collider = GetComponent<Collider2D>();
        }

        public override void Enable(ColourManager.Colour newColour) {
            collider.enabled = true;
        }

        public override void Disable(ColourManager.Colour newColour) {
            collider.enabled = false;
        }

    }
}
