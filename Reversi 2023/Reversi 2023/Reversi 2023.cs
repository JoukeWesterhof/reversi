using System;
using System.Drawing;
using System.Windows.Forms;
public class ReversiGame : Form
{
    private Button formaat4x4, formaat6x6, formaat8x8, formaat10x10;
    private Button NieuwSpelButton, helpButton;
    private Panel kubus;
    private Label roodLabel, blauwLabel, aanzetLabel;

    private int gridsize = 500;
    private bool grid10x10 = false;
    private bool grid8x8 = false;
    private bool grid6x6 = true;
    private bool grid4x4 = false;
    private int[,] board;
    private int aanzet;
    private bool welhelpen = false;
    public ReversiGame()
    {
        InitializeComponents();
        InitializeGame();
        UpdateGrid();
    }
    private void InitializeComponents()
    {
        Text = "Reversi";
        ClientSize = new Size(1000, 1000);

        formaat4x4 = CreateButton("4x4", 50);
        formaat6x6 = CreateButton("6x6", 150);
        formaat8x8 = CreateButton("8x8", 250);
        formaat10x10 = CreateButton("10x10", 350);
        NieuwSpelButton = CreateButton("Nieuw spel", 450);
        helpButton = CreateButton("Help", 550);
        helpButton.Click += (sender, e) => helpuitaan();

        roodLabel = CreateLabel("er zijn nu 2 stenen van rood", 100, 65);
        blauwLabel = CreateLabel("er zijn nu 2 stenen van blauw", 100, 115);
        aanzetLabel = CreateLabel(".....is aan zet", 50, 160);

        kubus = new Panel();
        kubus.BackColor = Color.White;
        kubus.Location = new Point(50, 200);
        kubus.Size = new Size(gridsize, gridsize);
        kubus.Paint += TekenBoard;
        kubus.Click += Kubus_Click;
        kubus.MouseClick += Kubus_MouseClick;
        kubus.TabStop = true;
        Controls.Add(kubus);
        NieuwSpelButton.Click += NieuwSpelButton_Click;

        formaat4x4.Click += Formaat4x4_Click;
        formaat6x6.Click += Formaat6x6_Click;
        formaat8x8.Click += Formaat8x8_Click;
        formaat10x10.Click += Formaat10x10_Click;

        Paint += Tekenen;
    }
    void Tekenen(object sender, PaintEventArgs pea)
    {
        pea.Graphics.FillEllipse(Brushes.Red, 50, 50, 50, 50);
        pea.Graphics.FillEllipse(Brushes.Blue, 50, 100, 50, 50);
    }
    private Button CreateButton(string text, int x)
    {
        Button button = new Button();
        Controls.Add(button);
        button.Location = new Point(x, 20);
        button.Text = text;
        return button;
    }
    private Label CreateLabel(string text, int x, int y)
    {
        Label label = new Label();
        Controls.Add(label);
        label.Text = text;
        label.Location = new Point(x, y);
        label.Size = new Size(200, 20);
        return label;
    }
    private void Formaat10x10_Click(object sender, EventArgs e)
    {
        grid10x10 = true;
        grid8x8 = false;
        grid6x6 = false;
        grid4x4 = false;
        UpdateGrid();
        kubus.Invalidate();
    }
    private void Formaat8x8_Click(object sender, EventArgs e)
    {
        grid10x10 = false;
        grid8x8 = true;
        grid6x6 = false;
        grid4x4 = false;
        UpdateGrid();
        kubus.Invalidate();
    }
    private void Formaat6x6_Click(object sender, EventArgs e)
    {
        grid10x10 = false;
        grid8x8 = false;
        grid6x6 = true;
        grid4x4 = false;
        UpdateGrid();
        kubus.Invalidate();
    }
    private void Formaat4x4_Click(object sender, EventArgs e)
    {
        grid10x10 = false;
        grid8x8 = false;
        grid6x6 = false;
        grid4x4 = true;
        UpdateGrid();
        kubus.Invalidate();
    }
    private void NieuwSpelButton_Click(object sender, EventArgs e)
    {
        InitializeGame();
        UpdateStatusLabel();
        UpdatehoeveelLabels();
        kubus.Invalidate();
    }
    private void UpdateGrid()
    {
        if (grid10x10)
        {
            gridsize = 500;
        }
        else if (grid8x8)
        {
            gridsize = 400;
        }
        else if (grid6x6)
        {
            gridsize = 300;
        }
        else if (grid4x4)
        {
            gridsize = 200;
        }
        InitializeGame();
    }
    private void InitializeGame()
    {

        board = new int[gridsize / 50, gridsize / 50];

        int mid = board.GetLength(0) / 2;
        board[mid - 1, mid - 1] = board[mid, mid] = 1;
        board[mid - 1, mid] = board[mid, mid - 1] = 2;
        aanzet = 1;
        UpdateStatusLabel();
    }
    private void helpuitaan()
    {
        welhelpen = !welhelpen;
        kubus.Invalidate();
    }
    private void TekenBoard(object o, PaintEventArgs pea)
    {
        int lijn;
        Pen Blackpen = new Pen(Color.Black, 0);
        Brush highlightBrush = new SolidBrush(Color.Yellow);

        for (lijn = 0; lijn <= gridsize; lijn += 50)
        {
            pea.Graphics.DrawLine(Blackpen, lijn, 0, lijn, gridsize);
            pea.Graphics.DrawLine(Blackpen, 0, lijn, gridsize, lijn);
        }
        if (welhelpen)
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (goeiezet(i * 50, j * 50))
                    {
                        pea.Graphics.FillRectangle(highlightBrush, i * 50, j * 50, 50, 50);
                    }
                }
            }
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] == 1)
                    pea.Graphics.FillEllipse(Brushes.Red, i * 50, j * 50, 50, 50);
                else if (board[i, j] == 2)
                    pea.Graphics.FillEllipse(Brushes.Blue, i * 50, j * 50, 50, 50);
            }
        }
    }
    private void Kubus_Click(object sender, EventArgs e)
    {
        MouseEventArgs me = (MouseEventArgs)e;
        hetspelzet(me.X - 50, me.Y - 200);
    }
    private void Kubus_MouseClick(object sender, MouseEventArgs e)
    {
        hetspelzet(e.X, e.Y);
    }
    private void UpdateStatusLabel()
    {
        aanzetLabel.Text = $"{(aanzet == 1 ? "Rood" : "Blauw")} is aan zet";
    }

    private void hetspelzet(int x, int y)
    {
        if (goeiezet(x, y))
        {
            int boardX = x / (gridsize / (grid10x10 ? 10 : (grid8x8 ? 8 : (grid6x6 ? 6 : 4))));
            int boardY = y / (gridsize / (grid10x10 ? 10 : (grid8x8 ? 8 : (grid6x6 ? 6 : 4))));

            zet(boardX, boardY);

            aanzet = (aanzet == 1) ? 2 : 1;

            UpdateStatusLabel();
            UpdatehoeveelLabels();

            var (gameOver, winner, scoroodifference) = IsGameOver();
            if (gameOver)
            {
                if (winner == "gelijkspel")
                {
                    MessageBox.Show("Het is gelijkspel");
                }
                else
                {
                    MessageBox.Show($"Gefeliciteerd {winner}! Je hebt gewonnen met {scoroodifference} stenen meer dan je tegenstander.");
                }

                InitializeGame();
            }
            kubus.Invalidate();
        }
    }
    private void UpdatehoeveelLabels()
    {
        roodLabel.Text = $"Er zijn nu {hoeveelStenen(1)} stenen van rood";
        blauwLabel.Text = $"Er zijn nu {hoeveelStenen(2)} stenen van blauw";
    }
    private int hoeveelStenen(int player)
    {
        if (board == null)
        {
            return 0;
        }
        int hoeveel = 0;
        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] == player)
                {
                    hoeveel++;
                }
            }
        }
        return hoeveel;
    }
    private bool goeiezet(int x, int y)
    {
        int boardX = x / (gridsize / (grid10x10 ? 10 : (grid8x8 ? 8 : (grid6x6 ? 6 : 4))));
        int boardY = y / (gridsize / (grid10x10 ? 10 : (grid8x8 ? 8 : (grid6x6 ? 6 : 4))));

        if (boardX >= 0 && boardX < board.GetLength(0) && boardY >= 0 && boardY < board.GetLength(1) && board[boardX, boardY] == 0)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int newX = boardX + i;
                    int newY = boardY + j;

                    if (newX >= 0 && newX < board.GetLength(0) && newY >= 0 && newY < board.GetLength(1))
                    {
                        if (board[newX, newY] == (aanzet == 1 ? 2 : 1))
                        {
                            while (true)
                            {
                                newX += i;
                                newY += j;

                                if (newX < 0 || newX >= board.GetLength(0) || newY < 0 || newY >= board.GetLength(1))
                                    break;

                                if (board[newX, newY] == 0)
                                    break;

                                if (board[newX, newY] == aanzet)
                                    return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }
    private void zet(int x, int y)
    {
        board[x, y] = aanzet;

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                int newX = x + i;
                int newY = y + j;

                if (newX >= 0 && newX < board.GetLength(0) && newY >= 0 && newY < board.GetLength(1))
                {
                    if (board[newX, newY] == (aanzet == 1 ? 2 : 1))
                    {
                        while (true)
                        {
                            newX += i;
                            newY += j;
                            if (newX < 0 || newX >= board.GetLength(0) || newY < 0 || newY >= board.GetLength(1))
                                break;

                            if (board[newX, newY] == 0)
                                break;

                            if (board[newX, newY] == aanzet)
                            {
                                int flipX = x + i;
                                int flipY = y + j;

                                while (flipX != newX || flipY != newY)
                                {
                                    board[flipX, flipY] = aanzet;
                                    flipX += i;
                                    flipY += j;
                                }
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
    private (bool, string, int) IsGameOver()
    {
        int hoeveelrood = hoeveelStenen(1);
        int hoeveelblauw = hoeveelStenen(2);

        if (hoeveelrood + hoeveelblauw == board.GetLength(0) * board.GetLength(1))
        {
            if (hoeveelrood > hoeveelblauw)
            {
                return (true, "rood", hoeveelrood - hoeveelblauw);
            }
            else if (hoeveelblauw > hoeveelrood)
            {
                return (true, "blauw", hoeveelblauw - hoeveelrood);
            }
            else
            {
                return (true, "gelijkspel", 0);
            }
        }
        bool geengoeiezetrood = true;
        bool geengoeiezetblauw = true;

        for (int i = 0; i < board.GetLength(0); i++)
        {
            for (int j = 0; j < board.GetLength(1); j++)
            {
                if (board[i, j] == 0 && goeiezet(i * 50, j * 50))
                {
                    if (aanzet == 1)
                    {
                        geengoeiezetrood = false;
                    }
                    else
                    {
                        geengoeiezetblauw = false;
                    }
                }
            }
        }
        if (geengoeiezetrood && geengoeiezetblauw)
        {
            if (hoeveelrood > hoeveelblauw)
            {
                return (true, "rood", hoeveelrood - hoeveelblauw);
            }
            else if (hoeveelblauw > hoeveelrood)
            {
                return (true, "blauw", hoeveelblauw - hoeveelrood);
            }
            else
            {
                return (true, "gelijkspel", 0);
            }
        }
        return (false, "", 0);
    }
    protected override void OnMouseClick(MouseEventArgs e)
    {
        base.OnMouseClick(e);
        hetspelzet(e.X - 50, e.Y - 200);
    }
    static void Main()
    {
        Application.Run(new ReversiGame());
    }
}