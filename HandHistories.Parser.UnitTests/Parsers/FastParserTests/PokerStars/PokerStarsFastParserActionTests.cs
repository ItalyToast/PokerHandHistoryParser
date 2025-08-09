using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using HandHistories.Objects.GameDescription;
using HandHistories.Parser.Parsers.FastParser.PokerStars;
using NUnit.Framework;
using HandHistories.Parser.UnitTests.Parsers.Base;

namespace HandHistories.Parser.UnitTests.Parsers.FastParserTests.PokerStars
{
    using Parser = PokerStarsFastParserImpl;
    [TestFixture]
    class PokerStarsFastParserActionTests : HandHistoryParserBaseTests
    {
        public PokerStarsFastParserActionTests()
            : base("PokerStars")
        {
        }

        protected PokerStarsFastParserImpl GetPokerStarsFastParser()
        {
            return new PokerStarsFastParserImpl();
        }

        [Test]
        public void ParseRegularActionLine_RaisesReachesCap_Works()
        {
            HandAction handAction =
                GetPokerStarsFastParser().ParseRegularActionLine(@"WANGZHEGL: raises $7.10 to $9.01 and has reached the $10 cap", 9, Street.Preflop);

            Assert.AreEqual(new HandAction("WANGZHEGL", HandActionType.RAISE, 9.01m, Street.Preflop), handAction);
        }

        [Test]
        public void ParseRegularActionLine_CallsReachesCap_Works()
        {
            HandAction handAction =
                GetPokerStarsFastParser().ParseRegularActionLine(@"tzuiop23: calls $62.35 and has reached the $80 cap", 8, Street.Preflop);

            Assert.AreEqual(new HandAction("tzuiop23", HandActionType.CALL, 62.35m, Street.Preflop), handAction);
        }

        [Test]
        public void ParseRegularActionLine_BetsReachesCap_Works()
        {
            HandAction handAction =
                GetPokerStarsFastParser().ParseRegularActionLine(@"tzuiop23: bets $62.84 and has reached the $80 cap", 8, Street.Preflop);

            Assert.AreEqual(new HandAction("tzuiop23", HandActionType.BET, 62.84m, Street.Preflop), handAction);
        }

        [Test]
        public void ParseRegularActionLine_Raise_Works()
        {
            HandAction handAction =
                GetPokerStarsFastParser().ParseRegularActionLine(@"chrisvb75: raises $2 to $3.55", 9, Street.Preflop);

            Assert.AreEqual(new HandAction("chrisvb75", HandActionType.RAISE, 3.55m, Street.Preflop), handAction);
        }

        [Test]
        public void ParseRegularActionLine_RaiseAllIn_Works()
        {
            HandAction handAction =
                 GetPokerStarsFastParser().ParseRegularActionLine(@"Piotr280688: raises $8.32 to $12.88 and is all-in", 11, Street.Flop);

            Assert.AreEqual(new HandAction("Piotr280688", HandActionType.RAISE, 12.88m, Street.Flop, true), handAction);
        }

        [Test]
        public void ParseRegularActionLine_BetsAllIn_Works()
        {
            HandAction handAction =
                 GetPokerStarsFastParser().ParseRegularActionLine(@"zeranex88: bets $3.03 and is all-in", 9, Street.Flop);

            Assert.AreEqual(new HandAction("zeranex88", HandActionType.BET, 3.03m, Street.Flop, true), handAction);
        }

        [Test]
        public void ParseRegularActionLine_CallsAllIn_Works()
        {
            HandAction handAction =
                 GetPokerStarsFastParser().ParseRegularActionLine(@"Fjell_konge: calls $7.56 and is all-in", 11, Street.Flop);

            Assert.AreEqual(new HandAction("Fjell_konge", HandActionType.CALL, 7.56m, Street.Flop, true), handAction);
        }

        [Test]
        public void ParseRegularActionLine_Calls_Works()
        {
            HandAction handAction =
               GetPokerStarsFastParser().ParseRegularActionLine(@"MECO-LEO: calls $1.23", 8, Street.Turn);

            Assert.AreEqual(new HandAction("MECO-LEO", HandActionType.CALL, 1.23m, Street.Turn), handAction);            
        }

