using Lab_7;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Lab_7
{
    public class Player
    {
        //Поля
        private string name;
        private string id;
        private PlayerClass pclass;
        private int lvl;
        private int bonus;
        private short wins;

        static public int NumberOfPlayers { get; private set; } = 0;

        static public int MonstersKilled { get; private set; } = 0;

        //Властивості
        public string Name
        {
            get { return name; }
            set
            {
                foreach (char s in value)
                {
                    if (!((s >= 'a' && s <= 'z') || (s >= 'A' && s <= 'Z') || (s >= '0' && s <= '9') || s == '_')) throw new ArgumentException();
                }
                if (value.Length < 3 || value.Length > 20) throw new ArgumentOutOfRangeException();
                name = value;
            }
        }

        public string ID
        {
            get { return id; }
            set
            {
                foreach (char s in value)
                {
                    if (!(s >= '0' && s <= '9')) throw new FormatException();
                }
                if (value.Length != 6) throw new ArgumentException();
                id = value;
            }
        }

        //Було змінено
        public int Lvl
        {
            get { return lvl; }
            private set
            {
                if (value < 1) throw new ArgumentException();
                lvl = value;
            }
        }

        [JsonProperty(PropertyName = "Player Class")]
        public PlayerClass PClass
        {
            get { return pclass; }
            set
            {
                if ((int)value < 1 || (int)value > 4) throw new ArgumentException();
                pclass = value;
            }
        }

        public int Bonus
        {
            get { return bonus; }
            private set
            {
                if (value < 0) throw new ArgumentException();
                bonus = value;
            }
        }

        [Newtonsoft.Json.JsonIgnore]
        public short Wins
        {
            get { return wins; }
            private set { wins = value; }
        }

        [Newtonsoft.Json.JsonIgnore]
        public long Power
        {
            get { return Lvl + Bonus; }
        }

        //Конструктори
        public Player()
        {
            Lvl = 1;
            Bonus = 0;
            Wins = 0;
            NumberOfPlayers++;
        }

        public Player(string name) : this(name, "", (PlayerClass)0, 1, 0) { }

        public Player(string name, PlayerClass pclass) : this(name, "", pclass, 1, 0) { }

        public Player(string name, string id, PlayerClass pclass) : this(name, id, pclass, 1, 0) { }

        //Додано новий конструктор для повного введення інформації
        public Player(string name, string id, PlayerClass pclass, int lvl, int bonus)
        {
            if (id == "") id = GenerateID();
            if (name == "") name = "Player_" + id;
            if ((int)pclass == 0) pclass = (PlayerClass)1;
            ID = id;
            Name = name;
            PClass = pclass;
            Lvl = lvl;
            Bonus = bonus;

            Wins = 0;
            NumberOfPlayers++;
        }

        //Методи
        public string Information()
        {
            return "Ім'я гравця: " + Name + "\nID: " + ID + "\nРівень гравця: " + Lvl + "\nКлас персонажа: " + (PlayerClass)PClass + "\nБонус: " + Bonus + "\nСила (Бонус + Рівень): " + Power + "\nПеремоги: " + Wins;
        }

        public override string ToString()
        {
            return $"{Name};{ID};{(int)PClass};{Lvl};{Bonus}";
        }

        public void LevelUp()
        {
            Lvl++;
        }

        public void LevelUp(int lvlBoost)
        {
            if (lvlBoost < 0) return;
            Lvl = Lvl + lvlBoost;
        }

        public void Win()
        {
            Wins++;
            MonstersKilled++;
        }

        public void BonusUp(int bonusBoost)
        {
            if (bonusBoost < 0) return;
            Bonus = Bonus + bonusBoost;
        }

        private string GenerateID()
        {
            short MaxSymbolsInID = 6;

            string generated_id = "";

            Random r = new Random();
            for (int i = 0; i < MaxSymbolsInID; i++)
            {
                generated_id = generated_id + r.Next(0, 9).ToString();
            }

            return generated_id;
        }

        static public bool DeletePlayer()
        {
            if (NumberOfPlayers == 0) return false;
            NumberOfPlayers--;
            return true;
        }

        static public void PlayersClear()
        {
            NumberOfPlayers = 0;
        }

        static public void RecountPlayers(List<Player> players)
        {
            NumberOfPlayers = players.Count();
        }

        static public void ResetMonstersKilled()
        {
            MonstersKilled = 0;
        }

        static public Player Parse(string s)
        {
            if (string.IsNullOrEmpty(s)) throw new Exception("Рядок не може бути пустим!");
            string[] parts = s.Split(";", StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 5) throw new Exception("Невірний формат введення даних!");

            foreach (char symb in parts[0])
            {
                if (!((symb >= 'a' && symb <= 'z') || (symb >= 'A' && symb <= 'Z') || (symb >= '0' && symb <= '9') || symb == '_')) throw new Exception("Недопустимі символи в імені!");
            }
            if (parts[0].Length < 3 || parts[0].Length > 20) throw new Exception("Довжина імені повинна бути від 3 до 20 символів!");

            foreach (char symb in parts[1])
            {
                if (!(symb >= '0' && symb <= '9')) throw new Exception("ID повинно складатися тільки з чисел!");
            }
            if (parts[1].Length != 6) throw new Exception("Кількість симоволів в ID повинна бути 6!");

            foreach (char symb in parts[2])
            {
                if (!(symb >= '0' && symb <= '9')) throw new Exception("Недопустимі символи при вводі класу!");
            }
            if (int.Parse(parts[2]) < 1 || int.Parse(parts[2]) > 4) throw new Exception("Даного класу не існує!");

            foreach (char symb in parts[3])
            {
                if (!int.TryParse(parts[3],out _)) throw new Exception("Недопустимі символи при вводі рівня!");
            }
            if (int.Parse(parts[3]) < 1) throw new Exception("Рівень не може бути нульовим або від'ємним!");

            foreach (char symb in parts[4])
            {
                if (!int.TryParse(parts[4], out _)) throw new Exception("Недопустимі символи при вводі бонуса!");
            }
            if (int.Parse(parts[4]) < 0) throw new Exception("Бонус не може бути від'ємним");

            return new Player(parts[0], parts[1], (PlayerClass)(int.Parse(parts[2])), int.Parse(parts[3]), int.Parse(parts[4]));
        }

        static public bool TryParse(string s, out Player player, out string error)
        {
            player = null;
            error = "";
            bool valid = false;

            try
            {
                player = Parse(s);
                valid = true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            return valid;
        }
    }
}
