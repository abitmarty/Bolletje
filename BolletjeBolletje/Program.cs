/*
 __  __  __  __
|  \/  ||  \/  | ©
| |\/| || |\/| | 
|_|  |_||_|  |_|

[ ] Settings screen -> pretty
[ ] Winner indication
[ ] Turn indicator -> pretty
[ ] Scalable buttons fall of when 3×3
[ ] Score centre
[ ] Emoji for players
[ ] Gray dot => Colour of player who's turn it is

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
    private Label labelP2Name;

    private Label labelP1Points;
    private Label labelP2Points;

    private Button buttonSettings;
    private Button buttonNewGame;
    private Panel panelGame;

    // Variables
    private static SettingsInitials defaultSettings = new SettingsInitials("Player 1", "Player 2", 5, 5);
    private SettingsInitials currentSettings = new SettingsInitials(defaultSettings.getP1Name(), defaultSettings.getP2Name(), defaultSettings.getTilesX(), defaultSettings.getTilesY());

    public int tileWidth = 51;
    public int tileHeight = 51;

    public int player1Score = 0;
    public int player2Score = 0;
    public Boolean helpOn = false;

    // public int diskWidth = 24;
    // public int diskHeight = diskWidth;
    Brush coolBlue = new SolidBrush(Color.FromArgb(107, 200, 252));
    Brush coolRed = new SolidBrush(Color.FromArgb(252, 107, 107));

    private int rectangleX = 150;
    private int rectangleY = 50;
    private int[,] field;
    private int[,] validMoveField;
    private Graphics panelGraphics;
    private PictureBox pictureBox1;
    private Boolean play1Turn = true;

    public ReversiForm()
    {
        // Set at top to initialise components
        this.InitializeComponent();
        this.buttonSettings.Click += this.openSettings;
        this.buttonNewGame.Click += this.newGame;
        this.buttonHelp.Click += this.sethelp;
        this.Paint += this.setScoreboard;

        this.BackColor = Color.FromArgb(251, 239, 217);

        this.startAMatch();
    }

    static void Main()
    {
        ReversiForm screen = new ReversiForm();
        Application.Run(screen);
    }

    public void sethelp(object obj, EventArgs ea)
    {
        this.helpOn = !this.helpOn;
        this.panelGame.Invalidate();
    }

    public void setScoreboard(object obj, PaintEventArgs pea)
    {
        // Draw smooth so we'll add anti aliassing
        pea.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

        // Draw left stats
        pea.Graphics.FillRectangle(coolBlue, 23, 69, 44, 50);
        pea.Graphics.FillEllipse(Brushes.White, 18, 42, 52, 52);
        pea.Graphics.FillEllipse(coolBlue, 22, 46, 44, 44);
        pea.Graphics.FillEllipse(coolBlue, 22, 95, 44, 44);

        // Draw right stats
        pea.Graphics.FillRectangle(coolRed, 84, 69, 44, 50);
        pea.Graphics.FillEllipse(Brushes.White, 79, 42, 52, 52);
        pea.Graphics.FillEllipse(coolRed, 83, 46, 44, 44);
        pea.Graphics.FillEllipse(coolRed, 83, 95, 44, 44);

    }


    public void keepPlayerScore()
    {
        this.player1Score = 0;
        this.player2Score = 0;

        for (int y = 0; y < this.currentSettings.getTilesY() + 1; y++)
        {
            for (int x = 0; x < this.currentSettings.getTilesX() + 1; x++)
            {
                // Adding 1 to the score of the respective player for each captured field
                if (this.field[x, y] == 1)
                {
                    this.player1Score++;
                    this.labelP1Points.Text = this.player1Score.ToString();
                }
                if (this.field[x, y] == 2)
                {
                    this.player2Score++;
                    this.labelP2Points.Text = this.player2Score.ToString();
                }
            }
        }
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

        // Draw smooth so we'll add anti aliassing
        this.panelGraphics = panelGame.CreateGraphics();
        this.panelGraphics.SmoothingMode = SmoothingMode.AntiAlias;


        // Set variables
        this.setPlayerNames();
        this.createFieldArray();
        this.startEnvironment();
        this.keepPlayerScore();

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
        this.setTurnName();
    }

    private void setTurnName()
    {
        // Set player turn
        if (this.play1Turn)
        {
            this.labelTurn.Text = this.currentSettings.getP1Name() + "'s turn";
        }
        else
        {
            this.labelTurn.Text = this.currentSettings.getP2Name() + "'s turn";
        }
    }
    private void panelGame_MouseClick(object sender, MouseEventArgs e)
    {
        // Get mouse coords
        int x = e.X;
        int y = e.Y;

        // Only allow move if value is empty
        // TODO: If there are no possible moves. Next person is at turn
        if (field[x / this.tileWidth, y / this.tileHeight] == 3)
        {
            if (play1Turn)
            {
                field[x / this.tileWidth, y / this.tileHeight] = 1;
                this.setOverTakenDisk(x, y);
                this.play1Turn = false;
            }
            else
            {
                field[x / this.tileWidth, y / this.tileHeight] = 2;
                this.setOverTakenDisk(x, y);
                this.play1Turn = true;
            }
        }

        // Draw field and invalidate panel only
        this.drawField();
        this.panelGame.Invalidate();

        // Give each player the possibility to set it off
        // this.helpOn = false;

        // Updates player scores on click
        this.keepPlayerScore();
    }

    public void setOverTakenDisk(int x, int y)
    {
        // Setting pixels to the corresponding array position
        x = x / this.tileWidth;
        y = y / this.tileHeight;

        // Similar to calculating surrounding disks
        // But now we overtake the disks
        this.overtakeLine(x, -1, y, -1);
        this.overtakeLine(x, 0, y, -1);
        this.overtakeLine(x, 1, y, -1);

        this.overtakeLine(x, 1, y, 0);
        this.overtakeLine(x, -1, y, 0);

        this.overtakeLine(x, -1, y, 1);
        this.overtakeLine(x, 0, y, 1);
        this.overtakeLine(x, 1, y, 1);
    }

    public Boolean overtakeLine(int xCoord, int xMove, int yCoord, int yMove)
    {
        // Its always player 1's turn
        // Exept if its not
        int currentPlayer = 1;
        if (!this.play1Turn)
        {
            currentPlayer = 2;
        }

        // Go to each line to check if its the opps
        // If placed off the board x
        if ((xCoord + xMove < 0) || (xCoord + xMove > this.currentSettings.getTilesX()))
        {
            return false;
        }

        // If placed of the board y
        if ((yCoord + yMove < 0) || (yCoord + yMove > this.currentSettings.getTilesY()))
        {
            return false;
        }

        // Check if current value is empty
        if (this.field[xCoord + xMove, yCoord + yMove] == 0 || this.field[xCoord + xMove, yCoord + yMove] == 3)
        {
            return false;
        }

        if (this.field[xCoord + xMove, yCoord + yMove] == currentPlayer)
        {
            return true;
        }
        else
        {
            if (overtakeLine(xCoord + xMove, xMove, yCoord + yMove, yMove))
            {
                // Recursing the function till we find the valid move
                // Or return false if the function breaks
                this.field[xCoord + xMove, yCoord + yMove] = currentPlayer;
                return true;
            }
        }

        return false;
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
                if (this.field[x, y] == 3 && this.helpOn)
                {
                    this.drawDisk(Brushes.Gray, x, y);
                }
            }
        }
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
                if (this.field[x, y] == 0 || this.field[x, y] == 3)   
                {
                    // All values around empty value in field
                    Boolean nw = this.isValidMove(x, - 1, y, -1);
                    Boolean nn = this.isValidMove(x, 0, y, -1);
                    Boolean ne = this.isValidMove(x, 1, y, -1);

                    Boolean ee = this.isValidMove(x, 1, y, 0);
                    Boolean ww = this.isValidMove(x, - 1, y, 0);

                    Boolean sw = this.isValidMove(x, - 1, y, 1);
                    Boolean ss = this.isValidMove(x, 0, y, 1);
                    Boolean se = this.isValidMove(x, 1, y, 1);

                    // If any of the values is true
                    if (nw || nn || ne || sw || ss || se || ee || ww) 
                    {
                        this.field[x, y] = 3;
                    }
                }
            }
        }

        this.drawPossibleMoves();
        //TODO: Als geen possible moves dan skip beurt :) #anglicismen
    }

    private Boolean isValidMove(int xCoord, int xMove, int yCoord, int yMove)
    {
        // Its always player 1's turn. That makes player 2 is the opposition
        // Exept if its not
        int opposition = 2;
        int currentPlayer = 1;
        if (!this.play1Turn)
        {
            opposition = 1;
            currentPlayer = 2;
        }

        // If placed off the board x
        if ((xCoord + xMove < 0) || (xCoord + xMove > this.currentSettings.getTilesX()))
        {
            return false;
        }

        // If placed of the board y
        if ((yCoord + yMove < 0) || (yCoord + yMove > this.currentSettings.getTilesY()))
        {
            return false;
        }

        // Check if current value is the opps
        if (this.field[xCoord + xMove, yCoord + yMove] != opposition)
        {
            return false;
        }
         
        //TODO: test following code
        // If two moves is out of field x
        if ((xCoord + xMove + xMove < 0) || (xCoord + xMove + xMove > this.currentSettings.getTilesX()))
        {
            return false;
        }

        // If two moves is out of field y
        if ((yCoord + yMove + yMove < 0) || (yCoord + yMove + yMove > this.currentSettings.getTilesY()))
        {
            return false;
        }

        return checkLine(currentPlayer, xCoord, xMove, yCoord, yMove);
    }

    public Boolean checkLine(int currentPlayer, int xCoord, int xMove, int yCoord, int yMove)
    {
        // Check if current player is on x or y depending on the situation
        if(this.field[xCoord, yCoord] == currentPlayer)
        {
            return true;
        }

        // If placed off the board x
        if ((xCoord + xMove < 0) || (xCoord + xMove > this.currentSettings.getTilesX()))
        {
            return false;
        }

        // If placed of the board y
        if ((yCoord + yMove < 0) || (yCoord + yMove > this.currentSettings.getTilesY()))
        {
            return false;
        }

        // Keep recursing function until either the player is found. Or we moved of th
        return checkLine(currentPlayer, xCoord + xMove, xMove, yCoord + yMove, yMove);
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

        int tempX = x * this.tileWidth + offsetX;
        int tempY = y * this.tileHeight + offsetY;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReversiForm));
            this.buttonNewGame = new System.Windows.Forms.Button();
            this.buttonHelp = new System.Windows.Forms.Button();
            this.labelTurn = new System.Windows.Forms.Label();
            this.labelP1Name = new System.Windows.Forms.Label();
            this.labelP1Points = new System.Windows.Forms.Label();
            this.labelP2Points = new System.Windows.Forms.Label();
            this.labelP2Name = new System.Windows.Forms.Label();
            this.buttonSettings = new System.Windows.Forms.Button();
            this.panelGame = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonNewGame
            // 
            this.buttonNewGame.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonNewGame.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonNewGame.FlatAppearance.BorderSize = 0;
            this.buttonNewGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNewGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.buttonNewGame.Location = new System.Drawing.Point(0, 220);
            this.buttonNewGame.Name = "buttonNewGame";
            this.buttonNewGame.Size = new System.Drawing.Size(150, 40);
            this.buttonNewGame.TabIndex = 0;
            this.buttonNewGame.Text = "New Game";
            this.buttonNewGame.UseVisualStyleBackColor = false;
            // 
            // buttonHelp
            // 
            this.buttonHelp.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonHelp.FlatAppearance.BorderSize = 0;
            this.buttonHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHelp.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonHelp.Location = new System.Drawing.Point(0, 265);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(150, 40);
            this.buttonHelp.TabIndex = 1;
            this.buttonHelp.Text = "Help";
            this.buttonHelp.UseVisualStyleBackColor = true;
            // 
            // labelTurn
            // 
            this.labelTurn.AutoSize = true;
            this.labelTurn.Location = new System.Drawing.Point(32, 153);
            this.labelTurn.Name = "labelTurn";
            this.labelTurn.Size = new System.Drawing.Size(29, 13);
            this.labelTurn.TabIndex = 2;
            this.labelTurn.Text = "Turn";
            // 
            // labelP1Name
            // 
            this.labelP1Name.AutoSize = true;
            this.labelP1Name.Location = new System.Drawing.Point(22, 20);
            this.labelP1Name.Name = "labelP1Name";
            this.labelP1Name.Size = new System.Drawing.Size(49, 13);
            this.labelP1Name.TabIndex = 3;
            this.labelP1Name.Text = "P1 name";
            // 
            // labelP1Points
            // 
            this.labelP1Points.AutoSize = true;
            this.labelP1Points.BackColor = System.Drawing.Color.Transparent;
            this.labelP1Points.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelP1Points.Location = new System.Drawing.Point(35, 102);
            this.labelP1Points.Name = "labelP1Points";
            this.labelP1Points.Size = new System.Drawing.Size(96, 24);
            this.labelP1Points.TabIndex = 4;
            this.labelP1Points.Text = "P1 points";
            this.labelP1Points.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelP2Points
            // 
            this.labelP2Points.AutoSize = true;
            this.labelP2Points.BackColor = System.Drawing.Color.Transparent;
            this.labelP2Points.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelP2Points.Location = new System.Drawing.Point(98, 102);
            this.labelP2Points.Name = "labelP2Points";
            this.labelP2Points.Size = new System.Drawing.Size(96, 24);
            this.labelP2Points.TabIndex = 6;
            this.labelP2Points.Text = "P2 points";
            this.labelP2Points.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelP2Name
            // 
            this.labelP2Name.AutoSize = true;
            this.labelP2Name.Location = new System.Drawing.Point(125, 20);
            this.labelP2Name.Name = "labelP2Name";
            this.labelP2Name.Size = new System.Drawing.Size(49, 13);
            this.labelP2Name.TabIndex = 5;
            this.labelP2Name.Text = "P2 name";
            // 
            // buttonSettings
            // 
            this.buttonSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonSettings.FlatAppearance.BorderSize = 0;
            this.buttonSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSettings.Location = new System.Drawing.Point(0, 310);
            this.buttonSettings.Name = "buttonSettings";
            this.buttonSettings.Size = new System.Drawing.Size(150, 40);
            this.buttonSettings.TabIndex = 7;
            this.buttonSettings.Text = "Settings";
            this.buttonSettings.UseVisualStyleBackColor = true;
            // 
            // panelGame
            // 
            this.panelGame.Location = new System.Drawing.Point(237, 54);
            this.panelGame.Name = "panelGame";
            this.panelGame.Size = new System.Drawing.Size(200, 100);
            this.panelGame.TabIndex = 8;
            this.panelGame.Paint += new System.Windows.Forms.PaintEventHandler(this.createPanelGameField);
            this.panelGame.MouseClick += new System.Windows.Forms.MouseEventHandler(this.panelGame_MouseClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 373);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(121, 106);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // ReversiForm
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(501, 429);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panelGame);
            this.Controls.Add(this.buttonSettings);
            this.Controls.Add(this.labelP2Points);
            this.Controls.Add(this.labelP2Name);
            this.Controls.Add(this.labelP1Points);
            this.Controls.Add(this.labelP1Name);
            this.Controls.Add(this.labelTurn);
            this.Controls.Add(this.buttonHelp);
            this.Controls.Add(this.buttonNewGame);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "ReversiForm";
            this.ShowIcon = false;
            this.Text = "Reversi";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

}