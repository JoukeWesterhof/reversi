using System;
using System.Drawing;
using System.Windows.Forms;
Form scherm = new Form();
scherm.Text = "Reversi";
scherm.ClientSize = new Size(900, 900);

Button formaat1 = new Button(); scherm.Controls.Add(formaat1);
formaat1.Location = new Point(50, 20); formaat1.Text = "4x4";
Button formaat2 = new Button(); scherm.Controls.Add(formaat2);
formaat2.Location = new Point(150, 20); formaat2.Text = "6x6";
Button formaat3 = new Button(); scherm.Controls.Add(formaat3);
formaat3.Location = new Point(250, 20); formaat3.Text = "8x8";
Button formaat4 = new Button(); scherm.Controls.Add(formaat4);
formaat4.Location = new Point(350, 20); formaat4.Text = "10x10";

Button New = new Button(); scherm.Controls.Add(New);
New.Location = new Point(450, 20); New.Text = "Nieuw spel";
Button help = new Button(); scherm.Controls.Add(help);
help.Location = new Point(550, 20); help.Text = "Help";

void teken(object sender, PaintEventArgs pea)
{
    pea.Graphics.FillEllipse(Brushes.Red, 50, 50, 50, 50);
    pea.Graphics.FillEllipse(Brushes.Blue, 50, 100, 50, 50);
}
scherm.Paint += teken;

Label rood = new Label();
scherm.Controls.Add(rood);
rood.Text = "(tel de stenen van rood)";
rood.Location = new Point(100, 65);
rood.Size = new Size(200, 20);

Label blauw = new Label();
scherm.Controls.Add(blauw);
blauw.Text = "(tel de stenen van blauw)";
blauw.Location = new Point(100, 115);
blauw.Size = new Size(200, 20);

Label aanzet = new Label(); scherm.Controls.Add(aanzet);
aanzet.Location = new Point(50, 160); aanzet.Text = ".....is aan zet";

//kubus
Label kubus = new Label();
Bitmap plaatje = new(312, 312);
kubus.BackColor = Color.White;
kubus.Image = plaatje;
Graphics gr = Graphics.FromImage(plaatje);
kubus.Paint += Board;

scherm.ClientSize = new Size(800, 700);
kubus.Location = new Point(50, 200);
kubus.Size = new Size(302, 302);
scherm.Controls.Add(kubus);

void Board(object o, PaintEventArgs pea)
{
    // Maak een nieuw game board aan van 8x8
    Board board = new Board(8);

    int lijn;
    bool knop8 = false;
    bool knop6 = false;
    bool knop4 = false;
    int gridsize = 300;

    if (knop8 == true)
    {
        gridsize = 400;
    }
    if (knop6 == true)
    {
        gridsize = 300;
    }
    if (knop4 == true)
    {
        gridsize = 200;
    }

    Pen Blackpen = new Pen(Color.Black, 0);
    for (lijn = 0; lijn <= gridsize; lijn += 50)

    {
        pea.Graphics.DrawLine(Blackpen, lijn, 0, lijn, gridsize);
        pea.Graphics.DrawLine(Blackpen, 0, lijn, gridsize, lijn);
    }

}
kubus.Invalidate();


kubus.MouseClick += Klik;
kubus.MouseMove += Beweeg;

Point hier = new Point(0, 0);
int aantal = 1;

void Beweeg(object o, MouseEventArgs mea)
{
    hier = mea.Location;
    kubus.Invalidate();

}

void Klik(object o, MouseEventArgs mea)
{

    kubus.Invalidate();
}

Application.Run(scherm);
class Board
{
    static void Main(string[] args)
    {
        // Nieuw bord van 8x8
        Board board = new Board(8);

        // Run de game loop
        //PlayGame(board);

    }

    private readonly int[,] _board;
    private readonly int _size;

    public Board(int size)
    {
        _size = size;
        _board = new int[size, size];

    }


    public int this[int row, int col]
    {
        get => _board[row, col];
        set => _board[row, col] = value;
    }

    public int Size => _size;


    public bool IsValidMove(int row, int col, int player)
    {
        // Is het vakje al bezet?
        if (_board[row, col] != 0)
        {
            return false;
        }

        // Check de 8 omringende cellen voor de tegenstander zn stenen
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                int r = row + i;
                int c = col + j;
                if (r >= 0 && r < _size && c >= 0 && c < _size && _board[r, c] == player)
                {
                    // Tegenstander zn steen gevonden, check nu de friendly stenen in dezelfde richtong
                    r += i;
                    c += j;
                    while (r >= 0 && r < _size && c >= 0 && c < _size && _board[r, c] == player)
                    {
                        r += i;
                        c += j;
                    }

                    if (r >= 0 && r < _size && c >= 0 && c < _size && _board[r, c] == -player)
                    {
                        // friendly steen gevonden, dus je kan deze move maken
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public void MakeMove(int row, int col, int player)
    {
        _board[row, col] = player;

        // flip tegenstanders stenen in de tegengestelde richting
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                int r = row + i;
                int c = col + j;
                if (r >= 0 && r < _size && c >= 0 && c < _size && _board[r, c] == -player)
                {
                    // tegenstander steen gevonden, check voor friendly stenen in dezelfde richting
                    r += i;
                    c += j;
                    while (r >= 0 && r < _size && c >= 0 && c < _size && _board[r, c] == -player)
                    {
                        r += i;
                        c += j;
                    }

                    if (r >= 0 && r < _size && c >= 0 && c < _size && _board[r, c] == player)
                    {
                        // friendly piece gevonden, flip tegenstanders stenen tusssenin
                        r -= i;
                        c -= j;
                        while (_board[r, c] == -player)
                        {
                            _board[r, c] = player;
                            r -= i;
                            c -= j;
                        }
                    }
                }
            }
        }
    }

    public int GetScore(int player)
    {
        int score = 0;
        for (int i = 0; i < _size; i++)
        {
            for (int j = 0; j < _size; j++)
            {
                if (_board[i, j] == player)
                {
                    score++;
                }
            }
        }
        return score;
    }

    public bool IsGameOver()
    {
        for (int i = 0; i < _size; i++)
        {
            for (int j = 0; j < _size; j++)
            {
                if (_board[i, j] == 0)
                {
                    return false;
                }
            }
        }
        return true;  
    }
}