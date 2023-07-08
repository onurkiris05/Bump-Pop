using UnityEngine.SceneManagement;

namespace Game.Manager
{
    public class SceneController : StaticInstance<SceneController>
    {
        public void LoadNextScene()
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadSceneAsync(nextSceneIndex);
            else
                SceneManager.LoadSceneAsync(0);
        }

        public void RestartScene()
        {
            SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        }
    }
}