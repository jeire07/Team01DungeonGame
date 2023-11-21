using static System.Console;

namespace Team01DungeonGame
{
    public class Battle
    {
        enum Scene
        {
            playerPick, playerAtk, playerSkill, playerEnd,
            monster, victory, defeat, run, healing, result, exitDungeon
        }

        enum AtkEffect { normal, skill, critical, avoid }

        public int Stage { get; set; }
        private List<Monster> _monsters { get; set; }
        private Character _player { get; set; }
        private int _playerDamage { get; set; }
        private int _monsterIdx { get; set; }

        private AtkEffect atkType = AtkEffect.normal;
        private int _enterHP;

        public Battle(int stage, Character player)
        {
            Stage = stage;  // 전투 스테이지 설정 = 생성되는 몬스터 레벨
            _player = player;  // 플레이어 캐릭터 데이터 복사
            _monsters = new List<Monster>();  // 몬스터 생셩 대열

            MakeStage();  // 무작위로 몬스터 생성
            _playerDamage = 0;
            _monsterIdx = 0;
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
            _enterHP = _player.HP;
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
                case Scene.victory:
                    scene = VictoryScene();
                    break;
                case Scene.defeat:
                    scene = DefeatScene();
                    break;
                case Scene.run:
                    scene = RunScene();
                    break;
                case Scene.healing:
                    scene = HealingScene();
                    break;
            }
            return scene;
        }

