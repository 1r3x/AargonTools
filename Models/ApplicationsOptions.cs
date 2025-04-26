using System.Collections.Generic;

namespace AargonTools.Models
{
    public class ApplicationsOptions
    {
        public List<string> Whitelist { get; set; }
        public List<UserWhitelist> UserWhitelists { get; set; }
    }
    public class UserWhitelist
    {
        public string UserId { get; set; }
        public List<string> IPRanges { get; set; }
    }
}
