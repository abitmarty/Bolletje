﻿/*
 __  __  __  __
|  \/  ||  \/  | ©
| |\/| || |\/| | 
|_|  |_||_|  |_|

*/

using BolletjeBolletje;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;

class ReversiForm : Form
{
    // Visuals
    private Button buttonHelp;
    private Label labelTurn;
    private Label labelP1Name;
    private Label labelP1Points;
    private Label labelP2Points;
    private Label labelP2Name;
    private Button buttonSettings;
    private Button buttonNewGame;
    private Panel panelGame;

    // Variables
    private static SettingsInitials defaultSettings = new SettingsInitials("Player 1", "Player 2", 5, 5);
    private SettingsInitials currentSettings = new SettingsInitials(defaultSettings.getP1Name(), defaultSettings.getP2Name(), defaultSettings.getTilesX(), defaultSettings.getTilesY());

    public int tileWidth = 51;
    public int tileHeight = 51;
    // public int diskWidth = 24;
    // public int diskHeight = diskWidth;
    Brush coolBlue = new SolidBrush(Color.FromArgb(107, 200, 252));
    Brush coolRed = new SolidBrush(Color.FromArgb(252,107, 107));

    private int rectangleX = 164;
    private int rectangleY = 60;
    private int[,] field;
    private int[,] validMoveField;
    private Graphics panelGraphics;
    private Boolean play1Turn = true;

    public ReversiForm()
    {
        // Set at top to initialise components
        this.InitializeComponent();
        this.buttonSettings.Click += this.openSettings;
        this.buttonNewGame.Click += this.newGame;

        this.BackColor = Color.FromArgb(251, 239, 217);

        this.startAMatch();
    }

    static void Main()
    {
        ReversiForm screen = new ReversiForm();
        Application.Run(screen);
    }

    private void newGame(Object obj, EventArgs ea)
    {
        this.play1Turn = true;
        this.startAMatch();
    }

    private void startAMatch()
    {
        // Build panel with graphics
        this.buildPanel();
        this.panelGraphics = panelGame.CreateGraphics();

        // Set variables
        this.setPlayerNames();
        this.createFieldArray();
        this.startEnvironment();

        // Field and form invalidation
        this.panelGame.Invalidate();
        this.Invalidate();
    }

    private void startEnvironment()
    {
        int startX = this.currentSettings.getTilesX() / 2;
        int startY = this.currentSettings.getTilesY() / 2;

        this.field[startX , startY ] = 1;
        this.field[startX , startY + 1] = 2;
        this.field[startX + 1 , startY] = 2;
        this.field[startX +1 , startY +1 ] = 1;
    }

    public void createFieldArray()
    {
        field = new int[this.currentSettings.getTilesX() + 1, this.currentSettings.getTilesY() + 1];
    }

    private void setPlayerNames()
    {
        this.labelP1Name.Text = this.currentSettings.getP1Name();
        this.labelP2Name.Text = this.currentSettings.getP2Name();
    }

    public void openSettings(object obj, EventArgs pea)
    {
        Settingsform screen = new Settingsform(currentSettings);
        screen.Show();
    }

    public void setScreenSize()
    {
        int width = this.panelGame.Width + 200;
        int height = this.panelGame.Height + 100;
        this.ClientSize = new System.Drawing.Size(width, height);
    }

    public void buildPanel()
    {
        int xTiles = this.currentSettings.getTilesX();
        int yTiles = this.currentSettings.getTilesY();
   
        int rectangleWidth = this.tileWidth * (xTiles + 1);
        int rectangleHeight = this.tileHeight * (yTiles + 1);

        // Panel do + 1 for the last line of the grid
        this.panelGame.Location = new Point(this.rectangleX, this.rectangleY);
        this.panelGame.Size = new Size(rectangleWidth + 1, rectangleHeight + 1);
        this.panelGame.BackColor = Color.White;

        this.setScreenSize();
    }

    public void drawGrid()
    {
        // Vertical lines
        for (int x = 0; x <= (this.currentSettings.getTilesX() + 1) * this.tileWidth; x++)
        {
            if (x % this.tileWidth == 0)
            {
                this.panelGraphics.DrawLine(Pens.Black, x, 0, x, this.panelGame.Height);
            }
        }

        // Horizontal lines
        for (int y = 0; y <= (this.currentSettings.getTilesY() + 1) * this.tileHeight; y++)
        {
            if (y % this.tileHeight == 0)
            {
                this.panelGraphics.DrawLine(Pens.Black, 0, y, this.panelGame.Width + this.rectangleX, y);
            }
        }
    }

    private void createPanelGameField(object sender, PaintEventArgs e)
    {
        // All draw methods that are performed on the label
        this.drawGrid();
        this.drawField();
        this.calculatePossibleMoves();
    }

