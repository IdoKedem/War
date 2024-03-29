using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War
{
    class Globals
    { public static bool stopGame = false; }

    class Player
    {
        private static Random r1 = new Random();

        private int[] hand = new int[104];
        private int[] table = new int[104];
        private string name;

        public Player(string name)
        {
            this.name = name;
            CreateHand();
            ShuffleHand();
        }

        public void CreateHand()
        {
            for (int i = 0; i < 13; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    this.hand[i * 4 + j] = i + 2;
                }
            }
        }
        public void ShuffleHand()
        {
            int card1, card2, temp;

            for (int i = 0; i < 1000; i++)
            {
                card1 = r1.Next(52);
                card2 = r1.Next(52);

                temp = this.hand[card1];
                this.hand[card1] = this.hand[card2];
                this.hand[card2] = temp;
            }
        }
        public void Shrink(int[] arr, int start, int amount)
        {
            for (int i = start; i < arr.Length - amount; i++)
            { arr[i] = arr[i + amount]; }

            for (int i = arr.Length - amount; i < arr.Length; i++)
            { this.hand[i] = 0; }
        }

        public string GetName()
        { return this.name; }
        public int[] GetHand()
        { return this.hand; }
        public int[] GetTable()
        { return this.table; }

        public void OpenCard()
        {
            Console.WriteLine(this.name + " Plays " + Program.CardName(this.hand[0]));

            Array.Copy(this.hand, 0, this.table, Program.LastValue(this.table) + 1, 1);
            Shrink(this.hand, 0, 1);
        }
        public void War()
        {
            Console.WriteLine(this.name + " places two cards");

            Array.Copy(this.hand, 0, this.table, Program.LastValue(this.table) + 1, 2);
            Shrink(this.hand, 0, 2);

            OpenCard();
        }
    }

    class Program
    {
        public static int LastValue(int[] arr)
        {
            int last = -1;
            for (int i = 0; i < arr.Length; i++)
            {
                if (arr[i] != 0)
                    last = i;
            }
            return last;
        }
        public static string CardName(int num)
        {
            switch (num)
            {
                case 11:
                    return "Jack";
                case 12:
                    return "Queen";
                case 13:
                    return "King";
                case 14:
                    return "Ace";
                default:
                    return num.ToString();
            }
        }

        public static Player RoundWinner(Player p1, Player p2)
        {
            int card1 = p1.GetTable()[LastValue(p1.GetTable())]; // last card in each player's table array
            int card2 = p2.GetTable()[LastValue(p2.GetTable())];

            if (card1 > card2)
            {
                Console.WriteLine(p1.GetName() + " won this round");
                Console.WriteLine();

                Collect(p1, p2);
                return p1;
            }
            else if (card2 > card1)
            {
                Console.WriteLine(p2.GetName() + " won this round");
                Console.WriteLine();

                Collect(p2, p1);
                return p2;
            }

            Console.WriteLine("Draw");
            Console.WriteLine();

            if (LastValue(p1.GetHand()) + 1 < 3) // has less than 3 cards
            {
                Globals.stopGame = true;
                return p2;
            }
            if (LastValue(p2.GetHand()) + 1 < 3)
            {
                Globals.stopGame = true;
                return p1;
            }

            p1.War();
            p2.War();

            return RoundWinner(p1, p2);
        }
        public static void Collect(Player winner, Player loser)
        {
            int[] winnerTable = winner.GetTable();
            int[] loserTable = loser.GetTable();

            Array.Copy(winnerTable, 0, winner.GetHand(), LastValue(winner.GetHand()) + 1, LastValue(winnerTable) + 1);
            Array.Clear(winnerTable, 0, LastValue(winnerTable) + 1);

            Array.Copy(loserTable, 0, winner.GetHand(), LastValue(winner.GetHand()) + 1, LastValue(loserTable) + 1);
            Array.Clear(loserTable, 0, LastValue(loserTable) + 1);

            Console.WriteLine("Table cleared!");
            Console.WriteLine();

            Console.WriteLine(winner.GetName() + " card count: " + (LastValue(winner.GetHand()) + 1));
            Console.WriteLine(loser.GetName() + " card count: " + (LastValue(loser.GetHand()) + 1));
            Console.WriteLine();
        }

        public static void Game()
        {
            Player p1 = new Player("Player 1");
            Player p2 = new Player("Player 2");
            Player winner = p1;   // enables use of winner after while

            Console.WriteLine("Game Start! ");
            Console.WriteLine();

            while (LastValue(p1.GetHand()) >= 0 && LastValue(p2.GetHand()) >= 0 && !Globals.stopGame)
            {
                p1.OpenCard();
                p2.OpenCard();
                Console.WriteLine();

                winner = RoundWinner(p1, p2);
            }
            Console.WriteLine(winner.GetName() + " WON");
        }

        static void Main(string[] args)
        { Game(); }
    }
}
