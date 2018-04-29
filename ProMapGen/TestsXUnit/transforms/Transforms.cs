namespace TestsXUnit.transforms
{
    using System;

    /// <summary>
    /// Transforms
    /// </summary>
    public class Transforms
    {
        /// <summary>
        /// String from Guid
        /// </summary>
        /// <param name="g">Guid</param>
        /// <returns>String</returns>
        public static string StringFromGuid(Guid g)
        {
            return g.ToString();
        }

        /// <summary>
        /// Trimmed String from String
        /// </summary>
        /// <param name="s">String</param>
        /// <returns>Trimmed String</returns>
        public static string TrimedStringFromString(string s)
        {
            return s?.Trim();
        }
    }
}
