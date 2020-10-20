/*
 __  __  __  __
|  \/  ||  \/  | ©
| |\/| || |\/| | 
|_|  |_||_|  |_|

// Prakticum 2:     Reversi Game by MM
// Authors:         Martijn Totté and Maarten Gerritse
// Studentnumbers:  1235002 and 8845874
// C# Imperatief Programmeren

De standaard functionaliteiten als de huidige score, de status/beurtindicator, schaalbaarheid van het bord,
het in het midden plaatsen van de startstenen, beurt overslaan de knoppen voor een nieuw spel en de help knop met de mogelijke stappen staan allemaal in het programma.

Naast de vereiste functionaliteiten hebben wij aan ons Reversi programma het volgende toegevoegd:
+ Een settings knop waar de instellingen in een apart pop-up scherm zitten.
+ Spelers kunnen zelf een naam kiezen
+ Spelers kunnen zelf een emoji kiezen om mee te spelen uit een lijst van 10 opties.
+ Spelers kunnen zelf de grootte van de tiles bepalen
+ De invoer van de settings velden wordt gechecked. Op het moment dat een foutieve waarde wordt ingevoerd sluit het settings scherm niet na het klikken op de save knop. Bij juiste waardes sluit de settings wel na het klikken op de save knop.
+ Flat design in de knoppen, on hover en in het speelbord
+ Anti alliassing op alle getekende objecten
+ Het veld, scherm én de hokjes zijn schaalbaar op basis van de bij de settings ingevoerde informatie
+ Speciale speler indicatie rondom de emoji voor de speler die aan de beurt is
+ De laatste drie bewegingen staan onder aan het scherm
+ Zet u geluid aan :) (Victory royale geluids effect bij winst)
+ U vind het project op github https://github.com/abitmarty/Bolletje






@Maarten zie de commits https://github.com/abitmarty/Bolletje
[ ] Game mechanics finished (mistakes in allowed fields)
[ ] Fix tab index



Extra
[ ] Turn indicator -> pretty

*/

using BolletjeBolletje;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Media;
using BolletjeBolletje.Properties;
using System.Collections.Generic;

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

    private PictureBox pictureBox1;
    private PictureBox pictureBox2;

    // Variables
    private static SettingsInitials defaultSettings = new SettingsInitials("Player 1", "Player 2", 5, 5, "Man", "Fish", 1, 1);
    private SettingsInitials currentSettings = new SettingsInitials(defaultSettings.getP1Name(), defaultSettings.getP2Name(), defaultSettings.getTilesX(), defaultSettings.getTilesY(), defaultSettings.getP1Icon(), defaultSettings.getP2Icon(), defaultSettings.getTileSizeX(), defaultSettings.getTileSizeY());

    private int player1Score = 0;
    private int player2Score = 0;
    private Boolean helpOn = false;

    private Boolean player1CanPlay = true;
    private Boolean player2CanPlay = true;

    private Brush coolBlue = new SolidBrush(Color.FromArgb(107, 200, 252));
    private Brush coolRed = new SolidBrush(Color.FromArgb(252, 107, 107));

    private int rectangleX = 150;
    private int rectangleY = 50;
    private int[,] field;
    private int[,] validMoveField;
    private Graphics panelGraphics;
    private Boolean play1Turn = true;
    private Label label1;
    private Label label2;
    private Label label3;
    List<String[]> stepList = new List<String[]>();

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

        // Draw left stats (active player gets highlighted)
        pea.Graphics.FillRectangle(coolBlue, 23, 69, 44, 50);
        pea.Graphics.FillEllipse(coolBlue, 23, 95, 44, 44);
        if (this.play1Turn)
        {
            pea.Graphics.FillEllipse(coolBlue, 16, 40, 56, 56);
        }
        pea.Graphics.FillEllipse(Brushes.White, 18, 42, 52, 52);
        pea.Graphics.FillEllipse(coolBlue, 22, 46, 44, 44);

        // Draw right stats (active player gets highlighted)
        pea.Graphics.FillRectangle(coolRed, 84, 69, 44, 50);
        pea.Graphics.FillEllipse(coolRed, 84, 95, 44, 44);
        if (!this.play1Turn)
        {
            pea.Graphics.FillEllipse(coolRed, 77, 40, 56, 56);
        }
        pea.Graphics.FillEllipse(Brushes.White, 79, 42, 52, 52);
        pea.Graphics.FillEllipse(coolRed, 83, 46, 44, 44);

        // Draw scorebar
        double totalScore = this.player1Score + this.player2Score;
        double factor = this.panelGame.Height / totalScore;
        double doubleLengthP1 = factor * this.player1Score;
        double doubleLengthP2 = factor * this.player2Score;

        int lengthP1 = (int)(Math.Ceiling(doubleLengthP1));
        int lengthP2 = (int)(Math.Floor(doubleLengthP2));

        pea.Graphics.FillRectangle(coolRed, this.rectangleX + this.panelGame.Width + 20, this.rectangleY + lengthP1, 5, lengthP2);
        pea.Graphics.FillRectangle(coolBlue, this.rectangleX + this.panelGame.Width + 20, this.rectangleY, 5, lengthP1);

        this.label1.Location = new Point(150, this.panelGame.Height + 55);
        this.label2.Location = new Point(240, this.panelGame.Height + 55);
        this.label3.Location = new Point(350, this.panelGame.Height + 55);
    }

    public void setLastMoves()
    {
        try
        {
            string[] madeStepArr = this.stepList[this.stepList.Count - 1];
            this.label1.Text = "Last move: " + madeStepArr[1];
            if (madeStepArr[0] == "False")
            {
                this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(200)))), ((int)(((byte)(252)))));
            }
            else
            {
                this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(107)))), ((int)(((byte)(107)))));
            }
        }
        catch (Exception e) { }

        try
        {
            string[] madeStepArr = this.stepList[this.stepList.Count - 2];
            this.label2.Text = "Previous move: " + madeStepArr[1];
            if (madeStepArr[0] == "False")
            {
                this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(200)))), ((int)(((byte)(252)))));
            }
            else
            {
                this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(107)))), ((int)(((byte)(107)))));
            }
        }
        catch (Exception e) { }

        try
        {
            string[] madeStepArr = this.stepList[this.stepList.Count - 3];
            this.label3.Text = "Former move: " + madeStepArr[1];
            if (madeStepArr[0] == "False")
            {
                this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(200)))), ((int)(((byte)(252)))));
            }
            else
            {
                this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(107)))), ((int)(((byte)(107)))));
            }
        }
        catch (Exception e) { }

