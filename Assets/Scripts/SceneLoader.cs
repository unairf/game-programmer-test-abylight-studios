using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AbylightGPT
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private Image bar;
        [SerializeField] private TMPro.TextMeshProUGUI text;

        private FlowManager flowManager;

        private void Start()
        {
            // Check if the bar and text are assigned
            if (bar == null) Debug.LogError("No bar assigned to SceneLoader", this);
            if (text == null) Debug.LogError("No text assigned to SceneLoader", this);

            // Reset the progress bar
            UpdateProgressBar(0);

            // Get the FlowManager instance and load the next scene
            flowManager = FlowManager.Instance;
            flowManager.LoadNextScene();
        }

        // Update the progress bar progress each frame
        private void Update() => UpdateProgressBar(flowManager.LoadingProgress);

        private void UpdateProgressBar(float progress)
        {
            bar.fillAmount = progress;
            text.text = (progress * 100).ToString("F0") + "%";
        }
    }
}
