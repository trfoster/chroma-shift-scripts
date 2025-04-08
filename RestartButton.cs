using UnityEngine;
using UnityEngine.SceneManagement;
namespace ChromaShift {
    public class RestartButton : MonoBehaviour {
        public void OnClick() {
            SceneManager.LoadScene(0);
        }
    }
}
