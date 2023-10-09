using UnityEngine;
using UnityEngine.SceneManagement;

namespace AbylightGPT
{
    public class FlowManager : MonoBehaviour
    {
        // Makes FlowManager a singleton, accessible from other scripts
        public static FlowManager Instance { get; private set; }

        private void Awake()
        {
            // Assigns the singleton instance if it is null, otherwise destroys the new instance
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }
            else
                Destroy(gameObject);

        }


        /* These methods are called from the context menu of the FlowManager component.
         * [ContextMenu] is added for testing purposes, will be replaced in later tasks.
         */
        [ContextMenu("Go to Init")]
        public void GoToInit()
        {
            LoadScene("Init");
        }

        [ContextMenu("Go to Menu")]
        public void GoToMenu()
        {
            LoadScene("Menu");
        }

        [ContextMenu("Go to InGame")]
        public void GoToInGame()
        {
            LoadScene("InGame");
        }

        private void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
