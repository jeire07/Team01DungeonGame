using System.Threading;
using static System.Console;

namespace Team01DungeonGame
{
    public class Battle
    {
        enum Scene
        {
            playerPick, playerAtk, playerSkill, playerEnd, monster, result, exitDungeon
        }

        public int Stage { get; set; }
        private int _monsterCount { get; set; }
        private List<Monster> _monsters { get; set; }
        public Character _player { get; set; }

        public Battle(int stage, Character player)
        {
            Stage = stage;
            _player = player;
            _monsters = new List<Monster>();
            MakeStage();
            _monsterCount = _monsters.Count;
        }

        private void MakeStage()
        {
            Random random = new Random();
            int randNum = random.Next(1, 5);
            int randType = random.Next(0, 3);
            Monster monster;

            if (Stage >= 1)
            {
                monster = new Monster(Stage, (MonsterType)randType);
                _monsters.Add(monster);
            }

            if (Stage >= 2)
            {
                if (randNum > 3)
                {
                    randType = random.Next(0, 3);
                    _monsters.Add(new Monster(Stage, (MonsterType)randType));
                }
            }

            if (Stage >= 3)
            {
                if (randNum > 2)
                {
                    randType = random.Next(0, 3);
                    _monsters.Add(new Monster(Stage, (MonsterType)randType));
                }
            }

            if (Stage >= 4)
            {
                if (randNum > 1)
                {
                    randType = random.Next(0, 3);
                    _monsters.Add(new Monster(Stage, (MonsterType)randType));
                }
            }
        }

        public void PlayBattle()
        {
            Scene scene = Scene.playerPick;
            while (scene != Scene.exitDungeon)
            {
                scene = SceneManager(scene);
            }
        }

        private Scene SceneManager(Scene scene)
        {
            switch (scene)
            {
                case Scene.playerPick:
                    scene = PlayerPickScene();
                    break;
                case Scene.playerAtk:
                    scene = PlayerAttackScene();
                    break;
                case Scene.playerSkill:
                    scene = PlayerSkillScene();
                    break;
                case Scene.playerEnd:
                    scene = PlayerEndScene();
                    break;
                case Scene.monster:
                    scene = MonsterScene();
                    break;
                case Scene.result:
                    scene = ResultScene();
                    break;
            }
            return scene;
        }

        private Scene PlayerPickScene()
        {
            Scene scene = 0;
            Clear();

            WriteLine();
            PrintColoredText(" Battle!!");
            WriteLine("");

            int monsterCount = _monsters.Count;
            for (int i = 0; i < monsterCount; i++)
            {
                _monsters[i].MonsterInfo(false, i + 1);
            }

            WriteLine();
            WriteLine(" 0. 도망가기");
            WriteLine(" 1. 기본 공격");
            WriteLine(" 2. 스킬 공격");
            WriteLine();
            WriteLine(" 원하시는 행동을 입력해주세요.");

            int input = CheckValidInput(0, 2);
            switch (input)
            {
                case 0:
                    scene = Scene.result;
                    break;
                case 1:
                    scene = Scene.playerAtk;
                    break;
                case 2:
                    scene = Scene.playerSkill;
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

            int monsterCount = _monsters.Count;
            for (int i = 0; i < monsterCount; i++)
            {
                _monsters[i].MonsterInfo(true, i + 1);
            }

            WriteLine();
            WriteLine(" 0. 나가기");
            WriteLine();
            WriteLine(" 공격할 대상을 선택해주세요.");

            int input = CheckMonsterInput(0, monsterCount);
            switch (input)
            {
                case 0:
                    scene = Scene.playerAtk;
                    break;
                default:
                    _monsters[input - 1].TakeDamage(PlayerDamage(_player.Atk + Item.AtkBonus));
                    scene = Scene.playerEnd;
                    break;
            }
            return scene;
        }

        private Scene PlayerSkillScene()
        {
            Scene scene = Scene.playerPick;
            Clear();

            return scene;
        }

        private Scene PlayerEndScene()
        {
            Scene scene = Scene.playerPick;
            Clear();

            return scene;
        }

        private Scene MonsterScene()
        {
            Scene scene = 0;
            Clear();

            // 남아있는 몬스터를 가져온다.
            foreach (Monster monster in _monsters)
            {
                if (monster.IsTurn == true)
                {
                    if (monster.IsAlive == true)
                    {
                        int minDamage = (int)Math.Ceiling(_player.Atk * 0.9f);
                        int maxDamage = (int)Math.Ceiling(_player.Atk * 1.1f);

                        Random range = new Random();
                        int damage = range.Next(minDamage, maxDamage);

                        WriteLine(" Battle!!");
                        WriteLine();
                        WriteLine($"LV.{monster.Level} {monster.Name} 의 공격!");
                        WriteLine($"{_player} 를(을) 맞췄습니다.  [데미지 : {damage}]");
                        WriteLine();
                        WriteLine($"{_player.Level} {_player}");
                        WriteLine($"HP {_player.HP} -> {_player.HP - damage}");
                        _player.HP -= damage;
                        WriteLine();
                        WriteLine("다음");
                        Write(">> ");
                        ReadKey(true);
                    }

                    if (_player.IsAlive == false)
                    {
                        scene = Scene.result;
                    }
                }
            }
            return scene;
        }

        private Scene ResultScene()
        {
            Scene scene = Scene.playerPick;
            Clear();

            // FIXME : Highlighted texts

            if (true)
            {
                Console.Clear();
                Console.WriteLine("Battle!! - Result!");
                Console.WriteLine();
                Console.WriteLine("Victory");
                Console.WriteLine();
                Console.WriteLine("던전에서 몬스터 {0}마리를 잡았습니다.");
                Console.WriteLine();
                Console.WriteLine("Lv. {0} {1}");
                Console.WriteLine("HP {0} -> {1}");
                Console.WriteLine();
                Console.WriteLine("0. 다음");
            }

            if (false)
            {
                Console.Clear();
                Console.WriteLine("Battle!! - Result!");
                Console.WriteLine();
                Console.WriteLine("You Lose");
                Console.WriteLine();
                Console.WriteLine("Lv. {0} {1}");
                Console.WriteLine("HP {0} -> 0");
                Console.WriteLine();
                Console.WriteLine("0. 다음");
            }

            // Get User Input and change scene

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
                    ret -= 1;
                    if (ret >= min && ret <= max)
                    {
                        if (_monsters[ret].IsAlive)
                        {
                            return ret;
                        }
                        else
                        {
                            WriteLine(" 잘못된 입력입니다. 다시 입력해주세요");
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
        /// Player Attack에서 사용함.
        /// </summary>
        /// <param name="atk">player's atk field value</param>
        /// <returns></returns>
        private int PlayerDamage(int atk)
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

        /// <summary>
        /// 몬스터의 atk + atkBonus 값에 대해 +-10%의 데미지를
        /// 계산하는 method
        /// 몬스터 턴에서 해당 코드를 사용할 수도 아닐 수도 있음
        /// </summary>
        /// <param name="atk">monster's atk field value</param>
        /// <returns></returns>
        private int MonsterDamage(int atk)
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

        private void PrintColoredText(string text,
            ConsoleColor color = ConsoleColor.Yellow)
        {
            ForegroundColor = color;
            WriteLine(text);
            ResetColor();
        }

        private void PrintwithColoredText(string s1, string s2, string s3 = "",
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
