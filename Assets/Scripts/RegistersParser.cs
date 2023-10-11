using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AbylightGPT
{
    public class RegistersParser : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI debugText;

        private List<Register> registers = new List<Register>();

        public void ParseRegisters(string fileContent)
        {
            // Checks if the file content is valid, otherwise returns
            if (string.IsNullOrEmpty(fileContent))
            {
                Debug.LogError("Content is null or empty");
                return;
            }

            registers.Clear();
            StoreRegistersFromLines(SplitTextInLines(fileContent));
        }

        private string[] SplitTextInLines(string text)
        {
            return text.Split('\n');
        }

        // Populate the registers list with the data from the CSV file
        private void StoreRegistersFromLines(string[] lines)
        {
            for (int i = 1; i < lines.Length; i++) // First row is the header, so we skip it
            {
                if (!string.IsNullOrEmpty(lines[i]))
                {
                    registers.Add(GetRegisterFromString(lines[i]));
                }
            }

            // Show the registers int the text component
            string text = "";
            foreach (Register register in registers)
            {
                text += register.GetContent();
            }
            debugText.text = text;
        }

        

        private Register GetRegisterFromString(string line)
        {
            
            string[] fields = ParseCsvLine(line);
            RegisterType? registerTypeParsed = GetRegisterTypeFromString(fields[0]);
            string comment = fields[1];
            int? numberParsed = GetIntFromString(fields[2]);

            if (registerTypeParsed == null)
            {
                Debug.LogError("Cannot parse RegisterType value");
                return null;
            }

            if (numberParsed == null)
            {
                Debug.LogError("Cannot parse int value");
                return null;
            }
            
            Debug.Log("RegisterType: " + registerTypeParsed.Value + ", Number: " + numberParsed + ", Comment: " + comment);

            return new Register(registerTypeParsed.Value, comment, numberParsed.Value);
            

        }

        private RegisterType? GetRegisterTypeFromString(string text)
        {
            try
            {
                RegisterType parsedValue = (RegisterType)Enum.Parse(typeof(RegisterType), text);
                return parsedValue;
            }
            catch (ArgumentException)
            {
                Debug.LogError("Cannot parse RegisterType value");
            }
            return null;
        }

        private int? GetIntFromString(string text)
        {
            try
            {
                int parsedValue = int.Parse(text);
                return parsedValue;
            }
            catch (FormatException)
            {
                Debug.LogError("Cannot parse int value: " + text);
            }
            return null;
        }

        private string[] ParseCsvLine(string line)
        {
            List<string> fields = new List<string>();
            bool inQuote = false;
            char currentChar;

            System.Text.StringBuilder currentField = new System.Text.StringBuilder();

            for (int i = 0; i < line.Length; i++)
            {
                currentChar = line[i];

                if (currentChar == '"')
                {
                    inQuote = !inQuote;
                    continue;
                }

                if (currentChar == ',' && !inQuote)
                {
                    fields.Add(currentField.ToString());
                    currentField.Clear();
                }
                else
                {
                    currentField.Append(currentChar);
                }
            }
            fields.Add(currentField.ToString()); // Add the last field

            return fields.ToArray();
        }
    }
}
