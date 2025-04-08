using System;
using UnityEngine;

namespace ChromaShift {
    public class DoorBehaviour : MonoBehaviour {

        [SerializeField] private float distance;
        [SerializeField] private float time = 1;
        [SerializeField] private Direction direction;

        private enum Direction {
            Up,
            Right,
            Down,
            Left
        }

        private Vector2 moveDirection;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start() {
            moveDirection = direction switch {
                Direction.Up => Vector2.up,
                Direction.Right => Vector2.right,
                Direction.Down => Vector2.down,
                Direction.Left => Vector2.left,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private bool isMoving;
        private bool isMovingReversed;
        private float timeElapsed;

        // Update is called once per frame
        private void FixedUpdate() {
            if (!isMoving) return;
            timeElapsed += Time.deltaTime;
            Vector2 moveDelta = moveDirection * Time.deltaTime * distance / time;
            if (isMovingReversed) moveDelta *= -1;
            transform.position += new Vector3(moveDelta.x, moveDelta.y, 0);
            if (timeElapsed >= time) {
                isMoving = false;
                isMovingReversed = !isMovingReversed;
            }
        }

        public void Activate() {
            isMoving = true;
            timeElapsed = 0f;
        }
    }
}
