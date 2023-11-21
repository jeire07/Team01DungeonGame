using static System.Console;

namespace Team01DungeonGame
{
    public class Battle
    {
        enum Scene
        {
            playerPick, playerAtk, playerSkill, playerEnd, monster, result, exitDungeon, Healing
        }

        enum AtkEffect { normal, critical, avoid }

        public int Stage { get; set; }
        private int _monsterCount { get; set; }
        private List<Monster> _monsters { get; set; }
        private Character _player { get; set; }

        private AtkEffect isCritOrAvoid = AtkEffect.normal;
        private int _playerDamage = 0;
        private int _monsterIdx = 0;

        

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
                _monsters.Add(new Monster(Stage, (MonsterType)randType));
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
                case Scene.Healing:
                    scene = BattleHealingScene();
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
            WriteLine(" [내 정보]");
            _player.CharacterInfo();

            WriteLine();
            WriteLine(" 0. 도망가기");
            WriteLine(" 1. 기본 공격");
            WriteLine(" 2. 스킬 공격");
            WriteLine(" 3. 포션 사용");

            WriteLine();
            WriteLine(" 원하시는 행동을 입력해주세요.");

            int input = CheckValidInput(0, 3);
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
                case 3:
                    scene = Scene.Healing;
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
            WriteLine(" [내 정보]");
            _player.CharacterInfo();

            WriteLine();
            WriteLine(" 0. 나가기");
            WriteLine();
            WriteLine(" 공격할 대상을 선택해주세요.");

            int input = CheckMonsterInput(monsterCount);
            switch (input)
            {
                case 0:
                    scene = Scene.playerPick;
                    break;
                default:
                    _playerDamage = PlayerDamage(_player.Atk + Item.AtkBonus, out isCritOrAvoid);
                    _monsters[input - 1].TakeDamage(_playerDamage);
                    _monsterIdx = input - 1;
                    
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
            Scene scene = Scene.monster;
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
            WriteLine(" [내 정보]");
            _player.CharacterInfo();

            WriteLine();
            WriteLine($" {_player.Name} 의 공격!");

            Monster monster = _monsters[_monsterIdx];
            Write($" Lv.{monster.Level} {monster.Name} 를(을) 맞췄습니다.");
            Write($"[데미지 : {_playerDamage}] - ");
            
            switch(isCritOrAvoid)
            {
                case AtkEffect.normal:
                    WriteLine("기본 공격");
                    break;
                case AtkEffect.critical:
                    WriteLine("치명타 공격!");
                    break;
                case AtkEffect.avoid:
                    WriteLine("회피!");
                    break;
            }

            WriteLine();
            WriteLine($" Lv.{monster.Level} {monster.Name}");
            WriteLine($" {monster.Def} 방어");
            _playerDamage -= monster.Def;
            WriteLine($" HP {monster.HP} -> {monster.HP - _playerDamage}");
            monster.HP -= _playerDamage;
            WriteLine();

            foreach (Monster checkAlive in _monsters)
            {
                if (checkAlive.IsAlive == true)
                {
                    scene = Scene.monster;
                    break;
                }
                else
                {
                    scene = Scene.result;
                }
            }
            WriteLine(" 0. 다음");

            CheckValidInput(0, 0);

            return scene;
        }

