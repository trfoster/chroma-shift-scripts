using System.Collections.Generic;
using UnityEngine;

namespace ChromaShift {
    public class ColouredObject : MonoBehaviour {

        [SerializeField] private List<ColourManager.Colour> myColour = new();
        [SerializeField]
        private List<ColourToggleableMonoBehaviour> toggleables;

        void Awake() {
            ColourManager.OnColourChangeEvent += ColourManager_OnColourChangeEvent;
        }

        private void OnDestroy() {
            ColourManager.OnColourChangeEvent -= ColourManager_OnColourChangeEvent;
        }

        private void ColourManager_OnColourChangeEvent(object sender, ColourManager.ColourChangeEventArgs info) {
            if (myColour.Contains(info.newColour)) {
                foreach (var item in toggleables) {
                    item?.Enable(info.newColour);
                }
            } else {
                foreach (var item in toggleables) {
                    item?.Disable(info.newColour);
                }
            }
        }

    }

    public abstract class ColourToggleableMonoBehaviour : MonoBehaviour {
        public abstract void Enable(ColourManager.Colour newColour);
        public abstract void Disable(ColourManager.Colour newColour);
    }
}
