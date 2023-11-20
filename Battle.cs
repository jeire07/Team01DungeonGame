using static System.Console;

namespace Team01DungeonGame
{

    enum BattleScene
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
            List<Monster> _monsters = new List<Monster>(4);
            _monsterCount = _monsters.Count;
            _player = player;
            MakeStage();
        }

        public void PlayBattle()
        {
            BattleScene BattleScene = BattleScene.battleInit;
            while (BattleScene != BattleScene.exitDungeon)
            {
                BattleScene = SceneManager(BattleScene);
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
        
        private BattleScene InitScene()
        {
            BattleScene scene = 0;
            Clear();

            WriteLine();
            PrintColoredText(" Battle!!");
            WriteLine("");

            MakeStage();
            int monsterCount = Monster.Count;
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
                    scene = BattleScene.playerAtk;
                    break;
                default:
                    _monsters[input - 1].TakeDamage(PlayerDamage(_player.Atk + Item.AtkBonus));
                    scene = BattleScene.playerEnd;
                    break;
            }
            return scene;
        }

        private BattleScene SceneManager(BattleScene scene)
        {
            switch (scene)
            {
                case BattleScene.battleInit:
                    scene = InitScene();
                    break;
                case BattleScene.playerPick:
                    scene = PlayerPickScene();
                    break;
                case BattleScene.playerAtk:
                    scene = PlayerAttackScene();
                    break;
                case BattleScene.playerSkill:
                    scene = PlayerSkillScene();
                    break;
                case BattleScene.playerEnd:
                    scene = PlayerEndScene();
                    break;
                case BattleScene.monster:
                    scene = MonsterScene();
                    break;
                case BattleScene.result:
                    ResultScene();
                    break;
            }
            return scene;
        }

        private void ResultScene()
        {
            throw new NotImplementedException();
        }

        private BattleScene PlayerPickScene()
        {
            throw new NotImplementedException();
        }

        private BattleScene PlayerSkillScene()
        {
            throw new NotImplementedException();
        }

        private BattleScene PlayerEndScene()
        {
            throw new NotImplementedException();
        }

        private BattleScene MonsterScene()
        {
            BattleScene scene = 0;
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
                }
            }
            return scene;
        }

        private BattleScene SkillScene()
        {
            BattleScene scene = BattleScene.battleInit;
            Clear();

            return scene;
        }

        private BattleScene PlayerAttackScene()
        {
            BattleScene scene = 0;
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
                    scene = BattleScene.playerAtk;
                    break;
                default:
                    _monsters[input - 1].TakeDamage(PlayerDamage(_player.Atk + Item.AtkBonus));
                    scene = BattleScene.playerEnd;
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
