using static System.Console;

namespace Team01DungeonGame
{
    public class Program
    {
        static void Main(string[] args)
        {
            //TextGame runTextGame = new TextGame();
            //runTextGame.PlayText();

            DungeonPlay runDungeon = new DungeonPlay();
            runDungeon.Battle();
        }
    }
}