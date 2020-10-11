/*
 __  __  __  __
|  \/  ||  \/  | ©
| |\/| || |\/| | 
|_|  |_||_|  |_|

*/
// Try

using BolletjeBolletje;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

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

    // Variables
    private static SettingsInitials defaultSettings = new SettingsInitials("Player 1", "Player 2", 5, 5);
    private SettingsInitials currentSettings = new SettingsInitials(defaultSettings.getP1Name(), defaultSettings.getP2Name(), defaultSettings.getTilesX(), defaultSettings.getTilesY());

    public int tileWidth = 51;
    public int tileHeight = 51;
    public int diskWidth = 24;
    public int diskHeight = 24;

    private int rectangleWidth;
    private int rectangleHeight;
    private int rectangleX = 164;
    private int rectangleY = 60;

    public ReversiForm()
    {
        // Set at top to initialise components
        this.InitializeComponent();
        this.buttonSettings.Click += this.openSettings;
        this.buttonNewGame.Click += this.newGame;
        this.setPlayerNames();
        this.Paint += this.buildPanel;
    }

    static void Main()
    {
        ReversiForm screen = new ReversiForm();
        Application.Run(screen);
    }

    private void newGame(Object obj, EventArgs ea)
    {
        this.setPlayerNames();
        this.Invalidate();
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
        int width = this.rectangleWidth + 200;
        int height = this.rectangleHeight + 100;
        this.ClientSize = new System.Drawing.Size(width, height);

    }

    public void buildPanel(Object obj, PaintEventArgs pea)
    {
        int xTiles = this.currentSettings.getTilesX();
        int yTiles = this.currentSettings.getTilesY();
   
       
        rectangleWidth = this.tileWidth * (xTiles + 1) + 1;
        rectangleHeight = this.tileHeight * (yTiles + 1) + 1;

        pea.Graphics.FillRectangle(Brushes.White, this.rectangleX, this.rectangleY, rectangleWidth, rectangleHeight);

        this.setScreenSize();
        this.drawGrid(obj, pea);
    }

    public void drawGrid(Object obj, PaintEventArgs pea)
    {
        int defaultX = this.rectangleX;
        int defaultY = this.rectangleY;

        Console.WriteLine(defaultX);
        Console.WriteLine(defaultY);

        // Vertical lines
        pea.Graphics.DrawLine(Pens.Black, defaultX-10, defaultY, defaultX, this.rectangleHeight);
        pea.Graphics.DrawLine(Pens.Black, 0, 0, 0, 150);

        // Horizontal lines



        for (int x = 0; x < this.currentSettings.getTilesX(); x++)
        {



            if (x % 51 == 0)
            {
                pea.Graphics.DrawLine(Pens.Black, 0, 0, 0, 0);

            }




        }


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
            this.labelTurn.Size = new System.Drawing.Size(38, 17);
            this.labelTurn.TabIndex = 2;
            this.labelTurn.Text = "Turn";
            // 
            // labelP1Name
            // 
            this.labelP1Name.AutoSize = true;
            this.labelP1Name.Location = new System.Drawing.Point(21, 91);
            this.labelP1Name.Name = "labelP1Name";
            this.labelP1Name.Size = new System.Drawing.Size(64, 17);
            this.labelP1Name.TabIndex = 3;
            this.labelP1Name.Text = "P1 name";
            // 
            // labelP1Points
            // 
            this.labelP1Points.AutoSize = true;
            this.labelP1Points.Location = new System.Drawing.Point(21, 114);
            this.labelP1Points.Name = "labelP1Points";
            this.labelP1Points.Size = new System.Drawing.Size(67, 17);
            this.labelP1Points.TabIndex = 4;
            this.labelP1Points.Text = "P1 points";
            // 
            // labelP2Points
            // 
            this.labelP2Points.AutoSize = true;
            this.labelP2Points.Location = new System.Drawing.Point(94, 114);
            this.labelP2Points.Name = "labelP2Points";
            this.labelP2Points.Size = new System.Drawing.Size(64, 17);
            this.labelP2Points.TabIndex = 6;
            this.labelP2Points.Text = "P2 name";
            // 
            // labelP2Name
            // 
            this.labelP2Name.AutoSize = true;
            this.labelP2Name.Location = new System.Drawing.Point(94, 91);
            this.labelP2Name.Name = "labelP2Name";
            this.labelP2Name.Size = new System.Drawing.Size(67, 17);
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
            // ReversiForm
            // 
            this.ClientSize = new System.Drawing.Size(501, 429);
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