        [Test]
        public void ParseRegularActionLine_Checks_Works()
        {
            HandAction handAction =
               GetPokerStarsFastParser().ParseRegularActionLine(@"Piotr280688: checks", 11, Street.Turn);

            Assert.AreEqual(new HandAction("Piotr280688", HandActionType.CHECK, 0m, Street.Turn), handAction);
        }

        [Test]
        public void ParseRegularActionLine_Bets_Works()
        {
            HandAction handAction =
               GetPokerStarsFastParser().ParseRegularActionLine(@"MS13ZEN: bets $1.76", 7, Street.River);

            Assert.AreEqual(new HandAction("MS13ZEN", HandActionType.BET, 1.76m, Street.River), handAction);
        }

        [Test]
        public void ParsePostingActionLine_SmallBlind_Works()
        {
            HandAction handAction = Parser.ParsePostingActionLine(@"bingo185: posts small blind $2.55", 8, false, false);

            Assert.AreEqual(new HandAction("bingo185", HandActionType.SMALL_BLIND, 2.55m, Street.Preflop), handAction);
        }

        [Test]
        public void ParsePostingActionLine_BigBlind_Works()
        {
            HandAction handAction = Parser.ParsePostingActionLine(@"gaydaddy: posts big blind $1.23", 8, true, false);

            Assert.AreEqual(new HandAction("gaydaddy", HandActionType.BIG_BLIND, 1.23m, Street.Preflop), handAction);
        }

        [Test]
        public void ParsePostingActionLine_Ante_Works()
        {
            HandAction handAction = Parser.ParsePostingActionLine(@"DallasAP2: posts the ante $1.05", 9, true, true);

            Assert.AreEqual(new HandAction("DallasAP2", HandActionType.ANTE, 1.05m, Street.Preflop), handAction);
        }

        [Test]
        public void ParsePostingActionLine_PostDead_Works()
        {

            //fertre: posts small & big blinds $0.75
            HandAction handAction = Parser.ParsePostingActionLine(@"fertre: posts small & big blinds $1.75", 6, true, true);

            Assert.AreEqual(new HandAction("fertre", HandActionType.POSTS, 1.75m, Street.Preflop), handAction);
        }

        [Test]
        public void ParseRegularActionLine_Folds_Works()
        {
            HandAction handAction =
                GetPokerStarsFastParser().ParseRegularActionLine(@"gaydaddy: folds", 8, Street.Preflop);

            Assert.AreEqual(new HandAction("gaydaddy", HandActionType.FOLD, 0, Street.Preflop), handAction);
        }

        [Test]
        public void ParseUncalledBetLine_Works()
        {
            HandAction handAction =
                GetPokerStarsFastParser().ParseUncalledBetLine(@"Uncalled bet ($6) returned to woeze lenpip", Street.Preflop);

            Assert.AreEqual(new HandAction("woeze lenpip", HandActionType.UNCALLED_BET, 6, Street.Preflop), handAction);
        }
      
        [Test]
        public void ParseCollectedLine_WithSidePot_Works()
        {
            WinningsAction handAction =
                GetPokerStarsFastParser().ParseCollectedLine(@"templargio collected $6.08 from side pot-2", Street.Preflop);

            Assert.AreEqual(new WinningsAction("templargio", WinningsActionType.WINS_SIDE_POT, 6.08m, 2), handAction);
        }

        [Test]
        public void ParseCollectedLine_Works()
        {
            WinningsAction handAction =
                GetPokerStarsFastParser().ParseCollectedLine(@"alecc frost collected $1.25 from pot", Street.Preflop);

            Assert.AreEqual(new WinningsAction("alecc frost", WinningsActionType.WINS, 1.25m, 0), handAction);
        }

