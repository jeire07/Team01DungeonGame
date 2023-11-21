using static System.Console;
using static System.Formats.Asn1.AsnWriter;

namespace Team01DungeonGame
{
    public class Battle
    {
        enum Scene
        {
            playerPick, playerAtk, playerSkill, playerEnd, monster, victory, defeat, run, exitDungeon, Healing
        }

        enum AtkEffect { normal, critical, avoid }

        public int Stage { get; set; }
        private int _monsterCount { get; set; }
        private List<Monster> _monsters { get; set; }
        private Character _player { get; set; }

        private AtkEffect isCritOrAvoid = AtkEffect.normal;
        private int _playerDamage = 0;
        private int _monsterIdx = 0;
        private int _enterHp = 0;

        

        public Battle(int stage, Character player)
        {
            Stage = stage;  //전투 스테이지 설정
            _player = player;   //플레이어 설정
            _monsters = new List<Monster>();    //몬스터 리스트 초기화
            MakeStage();    //스테이지 생성
            _monsterCount = _monsters.Count;    //생성된 몬스터 수 설정
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
            _enterHp = _player.HP;
            Scene scene = Scene.playerPick; //초기 장면 설정
            while (scene != Scene.exitDungeon)  //exitDungeon 장면 나올떄까지 반복
            {
                scene = SceneManager(scene); //현재 장면을 SceneManager로 전달하여 다음 장면을 얻음
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
                case Scene.Healing:
                    scene = BattleHealingScene();
                    break;
            }
            return scene;
        }

        private Scene PlayerPickScene()
        {
            Scene scene = 0; // 초기 장면 설정
            Clear();        // 콘솔 화면을 지움

            WriteLine();
            PrintColoredText(" Battle!!");
            WriteLine("");

            int monsterCount = _monsters.Count;
            for (int i = 0; i < monsterCount; i++)
            {
                _monsters[i].MonsterInfo(false, i + 1); // 각 몬스터의 정보를 출력
            }

            WriteLine();
            WriteLine(" [내 정보]");
            _player.CharacterInfo();        // 플레이어의 정보를 출력

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
                    scene = Scene.run;   // 도망가기를 선택하면 전투 결과로 이동
                    break;
                case 1:
                    scene = Scene.playerAtk;    // 기본 공격을 선택하면 플레이어의 공격 장면으로 이동
                    break;
                case 2:
                    scene = Scene.playerSkill; // 스킬 공격을 선택하면 플레이어의 스킬 사용 장면으로 이동
                    break;
                case 3:
                    scene = Scene.Healing;
                    break;
            }
            return scene;   // 다음으로 진행할 장면을 반환
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

            int input = CheckMonsterInput(monsterCount);      // 몬스터 선택을 위한 입력을 받음
            switch (input)
            {
                case 0:
                    scene = Scene.playerPick;   // 나가기를 선택하면 플레이어 선택 장면으로 이동
                    break;
                default:
                    _playerDamage = PlayerDamage(_player.Atk + Item.AtkBonus, out isCritOrAvoid); // 플레이어의 공격력을 계산
                    _monsters[input - 1].TakeDamage(_playerDamage);  // 선택한 몬스터에게 플레이어의 공격력만큼 데미지를 입힘
                    _monsterIdx = input - 1;    // 선택한 몬스터의 인덱스를 저장

                    scene = Scene.playerEnd;    // 플레이어의 턴이 끝났음을 나타내는 장면으로 이동
                    break;
            }
            return scene;   // 다음으로 진행할 장면을 반환
        }