        private Scene PlayerPickScene()
        {
            Scene scene = Scene.playerPick;
            Clear();

            WriteLine();
            PrintColoredText(" Battle!!");
            WriteLine();

            // 각 몬스터의 정보 출력
            int monsterCount = _monsters.Count;
            for (int i = 0; i < monsterCount; i++)
            {
                _monsters[i].MonsterInfo(false, i + 1);
            }

            // 플레이어의 정보 출력
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
                    scene = Scene.run;
                    break;
                case 1:
                    scene = Scene.playerAtk;
                    break;
                case 2:
                    scene = Scene.playerSkill;
                    break;
                case 3:
                    scene = Scene.healing;
                    break;
            }
            return scene;
        }

        private Scene PlayerAttackScene()
        {
            Scene scene = Scene.playerPick;
            Clear();

            WriteLine();
            PrintColoredText(" Battle!!");
            WriteLine();

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

            int input = CheckMonsterInput(monsterCount);  // 공격할 몬스터의 번호 입력
            switch (input)
            {
                case 0:
                    scene = Scene.playerPick;
                    break;
                default:
                    _playerDamage = PlayerDamage(_player.Atk + Item.AtkBonus, out atkType);
                    _playerDamage = _monsters[input - 1].TakeDamage(_playerDamage);

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

            WriteLine();
            PrintColoredText(" Battle!!");
            WriteLine();

            int monsterCount = _monsters.Count;
            for (int i = 0; i < monsterCount; i++)
            {
                _monsters[i].MonsterInfo(true, i + 1);
            }

            WriteLine();
            WriteLine(" [내 정보]");
            _player.CharacterInfo();

            WriteLine();
            WriteLine(" 1. 알파 스트라이크 - MP 10");
            WriteLine("    공격력 * 2 로 하나의 적을 공격합니다.");
            WriteLine(" 2. 더블 스트라이크 - MP 15");
            WriteLine("    공격력 * 1.5 로 2명의 적을 랜덤으로 공격합니다.");
            WriteLine();
            WriteLine(" 0. 나가기");
            WriteLine();
            WriteLine(" 사용할 스킬을 선택해주세요.");

            int actInput = CheckValidInput(0, 2);
            switch (actInput)
            {
                case 0:
                    scene = Scene.playerPick;
                    break;
                case 1:
                    int MonsterNum = CheckMonsterInput(monsterCount);
                    switch (MonsterNum)
                    {
                        case 0:
                            break;
                        default:
                            if (_player.MP >= 10)
                            {
                                _playerDamage = PlayerDamage(_player.Atk + Item.AtkBonus, out atkType) * 2;
                                _monsters[MonsterNum - 1].TakeDamage(_playerDamage);
                                _monsterIdx = MonsterNum - 1;

                                _player.MP -= 10;
                                scene = Scene.playerEnd;
                            }
                            else
                            {
                                WriteLine(" MP가 부족합니다.");
                                WriteLine(" 0. 돌아가기");
                                CheckValidInput(0, 0);
                                scene = Scene.playerSkill;
                            }
                            break;
                    }
                    break;
                case 2:
                    if (_player.MP >= 10)
                    {
                        int numEnemiesToAttack = 2;  // 랜덤으로 선택할 적의 수

                        for (int i = 0; i < numEnemiesToAttack; i++)
                        {
                            int randomEnemyIndex = RandomMonsterIndex(monsterCount);
                            _playerDamage = PlayerDamage(_player.Atk * 1.5f + Item.AtkBonus, out atkType);
                            _monsters[randomEnemyIndex].TakeDamage(_playerDamage);
                            _monsterIdx = randomEnemyIndex;

                            scene = Scene.playerEnd;
                        }
                        _player.MP -= 15;
                    }
                    else
                    {
                        WriteLine(" MP가 부족합니다.");
                        WriteLine(" 0. 돌아가기");
                        CheckValidInput(0, 0);
                        scene = Scene.playerSkill;
                    }
                    break;
                default:
                    WriteLine(" 잘못된 입력입니다. 다시 입력해주세요");
                    scene = Scene.playerSkill;
                    break;
            }
            return scene;
        }

        private int RandomMonsterIndex(int monsterCount)  //임의로 작성한 스킬랜덤지정 메소드
        {
            Random random = new Random();
            return random.Next(0, monsterCount);
        }

        private Scene HealingScene()
        {
            Scene scene = Scene.playerEnd;
            bool isUsed;
            int countHP;
            int countMP;

            int indexHP = _player.ItemIndex("체력 포션");
            if(indexHP >= 0)
            {
                countHP = _player.Inventory[indexHP].ItemCount;
            }
            else
            {
                countHP = 0;
            }

            int indexMP = _player.ItemIndex("마나 포션");
            if(indexMP >= 0)
            {
                countMP = _player.Inventory[indexMP].ItemCount;
            }
            else
            {
                countMP = 0;
            }
            

            Clear();
            WriteLine();
            PrintColoredText(" 힐링 아이템");
            WriteLine(" 포션을 사용하면 30 회복 할 수 있습니다.");
            WriteLine($" 체력 포션 {countHP} 개 / 마나 포션 {countMP}");
            WriteLine();
            WriteLine($" 체력: {_player.HP} / {_player.MaxHP}");
            WriteLine($" 마나: {_player.MP}/ {_player.MaxMP}");
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
                    isUsed = _player.UseHealPotion();

                    if (isUsed)
                    {
                        scene = Scene.monster;
                    }
                    else
                    {
                        scene = Scene.healing;
                    }
                    WriteLine("0. 다음");
                    CheckValidInput(0, 0);
                    break;
                case 2:
                    isUsed = _player.UseManaPotion();

                    if (isUsed)
                    {
                        scene = Scene.monster;
                    }
                    else
                    {
                        scene = Scene.healing;
                    }
                    WriteLine("0. 다음");
                    CheckValidInput(0, 0);
                    break;
            }
            return scene;
        }

        private Scene PlayerEndScene()
        {
            Scene scene = Scene.monster;
            Clear();

            WriteLine();
            PrintColoredText(" Battle!!");
            WriteLine();

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
            Write($" Lv.{monster.Level} {monster.Name} 를(을) 공격했습니다.");
            Write($"[데미지 : {_playerDamage}] - ");
            
            switch(atkType)
            {
                case AtkEffect.normal:
                    WriteLine("기본 공격");
                    break;
                case AtkEffect.skill:
                    WriteLine("스킬 공격");
                    break;
                case AtkEffect.critical:
                    WriteLine("치명타 공격!");
                    break;
                case AtkEffect.avoid:
                    WriteLine("회피했습니다!");
                    break;
            }

            WriteLine();
            WriteLine($" Lv.{monster.Level} {monster.Name}");
            WriteLine($" {monster.Def} 방어");
            WriteLine($" HP {monster.HP + _playerDamage} -> {monster.HP}");
            WriteLine();
            _playerDamage = 0;

            foreach (Monster checkAlive in _monsters)
            {
                if (checkAlive.IsAlive == true)
                {
                    scene = Scene.monster;
                    break;
                }
                else
                {
                    scene = Scene.victory;
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
                    damage = _player.TakeDamage(damage);

                    WriteLine(" Battle!!");
                    WriteLine();
                    WriteLine($" Lv.{monster.Level} {monster.Name} 의 공격!");
                    WriteLine($" {_player.Name} 을(를) 맞췄습니다.  [데미지 : {_player.Def+damage}]");
                    WriteLine();
                    WriteLine($" Lv.{_player.Level} {_player.Name}");
                    WriteLine($" {_player.Def} 방어, 입은 피해 : {damage}");
                    WriteLine($" HP {_player.HP + damage} -> {_player.HP}");
                    WriteLine();
                    WriteLine(" 0. 다음");
                    Write(" >> ");

                    CheckValidInput(0, 0);
                }
                    
                if (_player.IsAlive == false)
                {
                    scene = Scene.defeat;
                    break;
                }
            }
            return scene;
        }

        private Scene VictoryScene()
        {
            Scene scene = Scene.playerPick;
            Clear();

            // FIXME : Highlighted texts

            WriteLine();
            PrintColoredText(" Battle!! - Result!");
            WriteLine();
            PrintwithColoredText(" ", "Victory", "");
            WriteLine();
            WriteLine(" 던전에서 몬스터 {0}마리를 잡았습니다.", _monsters.Count);
            WriteLine();
            WriteLine(" Lv. {0} {1}", _player.Level, _player.Name);
            WriteLine(" HP {0} -> {1}", _enterHP, _player.HP);
            WriteLine();
            WriteLine(" 0. 다음");

            CheckValidInput(0, 0);
            scene = Scene.exitDungeon;

            return scene;
        }

        private Scene DefeatScene()
        {
            Scene scene;

            Clear();
            WriteLine();
            PrintColoredText(" Battle!! - Result!");
            WriteLine();
            PrintwithColoredText(" ", "You Lose", "");
            WriteLine();
            WriteLine(" Lv. {0} {1}", _player.Level, _player.Name);
            WriteLine(" HP {0} -> 0", _enterHP);
            WriteLine();
            WriteLine(" 0. 다음");

            CheckValidInput(0, 0);
            scene = Scene.exitDungeon;

            return scene;
        }

        private Scene RunScene()
        {
            Scene scene = Scene.exitDungeon;

            Clear();
            WriteLine("");
            PrintColoredText($" {_player.Name}은/는 무사히 도망쳤다.");
            WriteLine("");
            WriteLine(" 0. 다음");

            CheckValidInput(0, 0);
            scene = Scene.exitDungeon;

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
