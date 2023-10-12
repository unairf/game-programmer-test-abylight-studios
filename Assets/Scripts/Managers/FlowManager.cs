
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AbylightGPT
{
    public class FlowManager : MonoBehaviour
    {
        // Makes FlowManager a singleton, accessible from other scripts
        public static FlowManager Instance { get; private set; }

        private string nextSceneToLoad;
        private AsyncOperation asyncLoadingOperation;

        // Returns the loading progress of the current scene
        public float LoadingProgress
        {
            get
            {
                if (asyncLoadingOperation != null)
                    return Mathf.Clamp01(asyncLoadingOperation.progress / 0.9f);
                else
                    return 0;
            }
        }


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
            // Set the next scene to load and load the loading screen
            nextSceneToLoad = sceneName;
            SceneManager.LoadScene("LoadingScreen");
        }

        public void LoadNextScene()
        {
            if (!string.IsNullOrEmpty(nextSceneToLoad))
                StartCoroutine(LoadNextSceneEnumerator());
        }

        private IEnumerator LoadNextSceneEnumerator()
        {
            // Load scene asynchronously
            asyncLoadingOperation = SceneManager.LoadSceneAsync(nextSceneToLoad, LoadSceneMode.Single);

            // Don't allow the scene to activate until it's fully loaded
            asyncLoadingOperation.allowSceneActivation = false;

            // Wait until the scene is fully loaded
            while (!asyncLoadingOperation.isDone)
            {
                if (asyncLoadingOperation.progress >= 0.9f)
                {
                    // Load has finished. Activate the scene and force garbage collection
                    asyncLoadingOperation.allowSceneActivation = true;
                    nextSceneToLoad = null;
                    ForceGarbageCollector();
                }

                yield return null;
            }
        }

        // Forces garbage collection, separated in a method for scalability
        public void ForceGarbageCollector() => System.GC.Collect();
    }
}
