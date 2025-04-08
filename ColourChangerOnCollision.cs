using UnityEngine;

namespace ChromaShift
{
    public class ColourChangerOnCollision : MonoBehaviour
    {
        [SerializeField] public ColourManager.Colour myColour;

        private BoxCollider2D collider;

        private void Awake()
        {
            collider = GetComponent<BoxCollider2D>();

            
        }
        private void FixedUpdate()
        {
            Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, collider.size, 0);

            foreach (Collider2D hit in hits)
            {
                
                // Ignore our own collider.
                if (hit == collider)
                    continue;

                ColliderDistance2D colliderDistance = hit.Distance(collider);
                if (!colliderDistance.isOverlapped) continue;

                if (hit.CompareTag("Player"))
                {
                    ColourManager.Instance.VisibleColour = myColour;
                }
            }
        }
    }
}
