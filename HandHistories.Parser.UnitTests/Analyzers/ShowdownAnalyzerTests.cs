using System.Collections.Generic;
using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using HandHistories.Objects.Hand;
using HandHistories.Objects.Players;
using HandHistories.Parser.Analyzers;
using NUnit.Framework;

namespace HandHistories.Parser.UnitTests.Analyzers
{
    [TestFixture]
    public class ShowdownAnalyzerTests
    {
        private static Player P(string name, int seat) => new Player(name, 100m, seat);

        private static HandHistory HandWith(IEnumerable<Player> players, IEnumerable<HandAction> actions)
        {
            var hand = new HandHistory();
            foreach (var p in players) hand.Players.Add(p);
            hand.HandActions = new List<HandAction>(actions);
            return hand;
        }

        [Test]
        public void HeadsUpShowdown_BothShow_BothAreShownAtShowdown()
        {
            var hero = P("Hero", 1);
            var villain = P("Villain", 2);
            hero.HoleCards = HoleCards.FromCards("AsKs");
            villain.HoleCards = HoleCards.FromCards("QhQd");

            var hand = HandWith(new[] { hero, villain }, new[]
            {
                new HandAction("Hero", HandActionType.SMALL_BLIND, 0.5m, Street.Preflop),
                new HandAction("Villain", HandActionType.BIG_BLIND, 1m, Street.Preflop),
                new HandAction("Hero", HandActionType.CALL, 0.5m, Street.Preflop),
                new HandAction("Villain", HandActionType.CHECK, 0m, Street.Preflop),
                new HandAction("Hero", HandActionType.SHOW, 0m, Street.Showdown),
                new HandAction("Villain", HandActionType.SHOW, 0m, Street.Showdown),
            });
            hand.Hero = hero;

            ShowdownAnalyzer.Populate(hand);

            Assert.IsTrue(hand.WentToShowdown);
            Assert.AreEqual(RevealAction.ShownAtShowdown, hero.RevealAction);
            Assert.AreEqual(RevealAction.ShownAtShowdown, villain.RevealAction);
        }

        [Test]
        public void EveryoneFoldsPreflop_NoShowdown_HeroStillHasCards()
        {
            var hero = P("Hero", 1);
            var v1 = P("V1", 2);
            var v2 = P("V2", 3);
            hero.HoleCards = HoleCards.FromCards("7c2d"); // from "Dealt to" line

            var hand = HandWith(new[] { hero, v1, v2 }, new[]
            {
                new HandAction("Hero", HandActionType.SMALL_BLIND, 0.5m, Street.Preflop),
                new HandAction("V1", HandActionType.BIG_BLIND, 1m, Street.Preflop),
                new HandAction("V2", HandActionType.FOLD, 0m, Street.Preflop),
                new HandAction("Hero", HandActionType.FOLD, 0m, Street.Preflop),
            });
            hand.Hero = hero;

            ShowdownAnalyzer.Populate(hand);

            Assert.IsFalse(hand.WentToShowdown);
            Assert.AreEqual(RevealAction.NotShown, hero.RevealAction);
            Assert.IsNotNull(hero.HoleCards, "Hero cards stay known even though not shown to the table.");
            Assert.AreEqual(RevealAction.NotShown, v1.RevealAction);
            Assert.AreEqual(RevealAction.NotShown, v2.RevealAction);
        }

        [Test]
        public void UncontestedWinnerFlashes_IsShownVoluntarily()
        {
            var hero = P("Hero", 1);
            var villain = P("Villain", 2);
            hero.HoleCards = HoleCards.FromCards("AsAh");

            var hand = HandWith(new[] { hero, villain }, new[]
            {
                new HandAction("Hero", HandActionType.SMALL_BLIND, 0.5m, Street.Preflop),
                new HandAction("Villain", HandActionType.BIG_BLIND, 1m, Street.Preflop),
                new HandAction("Hero", HandActionType.RAISE, 3m, Street.Preflop),
                new HandAction("Villain", HandActionType.FOLD, 0m, Street.Preflop),
                new HandAction("Hero", HandActionType.SHOW, 0m, Street.Preflop),
            });
            hand.Hero = hero;

            ShowdownAnalyzer.Populate(hand);

            Assert.IsFalse(hand.WentToShowdown);
            Assert.AreEqual(RevealAction.ShownVoluntarily, hero.RevealAction);
            Assert.AreEqual(RevealAction.NotShown, villain.RevealAction);
        }

        [Test]
        public void MucksAtShowdown_ButCardsLeakedInSummary_IsMuckedAtShowdown()
        {
            var hero = P("Hero", 1);
            var villain = P("Villain", 2);
            hero.HoleCards = HoleCards.FromCards("QcJc");          // from "Dealt to"
            villain.HoleCards = HoleCards.FromCards("AhKh");       // from "shows"

            var hand = HandWith(new[] { hero, villain }, new[]
            {
                new HandAction("Hero", HandActionType.SMALL_BLIND, 0.5m, Street.Preflop),
                new HandAction("Villain", HandActionType.BIG_BLIND, 1m, Street.Preflop),
                new HandAction("Hero", HandActionType.CALL, 0.5m, Street.Preflop),
                new HandAction("Villain", HandActionType.CHECK, 0m, Street.Preflop),
                new HandAction("Villain", HandActionType.SHOW, 0m, Street.Showdown),
                new HandAction("Hero", HandActionType.MUCKS, 0m, Street.Showdown),
            });
            hand.Hero = hero;

            ShowdownAnalyzer.Populate(hand);

            Assert.IsTrue(hand.WentToShowdown);
            Assert.AreEqual(RevealAction.MuckedAtShowdown, hero.RevealAction);
            Assert.IsNotNull(hero.HoleCards, "Mucked hero cards leaked via summary must remain accessible.");
            Assert.AreEqual(RevealAction.ShownAtShowdown, villain.RevealAction);
        }

        [Test]
        public void NullHandOrMissingParts_DoesNotThrow()
        {
            Assert.DoesNotThrow(() => ShowdownAnalyzer.Populate(null));

            var hand = new HandHistory();
            hand.HandActions = null;
            Assert.DoesNotThrow(() => ShowdownAnalyzer.Populate(hand));
        }
    }
}
