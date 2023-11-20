using static System.Console;

namespace Team01DungeonGame
{
    public enum JobType { human, warrior, mage, developer };

    public class Character
    {
        public string Name { get; set; }
        public JobType Job { get; set; }
        public int Level { get; set; }
        public float Atk { get; set; }
        public int Def { get; set; }
        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int MP { get; set; }
        public int MaxMP { get; set; }
        public int Gold { get; set; }
        public bool IsAlive { get; set; }
        public int Exp { get; set; }
        public int MaxExp { get; set; }

        public List<Item> Inventory { get; }
        public Item[] Equips { get; }

        public Character(string name, JobType job = JobType.human)
        {
            Name = name;
            Job = job;
            Level = 1;
            Gold = 1500;
            IsAlive = true;
            Exp = 0;
            MaxExp = 10;

            Inventory = new List<Item>(20);
            Item[] Equips = new Item[9];

            switch (job)
            {
                case JobType.human:
                    Atk = 10;
                    Def = 5;
                    HP = 100;
                    MaxHP = 100;
                    MP = 50;
                    MaxMP = 50;
                    break;
                case JobType.warrior:
                    Atk = 15;
                    Def = 10;
                    MaxHP = 110;
                    MaxMP = 40;
                    break;
                case JobType.mage:
                    Atk = 5;
                    Def = 0;
                    MaxHP = 90;
                    MaxMP = 100;
                    break;
                case JobType.developer:
                    Level = 1000;
                    Atk = 10000;
                    Def = 10000;
                    HP = 10000;
                    MaxHP = 10000;
                    MP = 10000;
                    MaxMP = 10000;
                    break;
            }
        }

        public string PrintJob()
        {
            string jobName = "직업이름";
            switch (Job)
            {
                case JobType.human:
                    jobName = "무직";
                    break;
                case JobType.warrior:
                    jobName = "전사";
                    break;
                case JobType.mage:
                    jobName = "마법사";
                    break;
                case JobType.developer:
                    jobName = "개발자";
                    break;
            }
            return jobName;
        }

        public bool IsEquipped(Item item)
        {
            return item.IsEquipped;
        }

        public void ToggleEquipStatus(int idx)
        {
            if (Inventory[idx].IsEquipped)
            {
                Item.AtkBonus -= Inventory[idx].Atk;
                Item.DefBonus -= Inventory[idx].Def;
                Item.HPBonus  -= Inventory[idx].HP;

                Inventory[idx].IsEquipped = false;
            }
            else
            {
                Item.AtkBonus += Inventory[idx].Atk;
                Item.DefBonus += Inventory[idx].Def;
                Item.HPBonus  += Inventory[idx].HP;

                Inventory[idx].IsEquipped = true;
            }
        }

        public bool IsExist(Item item)
        {
            return Inventory.Any(item => item.Name == item.Name);
        }

        public void AddItem(Item item)
        {
            if (IsExist(item) && !item.Equipable)
            {
                item.Count++;
            }
            else
            {
                Inventory.Add(item);
                item.IsEquipped = false;
            }
        }

        public void SubtractItem(Item item)
        {
            if (!IsExist(item))
            {
                WriteLine("없는 아이템입니다.");
            }
            else if (item.Count > 1)
            {
                item.Count--;
            }
            else
            {
                item.IsEquipped = false;
                Inventory.Remove(item);
            }
        }

        /// <summary>
        /// 몬스터의 레벨 1당 경험치 1을 받는다.
        /// 전투 결과창에서 해당 method를 사용한다.
        /// </summary>
        public void LevelUp()
        {
            if(Exp > MaxExp)
            {
                Level++;
                MaxExp = (int)Math.Ceiling(MaxExp * 1.5f);
                Atk += 0.5f;
                Def += 1;
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

        public void CharacterInfo()
        {
            if (IsAlive)
            {
                Write(" Lv.");
                ForegroundColor = ConsoleColor.Cyan;
                Write($"{Level} ");
                ResetColor();
                WriteLine($"{Name}");

                Write(" HP ");
                ForegroundColor = ConsoleColor.Yellow;
                WriteLine($"{HP} / {MaxHP}");
                ResetColor();

                Write(" MP ");
                ForegroundColor = ConsoleColor.Yellow;
                WriteLine($"{MP} / {MaxMP}");
                ResetColor();
            }
            else  // when monster died
            {
                ForegroundColor = ConsoleColor.DarkGray;
                WriteLine($" Lv.{Level} {Name} Dead");
                ResetColor();
            }
        }
    }
}
