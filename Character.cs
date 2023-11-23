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
            IsAlive = true;
            Exp = 0;
            MaxExp = 10;

            Item.AtkBonus = 0;
            Item.DefBonus = 0;
            Item.HPBonus = 0;
            Item.MPBonus = 0;

            Inventory = new List<Item>(20);
            Item[] Equips = new Item[9];

            switch (job)
            {
                case JobType.human:
                    Atk = 10;
                    Def = 3;
                    HP = 100;
                    MaxHP = 100;
                    MP = 50;
                    MaxMP = 50;
                    Gold = 10;
                    break;
                case JobType.warrior:
                    Atk = 15;
                    Def = 2;
                    HP = 150;
                    MaxHP = 150;
                    MP = 40;
                    MaxMP = 40;
                    Gold = 1000;
                    break;
                case JobType.mage:
                    Atk = 10;
                    Def = 0;
                    HP = 100;
                    MaxHP = 100;
                    MP = 300;
                    MaxMP = 300;
                    Gold = 50;
                    break;
                case JobType.developer:
                    Level = 1000;
                    Atk = 1000;
                    Def = 1000;
                    HP = 9000;
                    MaxHP = 10000;
                    MP = 9000;
                    MaxMP = 10000;
                    Gold = 50000;
                    break;
            }
        }

        public string PrintJob()
        {
            string jobName = "직업이름";
            switch (Job)
            {
                case JobType.human:
                    jobName = "거지";
                    break;
                case JobType.warrior:
                    jobName = "바이킹";
                    break;
                case JobType.mage:
                    jobName = "람쥐썬더";
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
                Item.MPBonus  -= Inventory[idx].MP;
                HP -= Inventory[idx].HP;
                MP -= Inventory[idx].MP;

                Inventory[idx].IsEquipped = false;
            }
            else
            {
                if(Inventory[idx].Equipable)
                {
                    if (Inventory[idx].Name == "빨간 버튼")
                    {
                        Clear();
                        WriteLine();
                        WriteLine("==========================================");
                        WriteLine("  갑자기 몸이 섬광처럼 빛나기 시작했다!");
                        WriteLine("==========================================");
                        Thread.Sleep(1000);
                        WriteLine("    .    .    .    .    .    .    .  ");
                        WriteLine("   ...  ...  ...  ...  ...  ...  ... ");
                        WriteLine("  ...................................");
                        WriteLine("   ...  ...  ...  ...  ...  ...  ... ");
                        WriteLine("    .    .    .    .    .    .    .  \n");
                        Thread.Sleep(3000);
                        WriteLine("    그리고 형체를 찾을 수 없게 되었다.\n");
                        Thread.Sleep(1000);
                        WriteLine("              누르지 말껄...\n");
                        Thread.Sleep(1000);
                        WriteLine("               You Lose");
                        Thread.Sleep(2000);
                        Environment.Exit(0);
                    }
                    else
                    {
                        Item.AtkBonus += Inventory[idx].Atk;
                        Item.DefBonus += Inventory[idx].Def;
                        Item.HPBonus += Inventory[idx].HP;
                        Item.MPBonus += Inventory[idx].MP;
                        HP += Inventory[idx].HP;
                        MP += Inventory[idx].MP;
                    }
                    Inventory[idx].IsEquipped = true;
                }
                else
                {
                    WriteLine(" 장착할 수 없는 아이템입니다.");
                }
            }
        }

        public void ToggleEquipStatus(string name)
        {
            if (Inventory[InventoryIndex(name)].IsEquipped)
            {
                Item.AtkBonus -= Inventory[InventoryIndex(name)].Atk;
                Item.DefBonus -= Inventory[InventoryIndex(name)].Def;
                Item.HPBonus  -= Inventory[InventoryIndex(name)].HP;
                Item.MPBonus  -= Inventory[InventoryIndex(name)].MP;
                HP -= Inventory[InventoryIndex(name)].HP;
                MP -= Inventory[InventoryIndex(name)].MP;

                Inventory[InventoryIndex(name)].IsEquipped = false;
            }
            else
            {
                if (Inventory[InventoryIndex(name)].Equipable)
                {
                    Item.AtkBonus += Inventory[InventoryIndex(name)].Atk;
                    Item.DefBonus += Inventory[InventoryIndex(name)].Def;
                    Item.HPBonus  += Inventory[InventoryIndex(name)].HP;
                    Item.MPBonus  += Inventory[InventoryIndex(name)].MP;
                    HP += Inventory[InventoryIndex(name)].HP;
                    MP += Inventory[InventoryIndex(name)].MP;

                    Inventory[InventoryIndex(name)].IsEquipped = true;
                }
                else
                {
                    WriteLine(" 장착할 수 없는 아이템입니다.");
                }
            }
        }

        public int InventoryIndex(string name)
        {
            return Inventory.FindIndex(item => item.Name == name);
        }

        public int ItemIndex(List<Item> itemList, string name)
        {
            return itemList.FindIndex(item => item.Name == name);
        }

        public void AddItem(Item item)
        {
            if ((InventoryIndex(item.Name) != -1) && !item.Equipable)
            {
                item.ItemCount++;
            }
            else
            {
                Inventory.Add(item.DeepCopy());
                item.IsEquipped = false;
            }
        }

        public void AddItem(List<Item> itemList, string name)
        {
            Item item;
            if(ItemIndex(itemList, name) != -1)  // 월드 상에 존재하는 아이템인지 확인
            {
                if(InventoryIndex(name) != -1)  // 캐릭터 인벤토리에 이미 있는 아이템
                {
                    item = itemList[InventoryIndex(name)];

                    if (!item.Equipable)  // 장비템은 중첩 불가
                    {
                        item.ItemCount++;
                    }
                    else  // 중첩 가능 아이템
                    {
                        item = itemList[ItemIndex(itemList, name)];

                        Inventory.Add(item);
                        item.IsEquipped = false;
                    }
                }
                else  // 캐릭터 인벤토리에 없는 아이템
                {
                    item = itemList[ItemIndex(itemList, name)].DeepCopy();

                    Inventory.Add(item);
                    item.IsEquipped = false;
                }
            }
            else
            {
                WriteLine(" 존재하지 않는 아이템입니다.");
                WriteLine(" 0. 확인");
                ReadKey(true);
            }
        }

        public void SubtractItem(string name)
        {
            int index = InventoryIndex(name);

            if (index == -1)
            {
                WriteLine(" 없는 아이템입니다.");
                WriteLine(" 0. 확인");
                ReadKey(true);
            }
            else if (Inventory[index].ItemCount > 1)
            {
                Inventory[index].ItemCount--;
            }
            else
            {
                if (Inventory[index].IsEquipped)
                {
                    ToggleEquipStatus(name);
                }
                Inventory.Remove(Inventory[index]);
            }
        }

        /// <summary>
        /// 몬스터의 레벨 1당 경험치 1을 받는다.
        /// 전투 결과창에서 해당 method를 사용한다.
        /// </summary>
        public void LevelUp()
        {
            WriteLine();
            WriteLine("===========================");
            WriteLine();
            ForegroundColor = ConsoleColor.Yellow;
            WriteLine(" 레벨업!!!");
            ResetColor();
            WriteLine();
            WriteLine($" Lv. {Level} -> {Level+1}");
            WriteLine();
            WriteLine($" 공격: {Atk} -> {Atk + 0.5f}");
            WriteLine($" 방어: {Def} -> {Def + 1}");

            Exp -= MaxExp;
            Level++;
            MaxExp = (int)Math.Ceiling(MaxExp * 1.5f);

            WriteLine($" 경험치: {Exp} / {MaxExp}");
            WriteLine();
            WriteLine("===========================");
            WriteLine();

            Atk += 0.5f;
            Def += 1;
        }

        public void GetExp(int exp)
        {
            Exp += exp;

            while (Exp >= MaxExp)
            {
                LevelUp();
            }
        }

        public bool UseHealPotion()
        {
            if(InventoryIndex("체력 포션") != -1)
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
            if (InventoryIndex("마나 포션") != -1)
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
                WriteLine($" 현재 마나: {MP} / {MaxMP + Item.MPBonus}");

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
