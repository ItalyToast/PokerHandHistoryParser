using System.Collections.Generic;
using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using HandHistories.Objects.Hand;

namespace HandHistories.Parser.Analyzers
{
    /// <summary>
    /// Post-parse pass that populates <see cref="HandHistory.WentToShowdown"/> and
    /// each <see cref="Objects.Players.Player.RevealAction"/> from the already-parsed
    /// action list. Runs once per hand; derivation is a single linear walk.
    /// </summary>
    public static class ShowdownAnalyzer
    {
        public static void Populate(HandHistory hand)
        {
            if (hand == null || hand.Players == null || hand.HandActions == null)
            {
                return;
            }

            var folded = new HashSet<string>();
            var shown = new HashSet<string>();
            var mucked = new HashSet<string>();

            foreach (var action in hand.HandActions)
            {
                switch (action.HandActionType)
                {
                    case HandActionType.FOLD:
                        folded.Add(action.PlayerName);
                        break;
                    case HandActionType.SHOW:
                    case HandActionType.SHOWS_FOR_LOW:
                        shown.Add(action.PlayerName);
                        break;
                    case HandActionType.MUCKS:
                        mucked.Add(action.PlayerName);
                        break;
                }
            }

            int liveAtEnd = hand.Players.Count - folded.Count;
            hand.WentToShowdown = liveAtEnd >= 2;

            foreach (var player in hand.Players)
            {
                if (shown.Contains(player.PlayerName))
                {
                    player.RevealAction = hand.WentToShowdown
                        ? RevealAction.ShownAtShowdown
                        : RevealAction.ShownVoluntarily;
                }
                else if (mucked.Contains(player.PlayerName))
                {
                    player.RevealAction = RevealAction.MuckedAtShowdown;
                }
                else
                {
                    player.RevealAction = RevealAction.NotShown;
                }
            }
        }
    }
}
