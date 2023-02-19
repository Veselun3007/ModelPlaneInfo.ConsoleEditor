using System.Text.RegularExpressions;

namespace ModelPlaneInfo.Entities
{
    /// <summary>
    /// A self-describing class
    /// entity "Aircraft Types"
    /// and contains validation methods
    /// input data
    /// </summary>
    partial class PlaneType
    {
        public static bool CheckName(string value, out string message)
        {
            if (string.IsNullOrEmpty(value))
            {
                message = "Need to enter a name";
                return false;
            }
            if (!Regex.IsMatch(value, @"\A([0-9A-ZА-ЯЇІЄҐЬ!'`]){4,30}\z", RegexOptions.IgnoreCase))
            {
                message = "It is necessary to specify a string of characters denoting the name of the aircraft type";
                return false;
            }
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
