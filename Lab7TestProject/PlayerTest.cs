using Lab_7;
using System.Xml.Linq;
namespace Lab7TestProject
{
    [TestClass]
    public class PlayerTest
    {
        [TestInitialize]
        public void TestInitialize()
        {
            for (int i = 0; i < Player.NumberOfPlayers;)
            {
                Player.DeletePlayer();
            }
            Player.ResetMonstersKilled();
        }

        [TestMethod]
        public void InformationTest()
        {
            //Arrange
            Player TestPlayer = new Player("Player", "000000", (PlayerClass)1);
            string expected = "≤Ï'ˇ „‡‚ˆˇ: Player\nID: 000000\n–≥‚ÂÌ¸ „‡‚ˆˇ: 1\n Î‡Ò ÔÂÒÓÌ‡Ê‡: ¬ÓøÌ\n¡ÓÌÛÒ: 0\n—ËÎ‡ (¡ÓÌÛÒ + –≥‚ÂÌ¸): 1\nœÂÂÏÓ„Ë: 0";

            //Act
            string actual = TestPlayer.Information();

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void LevelUpTest_on_1()
        {
            //Arrange
            Player TestPlayer = new Player("", "", (PlayerClass)0);
            int expected = 2;

            //Act
            TestPlayer.LevelUp();
            int actual = TestPlayer.Lvl;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(10, 11)]
        [DataRow(4, 5)]
        [DataRow(-1, 1)]
        public void LevelUpTest_more_than_1(int lvlBoost, int expected)
        {
            //Arrange
            Player TestPlayer = new Player("", "", (PlayerClass)0);

            //Act
            TestPlayer.LevelUp(lvlBoost);
            int actual = TestPlayer.Lvl;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WinTest()
        {
            //Arrange
            Player TestPlayer = new Player("", "", (PlayerClass)0);
            int expected = 1;

            //Act
            TestPlayer.Win();
            int actual = TestPlayer.Wins;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow(5, 5)]
        [DataRow(-2, 0)]
        public void BonusUpTest(int bonusBoost, int expected)
        {
            //Arrange
            Player TestPlayer = new Player("", "", (PlayerClass)0);

            //Act
            TestPlayer.BonusUp(bonusBoost);
            int actual = TestPlayer.Bonus;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DeletePlayerTest_one_player_deleted()
        {
            //Arrange
            Player TestPlayer1 = new Player("", "", (PlayerClass)0);
            Player TestPlayer2 = new Player("", "", (PlayerClass)0);
            int expected_number = 1;

            //Act
            bool actual_delete = Player.DeletePlayer();
            int actual_number = Player.NumberOfPlayers;

            //Assert
            Assert.AreEqual(expected_number, actual_number);
            Assert.IsTrue(actual_delete);
        }

        [TestMethod]
        public void PlayersClearTest()
        {
            //Arrange
            Player TestPlayer1 = new Player("", "", (PlayerClass)0);
            Player TestPlayer2 = new Player("", "", (PlayerClass)0);
            int expected_number = 0;

            //Act
            Player.PlayersClear();
            int actual_number = Player.NumberOfPlayers;

            //Assert
            Assert.AreEqual(expected_number, actual_number);
        }

        public void RecountPlayersTest()
        {
            //Arrange
            string Name = "";
            List<Player> players = new List<Player>();
            players.Add(new Player(Name));
            players.Add(new Player(Name));
            Player newPlayer = new Player("Player");
            int expected_number = 2;

            //Act
            Player.RecountPlayers(players);
            int actual_number = Player.NumberOfPlayers;

            //Assert
            Assert.AreEqual(expected_number, actual_number);
        }

        [TestMethod]
        public void DeletePlayerTest_no_players_deleted()
        {
            //Arrange
            int expected = 0;

            //Act
            bool actual_delete = Player.DeletePlayer();
            int actual = Player.NumberOfPlayers;

            //Assert
            Assert.AreEqual(expected, actual);
            Assert.IsFalse(actual_delete);
        }

        [TestMethod]
        public void ResetMonstersKilledTest()
        {
            //Arrange
            Player TestPlayer1 = new Player("", "", (PlayerClass)0);
            Player TestPlayer2 = new Player("", "", (PlayerClass)0);
            int expected = 3;

            //Act
            TestPlayer1.Win();
            TestPlayer1.Win();
            TestPlayer2.Win();
            int actual = Player.MonstersKilled;

            //Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("Happy")]
        [DataRow("œË‚≥Ú;101010;1;1;0")]
        [DataRow("Pl;101010;1;1;0")]
        [DataRow("adsfasdfasdfasdfasdfasdfadsf;101010;2;1;0")]
        [DataRow("Player;asdf;3;1;0")]
        [DataRow("Player;10101;4;1;0")]
        [DataRow("Player;101010;adf;1;0")]
        [DataRow("Player;101010;-1;1;0")]
        [DataRow("Player;101010;5;asdf;0")]
        [DataRow("Player;101010;1;-1;0")]
        [DataRow("Player;101010;1;1;-1")]
        [DataRow("Player;101010;1;1;asdf")]
        public void ParseTest_throw_Exceptions(string s)
        {
            //Arrange

            //Act + Assert
            Assert.ThrowsException<Exception>(() => Player.Parse(s));
        }

        public void ParseTest_correct_data()
        {
            //Arrange
            string s = "Player;101010;3;1;0";

            //Act
            Player TestPlayer = Player.Parse(s);

            //Assert
            Assert.AreEqual(s, TestPlayer.ToString());
        }

        [TestMethod]
        [DataRow("")]
        [DataRow("Happy")]
        [DataRow("œË‚≥Ú;101010;1;1;0")]
        [DataRow("Pl;101010;1;1;0")]
        [DataRow("adsfasdfasdfasdfasdfasdfadsf;101010;2;1;0")]
        [DataRow("Player;asdf;3;1;0")]
        [DataRow("Player;10101;4;1;0")]
        [DataRow("Player;101010;adf;1;0")]
        [DataRow("Player;101010;-1;1;0")]
        [DataRow("Player;101010;5;asdf;0")]
        [DataRow("Player;101010;1;-1;0")]
        [DataRow("Player;101010;1;1;-1")]
        [DataRow("Player;101010;1;1;asdf")]
        public void TryParseTest_incorrect_data(string s)
        {
            //Arrange
            Player TestPlayer;
            string error;

            //Act
            bool res = Player.TryParse(s, out TestPlayer, out error);

            //Assert
            Assert.IsFalse(res);
        }

        [TestMethod]
        public void TryParseTest_correct_data()
        {
            //Arrange
            string s = "Player;121212;4;1;0";
            Player TestPlayer;
            string error;

            //Act
            bool res = Player.TryParse(s, out TestPlayer, out error);

            //Assert
            Assert.IsTrue(res);
        }

        //
        //
        //
        //
        //“≈—“”¬¿ÕÕﬂ ¬À¿—“»¬Œ—“≈…
        //
        //
        //
        //

        [TestMethod]
        public void IDTest_input_incorrect_symbol()
        {
            //Arrange
            Player TestPlayer = new Player("", "", (PlayerClass)1);
            string id = "1212fc";

            //Act + Assert
            Assert.ThrowsException<FormatException>(() => TestPlayer.ID = id);
        }

        [TestMethod]
        public void IDTest_input_incorrect_number_of_symbols()
        {
            //Arrange
            Player TestPlayer = new Player("", "", (PlayerClass)1);
            string id = "12123";

            //Act + Assert
            Assert.ThrowsException<ArgumentException>(() => TestPlayer.ID = id);
        }

        [TestMethod]
        public void IDTest_input_correct()
        {
            //Arrange
            Player TestPlayer = new Player("", "", (PlayerClass)1);
            string id = "121212";

            //Act
            TestPlayer.ID = id;

            //Assert
            Assert.AreEqual(id, TestPlayer.ID);
        }

        [TestMethod]
        [DataRow("Pl")]
        [DataRow("adfadsfasdfasdfasdfasdfasdfasdfasdf")]
        public void NameTest_input_incorrect_number_of_symbols(string name)
        {
            //Arrange
            Player TestPlayer = new Player("", "", (PlayerClass)1);

            //Act + Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => TestPlayer.Name = name);
        }

        [TestMethod]
        public void NameTest_input_incorrect_symbols()
        {
            //Arrange
            Player TestPlayer = new Player("", "", (PlayerClass)1);
            string name = "œÂÂÏÓ„‡!";

            //Act + Assert
            Assert.ThrowsException<ArgumentException>(() => TestPlayer.Name = name);
        }

        [TestMethod]
        public void NameTest_input_correct()
        {
            //Arrange
            Player TestPlayer = new Player("", "", (PlayerClass)1);
            string name = "Player";

            //Act
            TestPlayer.Name = name;

            //Assert
            Assert.AreEqual(name, TestPlayer.Name);
        }

        [TestMethod]
        public void PClassTest_input_incorrect_data()
        {
            //Arrange
            Player TestPlayer = new Player("", "", (PlayerClass)1);
            int ClassNumber = 12;

            //Act + Assert
            Assert.ThrowsException<ArgumentException>(() => TestPlayer.PClass = (PlayerClass)ClassNumber);
        }

        [TestMethod]
        public void PClassTest_input_correct_data()
        {
            //Arrange
            Player TestPlayer = new Player("", "", (PlayerClass)1);
            PlayerClass playerClass = PlayerClass.Ã‡„;

            //Act
            TestPlayer.PClass = playerClass;

            //Assert
            Assert.AreEqual(playerClass, TestPlayer.PClass);
        }
    }
}