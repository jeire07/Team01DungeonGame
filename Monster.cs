using static System.Console;

namespace Team01DungeonGame
{
    public enum MonsterType { minion, insect, canon }

    public class Monster
    {
        public int Level { get; set; }
        public MonsterType Job { get; set; }
        public string Name { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int HP { get; set; }
        public int Gold { get; set; }
        public bool IsAlive { get; set; }

        public Monster(int level, MonsterType job)
        {
            Level = level;
            Job = job;

            switch (job)
            {
                case MonsterType.minion:
                    Name = "미니언";
                    Atk = 5 + level;
                    Def = 0;
                    HP = 15 + level;
                    Gold = 20 + level;
                    break;
                case MonsterType.insect:
                    Name = "공허충";
                    Atk = 5 + level;
                    Def = 0;
                    HP = 10 + level;
                    Gold = 15 + level;
                    break;
                case MonsterType.canon:
                    Name = "대포미니언";
                    Atk = 8 + level;
                    Def = 0;
                    HP = 25 + level;
                    Gold = 33 + level;
                    break;
            }

            IsAlive = true;
        }

        public int TakeDamage(int damage)
        {
            int hp = HP;
            hp -= damage;

            if (hp < 0)
            {
                IsAlive = false;
                return 0;
            }
            else
            {
                return hp;
            }
        }

        public void MonsterInfo(bool withNumber = false, int idx = 0)
        {
            if(IsAlive)
            {
                if (withNumber)
                {
                    ForegroundColor = ConsoleColor.Cyan;
                    Write($"{idx} ");
                    ResetColor();

                    Write("Lv.");
                    ForegroundColor = ConsoleColor.Magenta;
                    Write($"{Level} ");
                    ResetColor();

                    Write($"{Name} HP");
                    ForegroundColor = ConsoleColor.Magenta;
                    WriteLine($"{HP}");
                    ResetColor();
                }
                else
                {
                    Write("Lv.");
                    ForegroundColor = ConsoleColor.Magenta;
                    Write($"{Level} ");
                    ResetColor();

                    Write($"{Name} HP");
                    ForegroundColor = ConsoleColor.Magenta;
                    WriteLine($"{HP}");
                    ResetColor();
                }
            }
            else  // when monster died
            {
                ForegroundColor = ConsoleColor.Gray;
                Write($"{idx} Lv.{Level} {Name} Dead");
            }
        }
    }

    public class Dungeon
    {
        public int Stage { get; set; }
        public int MonsterCount { get; set; }
        public List<Monster> Monsters { get; set; }

        public Dungeon(int stage)
        {
            Stage = stage;
            Monsters = new List<Monster>(4);
        }
    }

    public class DungeonPlay
    {
        enum Scene { battle, playerTurn, playerAttack, playerResult, monsterTurn, kill }

        private List<Item> _items;
        private Character _player = new Character("testman");
        private Dungeon _dungeon;
        private Random _random = new Random();

        private Scene _scene;

        private void IntroScene()
        {
            bool ValidName = false;
            string userInput = "username";

            Clear();

            WriteLine();
            WriteLine(" 완제품이 없는 세상에 오신 것을 환영합니다.");
            WriteLine();

            while (!ValidName)
            {
                WriteLine(" 당신의 이름은 무엇입니까?");
                Write(" ");
                userInput = ReadLine() ?? "jeire";
                if (userInput.Length < 9)
                {
                    ValidName = true;
                }
                else
                {
                    WriteLine();
                    WriteLine(" 8글자 이내로 작성해주세요");
                    WriteLine(" 당신의 이름은 무엇으로 하겠습니까?");
                    continue;
                }
            }
            _player.Name = userInput;

            WriteLine(" 어떤 직업을 하실랍니까?");
            WriteLine();
            WriteLine(" 1. 무직");
            WriteLine(" 2. 전사");
            WriteLine(" 3. 마법사");
            WriteLine();
            WriteLine(" 원하시는 직업을 골라주세요");

            switch ((JobType)CheckValidInput(1, 3) - 1)
            {
                case JobType.human:
                    _player.Job = JobType.human;
                    break;
                case JobType.warrior:
                    _player.Job = JobType.warrior;
                    break;
                case JobType.mage:
                    _player.Job = JobType.mage;
                    break;
                default:
                    _player.Job = JobType.human;
                    break;
            }
        }

