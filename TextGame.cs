using static System.Console;

namespace Team01DungeonGame
{
    public class TextGame
    {
        enum Scene
        {
            main, status, inventory, market, upgrade, gatcha, rest, healing, dungeon, stagePick,
            equipment, buy, sell, kill
        }

        private Scene _scene;

        private Character _player;

        private Character _merchant;

        private List<Item> _items;

        private Battle _battle;

        private int _stage = 1;

        public void PlayText()
        {
            _scene = 0;
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
            string userName = "username";
            JobType userJob = JobType.human;

            Clear();

            WriteLine();
            WriteLine("     ================================================================");
            WriteLine("        __                  ___                                    \r");
            WriteLine("       /__\\_ _ ___ _   _   /   \\_   _ _ __   __ _  ___  ___  _ __  \r");
            WriteLine("      /_\\/ _` / __| | | | / /\\ / | | | '_ \\ / _` |/ _ \\/ _ \\| '_ \\ \r");
            WriteLine("     //_| (_| \\__ \\ |_| |/ /_//| |_| | | | | (_| |  __/ (_) | | | |\r");
            WriteLine("     \\__/\\__,_|___/\\__, /___,'  \\__,_|_| |_|\\__, |\\___|\\___/|_| |_|\r");
            WriteLine("                   |___/                    |___/                  ");
            WriteLine("     ================================================================");
            WriteLine("                             Press Any Button                        ");
            ReadKey();

            Clear();
            while (!ValidName)
            {
                WriteLine();
                WriteLine(" 환영합니다.");
                WriteLine();
                WriteLine(" 당신의 이름은 무엇입니까?");
                Write(" ");
                WriteLine();
                Write(" >> ");
                userName = ReadLine() ?? "jeire";
                if (userName.Length < 9)
                {
                    ValidName = true;
                }
                else
                {
                    WriteLine();
                    WriteLine(" 이름을 8글자 이내로 작성 해주세요.");
                    WriteLine(" 너무 길면 부르기 힘들잖아요.");
                    WriteLine();
                    continue;
                }
            }
            WriteLine();
            WriteLine(" 이 세상을 살아가기 위해선 몸이 필요합니다.");
            WriteLine();
            WriteLine(" 1. 거지     : 가진게 없어요");
            WriteLine();
            WriteLine(" 2. 바이킹   : 바이킹 종족의 후예");
            WriteLine();
            WriteLine(" 3. 람쥐썬더 : 바람이여!");
            WriteLine();
            PrintwithColoredText(" You의 ","Body","를 골라주세요");

            userJob = (JobType)CheckValidInput(1, 3) - 1;
            _player = new Character(userName, userJob);
        }