        private Scene MonsterScene()
        {
            Scene scene = Scene.playerPick;
            Clear();

            WriteLine();

            // 남아있는 몬스터를 가져온다.
            foreach (Monster monster in _monsters)
            {
                if (monster.IsAlive == true)
                {
                    int minDamage = (int)Math.Ceiling(monster.Atk * 0.9f);
                    int maxDamage = (int)Math.Ceiling(monster.Atk * 1.1f);

                    Random range = new Random();
                    int damage = range.Next(minDamage, maxDamage);

                    WriteLine(" Battle!!");
                    WriteLine();
                    WriteLine($" Lv.{monster.Level} {monster.Name} 의 공격!");
                    WriteLine($" {_player.Name} 을(를) 맞췄습니다.  [데미지 : {damage}]");
                    WriteLine();
                    WriteLine($" Lv.{_player.Level} {_player.Name}");
                    damage = (monster.Atk > _player.Def) ? monster.Atk - _player.Def : 0;
                    WriteLine($" {_player.Def} 방어");
                    WriteLine($" HP {_player.HP} -> {_player.HP - damage}");
                    _player.HP -= damage;
                    WriteLine();
                    WriteLine(" 0. 다음");
                    Write(" >> ");

                    CheckValidInput(0, 0);
                }
                    
                if (_player.IsAlive == false)
                {
                    scene = Scene.result;
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
                Clear();
                WriteLine("Battle!! - Result!");
                WriteLine();
                WriteLine("Victory");
                WriteLine();
                WriteLine("던전에서 몬스터 {0}마리를 잡았습니다.");
                WriteLine();
                WriteLine("Lv. {0} {1}");
                WriteLine("HP {0} -> {1}");
                WriteLine();
                WriteLine("0. 다음");
            }
            else
            {
                Clear();
                WriteLine("Battle!! - Result!");
                WriteLine();
                WriteLine("You Lose");
                WriteLine();
                WriteLine("Lv. {0} {1}");
                WriteLine("HP {0} -> 0");
                WriteLine();
                WriteLine("0. 다음");
            }

            // Get
            //
            // Input and change scene

            return scene;
        }

        private int CheckMonsterInput(int max)
        {
            while (true)
            {
                Write(" ");
                string input = ReadLine();

                bool parseSuccess = int.TryParse(input, out var ret);
                if (parseSuccess)
                {
                    if (ret == 0)
                    {
                        return ret;
                    }
                    else if (_monsters[ret - 1].IsAlive)
                    {
                        return ret;
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
        private int PlayerDamage(float atk, out AtkEffect isCritOrAvoid)
        {
            int minDamage = (int)Math.Ceiling(atk * 0.9f);
            int maxDamage = (int)Math.Ceiling(atk * 1.1f);

            Random range = new Random();
            int damage = range.Next(minDamage, maxDamage);

            // 치명타 & 회피 확률
            int critOrAvoid = range.Next(0, 20);

            if (critOrAvoid < 3)  // 치명타 -> 0, 1, 2 -> 15% probability
            {
                isCritOrAvoid = AtkEffect.critical;
                return (int)Math.Ceiling(damage * 1.6f);
            }
            else if (critOrAvoid > 17)  // 회피 -> 18, 19 -> 10% probability
            {
                isCritOrAvoid = AtkEffect.avoid;
                return 0;
            }
            else
            {
                isCritOrAvoid = AtkEffect.normal;
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
        private Scene BattleHealingScene()
        {
            Scene scene = Scene.playerEnd;

            Clear();
            WriteLine();
            PrintColoredText(" 힐링 아이템");
            WriteLine(" 포션을 사용하면 30 회복 할 수 있습니다.");
            WriteLine($" 체력 포션 {_player.healPotion} 개 / 마나 포션 {_player.manaPotion}");
            WriteLine();
            WriteLine($" 체력: {_player.HP}/{_player.MaxHP}");
            WriteLine($" 마나: {_player.MP}/{_player.MaxMP}");
            WriteLine();
            WriteLine(" 1. 체력 포션");
            WriteLine(" 2. 마나 포션");
            WriteLine(" 0. 나가기");
            WriteLine();
            WriteLine(" 원하시는 행동을 입력해주세요.");

            switch (CheckValidInput(0, 2))
            {
                case 0:
                    scene = Scene.playerPick;
                    break;
                case 1:
                    if (_player.healPotion > 0)
                    {
                        _player.HP += 30;
                        if (_player.HP > _player.MaxHP)
                        {
                            _player.HP = _player.MaxHP;
                        }
                        _player.healPotion--;
                        Clear();
                        WriteLine();
                        WriteLine(" 체력 포션을 사용했습니다.");
                        WriteLine($" 현재 체력: {_player.HP}/{_player.MaxHP}");
                        ReadKey(true);
                        scene = Scene.playerEnd;
                    }
                    else
                    {
                        Clear();
                        WriteLine();
                        WriteLine(" 체력 포션이 부족합니다.");
                        ReadKey(true);
                        scene = Scene.Healing;
                    }
                    break;
                case 2:
                    if (_player.manaPotion > 0)
                    {
                        _player.MP += 30;
                        if (_player.MP > _player.MaxMP)
                        {
                            _player.MP = _player.MaxMP;
                        }
                        _player.manaPotion--;
                        Clear();
                        WriteLine();
                        WriteLine(" 마나 포션을 사용했습니다.");
                        WriteLine($" 현재 체력: {_player.MP}/{_player.MaxMP}");
                        ReadKey(true);
                        scene = Scene.playerEnd;
                    }
                    else
                    {
                        Clear();
                        WriteLine();
                        WriteLine(" 마나 포션이 부족합니다.");
                        ReadKey(true);
                        scene = Scene.Healing;
                    }
                    break;
            }
            return scene;
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
