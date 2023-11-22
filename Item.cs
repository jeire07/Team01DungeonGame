using static System.Console;

namespace Team01DungeonGame
{
    public enum EquipType
    {
        twoHand, oneHand, head, body, shoes, robe,
        globes, ring1, ring2, potion, material
    }

    public class Item
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public EquipType Type { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int HP { get; set; }
        public int MP { get; set; }
        public int Cost { get; set; }
        public bool Equipable { get; set; }
        public bool IsEquipped { get; set; }
        public int ItemCount { get; set; }

        public static int AtkBonus { get; set; }
        public static int DefBonus { get; set; }
        public static int HPBonus { get; set; }
        public static int MPBonus { get; set; }

        public Item(string name, string description, EquipType type,
            int atk, int def, int hp, int mp, int gold,
            bool equipable = true, bool isEquipped = false, int itemCount = 1)
        {
            Name = name;
            Description = description;
            Type = type;
            Atk = atk;
            Def = def;
            HP = hp;
            MP = mp;
            Cost = gold;
            Equipable = equipable;
            IsEquipped = isEquipped;
            ItemCount = itemCount;
        }

        public Item DeepCopy()
        {
            Item item = new Item(this.Name, this.Description, this.Type,
                                 this.Atk, this.Def, this.HP, this.MP, this.Cost,
                                 this.Equipable, this.IsEquipped, this.ItemCount);
            
            return item;
        }

        public void ItemInfo(bool withNumber = false, int idx = 0)
        {
            if (withNumber)
            {
                ForegroundColor = ConsoleColor.DarkMagenta;
                Write($" {idx.ToString("00")}");
                ResetColor();
            }

            if (IsEquipped)
            {
                Write(" [");
                ForegroundColor = ConsoleColor.Cyan;
                Write("E");
                ResetColor();
                Write("] ");
                Write(PadRightText($"{Name}", 17));
            }
            else
            {
                Write(PadRightText($"     {Name}", 22));
            }
            Write(PadRightStat("Atk ", Atk, "", 10));
            Write(PadRightStat("Def ", Def, "", 10));
            Write(PadRightStat("HP ", HP, "", 10));
            Write(PadRightStat("MP ", MP, "", 10));
            Write(PadRightStat("", ItemCount, "개", 8));
            Write(PadRightStat("", Cost, " G", 11));
            WriteLine($" | {Description}");
        }

        public string PadRightStat(string text1, int value, string text2, int length)
        {
            string text = value >= 0 ? $" | {text1}+{value}{text2}" : $" | {text1}{value}{text2}";
            int padding = length - PrintableLength(text);
            return text.PadRight(text.Length + padding);
        }

        public string PadRightText(string text, int length)
        {
            int padding = length - PrintableLength(text);
            return text.PadRight(text.Length + padding);
        }

        public int PrintableLength(string str)
        {
            int length = 0;
            foreach (char c in str)
            {
                if (char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.OtherLetter)
                {
                    length += 2; // 한글과 같은 넓은 문자에 대해 길이를 2로 취급
                }
                else
                {
                    length += 1; // 나머지 문자에 대해 길이를 1로 취급
                }
            }
            return length;
        }
    }
}
