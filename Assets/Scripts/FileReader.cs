using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace AbylightGPT
{
    public class FileReader : MonoBehaviour
    {
        [Tooltip("Path of the file to read. Leave empty if it'll be set from C#")]
        [SerializeField] private string filePath = "";

        public UnityEvent<string> OnFileRead;
        
        
        public void ReadFile()
        {
            OnFileRead?.Invoke(GetFileContent(filePath));
        }

        public void ReadFile(string path)
        {
            Debug.Log(GetFileContent(path));
            OnFileRead?.Invoke(GetFileContent(path));
        }

        private string GetFileContent(string path)
        {
            // Used File rather than StreamReader for simplicity

            // Checks if the file exists, otherwise returns an empty string
            if (File.Exists(path))
                return File.ReadAllText(path);
            else
            {
                Debug.LogError("File not found: " + path);
                return string.Empty;
            }
        }
    }
}
