using Lab_7;
using Newtonsoft.Json;

Console.OutputEncoding = System.Text.Encoding.UTF8;

short MenuPoint;
bool FirstVisitPoint1 = true;
int MaxPlayersLimit = 1;
Console.WriteLine("Текст для коміту");
List<Player> players = new List<Player>();

while (true)
{
    try
    {
        Console.Write("1 - додати гравця\n" +
            "2 – вивести на екран об’єкти\n" +
            "3 – знайти об’єкт\n" +
            "4 – видалити об’єкт\n" +
            "5 – демонстрація поведінки\n" +
            "6 - демонстрація роботи static методів\n" +
            "7 - зберегти колекцію об'єктів у файл\n" +
            "8 - зчитати колекцію об'єктів з файлу\n" +
            "9 - очистити колекцію об'єктів\n" +
            "0 – вийти з програми\n" +
            "-->");
        MenuPoint = short.Parse(Console.ReadLine());
        switch (MenuPoint)
        {
            case 0:
                Environment.Exit(0);
                break;

            //Створення нового гравця
            case 1:
                Console.Clear();
                if (FirstVisitPoint1)
                {
                    bool ErrorMaxPlayersLimit;
                    do
                    {
                        ErrorMaxPlayersLimit = false;
                        try
                        {
                            Console.Write("Введіть максимальну допустиму кількість гравців-->");
                            MaxPlayersLimit = int.Parse(Console.ReadLine());
                            if (MaxPlayersLimit < 1) throw new ArgumentException();
                        }
                        catch (FormatException)
                        {
                            ErrorMaxPlayersLimit = true;
                            Console.WriteLine("Помилка: Неправильний формат введеного значення!\n");
                        }
                        catch (ArgumentException)
                        {
                            ErrorMaxPlayersLimit = true;
                            Console.WriteLine("Помилка: Максимальна кількість гравців не може бути менше одного!\n");
                        }
                    } while (ErrorMaxPlayersLimit);
                    Console.Clear();
                    FirstVisitPoint1 = false;
                }
                if (players.Count == MaxPlayersLimit)
                {
                    Console.WriteLine("Ви створили максимальну кількість гравців.\nВи можете видалити гравця, щоб додати нового...");
                    break;
                }
                bool ErrorNewPlayerPoint = false;
                short NewPlayerPoint = 1;
                do
                {
                    ErrorNewPlayerPoint = false;
                    try
                    {
                        Console.Write("Створити гравця:\n1 - Все вручну\n2 - Вручну ім'я та клас (рекомендовано)\n3 - Вручну тільки ім'я\n4 - Все автоматично\n5 - Ввести рядок з характеристиками (спрощений)\n6 - Ввести рядок з характеристиками (повний)\n-->");
                        NewPlayerPoint = short.Parse(Console.ReadLine());
                        if (NewPlayerPoint < 1 || NewPlayerPoint > 6) throw new ArgumentOutOfRangeException();
                    }
                    catch (FormatException)
                    {
                        ErrorNewPlayerPoint = true;
                        Console.WriteLine("Помилка: Недопустимі символи!\n");
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        ErrorNewPlayerPoint = true;
                        Console.WriteLine("Помилка: Даного варіанту не існує!\n");
                    }
                } while (ErrorNewPlayerPoint);

                string Name = "";
                string ID = "";
                PlayerClass SelectedPlayerClass = (PlayerClass)1;

                //Введення ім'я
                if (NewPlayerPoint < 4)
                {
                    bool ErrorPlayerName;
                    Name = "";
                    do
                    {
                        try
                        {
                            ErrorPlayerName = false;
                            Console.Write("Введіть ім'я нового гравця\n-->");
                            Name = Console.ReadLine();
                            foreach (char s in Name)
                            {
                                if (!((s >= 'a' && s <= 'z') || (s >= 'A' && s <= 'Z') || (s >= '0' && s <= '9') || s == '_')) throw new ArgumentException();
                            }
                            if (Name.Length < 3 || Name.Length > 20) throw new ArgumentOutOfRangeException();
                        }
                        catch (ArgumentOutOfRangeException)
                        {
                            ErrorPlayerName = true;
                            Console.WriteLine("Помилка: Довжина імені повинна бути від 3 до 20 символів!\n");
                        }
                        catch (ArgumentException)
                        {
                            ErrorPlayerName = true;
                            Console.WriteLine("Помилка: Недопустимі символи!\n");
                        }
                    } while (ErrorPlayerName);
                }

                //Введення ID
                if (NewPlayerPoint < 2)
                {
                    bool ErrorIDInput = false;
                    do
                    {
                        ErrorIDInput = false;
                        try
                        {
                            Console.Write("Введіть ID-->");
                            ID = Console.ReadLine();
                            foreach (char s in ID)
                            {
                                if (!(s >= '0' && s <= '9')) throw new FormatException();
                            }
                            if (ID.Length != 6) throw new ArgumentException();
                        }
                        catch (FormatException)
                        {
                            ErrorIDInput = true;
                            Console.WriteLine("Помилка: ID повинно складатися тільки з чисел!\n");
                        }
                        catch (ArgumentException)
                        {
                            ErrorIDInput = true;
                            Console.WriteLine("Помилка: Кількість симоволів в ID повинна бути 6!\n");
                        }
                    } while (ErrorIDInput);
                }
                else ID = GenerateID();

                //Вибір класу
                if (NewPlayerPoint < 3)
                {
                    bool ErrorClass;
                    do
                    {
                        ErrorClass = false;
                        try
                        {
                            Console.Write("Оберіть клас гравця:\n1 - воїн\n2 - стрілець\n3 - маг\n4 - призивач\n-->");
                            int pc = int.Parse(Console.ReadLine());
                            if (pc < 1 || pc > 4) throw new ArgumentException();
                            SelectedPlayerClass = (PlayerClass)pc;
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Помилка: Неправильний формат введеного значення!\n");
                            ErrorClass = true;
                        }
                        catch (ArgumentException)
                        {
                            Console.WriteLine("Даного класу не існує...\n");
                            ErrorClass = true;
                        }
                    } while (ErrorClass);
                }

                if (NewPlayerPoint == 5)
                {
                    Console.WriteLine("\nВведіть дані для створення нового гравця:\n" +
                        "Дані потрібно ввести за наступною формулою: Ім'я_гравця;ID_гравця;Номер_класу_гравця\n" +
                        "(Приклад: Player_000000;121212;1)\n" +
                        "(Номери класів: 1 - воїн, 2 - стрілець, 3 - маг, 4 - призивач)\n");
                    bool ErrorTryParse = false;
                    string ErrorText;
                    Player NewPlayer;
                    do
                    {
                        ErrorTryParse = false;
                        Console.Write("-->");
                        string s = Console.ReadLine();
                        s = s + ";1;0";
                        Console.WriteLine(s);
                        ErrorTryParse = !Player.TryParse(s, out NewPlayer, out ErrorText);
                        if (ErrorTryParse) Console.WriteLine("Помилка: " + ErrorText);
                    } while (ErrorTryParse);
                    players.Add(NewPlayer); //Додавання гравця в List (5)
                    Console.Clear();
                    break;
                }

                if (NewPlayerPoint == 6)
                {
                    Console.WriteLine("\nВведіть дані для створення нового гравця:\n" +
                        "Дані потрібно ввести за наступною формулою: Ім'я_гравця;ID_гравця;Номер_класу_гравця;Рівень_гравця;Бонус_гравця\n" +
                        "(Приклад: Player_000000;121212;1;3;6)\n" +
                        "(Номери класів: 1 - воїн, 2 - стрілець, 3 - маг, 4 - призивач)\n");
                    bool ErrorTryParse = false;
                    string ErrorText;
                    Player NewPlayer;
                    do
                    {
                        ErrorTryParse = false;
                        Console.Write("-->");
                        string s = Console.ReadLine();
                        ErrorTryParse = !Player.TryParse(s, out NewPlayer, out ErrorText);
                        if (ErrorTryParse) Console.WriteLine("Помилка: " + ErrorText);
                    } while (ErrorTryParse);
                    players.Add(NewPlayer); //Додавання гравця в List (6)
                    Console.Clear();
                    break;
                }
                //Додавання гравців
                if (NewPlayerPoint == 4) players.Add(new Player() { Name = "Player_" + ID, ID = ID, PClass = SelectedPlayerClass});
                if (NewPlayerPoint == 3) players.Add(new Player(Name));
                if (NewPlayerPoint == 2) players.Add(new Player(Name, SelectedPlayerClass));
                if (NewPlayerPoint == 1) players.Add(new Player(Name, ID, SelectedPlayerClass));

                Console.Clear();
                break;

            //Виведення інформації про гравців на екран
            case 2:
                if (players.Count == 0)
                {
                    Console.Write("Немає створених гравців...\n");
                }
                else
                {
                    Console.Clear();
                    for (int i = 0; i < players.Count; i++)
                    {
                        Console.Write((i + 1) + ". " + players[i].Information());
                        Console.Write("\n\n-------------------------\n\n");
                    }
                    Console.WriteLine("Кількість гравців: " + Player.NumberOfPlayers);
                    Console.ReadKey();
                    Console.Clear();
                }
                break;

            //Пошук гравців
            case 3:
                if (players.Count == 0)
                {
                    Console.Write("Немає створених гравців...\n");
                }
                else
                {
                    Console.Clear();
                    int FindPoint = 1;
                    bool ErrorFindPoint = false;
                    do
                    {
                        ErrorFindPoint = false;
                        try
                        {
                            Console.Write("1 - за іменем\n2 - за ID\n-->");
                            FindPoint = int.Parse(Console.ReadLine());
                            if (FindPoint < 1 || FindPoint > 2) throw new ArgumentException();
                        }
                        catch (FormatException)
                        {
                            ErrorFindPoint = true;
                            Console.WriteLine("Помилка: Неправильний формат введеного значення!\n");
                        }
                        catch (ArgumentException)
                        {
                            ErrorFindPoint = true;
                            Console.WriteLine("Помилка: Даного варіанта не існує!\n");
                        }
                    } while (ErrorFindPoint);
                    Console.Clear();
                    switch (FindPoint)
                    {
                        case 1: //Пошук гравця за ім'ям
                            Console.Write("Введіть ім'я гравця-->");
                            string SearchedName = Console.ReadLine();
                            Console.Clear();
                            List<Player> finding_players = players.FindAll(x => x.Name == SearchedName);
                            if (finding_players.Count != 0)
                            {
                                for (int i = 0; i < finding_players.Count; i++)
                                {
                                    Console.Write((i + 1) + ". " + finding_players[i].Information());
                                    Console.Write("\n\n-------------------------\n\n");
                                }
                            }
                            else Console.WriteLine("Не вдалося знайти гравця з таким іменем...");
                            Console.ReadKey();
                            Console.Clear();
                            break;

                        case 2: //Пошук гравця за ID
                            Console.Write("Введіть ID гравця-->");
                            string SearchedID = Console.ReadLine();
                            Console.Clear();
                            Player? finding_player = players.Find(x => x.ID == SearchedID);
                            if (finding_player != null) Console.WriteLine(finding_player.Information());
                            else Console.WriteLine("Не вдалося знайти гравця з таким ID...");
                            Console.ReadKey();
                            Console.Clear();
                            break;
                    }
                }
                break;

            //Видалення гравця
            case 4:
                if (players.Count == 0)
                {
                    Console.Write("Немає створених гравців...\n");
                }
                else
                {
                    Console.Clear();
                    int DeletePoint = 1;
                    bool ErrorFindingByIdOrName = false;
                    do
                    {
                        ErrorFindingByIdOrName = false;
                        try
                        {
                            Console.Write("1 - за індексом\n2 - за ID\n-->");
                            DeletePoint = int.Parse(Console.ReadLine());
                            if (DeletePoint < 1 || DeletePoint > 2) throw new ArgumentException();
                        }
                        catch (FormatException)
                        {
                            ErrorFindingByIdOrName = true;
                            Console.WriteLine("Помилка: Неправильний формат введеного значення!\n");
                        }
                        catch (ArgumentException)
                        {
                            ErrorFindingByIdOrName = true;
                            Console.WriteLine("Помилка: Даного варіанту не існує!\n");
                        }
                    } while (ErrorFindingByIdOrName);
                    Console.Clear();
                    for (int i = 0; i < players.Count; i++)
                    {
                        Console.Write((i + 1) + ". " + players[i].Information());
                        Console.Write("\n\n-------------------------\n\n");
                    }
                    switch (DeletePoint)
                    {
                        case 1: //Видалення гравця за індексом
                            int DelIn = 0;
                            bool ErrorDelIn;
                            do
                            {
                                ErrorDelIn = false;
                                try
                                {
                                    Console.Write($"Введіть індекс гравця, якого хочете видалити-->");
                                    DelIn = int.Parse(Console.ReadLine()) - 1;
                                    if (DelIn < 0 || DelIn >= players.Count) throw new ArgumentOutOfRangeException();
                                }
                                catch (FormatException)
                                {
                                    ErrorDelIn = true;
                                    Console.WriteLine("Помилка: Неправильний формат введеного значення!\n");
                                }
                                catch (ArgumentOutOfRangeException)
                                {
                                    ErrorDelIn = true;
                                    Console.WriteLine("Помилка: Невірний індекс\n");
                                }
                            } while (ErrorDelIn);
                            players.RemoveAt(DelIn);
                            Player.DeletePlayer();
                            Console.WriteLine("Гравця видалено!");
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        case 2: //Видалення гравця за ID
                            string DelID = "";
                            bool ErrorWithID = false;
                            do
                            {
                                ErrorWithID = false;
                                try
                                {
                                    Console.Write("Введіть ID гравця, якого хочете видалити-->");
                                    DelID = Console.ReadLine();
                                    foreach (char s in DelID)
                                    {
                                        if (!(s >= '0' && s <= '9')) throw new FormatException();
                                    }
                                    if (DelID.Length != 6) throw new ArgumentException();
                                }
                                catch (FormatException)
                                {
                                    ErrorWithID = true;
                                    Console.WriteLine("Помилка: ID повинно складатися тільки з чисел!\n");
                                }
                                catch (ArgumentException)
                                {
                                    ErrorWithID = true;
                                    Console.WriteLine("Помилка: Кількість симоволів в ID повинна бути 6!\n");
                                }
                            } while (ErrorWithID);
                            Player? deleted = players.Find(x => x.ID == DelID);
                            if (deleted != null)
                            {
                                players.Remove(deleted);
                                Player.DeletePlayer();
                                Console.WriteLine("Гравця видалено!");
                            }
                            else Console.WriteLine("Гравця з таким ID не знайдено!");
                            Console.ReadKey();
                            Console.Clear();
                            break;
                    }
                }
                break;

            //Демонстрація поведінки
            case 5:
                if (players.Count == 0)
                {
                    Console.Write("Немає створених гравців...\n");
                }
                else
                {
                    Console.Clear();
                    int SelectedPlayer = 0;
                    bool ErrorSelectedPlayer;
                    do
                    {
                        ErrorSelectedPlayer = false;
                        try
                        {
                            for (int i = 0; i < players.Count; i++)
                            {
                                Console.Write((i + 1) + ". " + players[i].Information());
                                Console.Write("\n\n-------------------------\n\n");
                            }
                            Console.Write("Оберіть гравця-->");
                            SelectedPlayer = int.Parse(Console.ReadLine());
                            if (SelectedPlayer < 1 || SelectedPlayer > players.Count) throw new ArgumentException();
                        }
                        catch (FormatException)
                        {
                            ErrorSelectedPlayer = true;
                            Console.WriteLine("Помилка: Неправильний формат введеного значення!\n");
                        }
                        catch (ArgumentException)
                        {
                            ErrorSelectedPlayer = true;
                            Console.WriteLine("Помилка: Даного гравця не існує!\n");
                        }
                    }
                    while (ErrorSelectedPlayer);
                    SelectedPlayer--;
                    Console.Clear();
                    int SelectedQuest = 1;
                    bool ErrorSelectedQuest;
                    do
                    {
                        ErrorSelectedQuest = false;
                        try
                        {
                            Console.WriteLine("Ви обрали гравця " + players[SelectedPlayer].Name);
                            Console.Write("1 - піти на завдання\n2 - вбити монстра\n-->");
                            SelectedQuest = int.Parse(Console.ReadLine());
                            if (SelectedQuest < 1 || SelectedQuest > 2) throw new ArgumentException();
                        }
                        catch (FormatException)
                        {
                            ErrorSelectedQuest = true;
                            Console.WriteLine("Помилка: Неправильний формат введеного значення!\n");
                        }
                        catch (ArgumentException)
                        {
                            ErrorSelectedQuest = true;
                            Console.WriteLine("Помилка: Даного варіанту не існує!\n");
                        }
                    }
                    while (ErrorSelectedQuest);
                    Console.Clear();
                    Random r = new Random();
                    switch (SelectedQuest)
                    {
                        case 1:
                            int bonusBoost = r.Next(1, 7);
                            Console.WriteLine("Під час виконання завдання ви знайшли якийсь артефакт.\nВаш бонус збільшено на " + bonusBoost + "!");
                            players[SelectedPlayer].BonusUp(bonusBoost);
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        case 2:
                            bool MonsterBoss = false;
                            int Monster = r.Next(1, 4);
                            int MonsterPower;
                            if (Monster == 1) MonsterBoss = true;
                            if (MonsterBoss) MonsterPower = r.Next(10, 20);
                            else MonsterPower = r.Next(1, 10);
                            if (MonsterBoss) Console.WriteLine("Вам зустрівся Посилений монстр. Ця битва буде складною...");
                            else Console.WriteLine("Вам зустрівся монстр. Ви будете битися...");
                            Console.WriteLine("(Для перемоги над монстром ваша сила повинна бути більшою за силу монстра)");
                            Console.ReadKey();
                            Console.Clear();
                            Console.WriteLine("Бій:\n\nВаша сила: " + players[SelectedPlayer].Power + "\nСила монстра: " + MonsterPower);
                            Console.ReadKey();
                            Console.WriteLine();
                            if (players[SelectedPlayer].Power > MonsterPower)
                            {
                                if (MonsterBoss)
                                {
                                    Console.WriteLine("Ви перемагаєте!\nЗа перемогу над посиленим монстром ваш рівень збільшено на 2!");
                                    players[SelectedPlayer].LevelUp(2);
                                }
                                else
                                {
                                    Console.WriteLine("Ви перемагаєте!\nЗа перемогу над монстром ваш рівень збільшено на 1!");
                                    players[SelectedPlayer].LevelUp();
                                }
                                players[SelectedPlayer].Win();
                                Console.ReadKey();
                                Console.Clear();

                            }
                            else
                            {
                                Console.WriteLine("Монстр занадто сильний. Ви втікаєте...");
                                Console.ReadKey();
                                Console.Clear();
                            }
                            break;
                    }
                }
                break;

            //Демонстрація роботи static-методів
            case 6:
                Console.Clear();
                Console.WriteLine("Кількість вбитих гравцями монстрів = " + Player.MonstersKilled);
                Console.Write("Бажаєте скинути показник?\n[reset] - бажаю cкинути\n-->");
                string iwantreset = Console.ReadLine();
                if (iwantreset == "reset") Player.ResetMonstersKilled();
                Console.Clear();
                break;


            case 7:
                if (players.Count == 0)
                {
                    Console.Write("Немає створених гравців...\n");
                    break;
                }
                Console.Clear();
                int SavingCollectionPoint = 1;
                bool ErrorSavingCollectionPoint = false;
                do
                {
                    ErrorSavingCollectionPoint = false;
                    try
                    {
                        Console.Write("1 - зберегти у файл *.csv\n2 - зберегти у файл *.json\n-->");
                        SavingCollectionPoint = int.Parse(Console.ReadLine());
                        if (SavingCollectionPoint < 1 || SavingCollectionPoint > 2) throw new ArgumentException();
                    }
                    catch (FormatException)
                    {
                        ErrorSavingCollectionPoint = true;
                        Console.WriteLine("Помилка: Неправильний формат введеного значення!\n");
                    }
                    catch (ArgumentException)
                    {
                        ErrorSavingCollectionPoint = true;
                        Console.WriteLine("Помилка: Даного варіанту не існує!\n");
                    }
                } while (ErrorSavingCollectionPoint);

                switch (SavingCollectionPoint)
                {
                    case 1:
                        Console.Write("Введіть шлях, де буде збережено файл-->");
                        string path = Console.ReadLine();
                        string error;
                        SaveToFileCSV(players, path, out error);
                        if (error != "")
                        {
                            Console.Write(error);
                            Console.ReadKey();
                        }
                            break;

                    case 2:
                        Console.Write("Введіть шлях, де буде збережено файл-->");
                        string path2 = Console.ReadLine();
                        string error2;
                        SaveToFileJson(players, path2, out error2);
                        if (error2 != "")
                        {
                            Console.Write(error2);
                            Console.ReadKey();
                        }
                        break;
                }
                Console.Clear();
                break;

            case 8:
                Console.Clear();
                if (FirstVisitPoint1)
                {
                    bool ErrorMaxPlayersLimit;
                    do
                    {
                        ErrorMaxPlayersLimit = false;
                        try
                        {
                            Console.Write("Введіть максимальну допустиму кількість гравців-->");
                            MaxPlayersLimit = int.Parse(Console.ReadLine());
                            if (MaxPlayersLimit < 1) throw new ArgumentException();
                        }
                        catch (FormatException)
                        {
                            ErrorMaxPlayersLimit = true;
                            Console.WriteLine("Помилка: Неправильний формат введеного значення!\n");
                        }
                        catch (ArgumentException)
                        {
                            ErrorMaxPlayersLimit = true;
                            Console.WriteLine("Помилка: Максимальна кількість гравців не може бути менше одного!\n");
                        }
                    } while (ErrorMaxPlayersLimit);
                    Console.Clear();
                    FirstVisitPoint1 = false;
                }
                    Console.Clear();
                int ReadingFilePoint = 1;
                bool ErrorReadingFile = false;
                do
                {
                    ErrorReadingFile = false;
                    try
                    {
                        Console.Write("1 - зчитати з файлу *.csv\n2 - зчитати з файлу *.json\n-->");
                        ReadingFilePoint = int.Parse(Console.ReadLine());
                        if (ReadingFilePoint < 1 || ReadingFilePoint > 2) throw new ArgumentException();
                    }
                    catch (FormatException)
                    {
                        ErrorReadingFile = true;
                        Console.WriteLine("Помилка: Неправильний формат введеного значення!\n");
                    }
                    catch (ArgumentException)
                    {
                        ErrorReadingFile = true;
                        Console.WriteLine("Помилка: Даного варіанту не існує!\n");
                    }
                } while (ErrorReadingFile);

                switch (ReadingFilePoint)
                {
                    case 1:
                        Console.Write("Введіть шлях до файлу-->");
                        string path = Console.ReadLine();
                        string error;
                        List<Player> Loadedplayers = ReadFromFileCSV(path, out error);
                        players.InsertRange(players.Count, Loadedplayers);
                        if (error != "")
                        {
                            Console.WriteLine(error);
                        }
                        if (players.Count > MaxPlayersLimit) Console.WriteLine("Досягнуто максимальної кількості гравців. Зайві будуть видалені!");
                        for (; players.Count > MaxPlayersLimit;)
                        {
                            players.RemoveAt(players.Count - 1);
                            Player.DeletePlayer();
                        }
                        Player.RecountPlayers(players);
                        Console.ReadKey();
                        break;

                    case 2:
                        Console.Write("Введіть шлях до файлу-->");
                        string path2 = Console.ReadLine();
                        string error2;
                       List<Player> Loadedplayers2 = ReadFromFileJson(path2, out error2);
                        players.InsertRange(players.Count, Loadedplayers2);
                        if (error2 != "")
                        {
                            Console.Write(error2);
                        }
                        if (players.Count > MaxPlayersLimit) Console.WriteLine("Досягнуто максимальної кількості гравців. Зайві будуть видалені!");
                        for (; players.Count > MaxPlayersLimit;)
                        {
                            players.RemoveAt(players.Count - 1);
                            Player.DeletePlayer();
                        }
                        Player.RecountPlayers(players);
                        break;
                }
                Console.ReadKey();
                Console.Clear();
                break;

            case 9:
                players.Clear();
                Player.PlayersClear();
                Console.WriteLine("Колекцію об'єктів очищено!\n");
                break;

            default:
                Console.WriteLine("Даного варіанту не існує...");
                break;
        }
    }
    catch (FormatException)
    {
        Console.WriteLine("Помилка: Неправильний формат введеного значення!\n");
    }
}

