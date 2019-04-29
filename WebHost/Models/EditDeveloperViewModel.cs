using System.ComponentModel.DataAnnotations;

namespace Host.Models
{
    public class EditDeveloperViewModel
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Nickname { get; set; }

        public string[] Tags { get; set; }

        #region For Client side interactions

        public string Project { get; set; }

        public string Url { get; set; }

        #endregion
    }
}
