using static System.Console;

namespace Game05TextGame
{
    enum Scene
    {
        battleInit, playerPick, playerAtk, playerSkill, playerEnd, monster, result, exitDungeon
    }

    public class Battle
    {
        public int Stage { get; set; }
        private int _monsterCount { get; set; }
        private List<Monster> _monsters { get; set; }
        public Character _player { get; set; }

        public Battle(int stage, Character player)
        {
            Stage = stage;
            _monsters = new List<Monster>(4);
            _monsterCount = _monsters.Count;
            _player = player;
            MakeStage();
        }

        public void PlayBattle()
        {
            Scene _scene = Scene.battleInit;
            _player = new Character("username", JobType.human);
            while (_scene != Scene.exitDungeon)
            {
                _scene = SceneManager(_scene);
            }
        }

        private void MakeStage()
        {
            Random random = new Random();
            int randNum = random.Next(1, 5);
            int randType = random.Next(0, 3);

            if (Stage >= 1)
            {
                _monsters.Add(new Monster(Stage, (MonsterType)randType));
            }

            if (Stage >= 2)
            {
                if (randNum > 3)
                {
                    _monsters.Add(new Monster(Stage, (MonsterType)randType));
                }
            }

            if (Stage >= 3)
            {
                if (randNum > 2)
                {
                    _monsters.Add(new Monster(Stage, (MonsterType)randType));
                }
            }

            if (Stage >= 4)
            {
                if (randNum > 1)
                {
                    _monsters.Add(new Monster(Stage, (MonsterType)randType));
                }
            }
        }

        private Scene BattleScene()
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

        private Scene SceneManager(Scene scene)
        {
            switch (scene)
            {
                case Scene.battleInit:
                    scene = BattleScene();
                    break;
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
                    ResultScene();
                    break;
            }
            return scene;
        }

        private void ResultScene()
        {
            throw new NotImplementedException();
        }

        private Scene PlayerPickScene()
        {
            throw new NotImplementedException();
        }

        private Scene PlayerSkillScene()
        {
            throw new NotImplementedException();
        }

        private Scene PlayerEndScene()
        {
            throw new NotImplementedException();
        }

        private Scene MonsterScene()
        {
            throw new NotImplementedException();
        }

        private Scene SkillScene()
        {
            Scene scene = Scene.battleInit;
            Clear();

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
                        // 이 부분에 몬스터 클래스를 아이템으로 가지는 배열 혹은 컬렉션 필요
                        if (_monsters[ret].IsAlive)  // <- Monsters[]의 식별자 수정
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