string GenerateID()
{
    string ID = "";

    Random r = new Random();
    for (int i = 0; i < 6; i++)
    {
        ID = ID + r.Next(0, 9).ToString();
    }

    return ID;
}

void SaveToFileCSV(List<Player> players, string path, out string? error)
{
    error = null;
    List<string> lines = new List<string>();

    foreach (Player player in players)
    {
        lines.Add(player.ToString());
    }

    try
    {
        File.WriteAllLines(path, lines);
    }
    catch (Exception)
    {
        error = "Помилка: Не вдалося зберегти файл!";
    }
}

List<Player> ReadFromFileCSV(string path, out string error)
{
    error = "";
    List<Player> players = new List<Player>();
    try
    {
        List<string> lines = new List<string>();
        lines = File.ReadAllLines(path).ToList();
        foreach (string item in lines)
        {
            Player player;
            bool result = Player.TryParse(item, out player, out error);
            if (result) players.Add(player);
            else error += "Не вдалося зчитати інформацію з рядка: " + item + "\n";
        }
    }
    catch (IOException)
    {
        error += "Помилка: Не вдалося зчитати інформацію з файла!";
    }
    catch (Exception ex)
    {
        error += ex.Message;
    }
    return players;
}

void SaveToFileJson(List<Player> players, string path, out string error)
{
    error = "";
    try
    {
        string jsonstring = "";
        jsonstring = JsonConvert.SerializeObject(players);
        File.WriteAllText(path, jsonstring);
    }
    catch(Exception)
    {
        error = "Помилка: Не вдалося зберегти файл!";
    }
}

List<Player> ReadFromFileJson(string path, out string error)
{
    List<Player> players = null;
    error = "";

    try
    {
        string text = File.ReadAllText(path);
        players = JsonConvert.DeserializeObject<List<Player>>(text);
    }
    catch (IOException)
    {
        error = "Помилка: Не вдалося зчитати інформацію з файла!";
    }
    catch (Exception ex)
    {
        error = ex.Message;
    }
    return players;
}