    private void panelGame_MouseClick(object sender, MouseEventArgs e)
    {
        // Get mouse coords
        int x = e.X;
        int y = e.Y;

        // Only allow move if value is empty
        // TODO: If there are no possible moves. Next person is at turn
        if (field[x / 51, y / 51] == 3)
        {
            if (play1Turn)
            {
                field[x / 51, y / 51] = 1;
                this.play1Turn = false;
            }
            else
            {
                field[x / 51, y / 51] = 2;
                this.play1Turn = true;
            }
        }

        this.drawField();
        this.panelGame.Invalidate();
    }

    // In progress
    // TODO: Store possible move elipses in array
    public void drawPossibleMoves()
    {
        for (int y = 0; y < this.currentSettings.getTilesY() + 1; y++)
        {
            for (int x = 0; x < this.currentSettings.getTilesX() + 1; x++)
            {
                // If a move is possible its 1
                if (this.field[x, y] == 3)
                {
                    this.drawDisk(Brushes.Gray, x, y);
                }
            }
        }
    }

    private void calculatePossibleMoves()
    {
        // Removes all 3 values
        this.removePossibleMoves();

        // Loop through actual field
        for (int y = 0; y < this.currentSettings.getTilesY() + 1; y++)
        {
            for (int x = 0; x < this.currentSettings.getTilesX() + 1; x++)
            {
                // Check if field value has the value is empty
                if (this.field[x, y] == 0)   
                {
                    Boolean nw = this.isValidMove(x - 1, y + 1);
                    Boolean nn = this.isValidMove(x, y + 1);
                    Boolean ne = this.isValidMove(x + 1, y + 1);

                    Boolean ee = this.isValidMove(x + 1, y);
                    Boolean ww = this.isValidMove(x - 1, y);


                    Boolean sw = this.isValidMove(x - 1, y - 1);
                    Boolean ss = this.isValidMove(x, y - 1);
                    Boolean se = this.isValidMove(x + 1, y - 1);

                    if (nw || nn || ne || sw || ss || se || ee || ww) 
                    {
                        this.field[x, y] = 3;
                    }
                }
            }
        }
        this.drawPossibleMoves();
    }

    private void removePossibleMoves()
    {
        for (int y = 0; y < this.currentSettings.getTilesY() + 1; y++)
        {
            for (int x = 0; x < this.currentSettings.getTilesX() + 1; x++)
            {
                if (this.field[x, y] == 3)
                {
                    this.field[x, y] = 0;
                }
            }
        }
    }

    private Boolean isValidMove(int xCoord, int yCoord)
    {
        // Its alwyas player 1's turn. That makes player 2 is the opposition
        // Exept if its not
        int opposition = 2;
        int currentPlayer = 1;
        if (!this.play1Turn)
        {
            opposition = 1;
            currentPlayer = 2;
        }

        if (xCoord < 0 || xCoord > this.currentSettings.getTilesX())
        {
            return false;
        }
        if (yCoord < 0 || yCoord > this.currentSettings.getTilesY())
        {
            return false;
        }

        // Check if current value is the opps
        if (this.field[xCoord, yCoord] == opposition)
        {
            // Check if current player is in x row
            for (int x = 0; x < this.currentSettings.getTilesX() + 1; x++)
            {
                Console.WriteLine("Value x: " + x);
                Console.WriteLine("yCoord: " + yCoord);
                Console.WriteLine("Value array: " + this.field[x, yCoord]);

                if (this.field[x, yCoord] == currentPlayer)
                {
                    Console.WriteLine("Found");
                    Console.Write(this.field[x, yCoord]);
                    Console.WriteLine();

                    return true;
                }
            }
        }

        return false;
    }
    // In progress

    // TODO: Remove visualisers (console)
    public void drawField()
    {
        Console.WriteLine("New field");
        Console.WriteLine("=========");
        for (int y = 0; y < this.currentSettings.getTilesY() + 1; y++)
        {
            String temp = "";
            for (int x = 0; x < this.currentSettings.getTilesX() + 1; x++)
            {
                temp += this.field[x, y];
                if (this.field[x, y] == 1)
                {
                    this.drawDisk(this.coolBlue, x, y);
                }
                if (this.field[x, y] == 2)
                {
                    this.drawDisk(this.coolRed, x, y);
                }
            }
            Console.WriteLine(temp + "\n");
        }
    }

    public void drawDisk(Brush currentBrush, int x, int y)
    {
        int offsetX = 5;
        int offsetY = offsetX;

        int tempX = x * 51 + offsetX;
        int tempY = y * 51 + offsetY;
        this.panelGraphics.FillEllipse(currentBrush, tempX, tempY, 40, 40);
    }
    
