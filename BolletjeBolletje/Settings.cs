using BolletjeBolletje;
using System;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

class Settingsform : Form
{
    // Visuals
    private TextBox textBoxPlayer1;
    private TextBox textBoxPlayer2;
    private Label labelPlayer1;
    private Button buttonSave;
    private Label labelPlayer2;
    private Label labelXTiles;
    private Label labelYTiles;
    private TextBox textBoxXTiles;
    private TextBox textBoxYTiles;
    private ComboBox comboBox1;
    private Label label1;

    // Variables
    private SettingsInitials currentSettings;


    public Settingsform(SettingsInitials currentSettings)
    {
        // Set at top to initialise components
        this.InitializeComponent();

        // Select player icons from dropdown
        this.comboBox1.Items.AddRange(new object[] {
                        "Balloon",
                        "Carrot",
                        "Clown",
                        "Dog",
                        "Frog",
                        "Man",
                        "Pizza",
                        "Robot",
                        "Rocket"
        });

        // So the code can overwrite the currentsettings from the program.cs
        this.currentSettings = currentSettings;

        this.setTextboxes();

        this.buttonSave.Click += this.saveSettings;

        this.comboBox1.Text = "Man";


    }

    private void comboBox1GetString(object obj, EventArgs e)
    {

        // Standard icon is Man and Fish

        // When selected in dropdown, change to chosen name

        // Set the chosen image as player name

        Object selectedItem = comboBox1.SelectedItem;

        string icon1String = selectedItem.ToString();

        this.currentSettings.setP1Icon(icon1String);
        

    }

    public void saveSettings(Object obj, EventArgs ea)
    {
        this.currentSettings.setP1Name(this.textBoxPlayer1.Text);
        this.currentSettings.setP2Name(this.textBoxPlayer2.Text);

        this.currentSettings.setTilesX(Convert.ToInt32(this.textBoxXTiles.Text) - 1);
        this.currentSettings.setTilesY(Convert.ToInt32(this.textBoxYTiles.Text) - 1);
    }

    private void setTextboxes()
    {
        //TODO: If smaller than 3 give red error

        // Player names
        this.textBoxPlayer1.Text = this.currentSettings.getP1Name();
        this.textBoxPlayer2.Text = this.currentSettings.getP2Name();

        // X and Y tiles
        // Do plus one to be user friendly
        this.textBoxXTiles.Text = (this.currentSettings.getTilesX() + 1).ToString();
        this.textBoxYTiles.Text = (this.currentSettings.getTilesY() + 1).ToString();
    }

    public void openSettings(object obj, EventArgs pea)
    {
        ReversiForm screen = new ReversiForm();
        Application.Run(screen);
    }

    private void InitializeComponent()
    {
            this.textBoxPlayer1 = new System.Windows.Forms.TextBox();
            this.textBoxPlayer2 = new System.Windows.Forms.TextBox();
            this.labelPlayer1 = new System.Windows.Forms.Label();
            this.labelPlayer2 = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.labelXTiles = new System.Windows.Forms.Label();
            this.labelYTiles = new System.Windows.Forms.Label();
            this.textBoxXTiles = new System.Windows.Forms.TextBox();
            this.textBoxYTiles = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxPlayer1
            // 
            this.textBoxPlayer1.Location = new System.Drawing.Point(12, 38);
            this.textBoxPlayer1.Name = "textBoxPlayer1";
            this.textBoxPlayer1.Size = new System.Drawing.Size(100, 22);
            this.textBoxPlayer1.TabIndex = 0;
            // 
            // textBoxPlayer2
            // 
            this.textBoxPlayer2.Location = new System.Drawing.Point(147, 38);
            this.textBoxPlayer2.Name = "textBoxPlayer2";
            this.textBoxPlayer2.Size = new System.Drawing.Size(100, 22);
            this.textBoxPlayer2.TabIndex = 1;
            // 
            // labelPlayer1
            // 
            this.labelPlayer1.AutoSize = true;
            this.labelPlayer1.Location = new System.Drawing.Point(12, 22);
            this.labelPlayer1.Name = "labelPlayer1";
            this.labelPlayer1.Size = new System.Drawing.Size(99, 17);
            this.labelPlayer1.TabIndex = 2;
            this.labelPlayer1.Text = "Player 1 name";
            // 
            // labelPlayer2
            // 
            this.labelPlayer2.AutoSize = true;
            this.labelPlayer2.Location = new System.Drawing.Point(144, 22);
            this.labelPlayer2.Name = "labelPlayer2";
            this.labelPlayer2.Size = new System.Drawing.Size(99, 17);
            this.labelPlayer2.TabIndex = 3;
            this.labelPlayer2.Text = "Player 2 name";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(269, 164);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // labelXTiles
            // 
            this.labelXTiles.AutoSize = true;
            this.labelXTiles.Location = new System.Drawing.Point(8, 133);
            this.labelXTiles.Name = "labelXTiles";
            this.labelXTiles.Size = new System.Drawing.Size(46, 17);
            this.labelXTiles.TabIndex = 8;
            this.labelXTiles.Text = "X tiles";
            // 
            // labelYTiles
            // 
            this.labelYTiles.AutoSize = true;
            this.labelYTiles.Location = new System.Drawing.Point(144, 145);
            this.labelYTiles.Name = "labelYTiles";
            this.labelYTiles.Size = new System.Drawing.Size(46, 17);
            this.labelYTiles.TabIndex = 7;
            this.labelYTiles.Text = "Y tiles";
            // 
            // textBoxXTiles
            // 
            this.textBoxXTiles.Location = new System.Drawing.Point(11, 153);
            this.textBoxXTiles.Name = "textBoxXTiles";
            this.textBoxXTiles.Size = new System.Drawing.Size(100, 22);
            this.textBoxXTiles.TabIndex = 6;
            // 
            // textBoxYTiles
            // 
            this.textBoxYTiles.Location = new System.Drawing.Point(147, 165);
            this.textBoxYTiles.Name = "textBoxYTiles";
            this.textBoxYTiles.Size = new System.Drawing.Size(100, 22);
            this.textBoxYTiles.TabIndex = 5;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(11, 88);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(100, 24);
            this.comboBox1.TabIndex = 9;
            this.comboBox1.SelectionChangeCommitted += new System.EventHandler(this.comboBox1GetString);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 17);
            this.label1.TabIndex = 10;
            this.label1.Text = "Player 1";
            // 
            // Settingsform
            // 
            this.ClientSize = new System.Drawing.Size(384, 361);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.labelXTiles);
            this.Controls.Add(this.labelYTiles);
            this.Controls.Add(this.textBoxXTiles);
            this.Controls.Add(this.textBoxYTiles);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.labelPlayer2);
            this.Controls.Add(this.labelPlayer1);
            this.Controls.Add(this.textBoxPlayer2);
            this.Controls.Add(this.textBoxPlayer1);
            this.Name = "Settingsform";
            this.ResumeLayout(false);
            this.PerformLayout();

    }
}