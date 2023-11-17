﻿using static System.Console;

namespace Team01DungeonGame
{
	//public enum Scene
	//{
	//	main, status, inventory, market, rest,
	//	equipment, buy, sell, kill,
 //       battle, playerturn, playerattack, monsterturn
	//}
	
    public class TextGame
    {
        enum Scene
        {
            main, status, inventory, market, rest,
            equipment, buy, sell, kill
        }

        private Character _player;
		private Character _merchant;
		private List<Item> _items;

		private Scene _scene;

		public void PlayText()
		{
			_scene = 0;
			_player = new Character("username", JobType.human);
			IntroScene();
			GameDataSetting();
			while (_scene != Scene.kill)
			{
				_scene = SceneManager(_scene);
			}
		}

        private void IntroScene()
        {
            bool ValidName = false;
            string userInput = "username";

            Clear();

            WriteLine();
            WriteLine(" 완제품이 없는 세상에 오신 것을 환영합니다.");
            WriteLine();

            while (!ValidName)
            {
                WriteLine(" 당신의 이름은 무엇입니까?");
                Write(" ");
                userInput = ReadLine() ?? "jeire";
                if (userInput.Length < 9)
                {
                    ValidName = true;
                }
                else
                {
                    WriteLine();
                    WriteLine(" 8글자 이내로 작성해주세요");
                    WriteLine(" 당신의 이름은 무엇으로 하겠습니까?");
                    continue;
                }
            }
            _player.Name = userInput;

            WriteLine(" 어떤 직업을 하실랍니까?");
            WriteLine();
            WriteLine(" 1. 무직");
            WriteLine(" 2. 전사");
            WriteLine(" 3. 마법사");
            WriteLine();
            WriteLine(" 원하시는 직업을 골라주세요");

            switch ((JobType)CheckValidInput(1, 3) - 1)
            {
                case JobType.human:
                    _player.Job = JobType.human;
                    break;
                case JobType.warrior:
                    _player.Job = JobType.warrior;
                    break;
                case JobType.mage:
                    _player.Job = JobType.mage;
                    break;
                default:
                    _player.Job = JobType.human;
                    break;
            }
        }

        private void GameDataSetting()
		{
			// global item setting
			_items = new List<Item>();
			_items.Add(new Item("막대기", "나무막대기입니다", EquipType.oneHand, 1, 0, 0, 20));
			_items.Add(new Item("흰 옷", "백의민족의 옷, 흰 옷입니다", EquipType.body, 0, 5, 0, 1000));
			_items.Add(new Item("똥 묻은 옷", "병에 걸릴 것 같은 똥 묻은 옷입니다", EquipType.body, 0, 0, -10, 0));
			_items.Add(new Item("종이칼", "맨주먹이 나을 것 같습니다", EquipType.oneHand, -5, 0, 0, 50));
			_items.Add(new Item("종이방패", "맨손으로 막는 게 나을 것 같습니다.", EquipType.oneHand, 0, -5, 0, 50));

			_items.Add(new Item("짱돌", "잘 다듬어져서 던지기 좋습니다", EquipType.oneHand, 2, 0, 0, 10));
			_items.Add(new Item("철광석", "불순물이 섞인 철광석입니다", EquipType.material, 1, 0, 0, 100));
			_items.Add(new Item("소다회", "각종 식물을 태운 뒤 정제한 소다회입니다", EquipType.material, 1, 0, 0, 100));
			_items.Add(new Item("소금", "각종 식물을 태운 뒤 정제한 소다회입니다", EquipType.material, 1, 0, 0, 100));
			_items.Add(new Item("올리브", "당신이 아는 그 올리브 열매입니다.", EquipType.material, 1, 0, 0, 100));

			// 상인 정보 세팅
			_merchant = new Character("jeire", JobType.human);

			// 1. 아이템 클래스 내부에 이미 존재하는지 확인하는 함수 추가 -> Linq 써서 이미 존재하면 count += 1
			//    뺄 때도 count -= 1하다가, 
			// 2. 딕셔너리로 key값을 써서 하고, 밸류값으로 갯수 파악

			// 캐릭터 인벤토리 정보 세팅
			_player.AddItem(_items[0]);
			_player.AddItem(_items[1]);
			_player.AddItem(_items[2]);
			_player.AddItem(_items[3]);
			_player.AddItem(_items[4]);
			_player.ToggleEquipStatus(0);

			// 상인 판매 품목 세팅
			_merchant.AddItem(_items[5]);
			_merchant.AddItem(_items[6]);
			_merchant.AddItem(_items[7]);
			_merchant.AddItem(_items[8]);
			_merchant.AddItem(_items[9]);
        }

