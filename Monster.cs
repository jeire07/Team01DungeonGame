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
                    Name = "미니언";
                    Atk = 5 + level;
                    Def = 0;
                    HP = 15 + level;
                    MaxHP = 15 + level;
                    Gold = 20 + level;
                    break;
                case MonsterType.insect:
                    Name = "공허충";
                    Atk = 5 + level;
                    Def = 0;
                    HP = 10 + level;
                    MaxHP = 10 + level;
                    Gold = 15 + level;
                    break;
                case MonsterType.canon:
                    Name = "대포미니언";
                    Atk = 8 + level;
                    Def = 0;
                    HP = 25 + level;
                    MaxHP = 25 + level;
                    Gold = 33 + level;
                    break;
            }

            IsAlive = true;
        }

        public void TakeDamage(int damage)
        {
            HP -= damage;

            if (HP < 0)
            {
                HP = 0;
                IsAlive = false;
            }
        }

        public void MonsterInfo(bool withNumber = false, int idx = 0)
        {
            if (IsAlive)
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

                    Write($"{Name} HP ");
                    ForegroundColor = ConsoleColor.Yellow;
                    WriteLine($"{HP} / {MaxHP}");
                    ResetColor();
                }
                else
                {
                    Write("Lv.");
                    ForegroundColor = ConsoleColor.Magenta;
                    Write($"{Level} ");
                    ResetColor();

                    Write($"{Name} HP ");
                    ForegroundColor = ConsoleColor.Magenta;
                    WriteLine($"{HP} / {MaxHP}");
                    ResetColor();
                }
            }
            else  // when monster died
            {
                if (withNumber)
                {
                    ForegroundColor = ConsoleColor.Gray;
                    WriteLine($" Lv.{Level} {Name} Dead");
                }
                else
                {
                    ForegroundColor = ConsoleColor.Gray;
                    WriteLine($" Lv.{Level} {Name} Dead");
                }
            }
        }
    }
}