        [Test]
        public void ParseCollectedLine_WithColon_Works()
        {
            WinningsAction handAction =
                GetPokerStarsFastParser().ParseCollectedLine(@"wo_olly :D collected $0.57 from pot", Street.Preflop);

            Assert.AreEqual(new WinningsAction("wo_olly :D", WinningsActionType.WINS, 0.57m, 0), handAction);
        }

        [Test]
        public void ParseCollectedLine_MainPot_Works()
        {
            WinningsAction handAction =
                GetPokerStarsFastParser().ParseCollectedLine(@"bozzoTHEclow collected $245.23 from main pot", Street.Preflop);

            Assert.AreEqual(new WinningsAction("bozzoTHEclow", WinningsActionType.WINS, 245.23m, 0), handAction);
        }

        [Test]
        public void ParseCollectedLine_SidePot()
        {
            WinningsAction handAction =
                GetPokerStarsFastParser().ParseCollectedLine(@"bozzoTHEclow collected $136.82 from side pot", Street.Preflop);

            Assert.AreEqual(new WinningsAction("bozzoTHEclow", WinningsActionType.WINS_SIDE_POT, 136.82m, 1), handAction);
        }

        [Test]
        public void ParseCollectedLine_SidePot1()
        {
            WinningsAction handAction =
                GetPokerStarsFastParser().ParseCollectedLine(@"CinderellaBD collected $8.90 from side pot-1", Street.Preflop);

            Assert.AreEqual(new WinningsAction("CinderellaBD", WinningsActionType.WINS_SIDE_POT, 8.90m, 1), handAction);
        }

        [Test]
        public void ParseCollectedLine_SidePot2()
        {
            WinningsAction handAction =
                GetPokerStarsFastParser().ParseCollectedLine(@"CinderellaBD collected $7 from side pot-2", Street.Preflop);

            Assert.AreEqual(new WinningsAction("CinderellaBD", WinningsActionType.WINS_SIDE_POT, 7m, 2), handAction);
        }

        [Test]
        public void ParseMiscShowdownLine_DoesntShow_Works()
        {
            HandAction handAction =
                GetPokerStarsFastParser().ParseMiscShowdownLine(@"woezelenpip: doesn't show hand", 11);

            Assert.AreEqual(new HandAction("woezelenpip", HandActionType.MUCKS, 0m, Street.Showdown), handAction);
        }
       
        [Test]
        public void ParseMiscShowdownLine_PloHiLo_Works()
        {
            HandAction handAction =
                GetPokerStarsFastParser().ParseMiscShowdownLine(@"DOT19: shows [As 8h Ac Kd] (HI: two pair, Aces and Sixes)", -1, GameType.PotLimitOmahaHiLo);

            Assert.AreEqual(new HandAction("DOT19", HandActionType.SHOW, 0m, Street.Showdown), handAction);
        }

        [Test]
        public void ParseMiscShowdownLine_Mucks_Works()
        {
            HandAction handAction =
                GetPokerStarsFastParser().ParseMiscShowdownLine(@"Fjell_konge: mucks hand", 11);

            Assert.AreEqual(new HandAction("Fjell_konge", HandActionType.MUCKS, 0m, Street.Showdown), handAction);
        }        

        [Test]
        public void ParseMiscShowdownLine_Shows_Works()
        {
            HandAction handAction =
                GetPokerStarsFastParser().ParseMiscShowdownLine(@"RECHUK: shows [Ac Qh] (a full house, Aces full of Queens)", 6);

            Assert.AreEqual(new HandAction("RECHUK", HandActionType.SHOW, 0m, Street.Showdown), handAction);
        }