        private Scene SceneManager(Scene scene)
		{
			switch (scene)
			{
				case Scene.main:
					scene = MainScene();
					break;
				case Scene.status:
					scene = StatusScene();
					break;
				case Scene.inventory:
					scene = InventoryScene();
					break;
				case Scene.market:
					scene = MarketScene();
					break;
				case Scene.rest:
					scene = RestScene();
					break;
				case Scene.equipment:
					scene = EquipmentScene();
					break;
				case Scene.buy:
					scene = BuyScene();
					break;
				case Scene.sell:
					scene = SellScene();
					break;
				case Scene.kill:
					break;
			}
			return scene;
		}

		private Scene MainScene()
		{
			Scene scene = Scene.main;
			Clear();

			WriteLine();
			WriteLine(" 무엇을 하시겠습니까?");
			WriteLine();
			WriteLine(" 1. 상태창");
			WriteLine(" 2. 인벤토리");
			WriteLine(" 3. 상점");
			WriteLine(" 4. 휴식");
			WriteLine(" 9. 게임 종료");
			WriteLine();
			WriteLine(" 원하시는 행동을 입력해주세요.");

			switch (CheckValidInput(1, 9))
			{
				case 1:
					scene = Scene.status;
					break;
				case 2:
					scene = Scene.inventory;
					break;
				case 3:
					scene = Scene.market;
					break;
				case 4:
					scene = Scene.rest;
					break;
				case 9:
					scene = Scene.kill;
					break;
				default:
					scene = Scene.main;
					break;
			}
			return scene;
		}

		private Scene StatusScene()
        {
			Scene scene = Scene.main;
			Clear();

			WriteLine();
			PrintColoredText(" 상태보기");
			WriteLine(" 캐릭터의 정보를 표시합니다.");
			WriteLine();

			PrintwithColoredText(" Lv.", _player.Level.ToString("00"));
			WriteLine();
			WriteLine($" 이  름 : {_player.Name}");
			PrintwithColoredText(" 직  업 : ", _player.PrintJob());

			int bonusAtk = Item.AtkBonus;
            int bonusDef = Item.DefBonus;
			int bonusHP = Item.HPBonus;
			PrintwithColoredText(" 공격력 : ", (_player.Atk + bonusAtk).ToString(), bonusAtk >= 0 ? $" (+{bonusAtk})" : $" ({bonusAtk})");
			PrintwithColoredText(" 방어력 : ", (_player.Def + bonusDef).ToString(), bonusDef >= 0 ? $" (+{bonusDef})" : $" ({bonusDef})");
			PrintwithColoredText(" 체력   : ", (_player.HP + bonusHP).ToString(), bonusHP >= 0 ? $" (+{bonusHP})" : $" ({bonusHP})");
			PrintwithColoredText(" Gold   : ", _player.Gold.ToString(), "G");
			
			WriteLine();
			WriteLine(" 0. 나가기");
			WriteLine();
			WriteLine(" 원하시는 행동을 입력해주세요.");

			switch (CheckValidInput(0, 0))
			{
				case 0:
					scene = Scene.main;
					break;
			}
			return scene;
		}

		private Scene InventoryScene()
		{
			Scene scene = Scene.main;
			Clear();

			WriteLine();
			PrintColoredText(" 인벤토리");
			WriteLine(" 보유 중인 아이템을 관리할 수 있습니다.");
			WriteLine();
			WriteLine(" [아이템 목록]");

			int inventoryCount = _player.Inventory.Count;
			for (int i = 0; i < inventoryCount; i++)
			{
				_player.Inventory[i].ItemInfo();
			}

			WriteLine();
			WriteLine(" 0. 나가기");
			WriteLine(" 1. 장착관리");
			WriteLine();
			WriteLine(" 원하시는 행동을 입력해주세요.");

			switch (CheckValidInput(0, inventoryCount))
			{
				case 0:
					scene = Scene.main;
					break;
				case 1:
					scene = Scene.equipment;
					break;
			}
			return scene;
		}

		private Scene EquipmentScene()
		{
			Scene scene = Scene.main;
			Clear();

			WriteLine();
			PrintColoredText(" 인벤토리 - 장착 관리");
			WriteLine(" 아이템 장착을 관리할 수 있습니다.");
			WriteLine("");
			WriteLine(" [아이템 목록]");

			int inventoryCount = _player.Inventory.Count;
			for (int i = 0; i < inventoryCount; i++)
			{
				_player.Inventory[i].ItemInfo(true, i+1);
			}

			WriteLine();
			WriteLine(" 0. 나가기");
			WriteLine();
			WriteLine(" 원하시는 행동을 입력해주세요.");

			int input = CheckValidInput(0, inventoryCount);
			switch (input)
			{
				case 0:
					scene = Scene.inventory;
					break;
				default:
					_player.ToggleEquipStatus(input - 1);

					scene = Scene.equipment;
					break;
			}
			return scene;
		}

