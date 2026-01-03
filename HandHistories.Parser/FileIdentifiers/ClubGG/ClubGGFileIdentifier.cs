using HandHistories.Objects.GameDescription;
using HandHistories.Parser.Utils.Extensions;

namespace HandHistories.Parser.FileIdentifiers.ClubGG
{
    class ClubGGFileIdentifier : IFileIdentifier
    {
        public SiteName Site
        {
            get { return SiteName.ClubGG; }
        }

        public bool Match(string filetext)
        {
            return filetext.StartsWithFast("ClubGG Hand #");
        }
    }
}
