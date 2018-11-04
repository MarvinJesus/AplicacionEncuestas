using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Entities_POJO
{
    public class PictureForEntity
    {
        private readonly IList<string> AllowedFilesExtensions = new List<string> { ".png", ".jpg", ".jpeg", ".gif" };
        private readonly string Pattern = @"^.([a-z]{3,4})$";
        public byte[] Picture { get; set; }
        private string _extension { get; set; }
        public string Extension
        {
            get
            {
                return GetExtension();
            }
            set
            {
                _extension = value.ToLower();
            }
        }

        private string GetExtension()
        {
            if (!string.IsNullOrEmpty(_extension))
            {
                var validFormat = GetValidFormat();

                if (AllowedFilesExtensions.Contains(validFormat))
                {
                    return GetValidFormat();
                }
            }

            return ".jpg";
        }

        private string GetValidFormat()
        {
            var validFormat = _extension;

            if (_extension.IndexOf(".") != 0)
            {
                validFormat = _extension.Insert(0, ".");
            }

            var result = Regex.Match(validFormat, Pattern);

            return result.Success ? validFormat : ".jpg";
        }
    }
}
