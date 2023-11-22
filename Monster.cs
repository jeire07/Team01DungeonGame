using static System.Console;

namespace Team01DungeonGame
{
    public enum MonsterType { minion, insect, canon, zombie }

    public class Monster
    {
        public int Level { get; set; }
        public MonsterType Job { get; set; }
        public string Name { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int HP { get; set; }
        public int MaxHP { get; set; }
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
                    Name = "벌크업한 미니언";
                    Atk = 3 + level;
                    Def = 3 + level;
                    HP = 40 + level;
                    MaxHP = 40 + level;
                    Gold = 10 + level;
                    break;
                case MonsterType.insect:
                    Name = "당랑권 사마귀";
                    Atk = 9 + level;
                    Def = 1 + level;
                    HP = 20 + level;
                    MaxHP = 20 + level;
                    Gold = 20 + level;
                    break;
                case MonsterType.canon:
                    Name = "뚱뚱한 해적";
                    Atk = 7 + level;
                    Def = 3 + level;
                    HP = 70 + level;
                    MaxHP = 70 + level;
                    Gold = 70 + level;
                    break;
                case MonsterType.zombie:
                    Name = "좀비";
                    Atk = 5 + level;
                    Def = 5 + level;
                    HP = 30 + level;
                    MaxHP = 30 + level;
                    Gold = 30 + level;
                    break;
            }

            IsAlive = true;
        }

        public int TakeDamage(int damage)
        {
            if (damage >= Def)
            {
                damage -= Def;

                if (damage > HP)
                {
                    damage = HP;
                }
                HP -= damage;
            }
            else
            {
                damage = 0;
            }

            if (HP <= 0)
            {
                IsAlive = false;
            }
            return damage;
        }

        public void MonsterInfo(bool withNumber = false, int idx = 0)
        {
            if (IsAlive)
            {
                if (withNumber)
                {
                    ForegroundColor = ConsoleColor.Magenta;
                    Write($" {idx}");
                    ResetColor();

                    Write(" Lv.");
                    ForegroundColor = ConsoleColor.Cyan;
                    Write($"{Level} ");
                    ResetColor();

                    Write($"{Name} HP ");
                    ForegroundColor = ConsoleColor.Yellow;
                    WriteLine($"{HP} / {MaxHP}");
                    ResetColor();
                }
                else
                {
                    Write(" Lv.");
                    ForegroundColor = ConsoleColor.Cyan;
                    Write($"{Level} ");
                    ResetColor();

                    Write($"{Name} HP ");
                    ForegroundColor = ConsoleColor.Yellow;
                    WriteLine($"{HP} / {MaxHP}");
                    ResetColor();
                }
            }
            else  // when monster died
            {
                if (withNumber)
                {
                    ForegroundColor = ConsoleColor.DarkGray;
                    WriteLine($" Lv.{Level} {Name} Dead");
                    ResetColor();
                }
                else
                {
                    ForegroundColor = ConsoleColor.DarkGray;
                    WriteLine($" Lv.{Level} {Name} Dead");
                    ResetColor();
                }
            }
        }
    }
}