/*        foreach (String[] step in this.stepList)
        {
            Console.WriteLine(step[0] + " made " + step[1] );
        }*/
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
                }
                if (this.field[x, y] == 2)
                {
                    this.player2Score++;
                }
            }
        }

        this.labelP1Points.Text = this.player1Score.ToString();
        this.labelP2Points.Text = this.player2Score.ToString();
    }


    private void newGame(Object obj, EventArgs ea)
    {
        this.play1Turn = true;
        this.startAMatch();
    }

    private void startAMatch()
    {
        // Set labels to default
        this.label1.Text = "Last move: ";
        this.label2.Text = "Previous move: ";
        this.label3.Text = "Former move: ";
        this.label1.ForeColor = System.Drawing.Color.Black;
        this.label2.ForeColor = System.Drawing.Color.Black;
        this.label3.ForeColor = System.Drawing.Color.Black;

        // Clear list 
        this.stepList.Clear();

        // Build panel with graphics
        this.buildPanel();

        // Draw smooth so we'll add anti aliassing
        this.panelGraphics = panelGame.CreateGraphics();
        this.panelGraphics.SmoothingMode = SmoothingMode.AntiAlias;

        // Set variables
        this.setPlayerIcons();
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

    private void setPlayerIcons()
    {
        /*this.labelP1Name.Text = this.currentSettings.getP1Name();
        this.labelP2Name.Text = this.currentSettings.getP2Name();*/

        this.labelP1Name.Text = this.currentSettings.getP1Name();
        this.labelP2Name.Text = this.currentSettings.getP2Name();


        // Load corresponding image

        string player1Icon = this.currentSettings.getP1Icon();
        string player2Icon = this.currentSettings.getP2Icon();

        object O = Resources.ResourceManager.GetObject(player1Icon);
        this.pictureBox1.Image = (Image)O;

        object B = Resources.ResourceManager.GetObject(player2Icon);
        this.pictureBox2.Image = (Image)B;


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
   
        int rectangleWidth = this.currentSettings.getTileSizeX() * (xTiles + 1);
        int rectangleHeight = this.currentSettings.getTileSizeY() * (yTiles + 1);

        // Panel do + 1 for the last line of the grid
        this.panelGame.Location = new Point(this.rectangleX, this.rectangleY);
        this.panelGame.Size = new Size(rectangleWidth + 1, rectangleHeight + 1);
        this.panelGame.BackColor = Color.White;

        this.setScreenSize();
    }

    public void drawGrid()
    {
        // Vertical lines
        for (int x = 0; x <= (this.currentSettings.getTilesX() + 1) * this.currentSettings.getTileSizeX(); x++)
        {
            if (x % this.currentSettings.getTileSizeX() == 0)
            {
                this.panelGraphics.DrawLine(Pens.Black, x, 0, x, this.panelGame.Height);
            }
        }

        // Horizontal lines
        for (int y = 0; y <= (this.currentSettings.getTilesY() + 1) * this.currentSettings.getTileSizeY(); y++)
        {
            if (y % this.currentSettings.getTileSizeY() == 0)
            {
                this.panelGraphics.DrawLine(Pens.Black, 0, y, this.panelGame.Width + this.rectangleX, y);
            }
        }
    }

    // Panel event handeler
    private void createPanelGameField(object sender, PaintEventArgs e)
    {
        // First calculate the possible moves for the current player
        this.calculatePossibleMoves();

        // Updates player scores on click
        this.keepPlayerScore();

        // If there are no possible moves for the current player
        if (!this.possibleMovesAvailable())
        {
            // Give the opposition an extra turn
            // And set the ability to move for the current player to false
            if (this.play1Turn)
            {
                this.player1CanPlay = false;
            } else
            {
                this.player2CanPlay = false;
            }
            this.play1Turn = !this.play1Turn;

            // In case either player 1 or player 2 can move invalidate the field
            // This is a recursing function. Since createPanelGameField is the paint method called on invalidate
            if ((!this.player2CanPlay && this.player1CanPlay)|| (!this.player1CanPlay && this.player2CanPlay))
            {
                this.panelGame.Invalidate();
            }
        }
        else
        {
            this.player1CanPlay = true;
            this.player1CanPlay = true;
        }

        // All draw methods that are performed on the label
        this.drawGrid();
        this.drawField();
        this.setTurnName();
        this.Invalidate();

        // Check if anyone won
        if (!this.player2CanPlay && !this.player1CanPlay)
        {
            this.gameOver();
        }
    }

    public void gameOver()
    {
        if (this.player1Score > this.player2Score)
        {
            this.labelTurn.Text = this.currentSettings.getP1Name() + " won the game!";
        }
        else if (this.player1Score < this.player2Score)
        {
            this.labelTurn.Text = this.currentSettings.getP2Name() + " won the game!";
        }
        else
        {
            this.labelTurn.Text = "Unfortunately a tie has occurred";
        }

        try
        {
            SoundPlayer victoryLap = new SoundPlayer(BolletjeBolletje.Properties.Resources.fortnite);
            victoryLap.Play();
        }
        catch (Exception ex)
        {
            MessageBox.Show("No workey");
        }
    }

    public bool possibleMovesAvailable()
    {
        for (int y = 0; y < this.currentSettings.getTilesY() + 1; y++)
        {
            for (int x = 0; x < this.currentSettings.getTilesX() + 1; x++)
            {
                if (this.field[x, y] == 3)
                {
                    return true;
                }
            }
        }
        return false;
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

        // Only allow move if value is empty and eligible for a move (3)
        if (field[x / this.currentSettings.getTileSizeX(), y / this.currentSettings.getTileSizeY()] == 3)
        {
            if (play1Turn)
            {
                field[x / this.currentSettings.getTileSizeX(), y / this.currentSettings.getTileSizeY()] = 1;
                this.setOverTakenDisk(x, y);
                this.play1Turn = false;
            }
            else
            {
                field[x / this.currentSettings.getTileSizeX(), y / this.currentSettings.getTileSizeY()] = 2;
                this.setOverTakenDisk(x, y);
                this.play1Turn = true;
            }

            // Add step to step list
            String[] moveBy = { this.play1Turn.ToString(), (((x / this.currentSettings.getTileSizeX()) + 1) + ", " + ((y / this.currentSettings.getTileSizeY()) + 1)) };
            this.stepList.Add(moveBy);
        }
        // Set last moves
        this.setLastMoves();

        // Draw field and invalidate panel only
        this.panelGame.Invalidate();
        this.Invalidate();

        // Give each player the possibility to turn help off. (Uncomment the next line to enable)
        // this.helpOn = false;
    }

    public void setOverTakenDisk(int x, int y)
    {
        // Setting pixels to the corresponding array position
        x = x / this.currentSettings.getTileSizeX();
        y = y / this.currentSettings.getTileSizeY();

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
        //Console.WriteLine("New field");
        //Console.WriteLine("=========");
        for (int y = 0; y < this.currentSettings.getTilesY() + 1; y++)
        {
            //String temp = "";
            for (int x = 0; x < this.currentSettings.getTilesX() + 1; x++)
            {
                //temp += this.field[x, y];
                if (this.field[x, y] == 1)
                {
                    this.drawDisk(this.coolBlue, x, y);
                }
                if (this.field[x, y] == 2)
                {
                    this.drawDisk(this.coolRed, x, y);
                }
                if (this.field[x, y] == 3 && this.helpOn)
                {
                    this.drawDisk(Brushes.Gray, x, y);
                }
            }
            //Console.WriteLine(temp + "\n");
        }
    }

    public void drawDisk(Brush currentBrush, int x, int y)
    {
        int offsetX = this.currentSettings.getTileSizeX() / 10;
        int offsetY = this.currentSettings.getTileSizeY() / 10;

        int tempX = x * this.currentSettings.getTileSizeX() + offsetX;
        int tempY = y * this.currentSettings.getTileSizeY() + offsetY;

        // This way no matter how big the field or tile gets
        // The disk will stay centered and fill the tile
        this.panelGraphics.FillEllipse(currentBrush, tempX, tempY, (this.currentSettings.getTileSizeX() - 1) - (2 * offsetX), (this.currentSettings.getTileSizeY() - 1) - (2 * offsetY));
    }

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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonNewGame
            // 
            this.buttonNewGame.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonNewGame.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.buttonNewGame.FlatAppearance.BorderSize = 0;
            this.buttonNewGame.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNewGame.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonNewGame.Location = new System.Drawing.Point(0, 227);
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
            this.buttonHelp.Location = new System.Drawing.Point(0, 272);
            this.buttonHelp.Name = "buttonHelp";
            this.buttonHelp.Size = new System.Drawing.Size(150, 40);
            this.buttonHelp.TabIndex = 1;
            this.buttonHelp.Text = "Help";
            this.buttonHelp.UseVisualStyleBackColor = true;
            // 
            // labelTurn
            // 
            this.labelTurn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTurn.Location = new System.Drawing.Point(150, 12);
            this.labelTurn.Margin = new System.Windows.Forms.Padding(0);
            this.labelTurn.Name = "labelTurn";
            this.labelTurn.Size = new System.Drawing.Size(200, 25);
            this.labelTurn.TabIndex = 2;
            this.labelTurn.Text = "Turn";
            this.labelTurn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelP1Name
            // 
            this.labelP1Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelP1Name.Location = new System.Drawing.Point(23, 142);
            this.labelP1Name.Name = "labelP1Name";
            this.labelP1Name.Size = new System.Drawing.Size(45, 25);
            this.labelP1Name.TabIndex = 3;
            this.labelP1Name.Text = "P1 name";
            this.labelP1Name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelP1Points
            // 
            this.labelP1Points.BackColor = System.Drawing.Color.Transparent;
            this.labelP1Points.Cursor = System.Windows.Forms.Cursors.Default;
            this.labelP1Points.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelP1Points.Location = new System.Drawing.Point(23, 94);
            this.labelP1Points.Name = "labelP1Points";
            this.labelP1Points.Size = new System.Drawing.Size(45, 45);
            this.labelP1Points.TabIndex = 4;
            this.labelP1Points.Text = "P1 points";
            this.labelP1Points.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelP2Points
            // 
            this.labelP2Points.BackColor = System.Drawing.Color.Transparent;
            this.labelP2Points.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelP2Points.Location = new System.Drawing.Point(84, 94);
            this.labelP2Points.Name = "labelP2Points";
            this.labelP2Points.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.labelP2Points.Size = new System.Drawing.Size(45, 45);
            this.labelP2Points.TabIndex = 6;
            this.labelP2Points.Text = "P2 points";
            this.labelP2Points.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelP2Name
            // 
            this.labelP2Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            this.labelP2Name.Location = new System.Drawing.Point(83, 142);
            this.labelP2Name.Name = "labelP2Name";
            this.labelP2Name.Size = new System.Drawing.Size(45, 25);
            this.labelP2Name.TabIndex = 5;
            this.labelP2Name.Text = "P2 name";
            this.labelP2Name.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonSettings
            // 
            this.buttonSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonSettings.FlatAppearance.BorderSize = 0;
            this.buttonSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSettings.Location = new System.Drawing.Point(0, 317);
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
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(200)))), ((int)(((byte)(252)))));
            this.pictureBox1.Location = new System.Drawing.Point(33, 57);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(22, 22);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(107)))), ((int)(((byte)(107)))));
            this.pictureBox2.Location = new System.Drawing.Point(95, 57);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(22, 22);
            this.pictureBox2.TabIndex = 10;
            this.pictureBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(237, 342);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(289, 342);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(340, 342);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "label3";
            // 
            // ReversiForm
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(490, 367);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox2);
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
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(506, 406);
            this.Name = "ReversiForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Reversi";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

}