		private Scene MarketScene()
		{
			Scene scene = Scene.main;
			Clear();

			WriteLine();
			PrintColoredText(" 잡화점");
			WriteLine(" 아이템을 사고 팔 수 있습니다.");
			WriteLine();
			WriteLine(" [보유 골드]");
			PrintwithColoredText(" ", _player.Gold.ToString(), "G");
			WriteLine();
			WriteLine(" 1. 아이템 구매");
			WriteLine(" 2. 아이템 판매");
			WriteLine(" 0. 나가기");
			WriteLine();
			WriteLine(" 원하시는 행동을 입력해주세요.");

			switch (CheckValidInput(0, 2))
			{
				case 0:
					scene = Scene.main;
					break;
				case 1:
					scene = Scene.buy;
					break;
				case 2:
					scene = Scene.sell;
					break;
			}
			return scene;
		}

		private Scene BuyScene()
		{
			Scene scene = Scene.main;
			Clear();

			WriteLine();
			PrintColoredText(" 잡화점");
			WriteLine(" 해당 번호를 입력하면 1개씩 구매할 수 있습니다.");
			WriteLine();
			WriteLine(" [보유 골드]");
			PrintwithColoredText(" ", _player.Gold.ToString(), "G");
			WriteLine();
			WriteLine(" [아이템 목록]");

			int inventoryCount = _merchant.Inventory.Count;
			for (int i = 0; i < inventoryCount; i++)
			{
				_merchant.Inventory[i].ItemInfo(true, i+1);
			}

			WriteLine();
			WriteLine(" 0. 나가기");
			WriteLine();
			WriteLine(" 원하시는 행동을 입력해주세요.");

			int input = CheckValidInput(0, inventoryCount);
			switch (input)
			{
				case 0:
					scene = Scene.market;
					break;
				default:
					if (_player.Gold >= _merchant.Inventory[input-1].Cost)
					{
						_player.Gold -= _merchant.Inventory[input-1].Cost;
						_player.AddItem(_merchant.Inventory[input-1]);

						scene = Scene.buy;
					}
					else
					{
						WriteLine(" Gold가 부족합니다.");
					}
					break;
			}
			return scene;
		}

		private Scene SellScene()
		{
			Scene scene = Scene.main;
			Clear();

			WriteLine();
			PrintColoredText(" 잡화점");
			WriteLine(" 해당 번호를 입력하면 1개씩 판매할 수 있습니다.");
			WriteLine();
			WriteLine(" [보유 골드]");
			PrintwithColoredText(" ", _player.Gold.ToString(), "G");
			WriteLine();
			WriteLine(" [아이템 목록]");

			int inventoryCount = _player.Inventory.Count;
			for (int i = 0; i < inventoryCount; i++)
			{
				_player.Inventory[i].ItemInfo(true, i+1);
			}

			WriteLine();
			WriteLine(" 0. 나가기");
			WriteLine();
			WriteLine(" 원하시는 행동을 입력해주세요.");

			int input = CheckValidInput(0, inventoryCount);
			switch (input)
			{
				case 0:
					scene = Scene.market;
					break;
				default:
					_player.Gold += (int)(_player.Inventory[input-1].Cost * 0.85f);
					_player.SubtractItem(_player.Inventory[input-1]);

					scene = Scene.sell;
					break;
			}
			return scene;
		}

		private Scene RestScene()
		{
			Scene scene = Scene.main;
			Clear();

			WriteLine();
			PrintColoredText(" 휴식하기");
			PrintwithColoredText(" ", "500", " G 를 내면 체력을 회복할 수 있습니다.");
			PrintwithColoredText(" (보유 골드 : ", $"{_player.Gold}", " G)");
			WriteLine();
			WriteLine(" 1. 휴식하기");
			WriteLine(" 0. 나가기");
			WriteLine();
			WriteLine(" 원하시는 행동을 입력해주세요.");

			switch (CheckValidInput(0, 1))
			{
				case 0:
					scene = Scene.main;
					break;
				case 1:
					if (_player.Gold >= 500)
					{
						_player.HP = 100;
						_player.Gold -= 500;
						WriteLine("휴식을 완료했습니다.");
					}
					else
					{
						WriteLine("Gold가 부족합니다.");
					}
					scene = Scene.rest;
					break;
			}
			return scene;
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

		public void PrintColoredText(string text,
			ConsoleColor color = ConsoleColor.Yellow)
		{
			ForegroundColor = color;
			WriteLine(text);
			ResetColor();
		}

		public void PrintwithColoredText(string s1, string s2, string s3 = "",
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
