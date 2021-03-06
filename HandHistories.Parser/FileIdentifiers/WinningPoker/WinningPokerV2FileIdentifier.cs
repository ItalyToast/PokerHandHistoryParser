﻿using HandHistories.Objects.GameDescription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandHistories.Parser.Utils.Extensions;

namespace HandHistories.Parser.FileIdentifiers.WinningPoker
{
    class WinningPokerV2FileIdentifier : IFileIdentifier
    {
        public SiteName Site
        {
            get { return SiteName.WinningPokerV2; }
        }

        public bool Match(string filetext)
        {
            return filetext.StartsWithFast("Hand #");
        }
    }
}
