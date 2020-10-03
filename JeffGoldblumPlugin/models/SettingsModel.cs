using Jeffsum;

namespace JeffGoldblumPlugin.Models
{
    /// <summary>
    /// Settings Model
    /// </summary>
    public class SettingsModel
    {
        /// <summary>
        /// Count of JeffsumType you wish to receive
        /// </summary>
        public int Count { get; set; } = 5;

        /// <summary>
        /// Jeffsum Type
        /// Words = 0 | Quotes = 1 | Paragraphs = 2
        /// </summary>
        //public JeffsumType JeffsumType { get; set; } = 0;
        public string JeffsumType { get; set; } = "Words";
    }
}
