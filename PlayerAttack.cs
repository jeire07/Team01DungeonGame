using static System.Console;

namespace Team01DungeonGame
{
    //public class Dungeon
    //{
    //    public int Stage { get; set; }
    //    public int MonsterCount { get; set; }
    //    public List<Monster> Monsters { get; set; }

    //    public Dungeon(int stage)
    //    {
    //        Stage = stage;
    //        Monsters = new List<Monster>(4);
    //    }

    //    private void MakeStage()
    //    {
    //        Random random = new Random();
    //        int randNum = random.Next(1, 5);
    //        int randType = random.Next(0, 3);

    //        if (Stage >= 1)
    //        {
    //            Monsters[0] = new Monster(Stage, (MonsterType)randType);
    //        }

    //        if (Stage >= 2)
    //        {
    //            if (randNum > 3)
    //            {
    //                Monsters[1] = new Monster(Stage, (MonsterType)randType);
    //            }
    //        }

    //        if (Stage >= 3)
    //        {
    //            if (randNum > 2)
    //            {
    //                Monsters[2] = new Monster(Stage, (MonsterType)randType);
    //            }
    //        }

    //        if (Stage >= 4)
    //        {
    //            if (randNum > 1)
    //            {
    //                Monsters[3] = new Monster(Stage, (MonsterType)randType);
    //            }
    //        }
    //    }
    //}

    public class PlayerAttack
    {
        /// <summary>
        /// 이 method는 플레이어의 공격턴을 위한 method이며
        /// TextGame Class의 하위 method이므로 이동이 필요합니다.
        /// 
        /// </summary>
        /// <returns></returns>
        private Scene PlayerAttackScene()
        {
            // 실제로는 클래스 내부에서 이미 인스턴스로 생성한 것을 가져다 쓸 것으로 추정함
            // 따라서 아랫줄은 에러를 안띄우기 위함이므로 삭제 필요
            // 주석이 없는데 에러가 뜨는 부분은 TextGame 클래스 내부로 이동 시 문제 없음
            List<Monster> monsters = new List<Monster>(); 

            Scene scene = 0;
            Clear();

            WriteLine();
            PrintColoredText(" Battle!!");
            WriteLine("");

            // monster 배열 이름 변경 필요, list가 아니라 배열인 경우 Length로 대체
            int monsterCount = monsters.Count;
            for (int i = 0; i < monsterCount; i++)
            {
                monsters[i].MonsterInfo(true, i+1);
            }

            WriteLine();
            WriteLine(" 0. 나가기");
            WriteLine();
            WriteLine(" 공격할 대상을 선택해주세요.");

            int input = CheckMonsterInput(0, monsterCount);
            switch (input)
            {
                case 0:
                    scene = Scene.playerTurn;
                    break;
                default:
                    monsters[input - 1].TakeDamage(PlayerDamage(_player.Atk + Item.AtkBonus));
                    scene = Scene.playerResult;
                    break;
            }
            return scene;
        }

        /// <summary>
        /// 공격할 몬스터의 번호를 사용자가 입력할 때, 입력이 유효한지 확인하는 method
        /// 사망한 몬스터일 경우에도 무효하며, 재입력 요구함
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private int CheckMonsterInput(int min, int max)
        {
            // 실제로는 클래스 내부에서 이미 인스턴스로 생성한 것을 가져다 쓸 것으로 추정함
            // 따라서 아랫줄은 에러를 안띄우기 위함이므로 삭제 필요
            Monster[] Monsters = new Monster[4];

            while (true)
            {
                Write(" ");
                string input = ReadLine();

                bool parseSuccess = int.TryParse(input, out var ret);
                if (parseSuccess)
                {
                    if (ret >= min && ret <= max)
                    {
                        // 이 부분에 몬스터 클래스를 아이템으로 가지는 배열 혹은 컬렉션 필요
                        if (Monsters[ret].IsAlive)  // <- Monsters[]의 식별자 수정
                        {
                            return ret;
                        }
                    }
                    else
                    {
                        WriteLine(" 잘못된 입력입니다. 다시 입력해주세요");
                    }
                }
            }
        }

        /// <summary>
        /// 사용자의 atk + atkBonus 값에 대해 +-10%의 데미지를
        /// 치명타와 명중율을 적용하여 가하는 데미지를 계산하는 method
        /// Player Attack에서 사용함.
        /// </summary>
        /// <param name="atk">player's atk field value</param>
        /// <returns></returns>
        public int PlayerDamage(int atk)
        {
            int minDamage = (int)Math.Ceiling(atk * 0.9f);
            int maxDamage = (int)Math.Ceiling(atk * 1.1f);

            Random range = new Random();
            int damage = range.Next(minDamage, maxDamage);

            // 치명타 & 회피 확률
            int critOrAvoid = range.Next(0, 20);

            if (critOrAvoid < 3)  // 치명타 -> 0, 1, 2 -> 15% probability
            {
                return (int)Math.Ceiling(damage * 1.6f);
            }
            else if (critOrAvoid > 17)  // 회피 -> 18, 19 -> 10% probability
            {
                return 0;
            }
            else
            {
                return damage;
            }
        }

        /// <summary>
        /// 몬스터의 atk + atkBonus 값에 대해 +-10%의 데미지를
        /// 계산하는 method
        /// 몬스터 턴에서 해당 코드를 사용할 수도 아닐 수도 있음
        /// </summary>
        /// <param name="atk">monster's atk field value</param>
        /// <returns></returns>
        public int MonsterDamage(int atk)
        {
            int minDamage = (int)Math.Ceiling(atk * 0.9f);
            int maxDamage = (int)Math.Ceiling(atk * 1.1f);

            Random range = new Random();
            int damage = range.Next(minDamage, maxDamage);
            return damage;
        }
    }
}
