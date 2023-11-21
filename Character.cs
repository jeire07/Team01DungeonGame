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
                    HP = 110;
                    MaxHP = 110;
                    MP = 40;
                    MaxMP = 40;
                    break;
                case JobType.mage:
                    Atk = 5;
                    Def = 0;
                    HP = 90;
                    MaxHP = 90;
                    MP = 100;
                    MaxMP = 100;
                    break;
                case JobType.developer:
                    Level = 1000;
                    Atk = 1000;
                    Def = 1000;
                    HP = 9000;
                    MaxHP = 10000;
                    MP = 9000;
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

                if(HP > (MaxHP + Item.HPBonus))
                {
                    HP = MaxHP + Item.HPBonus;
                }

                Inventory[idx].IsEquipped = true;
            }
        }

        public int ItemIndex(string name)
        {
            return Inventory.FindIndex(item => item.Name == name);
        }

        public void AddItem(Item item)
        {
            if ((ItemIndex(item.Name) != -1) && !item.Equipable)
            {
                item.ItemCount++;
            }
            else
            {
                Inventory.Add(item);
                item.IsEquipped = false;
            }
        }

        public void SubtractItem(string name)
        {
            int index = ItemIndex(name);

            if (index == -1)
            {
                WriteLine("없는 아이템입니다.");
            }
            else if (Inventory[index].ItemCount > 1)
            {
                Inventory[index].ItemCount--;
            }
            else
            {
                Inventory[index].IsEquipped = false;
                Inventory.Remove(Inventory[index]);
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

        public bool UseHealPotion()
        {
            if(ItemIndex("체력 포션") != -1)
            {
                SubtractItem("체력 포션");

                IsAlive = true;
                HP += 30;
                if (HP > (MaxHP + Item.HPBonus))
                {
                    HP = MaxHP + Item.HPBonus;
                }

                Clear();
                WriteLine();
                WriteLine(" 체력 포션을 사용했습니다.");
                WriteLine($" 현재 체력: {HP} / {MaxHP + Item.HPBonus}");

                return true;
            }
            else
            {
                Clear();
                WriteLine(" 체력 포션이 없습니다.");
                WriteLine($" 현재 체력: {HP} / {MaxHP + Item.MPBonus}");

                return false;
            }
        }

        public bool UseManaPotion()
        {
            if (ItemIndex("마나 포션") != -1)
            {
                SubtractItem("마나 포션");

                MP += 30;
                if (MP > (MaxMP + Item.MPBonus))
                {
                    MP = MaxMP + Item.MPBonus;
                }

                Clear();
                WriteLine();
                WriteLine(" 마나 포션을 사용했습니다.");
                WriteLine($" 현재 체력: {MP} / {MaxMP + Item.MPBonus}");

                return true;
            }
            else
            {
                Clear();
                WriteLine("마나 포션이 없습니다.");
                WriteLine($" 현재 체력: {MP} / {MaxMP + Item.MPBonus}");

                return false;
            }
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

            if (HP <= 0)
            {
                IsAlive = false;
            }
            return damage;
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
                WriteLine($"{HP} / {MaxHP + Item.HPBonus}");
                ResetColor();

                Write(" MP ");
                ForegroundColor = ConsoleColor.Yellow;
                WriteLine($"{MP} / {MaxMP + Item.MPBonus}");
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