        private Scene PlayerSkillScene()
        {
            Scene scene = Scene.playerPick;
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
            WriteLine(" 1. 알파 스트라이크 - MP 10");
            WriteLine("    공격력 * 2 로 하나의 적을 공격합니다.");
            WriteLine(" 2. 더블 스트라이크 - MP 15");
            WriteLine("    공격력 * 1.5 로 2명의 적을 랜덤으로 공격합니다.");
            WriteLine();
            WriteLine(" 0. 나가기");
            WriteLine();
            WriteLine(" 사용할 스킬을 선택해주세요.");

            int input = CheckMonsterInput(monsterCount);

            //int input = CheckValidInput(0, 2);
            switch (input)
            {
                case 0:
                    scene = Scene.playerPick;
                    break;
                case 1:
                    if (_player.MP >= 10)
                    {
                        _playerDamage = PlayerDamage((int)(_player.Atk * 1.5) + Item.AtkBonus, out isCritOrAvoid) * 2;
                        _monsters[input - 1].TakeDamage(_playerDamage);
                        _monsterIdx = input - 1;

                        _player.MP -= 10;
                        scene = Scene.playerEnd;
                    }
                    else
                    {
                        WriteLine(" MP가 부족합니다.");
                        scene = Scene.playerEnd;
                    }
                    break;
                case 2:
                    if (_player.MP >= 10)
                    {
                        int numEnemiesToAttack = 2;  // 랜덤으로 선택할 적의 수

                        for (int i = 0; i < numEnemiesToAttack; i++)
                        {
                            int randomEnemyIndex = RandommonsterIndex(monsterCount);
                            _playerDamage = PlayerDamage((int)(_player.Atk * 1.5) + Item.AtkBonus, out isCritOrAvoid);
                            _monsters[randomEnemyIndex].TakeDamage(_playerDamage);
                            _monsterIdx = randomEnemyIndex;

                            scene = Scene.playerEnd;
                        }
                        _player.MP -= 15;
                    }
                    else
                    {
                        WriteLine(" MP가 부족합니다.");
                        scene = Scene.playerEnd;
                    }
                    break;
                default:
                    WriteLine(" 잘못된 입력입니다. 다시 입력해주세요");
                    scene = Scene.playerEnd;
                    break;
            }
            return scene;
        }


        private int RandommonsterIndex(int monsterCount)  //임의로 작성한 스킬랜덤지정 메소드
        {
            Random random = new Random();
            return random.Next(0, monsterCount);
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

            switch (isCritOrAvoid)
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
            
            WriteLine($" HP {monster.BEFORE_HP} -> {monster.AFTER_HP}");
            WriteLine();

            WriteLine(" 0. 다음");

            monster.BEFORE_HP = monster.AFTER_HP;

            foreach (Monster checkAlive in _monsters)
            {
                if (checkAlive.IsAlive == true)
                {
                    scene = Scene.monster;
                    break;
                }
                
                scene = Scene.victory; // 살아있는 몬스터 없는 경우
            }

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

                    if (_player.IsAlive == false)
                    {
                        scene = Scene.defeat;
                    }
                }
            }
            return scene;
        }

        private Scene VictoryScene()
        {
            Scene scene = Scene.playerPick;
            Clear();

            // FIXME : Highlighted texts

            PrintColoredText("Battle!! - Result!");
            WriteLine();
            PrintwithColoredText("","Victory","");
            WriteLine();
            WriteLine("던전에서 몬스터 {0}마리를 잡았습니다.", _monsterCount);
            WriteLine();
            WriteLine("Lv. {0} {1}", _player.Level, _player.Name);
            WriteLine("HP {0} -> {1}", _enterHp, _player.HP);
            WriteLine();
            WriteLine("0. 다음");

            CheckValidInput(0, 0);
            scene = Scene.exitDungeon;

            return scene;
        }

        private Scene DefeatScene()
        {
            Scene scene;

            Clear();
            PrintColoredText("Battle!! - Result!");
            WriteLine();
            PrintwithColoredText("", "You Lose", "");
            WriteLine();
            WriteLine("Lv. {0} {1}", _player.Level, _player.Name);
            WriteLine("HP {0} -> 0", _enterHp);
            WriteLine();
            WriteLine("0. 다음");

            CheckValidInput(0, 0);
            scene = Scene.exitDungeon;

            return scene;
        }

        private Scene RunScene()
        {
            Scene scene = Scene.exitDungeon;

            Clear();
            WriteLine("");
            PrintColoredText($"{_player.Name}은/는 무사히 도망쳤다.");
            WriteLine("");
            WriteLine("0. 다음");

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