    //int[][] playField = new int [tilesx];

    // Declaring a value to a position in the playField
    //playField [1][3] == 2

    // tilesx * tilesy == totalTiles Ex. x6 * x6 = 36 tiles
    // totalTiles == total number of tiles
    // TODO totalTiles aanmaken

    // New game start position

    // 1 turn

    private void InitializeComponent()
    {
            this.buttonNewGame = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.labelTurn = new System.Windows.Forms.Label();
            this.labelP1Name = new System.Windows.Forms.Label();
            this.labelP1Points = new System.Windows.Forms.Label();
            this.labelP2Points = new System.Windows.Forms.Label();
            this.labelP2Name = new System.Windows.Forms.Label();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.panelGame = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // buttonNewGame
            // 
            this.buttonNewGame.Location = new System.Drawing.Point(24, 22);
            this.buttonNewGame.Name = "buttonNewGame";
            this.buttonNewGame.Size = new System.Drawing.Size(75, 23);
            this.buttonNewGame.TabIndex = 0;
            this.buttonNewGame.Text = "New game";
            this.buttonNewGame.UseVisualStyleBackColor = true;
            // 
            // buttonHelp
            // 
            this.buttonHelp.Location = new System.Drawing.Point(118, 22);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(75, 23);
            this.buttonHelp.TabIndex = 1;
            this.buttonHelp.Text = "Help";
            this.buttonHelp.UseVisualStyleBackColor = true;
            // 
            // labelTurn
            // 
            this.labelTurn.AutoSize = true;
            this.labelTurn.Location = new System.Drawing.Point(21, 60);
            this.labelTurn.Name = "labelTurn";
            this.labelTurn.Size = new System.Drawing.Size(29, 13);
            this.labelTurn.TabIndex = 2;
            this.labelTurn.Text = "Turn";
            // 
            // labelP1Name
            // 
            this.labelP1Name.AutoSize = true;
            this.labelP1Name.Location = new System.Drawing.Point(21, 91);
            this.labelP1Name.Name = "labelP1Name";
            this.labelP1Name.Size = new System.Drawing.Size(49, 13);
            this.labelP1Name.TabIndex = 3;
            this.labelP1Name.Text = "P1 name";
            // 
            // labelP1Points
            // 
            this.labelP1Points.AutoSize = true;
            this.labelP1Points.Location = new System.Drawing.Point(21, 114);
            this.labelP1Points.Name = "labelP1Points";
            this.labelP1Points.Size = new System.Drawing.Size(51, 13);
            this.labelP1Points.TabIndex = 4;
            this.labelP1Points.Text = "P1 points";
            // 
            // labelP2Points
            // 
            this.labelP2Points.AutoSize = true;
            this.labelP2Points.Location = new System.Drawing.Point(94, 114);
            this.labelP2Points.Name = "labelP2Points";
            this.labelP2Points.Size = new System.Drawing.Size(49, 13);
            this.labelP2Points.TabIndex = 6;
            this.labelP2Points.Text = "P2 name";
            // 
            // labelP2Name
            // 
            this.labelP2Name.AutoSize = true;
            this.labelP2Name.Location = new System.Drawing.Point(94, 91);
            this.labelP2Name.Name = "labelP2Name";
            this.labelP2Name.Size = new System.Drawing.Size(51, 13);
            this.labelP2Name.TabIndex = 5;
            this.labelP2Name.Text = "P2 points";
            // 
            // buttonSettings
            // 
            this.buttonSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.buttonSettings.Location = new System.Drawing.Point(344, 22);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(28, 23);
            this.buttonSettings.TabIndex = 7;
            this.buttonSettings.Text = "⚙";
            this.buttonSettings.UseVisualStyleBackColor = true;
            // 
            // panelGame
            // 
            this.panelGame.Location = new System.Drawing.Point(237, 70);
            this.panelGame.Name = "panelGame";
            this.panelGame.Size = new System.Drawing.Size(200, 100);
            this.panelGame.TabIndex = 8;
            this.panelGame.Paint += new System.Windows.Forms.PaintEventHandler(this.createPanelGameField);
            this.panelGame.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelGame_MouseClick);
            // 
            // ReversiForm
            // 
            this.ClientSize = new System.Drawing.Size(501, 429);
            this.Controls.Add(this.panelGame);
            this.Controls.Add(this.buttonSettings);
            this.Controls.Add(this.labelP2Points);
            this.Controls.Add(this.labelP2Name);
            this.Controls.Add(this.labelP1Points);
            this.Controls.Add(this.labelP1Name);
            this.Controls.Add(this.labelTurn);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonNewGame);
            this.Name = "ReversiForm";
            this.ResumeLayout(false);
            this.PerformLayout();

    }

}