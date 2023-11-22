using static System.Console;
using static System.Formats.Asn1.AsnWriter;

namespace Team01DungeonGame
{
    internal class Skill
    {
        public enum JobType { human, warrior, mage, developer };
        enum Scene
        {
            playerPick, playerAtk, playerSkill, playerEnd,
            monster, victory, defeat, run, healing, result, exitDungeon
        }
        enum AtkEffect { normal, skill, critical, avoid }

        public int Stage { get; set; }
        private List<Monster> _monsters { get; set; }
        private Character _player { get; set; }
        private int _playerDamage { get; set; }
        private int _monsterIdx { get; set; }

        private AtkEffect atkType = AtkEffect.normal;
        private int _enterHP;


        enum PlayerSkill
        {
            SpinSlash,      //무직 스킬
            MemoryCrush,

            AlphaStrike,    //전사 스킬
            DoubleStrike,

            MagicBullet,    //마법사 스킬
            Inferno
        }

        class Player
        {
            public JobType PlayerJob { get; set; }

            public Player(JobType job)
            {
                PlayerJob = job;
            }
        }

        //private void AlphaStrike()
        //{
        //    if (_player.MP >= 15)
        //    {
        //        for (int i = 0; i < 3; i++)
        //        {
        //            WriteLine();
        //            WriteLine(" 공격할 대상을 선택해주세요.");

        //            foreach (Monster checkAlive in _monsters)
        //            {
        //                if (checkAlive.IsAlive)
        //                {
        //                    scene = Scene.monster;
        //                    break;
        //                }
        //                else
        //                {
        //                    scene = Scene.victory;
        //                }
        //            }

        //            int MonsterNum = CheckMonsterInput(monsterCount);
        //            if (MonsterNum == 0)
        //            {
        //                scene = Scene.playerSkill;
        //                return;
        //            }

        //            _playerDamage = (int)(PlayerDamage(_player.Atk + Item.AtkBonus, out atkType) * 0.75);
        //            _monsters[MonsterNum - 1].TakeDamage(_playerDamage);
        //            _monsterIdx = MonsterNum - 1;
        //        }

        //        _player.MP -= 15;
        //        scene = Scene.playerEnd;
        //    }
        //    else
        //    {
        //        WriteLine("MP가 부족합니다.");
        //        WriteLine("0. 돌아가기");
        //        CheckValidInput(0, 0);
        //        scene = Scene.playerSkill;
        //    }
        //}





        //public Skill UseSkill()
        //{
        //    Skill playerSkill = GetPlayerSkill();

        //    switch (playerSkill)
        //    {
        //        case Skill.SpinSlash:
        //            Console.WriteLine("");
        //            break;
        //        case Skill.AlphaStrike:
        //            Console.WriteLine("");
        //            break;
        //        case Skill.RapidShot:
        //            Console.WriteLine("");
        //            break;
        //        default:
        //            Console.WriteLine("");
        //            break;
        //    }
        //    return playerSkill;
        //}

        //private Skill GetPlayerSkill()
        //{
        //    switch (PlayerJob)
        //    {
        //        case Job.human:
        //            return Skill.SpinSlash;
        //        case Job.human:
        //            return Skill.MemoryCrush;
        //        case Job.Warrior:
        //            return Skill.AlphaStrike;
        //        case Job.Warrior:
        //            return Skill.DoubleStrike;
        //        case Job.Mage:
        //            return Skill.MagicBullet;
        //        case Job.Mage:
        //            return Skill.Inferno;

        //        default:
        //            Console.WriteLine("Invalid job!");
        //            return Skill.Slash;
        //    }
        //}

        //class Program
        //{
        //    static void Main()
        //    {
        //        Player humanPlayer = new Player(JobType.human);
        //        Player warriorPlayer = new Player(JobType.warrior);
        //        Player magePlayer = new Player(JobType.mage);
        //        Player developerPlayer = new Player(JobType.developer);
        //    }

        //}
    }
}
