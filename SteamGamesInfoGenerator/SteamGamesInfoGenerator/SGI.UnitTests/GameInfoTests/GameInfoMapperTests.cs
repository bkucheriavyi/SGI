using Microsoft.VisualStudio.TestTools.UnitTesting;
using SGI.Core.GameInfo;

namespace SGI.UnitTests.GameInfoTests
{
    [TestClass]
    public class SteamGamesTextParserTests
    {
        [TestMethod]
        public void TextToEntityWithWellFormattedSegments()
        {
            //given
            var gameInfo = new GameInfo
            {
                Name = "Segment One",
                ActivationInfo = "Segment Two"
            };
            var fileParser = new GameInfoMapper();
            //when
            var result = fileParser.TextToEntity(@"Segment One | Segment Two");
            //then
            Assert.AreEqual(gameInfo.Name, result.Name);
            Assert.AreEqual(gameInfo.ActivationInfo, result.ActivationInfo);
        }

        [TestMethod]
        public void TextToEntityWithoutWellFormattedSegments()
        {
            //given
            var gameInfo = new GameInfo
            {
                Name = "Segment One",
                ActivationInfo = "Segment Two"
            };
            var fileParser = new GameInfoMapper();
            //when
            var result1 = fileParser.TextToEntity(@"Segment One| Segment Two");
            var result2 = fileParser.TextToEntity(@"Segment One |Segment Two");
            var result3 = fileParser.TextToEntity(@"    Segment One|Segment Two");
            var result4 = fileParser.TextToEntity(@"    Segment One |   Segment Two     ");
            //then
            Assert.AreEqual(gameInfo.Name, result1.Name);
            Assert.AreEqual(gameInfo.ActivationInfo, result1.ActivationInfo);
            Assert.AreEqual(gameInfo.Name, result2.Name);
            Assert.AreEqual(gameInfo.ActivationInfo, result2.ActivationInfo);
            Assert.AreEqual(gameInfo.Name, result3.Name);
            Assert.AreEqual(gameInfo.ActivationInfo, result3.ActivationInfo);
            Assert.AreEqual(gameInfo.Name, result4.Name);
            Assert.AreEqual(gameInfo.ActivationInfo, result4.ActivationInfo);
        }

        [TestMethod]
        public void TextToEntitySegmentsMoreThanExpected()
        {
            //given
            var fileParser = new GameInfoMapper();
            //when
            var result = fileParser.TextToEntity(@"Segment One | Segment Two | Segment Three ");
            //then
            Assert.AreEqual(null, result.Name);
            Assert.AreEqual(null, result.ActivationInfo);
        }
    }
}
