using static System.Console;

namespace Team01DungeonGame
{
    public enum JobType { human, warrior, mage };

	public class Character
	{
		public string Name { get; set; }
		public JobType Job { get; set; }
		public int Level { get; set; }
		public int Atk { get; set; }
		public int Def { get; set; }
		public int HP { get; set; }
		public int Gold { get; set; }
        public bool IsAlive { get; set; }
        public List<Item> Inventory { get; }
		public Item[] Equips { get; }

        public Character(string name, JobType job = JobType.human, int level = 1,
			int atk = 10, int def = 5, int hp = 100, int gold = 1500)
		{
			Name = name;
			Job = job;
			Level = level;
			Atk = atk;
			Def = def;
			HP = hp;
			Gold = gold;
			Inventory = new List<Item>(20);
			Item[] Equips = new Item[9];
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
	}
}
