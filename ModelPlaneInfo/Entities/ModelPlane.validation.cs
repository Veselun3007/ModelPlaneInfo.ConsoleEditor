using System;
using System.Text.RegularExpressions;

namespace ModelPlaneInfo.Entities
{
    /// <summary>
    /// A self-describing class
    /// entity "Airplane Models"
    /// and contains validation methods
    /// input data
    /// </summary>
    partial class ModelPlane
    {
        public static bool CheckName(string value, out string message)
        {
            if (string.IsNullOrEmpty(value))
            {
                message = "Need to enter a name";
                return false;
            }
            if (!Regex.IsMatch(value, @"\A([0-9A-ZА-ЯЇІЄҐЬ!""''\-\ ]){4,30}\z", RegexOptions.IgnoreCase))
            {
                message = "It is necessary to specify a string of characters denoting the name of the aircraft";
                return false;
            }
            message = "";
            return true;
        }
        public static bool CheckBeginnYear(int? value, out string message)
        {
            if (value == null)
            {
                message = "";
                return true;
            }
            else if (value < 1975 || value > Int32.Parse(DateTime.Now.Year.ToString()))
            {
                message = "The year must be between 1975 and the current year";
                return false;
            }
            message = "";
            return true;
        }
        public static bool CheckUsed(string value, out string message)
        {
            if (value == "") { }
            message = "";
            return true;
        }
        public static bool CheckDescription(string value, out string message)
        {
            if (value == "") { }
            message = "";
            return true;

        }
        public static bool CheckNote(string value, out string message)
        {
            if (value == "") { }
            message = "";
            return true;
        }
    }
}