        [Test]
        public void PPPokerHand_ParsesCorrectly()
        {
            // Test that PPPoker hands are parsed correctly using the PokerStars parser
            var parser = new PokerStarsFastParserImpl(SiteName.PPPoker);
            
            // Read the hand history from file
            string pppokerHand = SampleHandHistoryRepository.GetHandExample(PokerFormat.CashGame, Site, "PPPoker", "PPPokerHand");

            var hands = parser.SplitUpMultipleHands(pppokerHand).ToList();
            Assert.AreEqual(1, hands.Count, "Should split into exactly one hand");
            
            // Test that the hand can be parsed
            var handHistory = parser.ParseFullHandHistory(pppokerHand);
            
            // Verify key properties
            Assert.AreEqual(934479950000496, handHistory.HandId[0], "Hand ID should be parsed correctly");
            Assert.AreEqual(GameType.FiveCardPotLimitOmaha, handHistory.GameDescription.GameType, "Game type should be 5 Card Omaha Pot Limit");
            Assert.AreEqual(PokerFormat.CashGame, handHistory.GameDescription.PokerFormat, "Should be cash game format");
            Assert.AreEqual(6, handHistory.Players.Count, "Should have 6 players");
            Assert.AreEqual(93.99m, handHistory.TotalPot, "Total pot should be $93.99");
            Assert.AreEqual(5.10m, handHistory.Rake, "Rake should be $5.10");
            
            // Test that the winner is correctly identified
            var winners = handHistory.Winners;
            Assert.AreEqual(1, winners.Count, "Should have one winner");
            Assert.AreEqual("9673947", winners[0].PlayerName, "Winner should be 9673947");
            Assert.AreEqual(88.89m, winners[0].Amount, "Winner amount should be $88.89");
        }

        [Test]
        public void PPPokerHandSplitRegex_Works()
        {
            var parser = new PokerStarsFastParserImpl(SiteName.PPPoker);
            
            string multipleHands = @"PPPoker Hand #934479950000496:  5 Card Omaha Pot Limit ($0.30/$0.60 USD) - 2025/06/21 17:47:21 UTC
Table 'ClubId:2990864,Table:PPP_93447995' 6-max Seat #2 is the button
*** HOLE CARDS ***
*** SUMMARY ***
Total pot $93.99 | Rake $5.10

PPPoker Hand #934479950000497:  Hold'em No Limit ($0.50/$1.00 USD) - 2025/06/21 17:48:45 UTC
Table 'ClubId:2990864,Table:PPP_93447995' 6-max Seat #3 is the button
*** HOLE CARDS ***
*** SUMMARY ***
Total pot $50.00 | Rake $2.50";

            var hands = parser.SplitUpMultipleHands(multipleHands).ToList();
            Assert.AreEqual(2, hands.Count, "Should split into exactly two hands");
            
            Assert.IsTrue(hands[0].StartsWith("PPPoker Hand #934479950000496"), "First hand should start with correct PPPoker hand ID");
            
            Assert.IsTrue(hands[1].StartsWith("PPPoker Hand #934479950000497"), "Second hand should start with correct PPPoker hand ID");
        }

        [Test]
        public void PPPokerShowdownHand_ParsesHoleCardsFromSummary()
        {
            var parser = new PokerStarsFastParserImpl(SiteName.PPPoker);
            
            string pppokerHand = SampleHandHistoryRepository.GetHandExample(PokerFormat.CashGame, Site, "PPPoker", "ShowdownHand");

            var handHistory = parser.ParseFullHandHistory(pppokerHand);
            
            // Verify that hole cards are parsed from the summary section
            Assert.IsTrue(handHistory.Players["11641651"].hasHoleCards, "Player 11641651 should have hole cards");
            Assert.IsTrue(handHistory.Players["10284118"].hasHoleCards, "Player 10284118 should have hole cards");
            
            // Verify the specific hole cards
            var player1Cards = handHistory.Players["11641651"].HoleCards;
            var player2Cards = handHistory.Players["10284118"].HoleCards;
            
            Assert.AreEqual("8dThTcJhKh", player1Cards.ToString(), "Player 11641651 should have correct hole cards");
            Assert.AreEqual("8sKcAsAh5c", player2Cards.ToString(), "Player 10284118 should have correct hole cards");
            
            // Verify that players who didn't show cards don't have hole cards
            Assert.IsFalse(handHistory.Players["11856112"].hasHoleCards, "Player 11856112 should not have hole cards");
            Assert.IsFalse(handHistory.Players["11270058"].hasHoleCards, "Player 11270058 should not have hole cards");
        }
    }
}
