using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AbylightGPT
{
    [Serializable]
    public class Register
    {
        private RegisterType registerType;
        private string comment;
        private int value;

        public Register(RegisterType _registerType, string _comment, int _value)
        {
            registerType = _registerType;
            comment = _comment;
            value = _value;
        }

        public string GetContent()
        {
            return registerType.ToString() +" ["+comment +"] "+value.ToString() +"\n";
        }
    }

    // Used lowercase to match the CSV file, PascalCase is normally used for enums
    public enum RegisterType
    {
        building,
        car,
        character,
    }
}
