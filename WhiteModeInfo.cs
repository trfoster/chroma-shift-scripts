using UnityEngine;

namespace ChromaShift
{
    [RequireComponent (typeof(SpriteRenderer))]
    public class WhiteModeInfo : MonoBehaviour
    {
        SpriteRenderer renderer;
        private void Awake()
        {
            renderer = GetComponent<SpriteRenderer>();
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            renderer.enabled = ColourManager.Instance.canSeeWhite;
        }
    }
}
