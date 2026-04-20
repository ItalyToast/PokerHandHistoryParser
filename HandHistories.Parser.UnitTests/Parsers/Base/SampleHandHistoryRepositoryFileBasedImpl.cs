using System.Text;
using HandHistories.Objects.Cards;
using HandHistories.Objects.GameDescription;
using HandHistories.Parser.UnitTests.Utils.IO;
using System;
using System.IO;

namespace HandHistories.Parser.UnitTests.Parsers.Base
{
    internal class SampleHandHistoryRepositoryFileBasedImpl : ISampleHandHistoryRepository
    {
        private static readonly Encoding StrictUtf8 = Encoding.GetEncoding(
            Encoding.UTF8.WebName,
            EncoderFallback.ExceptionFallback,
            DecoderFallback.ExceptionFallback);

        private static readonly Encoding Windows1252 = GetWindows1252();

        private static Encoding GetWindows1252()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            return Encoding.GetEncoding(1252);
        }

        private readonly IFileReader _fileReader;
        private readonly string _version;

        public SampleHandHistoryRepositoryFileBasedImpl(IFileReader fileReader)
        {
            _fileReader = fileReader;
        }

        public SampleHandHistoryRepositoryFileBasedImpl(IFileReader fileReader, string version) : this(fileReader)
        {
            _version = version;
        }

        public string GetCancelledHandHandHistoryText(PokerFormat pokerFormat, SiteName siteName)
        {
            return GetHandText(pokerFormat, siteName, "ValidHandTests", "CancelledHand");
        }

        public string GetValidHandHandHistoryText(PokerFormat pokerFormat, SiteName siteName, bool isValid, int testNumber)
        {
            return GetHandText(pokerFormat, siteName, "ValidHandTests", (isValid ? "ValidHand" : "InvalidHand") + "_" + testNumber);
        }

        public string GetSeatExampleHandHistoryText(PokerFormat pokerFormat, SiteName siteName, SeatType seatType)
        {
            return GetHandText(pokerFormat, siteName, "Seats", seatType.ToString());
        }

        public string GetLimitExampleHandHistoryText(PokerFormat pokerFormat, SiteName siteName, string fileName)
        {
            return GetHandText(pokerFormat, siteName, "Limits", fileName);
        }

        public string GetBuyinExampleHandHistoryText(PokerFormat pokerFormat, SiteName siteName, string fileName)
        {
            return GetHandText(pokerFormat, siteName, "Buyins", fileName);
        }

        public string GetTableExampleHandHistoryText(PokerFormat pokerFormat, SiteName siteName, int tableTestNumber)
        {
            return GetHandText(pokerFormat, siteName, "Tables", "Table" + tableTestNumber);
        }

        public string GetGeneralHandHistoryText(PokerFormat pokerFormat, SiteName siteName, string testName)
        {
            return GetHandText(pokerFormat, siteName, "GeneralHands", testName);
        }

        public string GetFormatHandHistoryText(PokerFormat pokerFormat, SiteName siteName, string name)
        {
            return GetHandText(pokerFormat, siteName, "FormatTests", name);
        }

        public string GetGameTypeHandHistoryText(PokerFormat pokerFormat, SiteName siteName, GameType gameType)
        {
            return GetHandText(pokerFormat, siteName, "GameTypeTests", gameType.ToString());
        }

        public string GetCommunityCardsHandHistoryText(PokerFormat pokerFormat, SiteName siteName, Street street, int testNumber)
        {
            return GetHandText(pokerFormat, siteName, "StreetTests", street.ToString() + (testNumber == 1 ? "" : testNumber.ToString()));
        }

        public string GetMultipleHandExampleText(PokerFormat pokerFormat, SiteName siteName, int handCount)
        {
            return GetHandText(pokerFormat, siteName, "MultipleHandsTests", handCount + "MultipleHands");
        }

        public string GetHandExample(PokerFormat pokerFormat, SiteName siteName, string subFolder, string fileName)
        {
            return GetHandText(pokerFormat, siteName, subFolder, fileName);
        }

        private string GetHandText(PokerFormat pokerFormat, SiteName siteName, string subFolderName, string textFileName)
        {
            string workPath = AppDomain.CurrentDomain.BaseDirectory;
            string subFolder = System.IO.Path.Combine(workPath, GetSampleHandHistoryFolder(pokerFormat, siteName), subFolderName);
            string path = System.IO.Path.Combine(subFolder, textFileName) + ".txt";

            if (_fileReader.FileExists(path) == false)
            {
                return null;
            }

            return ReadSampleFile(path);
        }

        private static string ReadSampleFile(string path)
        {
            byte[] bytes = File.ReadAllBytes(path);

            int start = 0;
            int length = bytes.Length;
            if (length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
            {
                start = 3;
                length -= 3;
            }

            try
            {
                return StrictUtf8.GetString(bytes, start, length);
            }
            catch (DecoderFallbackException)
            {
                return Windows1252.GetString(bytes, start, length);
            }
        }

        private string GetSampleHandHistoryFolder(PokerFormat pokerFormat, SiteName siteName)
        {
            // Use Path.Combine for cross-platform separators; the previous hard-coded
            // "\" broke every file lookup on macOS/Linux, causing NullReferenceException
            // in callers that Split the returned text.
            return System.IO.Path.Combine("SampleHandHistories", siteName.ToString(), pokerFormat.ToString());
        }
    }
}