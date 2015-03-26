using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HandHistories.Objects.Actions;
using HandHistories.Objects.Cards;
using HandHistories.Objects.GameDescription;
using HandHistories.Parser.Parsers.FastParser.PokerStars;
using NUnit.Framework;
using HandHistories.Parser.Parsers.FastParser.FullTiltPoker;
using HandHistories.Parser.UnitTests.Parsers.Base;

namespace HandHistories.Parser.UnitTests.Parsers.FastParserTests.PokerStars
{
    [TestFixture]
    class FullTiltPokerFastParserActionTests : HandHistoryParserBaseTests 
    {
        public FullTiltPokerFastParserActionTests()
            : base("FullTilt")
        {
        }

        string[] GetBlindTest(string name)
        {
            return SampleHandHistoryRepository.GetHandExample(PokerFormat.CashGame, SiteName.FullTilt, "BlindTests", name)
                .Split(new string[]{"\r\n"}, StringSplitOptions.RemoveEmptyEntries);
        }

        List<HandAction> ParseBlindActions(string name)
        {
            List<HandAction> actions = new List<HandAction>();
            FullTiltPokerFastParserImpl.ParseBlindActions(GetBlindTest(name), ref actions, 0);

            return actions;
        }

        [Test]
        public void ParseBlindActions_Ante_Works()
        {
            var actions = ParseBlindActions("Ante");

            List<HandAction> expectedActions = new List<HandAction>()
                {
                    new HandAction("iason07", HandActionType.ANTE, 0.3m, Street.Preflop, 0),
                    new HandAction("Gaunt_13", HandActionType.ANTE, 0.3m, Street.Preflop, 0),
                    new HandAction("iason07", HandActionType.SMALL_BLIND, 1m, Street.Preflop, 0),
                    new HandAction("Gaunt_13", HandActionType.BIG_BLIND, 2m, Street.Preflop, 0),
                };

            Assert.AreEqual(expectedActions, actions);
        }

        [Test]
        public void ParseBlindActions_Adds_Works()
        {
            var actions = ParseBlindActions("Adding");

            List<HandAction> expectedActions = new List<HandAction>()
                {
                    new HandAction("iason07", HandActionType.SMALL_BLIND, 1m, Street.Preflop, 0),
                    new HandAction("Gaunt_13", HandActionType.BIG_BLIND, 2m, Street.Preflop, 0),
                };

            Assert.AreEqual(expectedActions, actions);
        }
    }
}
