

using System.ComponentModel.Design;

namespace Program
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Generating moves...");
            NoughtsCrosses nc = new NoughtsCrosses();
            nc.Tree(nc.board, 1);
            Console.WriteLine("Ready!");
            Console.WriteLine("Press <Enter> to play");
            Console.ReadLine();
            nc.Play();
            //209088 end states
            //total memory used 152 MB
        }
    }
    class NoughtsCrosses
    {


        public Matrix board = new Matrix(3, 3);
        int player_symbol = -1;
        Dictionary<Matrix, (int, int, int)> decision_tree = new Dictionary<Matrix, (int, int, int)>();
        Dictionary<Matrix, List<Matrix>> tree = new Dictionary<Matrix, List<Matrix>>();
        public List<Matrix> wins = new List<Matrix>();
        void resetBoard()
        {
            board = new Matrix(3,3);
        }
        
        public void Tree(Matrix start, int symbol=1)
        {
            foreach ((int,int) available in Available(start))
            {
                Matrix child = new Matrix(3, 3);
                start.Copy_To(child);
                child[available.Item1, available.Item2] = symbol;
                AddToTree(start,child);
                if (checkWin(child) == 0)
                {
                    Tree(child,-symbol);//keeps flipping between -1 and 1
                }
                else
                {
                    //Console.WriteLine();
                    //Print(child);
                    wins.Add(child);
                }
            }
        }

        public void Play()
        {
            int turn = 1;
            Print(board);
            while (checkWin(board)==0 && Available(board).Count()>0) {
                
                Console.WriteLine("Human moves:");
                ask_and_place(turn);
                Print(board);
                turn =-turn;
                //Console.WriteLine("NEXT MOVES");
                //List<Matrix> list = tree[board];
                //Console.WriteLine();
                //foreach (Matrix child in list)
                //{
                //    Print(child);
                //    Console.WriteLine("--------");
                //    Console.WriteLine();
                //}
                //Console.WriteLine("================================");
                int w;List<(int, int)> moves;
                (w,moves)=minimax(board,turn,new Matrix(3, 3));
                Console.WriteLine("Computer moves:");
                place(moves.Last().Item1,moves.Last().Item2,turn);
                turn = -turn;
                Print(board);
            }
            Console.WriteLine("GAME OVER!");
            Print(board);
        }

        (int, List<(int,int)>) minimax(Matrix board, int turn, Matrix parent)  //computer is minus one so minimise
        {
            int score = checkWin(board);
            List<(int, int)> bestMoves=new List<(int, int)>();
            if (score == 0 && Available(board).ToList().Count()>0)
            {
                List<int> scores = new List<int>();
                List<(int, int)> moves = new List<(int, int)>();
                foreach (Matrix child in tree[board])
                {
                    (int, List<(int, int)>) reply = minimax(child, -turn, board);
                    scores.Add(reply.Item1);
                    bestMoves = reply.Item2;
                    Matrix mmove = child + -1 * board;
                    (int, int) move = (-1, -1);
                    for (int r = 0; r < 3; r++)
                    {
                        for (int c = 0; c < 3; c++)
                        {
                            if (mmove[r, c] != 0) move = (r, c);
                        }
                    }
                    moves.Add(move);
                }
                //if(turn==1) return (scores.Max());
                //if(turn==-1) return (scores.Min());

                if (turn == 1) { bestMoves.Add(moves[scores.IndexOf(scores.Max())]); return (scores.Max(), bestMoves); }
                if (turn == -1) { bestMoves.Add(moves[scores.IndexOf(scores.Min())]); return (scores.Min(), bestMoves); }
                else { throw new Exception(); }
            }
            else
            {
                Matrix Mmove = board + -1 * parent;
                (int, int) Move = (-1, -1);
                for (int r = 0; r < 3; r++)
                {
                    for (int c = 0; c < 3; c++)
                    {
                        if (Mmove[r, c] != 0) Move = (r, c);
                    }
                }
                bestMoves.Add(Move);
                return (score, bestMoves);
            }
        }

        void AddToTree(Matrix parent, Matrix child)
        {
            if (tree.ContainsKey(parent))
            {
                if (tree[parent].Contains(child)) return;
                tree[parent].Add(child);
            }
            else
            {
                tree[parent] = new List<Matrix> { child };
            }
        }
        public void Print(Matrix board)
        {
            for (int r=0;r<3;r++)
            {
                for (int c=0;c<3;c++)
                {
                    Console.Write($"{( (board[r,c] == 0)? " ": ((board[r, c] == 1)? "x":"o") ) } | ");
                }
                Console.WriteLine();
            }
        }
        IEnumerable<(int,int)> Available(Matrix m)
        {
            for (int x=0;x<3;x++)
            {
                for (int y=0;y<3;y++)
                {
                    if (m[x,y]==0)
                    {
                        yield return (x,y);
                    }
                }
            }
        }

        //0 0   0 1   0 2
        //1 0   1 1   1 2
        //2 0   2 1   2 2

        void place(int row, int col, int symbol)
        {
            board[row,col]=symbol;
        }
        void ask_and_place()
        {
            Console.WriteLine("Enter row (0 indexed)");
            int r = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Enter col (0 indexed)");
            int c = Int32.Parse(Console.ReadLine());
            if (board[r, c] != 0)
            {
                Console.WriteLine("Taken!");
                ask_and_place();
            }
            else
            {
                place(r, c, player_symbol);
            }
            return;
        }
        void ask_and_place(int player_symbol)
        {
            Console.WriteLine("Enter row (0 indexed)");
            int r = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Enter col (0 indexed)");
            int c = Int32.Parse(Console.ReadLine());
            if (board[r, c] != 0)
            {
                Console.WriteLine("Taken!");
                ask_and_place();
            }
            place(r, c, player_symbol);
            return;
        }
        int checkWin(Matrix Board)
        {
            int win = 0;
            Matrix ones = new Matrix(1, 3);
            ones[0, 0] = 1;
            ones[0, 1] = 1;
            ones[0, 2] = 1;
            //ones[1, 0] = 1;
            //ones[1, 1] = 1;
            //ones[1, 2] = 1;
            //ones[2, 0] = 1;
            //ones[2, 1] = 1;
            //ones[2, 2] = 1;

            Matrix c = ones * Board;   // 1 by 3
            for (int n=0;n<3;n++)
            {
                if (c[0, n] == 3) return 1;
                if (c[0, n] == -3) return -1;
            }
            c = Board*(ones.Transpose());   // 3 by 1
            for (int n = 0; n < 3; n++)
            {
                if (c[n, 0] == 3) return 1;
                if (c[n, 0] == -3) return -1;
            }
            //check the damn diagonals
            if (Board[0, 0] == Board[1,1] & Board[1, 1] == Board[2,2] & Board[0,0]!=0)
            {
                return (int)Board[0,0];
            }
            if (Board[0, 2] == Board[1, 1] & Board[1, 1] == Board[2, 0] & Board[1, 1] != 0)
            {
                return (int)Board[1, 1];
            }
            return 0;
        }
    }
}