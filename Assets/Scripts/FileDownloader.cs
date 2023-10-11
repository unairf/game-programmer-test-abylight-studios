using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace AbylightGPT
{
    public class FileDownloader : MonoBehaviour
    {
        [Tooltip("URL of the file to download")]
        [SerializeField] private string fileUrl;

        [Tooltip("Path where the file will be stored. If empty it will be Application.persistentDataPath")]
        [SerializeField] private string folderPath = "";
        
        [Tooltip("Give the file a custom name and extension. Leave empty to use the default one.")]
        [SerializeField] private string fileName = "";
        
        [SerializeField] private UnityEvent<string> OnFileDownloaded;

        
        private string FileFullPath => Path.Combine(folderPath, fileName);

        void Start()
        {
            // Checks if the URL is valid
            if (!IsUrlValid(fileUrl))
                Debug.LogError("File URL is not valid");

            // Checks if the path is valid, otherwise sets it to the persistent data path
            if (!IsPathValid(folderPath))
                folderPath = Application.persistentDataPath;
        }

        public void DownloadFile()
        {
            if (IsUrlValid(fileUrl))
                StartCoroutine(DownloadFileEnumerator(fileUrl));
        }

        private IEnumerator DownloadFileEnumerator(string url)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            www.downloadHandler = new DownloadHandlerFile(FileFullPath);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogError(www.error);
            else
            {
                OnFileDownloaded?.Invoke(FileFullPath);
                Debug.Log("File downloaded: <color=green>" + FileFullPath + "</color>");
            }
        }

        private bool IsUrlValid(string url)
        {
            if (url.Contains(" ") || !url.StartsWith("https://"))
                return false;

            // Add any other conditions to check for valid URLs
            
            return IsStringValid(url);
        }

        // Checks if a path is valid and whether it exists
        private bool IsPathValid(string path)
        {
            if (IsStringValid(path))
                return System.IO.Directory.Exists(path);
            else
                return false;
        }

        // Checks if a string is not null or empty
        private bool IsStringValid(string str) => !string.IsNullOrEmpty(str);
        
    }
}
