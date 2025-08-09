using HandHistories.Objects.GameDescription;
using HandHistories.Parser.Utils.Extensions;

namespace HandHistories.Parser.FileIdentifiers.PPPoker
{
    class PPPokerFileIdentifier : IFileIdentifier
    {
        public SiteName Site
        {
            get { return SiteName.PPPoker; }
        }

        public bool Match(string filetext)
        {
            return filetext.StartsWithFast("PPPoker ");
        }
    }
}
