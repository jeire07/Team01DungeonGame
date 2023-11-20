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
        public bool IsTurn { get; set; }


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

        public void PrintMonsterAtkScene()  //몬스터 턴일때 배틀 메소드
        {
            // 남아있는 몬스터를 가져온다.
            foreach (Monster monster in _dungeon.Monsters)
            {
                if (monster.IsTurn == true)
                {
                    if (monster.IsAlive == true)
                    {
                        int minDamage = (int)Math.Ceiling(Atk * 0.9f);
                        int maxDamage = (int)Math.Ceiling(Atk * 1.1f);

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
                    ForegroundColor = ConsoleColor.Cyan;
                    Write($"{Level} ");
                    ResetColor();

                    Write($"{Name} HP");
                    ForegroundColor = ConsoleColor.Yellow;
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
}
