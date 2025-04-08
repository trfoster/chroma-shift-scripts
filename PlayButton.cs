using UnityEngine;
using UnityEngine.SceneManagement;
namespace ChromaShift {
    public class PlayButton : MonoBehaviour {
        public void OnClick() {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