        private void GameDataSetting()
        {
            // global item setting
            _items = new List<Item>();
            _items.Add(new Item("체력 포션", "HP를 30 회복합니다.", EquipType.potion, 0, 0, 0, 0, 200, false));
            _items.Add(new Item("마나 포션", "MP를 30 회복합니다.", EquipType.potion, 0, 0, 0, 0, 200, false));
            _items.Add(new Item("나무 가지", "나무 가지 입니다", EquipType.oneHand, 1, 0, 0, 0, 20));
            _items.Add(new Item("하얀 옷", "크기가 자유자제로 변합니다.", EquipType.body, 0, 1, 0, 0, 150));
            _items.Add(new Item("목장갑", "인부의 힘", EquipType.globes, 0, 0, 3, 0, 0));
            _items.Add(new Item("돌반지", "구멍을 어떻게 뚫었지?", EquipType.ring1, 0, 0, 5, 0, 50));
            _items.Add(new Item("빨간 버튼", "눌러볼까?", EquipType.body, -5, -5, -20, 0, 0));

            _items.Add(new Item("짱돌", "잘 다듬어져서 던지기 좋습니다", EquipType.oneHand, 2, 0, 0, 0, 10));
            _items.Add(new Item("삿갓", "햇빛이 잘 막아집니다.", EquipType.head, 0, 2, 0, 0, 150));
            _items.Add(new Item("수정 구슬", "신비한 마력이 느껴집니다.", EquipType.oneHand, 0, 1, 0, 10, 200));
            _items.Add(new Item("벌목 도끼", "벌목할 때 쓸 수 있는 도끼", EquipType.twoHand, 5, 0, 0, 0, 500));
            _items.Add(new Item("깨진 결혼 반지", "대부분 금값입니다.", EquipType.ring1, 0, 0, -10, 0, 1000));
            _items.Add(new Item("천년나무 지팡이", "후려치기 딱 좋습니다.", EquipType.twoHand, 4, 0, 0, 50, 1100));
            _items.Add(new Item("덤벨", "파워 머슬", EquipType.oneHand, 5, 0, 30, -20, 400));
            _items.Add(new Item("수정 검", "수정으로 만들어진 검", EquipType.oneHand, 4, 0, 0, 20, 800));
            _items.Add(new Item("장대", "장대 높이 뛰기를 할 수 있을 것 같습니다.", EquipType.oneHand, 3, 3, 0, 0, 300));
            _items.Add(new Item("철 갑옷", "무거운 철로 만들어져 있습니다.", EquipType.oneHand, -2, 8, 0, 0, 600));

            _items.Add(new Item("철광석", "불순물이 섞인 철광석입니다", EquipType.material, 0, 0, 0, 0, 100, false));
            _items.Add(new Item("소다회", "각종 식물을 태운 뒤 정제한 소다회입니다", EquipType.material, 0, 0, 0, 0, 100, false));
            _items.Add(new Item("소금", "짜다!", EquipType.material, 0, 0, 0, 0, 100, false));
            _items.Add(new Item("올리브", "당신이 아는 그 올리브 열매입니다.", EquipType.material, 0, 0, 0, 0, 100, false));

            // 상인 정보 세팅
            _merchant = new Character("jeire", JobType.human);

            // 1. 아이템 클래스 내부에 이미 존재하는지 확인하는 함수 추가 -> Linq 써서 이미 존재하면 count += 1
            //    뺄 때도 count -= 1하다가, 
            // 2. 딕셔너리로 key값을 써서 하고, 밸류값으로 갯수 파악

            // 캐릭터 인벤토리 정보 세팅
            _player.AddItem(_items, "나무 가지");
            _player.AddItem(_items, "하얀 옷");
            _player.AddItem(_items, "돌반지");

            _player.AddItem(_items, "체력 포션");
            _player.AddItem(_items, "마나 포션");

            _player.Inventory[_player.InventoryIndex("체력 포션")].ItemCount = 3;
            _player.Inventory[_player.InventoryIndex("마나 포션")].ItemCount = 3;
            _player.ToggleEquipStatus(0);

            // 상인 판매 품목 세팅
            for(int i = 0; i < _items.Count; i++)
            {
                _merchant.AddItem(_items[i]);
            }
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
                case Scene.upgrade:
                    scene = Upgrade();
                    break;
                case Scene.gatcha:
                    scene = Gatcha();
                    break;
                case Scene.rest:
                    scene = RestScene();
                    break;
                case Scene.healing:
                    scene = HealingScene();
                    break;
                case Scene.dungeon:
                    scene = DungeonScene();
                    break;
                case Scene.stagePick:
                    scene = StagePickScene();
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
            WriteLine(" 여기는 숲속의 작은 마을입니다.");
            WriteLine(" 무엇을 하시겠습니까? ");
            WriteLine();
            WriteLine(" 1. 상태창");
            WriteLine(" 2. 인벤토리");
            WriteLine(" 3. 상점");
            WriteLine(" 4. 트레이닝");
            WriteLine(" 5. 휴식");
            WriteLine(" 6. 포션 사용");
            WriteLine(" 7. 던전 포탈");
            WriteLine(" 9. 탈출");
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
                    scene = Scene.upgrade;
                    break;
                case 5:
                    scene = Scene.rest;
                    break;
                case 6:
                    scene = Scene.healing;
                    break;
                case 7:
                    scene = Scene.dungeon;
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
            WriteLine($" You  : {_player.Name}");
            PrintwithColoredText(" Body : ", _player.PrintJob());

            string bonusAtk = Item.AtkBonus >= 0 ? $" (+{Item.AtkBonus})" : $" ({Item.AtkBonus})";
            string bonusDef = Item.DefBonus >= 0 ? $" (+{Item.DefBonus})" : $" ({Item.DefBonus})";
            string bonusHP = Item.HPBonus >= 0 ? $" (+{Item.HPBonus})" : $" ({Item.HPBonus})";
            string bonusMP = Item.MPBonus >= 0 ? $" (+{Item.MPBonus})" : $" ({Item.MPBonus})";

            PrintwithColoredText(" 공격력 : ", (_player.Atk + Item.AtkBonus).ToString(), bonusAtk);
            PrintwithColoredText(" 방어력 : ", (_player.Def + Item.DefBonus).ToString(), bonusDef);
            PrintwithColoredText(" 체력   : ", $"{_player.HP} / {_player.MaxHP + Item.HPBonus}", bonusHP);
            PrintwithColoredText(" 마나   : ", $"{_player.MP} / {_player.MaxMP + Item.MPBonus}", bonusMP);
            PrintwithColoredText(" 경험치 : ", $"{_player.Exp} / {_player.MaxExp}");
            PrintwithColoredText(" Gold   : ", _player.Gold.ToString(), " G");
            
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
            WriteLine(" 가지고 있는 아이템 입니다.");
            WriteLine();
            WriteLine(" [아이템 목록]");

            int inventoryCount = _player.Inventory.Count;
            for (int i = 0; i < inventoryCount; i++)
            {
                _player.Inventory[i].ItemInfo();
            }

            WriteLine();
            WriteLine(" 0. 나가기");
            WriteLine(" 1. 장착");
            WriteLine();
            WriteLine(" 원하시는 행동을 입력해주세요.");

            switch (CheckValidInput(0, 1))
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
            PrintColoredText(" 인벤토리 - 장착");
            WriteLine(" 아이템 장착을 장착 할 수 있습니다.");
            WriteLine();
            WriteLine(" [아이템 목록]");

            int inventoryCount = _player.Inventory.Count;
            for (int i = 0; i < inventoryCount; i++)
            {
                _player.Inventory[i].ItemInfo(true, i + 1);
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
            PrintColoredText(" 만물상");
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
            PrintColoredText(" 만물상");
            WriteLine(" 해당 번호를 입력하면 1개씩 구매할 수 있습니다.");
            WriteLine();
            WriteLine(" [보유 골드]");
            PrintwithColoredText(" ", _player.Gold.ToString(), "G");
            WriteLine();
            WriteLine(" [아이템 목록]");

            int inventoryCount = _merchant.Inventory.Count;
            for (int i = 0; i < inventoryCount; i++)
            {
                _merchant.Inventory[i].ItemInfo(true, i + 1);
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
                    if (_player.Gold >= _merchant.Inventory[input - 1].Cost)
                    {
                        _player.Gold -= _merchant.Inventory[input - 1].Cost;
                        _player.AddItem(_merchant.Inventory[input - 1]);

                        scene = Scene.buy;
                    }
                    else
                    {
                        WriteLine(" Gold가 부족합니다.");
                        WriteLine();
                        WriteLine(" 0. 다음");

                        CheckValidInput(0, 0);
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
            PrintColoredText(" 만물상");
            WriteLine(" 해당 번호를 입력하면 1개씩 판매할 수 있습니다.");
            WriteLine();
            WriteLine(" [보유 골드]");
            PrintwithColoredText(" ", _player.Gold.ToString(), "G");
            WriteLine();
            WriteLine(" [아이템 목록]");

            int inventoryCount = _player.Inventory.Count;
            for (int i = 0; i < inventoryCount; i++)
            {
                _player.Inventory[i].ItemInfo(true, i + 1);
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
                    _player.Gold += (int)(_player.Inventory[input - 1].Cost * 0.85f);
                    _player.SubtractItem(_player.Inventory[input - 1].Name);

                    scene = Scene.sell;
                    break;
            }
            return scene;
        }

        private Scene Upgrade()
        {
            Scene scene = Scene.main;
            Clear();
            WriteLine();
            PrintColoredText(" 운동!");
            WriteLine();
            PrintwithColoredText(" ", "100", " G 를 내면 트레이너가 운동을 시켜줍니다.");
            PrintwithColoredText(" (보유 골드 : ", $"{_player.Gold}", " G)");
            WriteLine();

            WriteLine($" 1. 달리기 (체력): {_player.HP} / {_player.MaxHP + Item.HPBonus}");
            WriteLine($" 2. 명상   (마나): {_player.MP} / {_player.MaxMP + Item.MPBonus}");
            WriteLine($" 3. 팔굽혀펴기 (공격력): {_player.Atk + Item.AtkBonus}");
            WriteLine($" 4. 맷집 단련  (방어력): {_player.Def + Item.DefBonus}");
            WriteLine();
            PrintColoredText(" 5. 비법 전수");
            WriteLine();
            WriteLine(" 원하시는 운동을 선택해주세요.");
            WriteLine();
            WriteLine(" 0. 나가기");
            WriteLine();
            int userInput = CheckValidInput(0, 5);

            switch (userInput)
            {
                case 0:
                    scene = Scene.main;
                    break;
                case 1:
                    if (_player.Gold >= 100)
                    {
                        _player.HP += 10;
                        _player.MaxHP += 10;
                        _player.Gold -= 100;
                        Clear();
                        WriteLine();
                        WriteLine(" 체력이 10 강화됐다!");
                        WriteLine();
                        WriteLine(" 0. 다음");
                        CheckValidInput(0, 0);
                        scene = Scene.upgrade;
                        break;
                    }
                    else
                    {
                        Clear();
                        WriteLine();
                        WriteLine(" Gold가 부족합니다.");
                        WriteLine();
                        WriteLine(" 0. 다음");
                        CheckValidInput(0, 0);
                        scene = Scene.upgrade;
                        break;
                    }
                case 2:
                    if (_player.Gold >= 100)
                    {
                        _player.MP += 1;
                        _player.MaxMP += 1;
                        _player.Gold -= 100;
                        Clear();
                        WriteLine();
                        WriteLine(" 마나가 1 강화됐다!");
                        WriteLine();
                        WriteLine(" 0. 다음");
                        CheckValidInput(0, 0);
                        scene = Scene.upgrade;
                        break;
                    }
                    else
                    {
                        Clear();
                        WriteLine();
                        WriteLine(" Gold가 부족합니다.");
                        WriteLine();
                        WriteLine(" 0. 다음");
                        CheckValidInput(0, 0);
                        scene = Scene.upgrade;
                        break;
                    }
                case 3:
                    if (_player.Gold >= 100)
                    {
                        _player.Atk += 1;
                        _player.Gold -= 100;
                        Clear();
                        WriteLine();
                        WriteLine(" 공격이 1 강화됐다!");
                        WriteLine();
                        WriteLine(" 0. 다음");
                        CheckValidInput(0, 0);
                        scene = Scene.upgrade;
                        break;
                    }
                    else
                    {
                        Clear();
                        WriteLine();
                        WriteLine(" Gold가 부족합니다.");
                        WriteLine();
                        WriteLine(" 0. 다음");
                        CheckValidInput(0, 0);
                        scene = Scene.upgrade;
                        break;
                    }
                case 4:
                    if (_player.Gold >= 100)
                    {
                        _player.Def += 1;
                        _player.Gold -= 100;
                        Clear();
                        WriteLine();
                        WriteLine(" 방어가 1 강화됐다!");
                        WriteLine();
                        WriteLine(" 0. 다음");
                        CheckValidInput(0, 0);
                        scene = Scene.upgrade;
                        break;
                    }
                    else
                    {
                        Clear();
                        WriteLine();
                        WriteLine(" Gold가 부족합니다.");
                        WriteLine();
                        WriteLine(" 0. 다음");
                        CheckValidInput(0, 0);
                        scene = Scene.upgrade;
                        break;
                    }
                case 5:
                    scene = Scene.gatcha;
                    break;
            }

            return scene;
        }

        private Scene Gatcha()
        {
            Scene scene = Scene.main;
            Clear();
            WriteLine();
            PrintColoredText(" 비법 전수");
            WriteLine();
            PrintwithColoredText(" ", "500", " G 를 내면 트레이너가 비법을 전수해 줄 것입니다.");
            WriteLine(" 비법을 전수 받으면 효과가 랜덤할 수 있습니다.");
            PrintwithColoredText(" (보유 골드 : ", $"{_player.Gold}", " G)");
            WriteLine();

            WriteLine($" 1. 물은 답을 알고 있다. (체력) : {_player.HP} / {_player.MaxHP + Item.HPBonus}");
            WriteLine($" 2. 수학의 정석 (마나) : {_player.MP} / {_player.MaxMP + Item.MPBonus}");
            WriteLine($" 3. 법전 (공격력) : {_player.Atk + Item.AtkBonus}");
            WriteLine($" 4. 자기 방어술 (방어력) : {_player.Def + Item.DefBonus}");
            WriteLine();
            WriteLine(" 원하는 비법을 선택해주세요.");
            WriteLine();
            WriteLine(" 0. 나가기");
            WriteLine();

            Random random = new Random();
            int randNum = random.Next(1, 10);
            int userInput = CheckValidInput(0, 4);
            switch (userInput)
            {
                case 0:
                    scene = Scene.main;
                    break;
                case 1:
                    if (_player.Gold >= 500)
                    {
                        _player.HP += randNum;
                        _player.MaxHP += randNum;
                        _player.Gold -= 500;

                        Clear();
                        WriteLine();
                        WriteLine(" 물은 답을 알고 있는 것 같다. 숨이 막힌다.");
                        WriteLine($" 체력이 {randNum} 강화됐다!");
                        WriteLine();
                        WriteLine(" 0. 다음");
                        CheckValidInput(0, 0);
                        scene = Scene.gatcha;
                        break;
                    }
                    else
                    {
                        Clear();
                        WriteLine();
                        WriteLine(" Gold가 부족합니다.");
                        WriteLine();
                        WriteLine(" 0. 다음");
                        CheckValidInput(0, 0);
                        scene = Scene.upgrade;
                        break;
                    }
                case 2:
                    if (_player.Gold >= 500)
                    {
                        _player.MP += randNum;
                        _player.MaxMP += randNum;
                        _player.Gold -= 500;
                        Clear();
                        WriteLine();
                        WriteLine(" 뭐라 적혀 있는지 모르겠지만 머리가 똑똑해진 것 같습니다.");
                        WriteLine($" 마나가 {randNum} 강화됐다!");
                        WriteLine();
                        WriteLine(" 0. 다음");
                        CheckValidInput(0, 0);
                        scene = Scene.gatcha;
                        break;
                    }
                    else
                    {
                        Clear();
                        WriteLine();
                        WriteLine(" Gold가 부족합니다.");
                        WriteLine();
                        WriteLine(" 0. 다음");
                        CheckValidInput(0, 0);
                        scene = Scene.upgrade;
                        break;
                    }
                case 3:
                    if (_player.Gold >= 500)
                    {
                        _player.Atk += randNum;
                        _player.Gold -= 500;
                        Clear();
                        WriteLine();
                        WriteLine(" 책 모서리가 이렇게 아픈줄 몰랐다.");
                        WriteLine($" 공격이 {randNum} 강화됐다!");
                        WriteLine();
                        WriteLine(" 0. 다음");
                        CheckValidInput(0, 0);
                        scene = Scene.gatcha;
                        break;
                    }
                    else
                    {
                        Clear();
                        WriteLine();
                        WriteLine(" Gold가 부족합니다.");
                        WriteLine();
                        WriteLine(" 0. 다음");
                        CheckValidInput(0, 0);
                        scene = Scene.upgrade;
                        break;
                    }
                case 4:
                    if (_player.Gold >= 500)
                    {
                        _player.Def += randNum;
                        _player.Gold -= 500;
                        Clear();
                        WriteLine();
                        WriteLine(" 싸움을 잘 할 것처럼 보이는 자세를 배웠다.");
                        WriteLine($" 방어가 {randNum} 강화됐다!");
                        WriteLine();
                        WriteLine(" 0. 다음");
                        CheckValidInput(0, 0);
                        scene = Scene.gatcha;
                        break;
                    }
                    else
                    {
                        Clear();
                        WriteLine();
                        WriteLine(" Gold가 부족합니다.");
                        WriteLine();
                        WriteLine(" 0. 다음");
                        CheckValidInput(0, 0);
                        scene = Scene.upgrade;
                        break;
                    }
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
            WriteLine($" 체력: {_player.HP} / {_player.MaxHP + Item.HPBonus}");
            WriteLine($" 마나: {_player.MP} / {_player.MaxMP + Item.MPBonus}");
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
                        _player.HP = _player.MaxHP + Item.HPBonus;
                        _player.MP = _player.MaxMP + Item.MPBonus;
                        _player.Gold -= 500;
                        _player.IsAlive = true;

                        WriteLine(" 휴식을 완료했습니다.");
                        WriteLine();
                        WriteLine(" 0. 다음");

                        CheckValidInput(0, 0);

                        scene = Scene.rest;
                    }
                    else
                    {
                        WriteLine(" Gold가 부족합니다.");
                        WriteLine();
                        WriteLine(" 0. 다음");

                        CheckValidInput(0, 0);

                        scene = Scene.rest;
                    }
                    break;
            }


            return scene;
        }

        private Scene HealingScene()
        {
            Scene scene = Scene.main;
            int countHP;
            int countMP;

            int indexHP = _player.InventoryIndex("체력 포션");
            if (indexHP >= 0)
            {
                countHP = _player.Inventory[indexHP].ItemCount;
            }
            else
            {
                countHP = 0;
            }

            int indexMP = _player.InventoryIndex("마나 포션");
            if (indexMP >= 0)
            {
                countMP = _player.Inventory[indexMP].ItemCount;
            }
            else
            {
                countMP = 0;
            }

            Clear();
            WriteLine();
            PrintColoredText(" 힐링 아이템");
            WriteLine(" 포션을 사용하면 30 회복할 수 있습니다.");
            WriteLine($" 체력 포션 {countHP} 개 / 마나 포션 {countMP} 개");
            WriteLine();
            WriteLine($" 체력: {_player.HP} / {_player.MaxHP + Item.HPBonus}");
            WriteLine($" 마나: {_player.MP} / {_player.MaxMP + Item.MPBonus}");
            WriteLine();
            WriteLine(" 1. 체력 포션");
            WriteLine(" 2. 마나 포션");
            WriteLine(" 0. 나가기");
            WriteLine();
            WriteLine(" 원하시는 행동을 입력해주세요.");
            WriteLine();

            switch (CheckValidInput(0, 2))
            {
                case 0:
                    scene = Scene.main;
                    break;
                case 1:
                    _player.UseHealPotion();

                    WriteLine();
                    WriteLine(" 0. 다음");

                    CheckValidInput(0, 0);

                    scene = Scene.healing;
                    break;
                case 2:
                    _player.UseManaPotion();

                    WriteLine();
                    WriteLine(" 0. 다음");

                    CheckValidInput(0, 0);

                    scene = Scene.healing;
                    break;
            }
            return scene;
        }

        private Scene DungeonScene()
        {
            Scene scene = Scene.main;
            Clear();

            WriteLine();
            PrintColoredText(" 사냥을 할 수 있는 공간으로 이동하는 던전 포탈입니다.");
            WriteLine();
            WriteLine(" 1. 입장하기");
            WriteLine(" 0. 나가기");
            WriteLine();
            WriteLine(" 원하시는 행동을 입력해주세요.");

            switch (CheckValidInput(0, 1))
            {
                case 0:
                    scene = Scene.main;
                    break;
                case 1:
                    if (_player.HP == 0)
                    {
                        WriteLine(" 체력이 없습니다. 체력을 회복하고 시도해주세요");
                        WriteLine();
                        WriteLine(" 0. 돌아가기");
                        CheckValidInput(0, 0);
                        scene = Scene.dungeon;
                    }
                    else
                    {
                        scene = Scene.stagePick;
                    }
                    break;
            }
            return scene;
        }

        private Scene StagePickScene()
        {
            Scene scene = Scene.main;
            Clear();

            if (_player.Job == JobType.developer)
            {
                _stage = 1000;
            }

            WriteLine();
            PrintColoredText(" 던전 포탈 난이도 설정");
            WriteLine();
            WriteLine(" 0. 나가기");
            WriteLine();
            WriteLine(" 던전 난이도를 입력해주세요");
            WriteLine(" 현재 선택 가능 난이도: 1 ~ {0}", _stage);

            

            int userInput = CheckValidInput(0, _stage);

            switch (userInput)
            {
                case 0:
                    scene = Scene.dungeon;
                    break;
                default:
                    Battle battle = new Battle(userInput, _player);
                    _stage = battle.PlayBattle();
                    scene = Scene.dungeon;
                    break;
            }
            return scene;
        }

        private int CheckValidInput(int min, int max)
        {
            while (true)
            {
                WriteLine();
                Write(" >> ");
                string input = ReadLine();

                bool parseSuccess = int.TryParse(input, out var ret);
                if (parseSuccess)
                {
                    if (ret >= min && ret <= max)
                        return ret;
                    else
                    {
                        WriteLine();
                        WriteLine(" 잘못된 입력입니다. 다시 입력해주세요");
                    }
                }
                else
                {
                    if(input == "iam")
                    {
                        return (int)JobType.developer + 1;
                    }
                    else
                    {
                        WriteLine();
                        WriteLine(" 잘못된 입력입니다. 다시 입력해주세요");
                    }
                }
            }
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