        private void GameDataSetting()
        {
            // global item setting
            _items = new List<Item>();
            _items.Add(new Item("막대기", "나무막대기입니다", EquipType.oneHand, 1, 0, 0, 20));
            _items.Add(new Item("흰 옷", "백의민족의 옷, 흰 옷입니다", EquipType.body, 0, 5, 0, 1000));
            _items.Add(new Item("똥 묻은 옷", "병에 걸릴 것 같은 똥 묻은 옷입니다", EquipType.body, 0, 0, -10, 0));
            _items.Add(new Item("종이칼", "맨주먹이 나을 것 같습니다", EquipType.oneHand, -5, 0, 0, 50));
            _items.Add(new Item("종이방패", "맨손으로 막는 게 나을 것 같습니다.", EquipType.oneHand, 0, -5, 0, 50));

            _items.Add(new Item("짱돌", "잘 다듬어져서 던지기 좋습니다", EquipType.oneHand, 2, 0, 0, 10));
            _items.Add(new Item("철광석", "불순물이 섞인 철광석입니다", EquipType.material, 1, 0, 0, 100));
            _items.Add(new Item("소다회", "각종 식물을 태운 뒤 정제한 소다회입니다", EquipType.material, 1, 0, 0, 100));
            _items.Add(new Item("소금", "각종 식물을 태운 뒤 정제한 소다회입니다", EquipType.material, 1, 0, 0, 100));
            _items.Add(new Item("올리브", "당신이 아는 그 올리브 열매입니다.", EquipType.material, 1, 0, 0, 100));

            // 캐릭터 정보 세팅


            // 아이템 정보 세팅
            _player.AddItem(_items[0]);
            _player.AddItem(_items[1]);
            _player.AddItem(_items[2]);
            _player.AddItem(_items[3]);
            _player.AddItem(_items[4]);
            _player.ToggleEquipStatus(0);
        }

        public void Battle()
        {
            _scene = 0;
            IntroScene();
            GameDataSetting();
            while (_scene != Scene.kill)
            {
                _scene = SceneManager(_scene);
            }
        }

        private Scene SceneManager(Scene scene)
        {
            switch (scene)
            {
                case Scene.battle:
                    scene = BattleScene();
                    break;
                case Scene.playerTurn:
                    scene = PlayerScene();
                    break;
                case Scene.playerAttack:
                    scene = PlayerAttackScene();
                    break;
                case Scene.playerResult:
                    scene = PlayerResultScene();
                    break;
                case Scene.monsterTurn:
                    scene = MonsterScene();
                    break;
            }
            return scene;
        }

        private void MakeStage()
        {
            _dungeon = new Dungeon(1);
            int randNum = _random.Next(1, 5);
            int randType = _random.Next(0, 3);

            if (_dungeon.Stage >= 1)
            {
                _dungeon.Monsters[0] = new Monster(_dungeon.Stage, (MonsterType)randType);
            }

            if(_dungeon.Stage >= 2)
            {
                if(randNum > 3)
                {
                    _dungeon.Monsters[1] = new Monster(_dungeon.Stage, (MonsterType)randType);
                }
            }

            if(_dungeon.Stage >= 3)
            {
                if (randNum > 2)
                {
                    _dungeon.Monsters[2] = new Monster(_dungeon.Stage, (MonsterType)randType);
                }
            }

            if(_dungeon.Stage >= 4)
            {
                if (randNum > 1)
                {
                    _dungeon.Monsters[3] = new Monster(_dungeon.Stage, (MonsterType)randType);
                }
            }
        }

        private Scene BattleScene()
        {
            Scene scene = 0;
            MakeStage();

            Clear();

            WriteLine();
            PrintColoredText(" Battle!!");
            WriteLine("");

            int monsterCount = _dungeon.Monsters.Count;
            for (int i = 0; i < monsterCount; i++)
            {
                _dungeon.Monsters[i].MonsterInfo(true, i+1);
            }

            WriteLine();
            WriteLine(" 0. 나가기");
            WriteLine();
            WriteLine(" 공격할 대상을 선택해주세요.");

            return scene;
        }

