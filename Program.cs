using static System.Console;

namespace Team01DungeonGame
{
    public class Program
    {
        static void Main(string[] args)
        {
            //TextGame runTextGame = new TextGame();
            //runTextGame.PlayText();

            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
            WriteLine(RandomMonsterIndex());
        }

        static List<int> _monsters = new List<int> { 1, 2, 3, 4, 5};
        static int RandomMonsterIndex()  //임의로 작성한 스킬랜덤지정 메소드
        {
            Random random = new Random();
            List<int> indexes = new List<int>();

            for (int i = 0; i < _monsters.Count; i++)
            {
                if ((_monsters[i] % 2) == 0)
                {
                    indexes.Add(i);
                }
            }

            int randIndex = random.Next(0, indexes.Count + 1);
            randIndex = indexes[randIndex];
            return randIndex;
        }
    }
}