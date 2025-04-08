using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
namespace ChromaShift {
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(AudioSource))]
    public class CharacterController2D : MonoBehaviour {
        [SerializeField] private float speed = 9;

        [SerializeField] private float walkAcceleration = 75;

        [SerializeField] private float airAcceleration = 30;

        [SerializeField] private float groundDeceleration = 70;

        [SerializeField] private float jumpHeight = 4;

        private AudioSource[] audioSources;

        private BoxCollider2D boxCollider;
        private Vector2 velocity;
        private bool grounded;

        private InputAction moveAction;
        private InputAction jumpAction;

        private Animator animator;
        private void Awake() {
            boxCollider = GetComponent<BoxCollider2D>();
            audioSources = GetComponents<AudioSource>();
            animator = GetComponent<Animator>();
            moveAction = InputSystem.actions.FindAction("Move");
            jumpAction = InputSystem.actions.FindAction("Jump");
        }

        private bool isCollidingWithLever;
        private bool hasSwitchedLever;

        private void FixedUpdate() {
            float moveInput = moveAction.ReadValue<float>();

            if (grounded) {
                velocity.y = 0;

                if (jumpAction.IsPressed()) {
                    velocity.y = Mathf.Sqrt(2 * jumpHeight * Mathf.Abs(Physics2D.gravity.y));

                    AudioSource audioSource = Random.value < 0.5f ? audioSources[0] : audioSources[1];
                    audioSource.Play();
                }
            }

            float acceleration = grounded ? walkAcceleration : airAcceleration;
            float deceleration = grounded ? groundDeceleration : 0;

            if (moveInput != 0) {
                velocity.x = Mathf.MoveTowards(velocity.x, speed * moveInput, acceleration * Time.deltaTime);
            } else {
                velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
            }

            velocity.y += Physics2D.gravity.y * Time.deltaTime;

            transform.Translate(velocity * Time.deltaTime);

            grounded = false;

            // Retrieve all colliders we have intersected after velocity has been applied.
            Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.bounds.extents * 2, 0);

            isCollidingWithLever = false;

            foreach (Collider2D hit in hits) {
                // Ignore our own collider.
                if (hit == boxCollider)
                    continue;
                if (hit.CompareTag("ResetPlayer")) {
                    Debug.Log("Restarting Level");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                }
                if (hit.CompareTag("ColourChanger")) {
                    continue;
                }
                if (hit.CompareTag("Lever")) {
                    isCollidingWithLever = true;
                    if (hasSwitchedLever) continue;
                    hit.gameObject.GetComponent<LeverBehaviour>().Switch();
                    hasSwitchedLever = true;
                    continue;
                }
                if (hit.CompareTag("Finish"))
                {
                    Debug.Log("Loading Next Level");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }


                ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

                // Ensure that we are still overlapping this collider.
                // The overlap may no longer exist due to another intersected collider
                // pushing us out of this one.
                if (!colliderDistance.isOverlapped) continue;
                transform.Translate(colliderDistance.pointA - colliderDistance.pointB);
                // If we intersect an object beneath us, set grounded to true.
                if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90) {
                    grounded = true;
                    BoxCollider2D hitCollider = hit.gameObject.GetComponent<BoxCollider2D>();
                    float y = hitCollider.bounds.max.y + (boxCollider.bounds.extents.y);
                    Vector3 newPos = transform.position;
                    newPos.y = y;
                    if (velocity.y < 0) {
                        velocity.y = 0;
                    }
                    transform.position = newPos;
                }

                if (Vector2.Angle(colliderDistance.normal, Vector2.right) < 90) {
                    BoxCollider2D hitCollider = hit.gameObject.GetComponent<BoxCollider2D>();
                    float x = hitCollider.bounds.max.x + (boxCollider.bounds.extents.x);
                    Vector3 newPos = transform.position;
                    newPos.x = x;
                    if (velocity.x < 0) {
                        velocity.x = 0;
                    }
                    transform.position = newPos;
                }

                if (Vector2.Angle(colliderDistance.normal, Vector2.left) < 90) {
                    BoxCollider2D hitCollider = hit.gameObject.GetComponent<BoxCollider2D>();
                    float x = hitCollider.bounds.min.x - (boxCollider.bounds.extents.x);
                    Vector3 newPos = transform.position;
                    newPos.x = x;
                    if (velocity.x > 0) {
                        velocity.x = 0;
                    }
                    transform.position = newPos;
                }

                if (Vector2.Angle(colliderDistance.normal, Vector2.down) < 90) {
                    BoxCollider2D hitCollider = hit.gameObject.GetComponent<BoxCollider2D>();
                    float y = hitCollider.bounds.min.y - (boxCollider.bounds.extents.y);
                    Vector3 newPos = transform.position;
                    newPos.y = y;
                    if (velocity.y > 0) {
                        velocity.y = 0;
                    }
                    transform.position = newPos;
                }
            }

            if (!isCollidingWithLever) hasSwitchedLever = false;

            if (velocity.x > 0) {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            } else if (velocity.x < 0) {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            animator.SetFloat("speed", Mathf.Abs(velocity.x));
        }

    }
}