        private Scene PlayerScene()
        {
            Scene scene = 0;
            Clear();

            WriteLine();
            PrintColoredText(" Battle!!");
            WriteLine("");

            int monsterCount = _dungeon.Monsters.Count;
            for (int i = 0; i < monsterCount; i++)
            {
                _dungeon.Monsters[i].MonsterInfo(false, i+1);
            }

            WriteLine();
            WriteLine(" 0. 나가기");
            WriteLine();
            WriteLine(" 공격할 대상을 선택해주세요.");

            int input = CheckMonsterInput(0, monsterCount);
            switch (input)
            {
                case 0:
                    scene = Scene.playerTurn;
                    break;
                default:
                    _dungeon.Monsters[input - 1].TakeDamage(PlayerDamage(_player.Atk + Item.AtkBonus));
                    scene = Scene.playerResult;
                    break;
            }
            return scene;
        }

        private Scene PlayerAttackScene()
        {


            Scene scene = 0;
            Clear();

            WriteLine();
            PrintColoredText(" Battle!!");
            WriteLine("");

            int monsterCount = _dungeon.Monsters.Count;
            for (int i = 0; i < monsterCount; i++)
            {
                _dungeon.Monsters[i].MonsterInfo(true, i+1);
            }

            WriteLine();
            WriteLine(" 0. 나가기");
            WriteLine();
            WriteLine(" 공격할 대상을 선택해주세요.");

            int input = CheckMonsterInput(0, monsterCount);
            switch (input)
            {
                case 0:
                    scene = Scene.playerTurn;
                    break;
                default:
                    _dungeon.Monsters[input - 1].TakeDamage(PlayerDamage(_player.Atk + Item.AtkBonus));
                    scene = Scene.playerResult;
                    break;
            }
            return scene;
        }

        private int CheckMonsterInput(int min, int max)
        {
            while (true)
            {
                Write(" ");
                string input = ReadLine();

                bool parseSuccess = int.TryParse(input, out var ret);
                if (parseSuccess)
                {
                    if (ret >= min && ret <= max)
                    {
                        if (_dungeon.Monsters[ret].IsAlive)
                        {
                            return ret;
                        }
                    }
                    else
                    {
                        WriteLine(" 잘못된 입력입니다. 다시 입력해주세요");
                    }
                }
            }
        }

        /// <summary>
        /// 사용자의 atk + atkBonus 값에 대해 +-10%의 데미지를
        /// 치명타와 명중율을 적용하여 가하는 데미지를 계산하는 method
        /// </summary>
        /// <param name="atk">player's atk field value</param>
        /// <returns></returns>
        public int PlayerDamage(int atk)
        {
            int minDamage = (int)Math.Ceiling(atk * 0.9f);
            int maxDamage = (int)Math.Ceiling(atk * 1.1f);

            Random range = new Random();
            int damage = range.Next(minDamage, maxDamage);

            // 치명타 & 회피 확률
            int critOrAvoid = range.Next(0, 20);

            if (critOrAvoid < 3)  // 치명타 -> 0, 1, 2 -> 15% probability
            {
                return (int)Math.Ceiling(damage * 1.6f);
            }
            else if (critOrAvoid > 17)  // 회피 -> 18, 19 -> 10% probability
            {
                return 0;
            }
            else
            {
                return damage;
            }
        }

        private Scene PlayerResultScene()
        {
            Scene scene = 0;
            throw new NotImplementedException();
            return scene;
        }

        private Scene MonsterScene()
        {
            Scene scene = 0;
            throw new NotImplementedException();
            return scene;
        }

        public int MonsterDamage(int atk)
        {
            int minDamage = (int)Math.Ceiling(atk * 0.9f);
            int maxDamage = (int)Math.Ceiling(atk * 1.1f);

            Random range = new Random();
            int damage = range.Next(minDamage, maxDamage);
            return damage;
        }

        private int CheckValidInput(int min, int max)
        {
            while (true)
            {
                Write(" ");
                string input = ReadLine();

                bool parseSuccess = int.TryParse(input, out var ret);
                if (parseSuccess)
                {
                    if (ret >= min && ret <= max)
                        return ret;
                    else
                    {
                        WriteLine(" 잘못된 입력입니다. 다시 입력해주세요");
                    }
                }
            }
        }

        public void PrintColoredText(string text,
            ConsoleColor color = ConsoleColor.Yellow)
        {
            ForegroundColor = color;
            WriteLine(text);
            ResetColor();
        }

        public void PrintwithColoredText(string s1, string s2, string s3 = "",
            ConsoleColor color = ConsoleColor.Cyan)
        {
            Write(s1);
            ForegroundColor = color;
            Write(s2);
            ResetColor();
            WriteLine(s3);
        }
    }
}
