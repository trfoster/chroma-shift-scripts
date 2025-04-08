using System.Collections.Generic;
using UnityEngine;

namespace ChromaShift
{
    public class LeverBehaviour : MonoBehaviour {
        [SerializeField] private List<DoorBehaviour> doors;

        private Transform handleTransform;
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start() {
            handleTransform = transform.GetChild(0);
        }

        private bool isTurning;
        private bool isTurningReversed;
        private float timeElapsed;
        
        private void FixedUpdate() {
            if (!isTurning) return;
            timeElapsed += Time.deltaTime;
            float angleDelta = Time.deltaTime * -90f;
            if (isTurningReversed) angleDelta *= -1;
            handleTransform.rotation = Quaternion.Euler(0f, 0f, handleTransform.rotation.eulerAngles.z + angleDelta);
            if (timeElapsed >= 1f) {
                isTurning = false;
                isTurningReversed = !isTurningReversed;
            }
        }

        private bool isSwitchedOn;

        public void Switch() {
            if (isTurning) return;
            isTurning = true;
            timeElapsed = 0f;
            isSwitchedOn = !isSwitchedOn;
            foreach (DoorBehaviour door in doors) {
                door.Activate();
            }
        }
    }
}
