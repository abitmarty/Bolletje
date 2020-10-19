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
    private Button buttonSave;
    private Label labelPlayer2;
    private Label labelXTiles;
    private Label labelYTiles;
    private TextBox textBoxXTiles;
    private TextBox textBoxYTiles;
    private ComboBox comboBox1;
    private Label labelPlayer1;
    private ComboBox comboBox2;

    // Variables
    private SettingsInitials currentSettings;

    public Settingsform(SettingsInitials currentSettings)
    {
        // Set at top to initialise components
        this.InitializeComponent();
        this.BackColor = Color.FromArgb(251, 239, 217);
        this.Paint += drawingSettingsLine;
   

        Object[] allThings = new object[] {
                        "Balloon",
                        "Carrot",
                        "Clown",
                        "Dog",
                        "Fish",
                        "Frog",
                        "Man",
                        "Pizza",
                        "Robot",
                        "Rocket",
        };

        // Select player icons from dropdown
        this.comboBox1.Items.AddRange(allThings);
        this.comboBox2.Items.AddRange(allThings);

        // So the code can overwrite the currentsettings from the program.cs
        this.currentSettings = currentSettings;

        this.setTextboxes();

        this.buttonSave.Click += this.saveSettings;

        // Standard icon is Man and Fish
        this.comboBox1.Text = this.currentSettings.getP1Icon();
        this.comboBox2.Text = this.currentSettings.getP2Icon();
    }

    private void drawingSettingsLine(object obj, PaintEventArgs pea)
    {
        pea.Graphics.DrawLine(Pens.Black, 10, 120, 360, 120);
    }

    private void comboBox1GetString(object obj, EventArgs e)
    {

        // When selected in dropdown, change to chosen name
        // Set the chosen image as player name
        Object selectedItem = comboBox1.SelectedItem;
        string icon1String = selectedItem.ToString();
        this.currentSettings.setP1Icon(icon1String);
    }

    private void comboBox2GetString(object obj, EventArgs e)
    {
        // When selected in dropdown, change to chosen name
        // Set the chosen image as player name
        Object selectedItem = comboBox2.SelectedItem;
        string icon2String = selectedItem.ToString();
        this.currentSettings.setP2Icon(icon2String);
    }

    public void saveSettings(Object obj, EventArgs ea)
    {
        this.currentSettings.setP1Name(this.textBoxPlayer1.Text);
        this.currentSettings.setP2Name(this.textBoxPlayer2.Text);

        this.currentSettings.setTilesX(Convert.ToInt32(this.textBoxXTiles.Text) - 1);
        this.currentSettings.setTilesY(Convert.ToInt32(this.textBoxYTiles.Text) - 1);

        this.Close();
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
            this.labelPlayer2 = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.labelXTiles = new System.Windows.Forms.Label();
            this.labelYTiles = new System.Windows.Forms.Label();
            this.textBoxXTiles = new System.Windows.Forms.TextBox();
            this.textBoxYTiles = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.labelPlayer1 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // textBoxPlayer1
            // 
            this.textBoxPlayer1.Location = new System.Drawing.Point(11, 43);
            this.textBoxPlayer1.Name = "textBoxPlayer1";
            this.textBoxPlayer1.Size = new System.Drawing.Size(100, 22);
            this.textBoxPlayer1.TabIndex = 0;
            // 
            // textBoxPlayer2
            // 
            this.textBoxPlayer2.Location = new System.Drawing.Point(147, 43);
            this.textBoxPlayer2.Name = "textBoxPlayer2";
            this.textBoxPlayer2.Size = new System.Drawing.Size(100, 22);
            this.textBoxPlayer2.TabIndex = 1;
            // 
            // labelPlayer2
            // 
            this.labelPlayer2.AutoSize = true;
            this.labelPlayer2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayer2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(107)))), ((int)(((byte)(107)))));
            this.labelPlayer2.Location = new System.Drawing.Point(147, 15);
            this.labelPlayer2.Name = "labelPlayer2";
            this.labelPlayer2.Size = new System.Drawing.Size(69, 18);
            this.labelPlayer2.TabIndex = 3;
            this.labelPlayer2.Text = "Player 2";
            this.labelPlayer2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buttonSave
            // 
            this.buttonSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.buttonSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.buttonSave.FlatAppearance.BorderSize = 0;
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonSave.Location = new System.Drawing.Point(260, 152);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(100, 22);
            this.buttonSave.TabIndex = 4;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // labelXTiles
            // 
            this.labelXTiles.AutoSize = true;
            this.labelXTiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelXTiles.Location = new System.Drawing.Point(8, 133);
            this.labelXTiles.Name = "labelXTiles";
            this.labelXTiles.Size = new System.Drawing.Size(122, 18);
            this.labelXTiles.TabIndex = 8;
            this.labelXTiles.Text = "Horizontal tiles";
            // 
            // labelYTiles
            // 
            this.labelYTiles.AutoSize = true;
            this.labelYTiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelYTiles.Location = new System.Drawing.Point(144, 133);
            this.labelYTiles.Name = "labelYTiles";
            this.labelYTiles.Size = new System.Drawing.Size(100, 18);
            this.labelYTiles.TabIndex = 7;
            this.labelYTiles.Text = "Vertical tiles";
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
            this.textBoxYTiles.Location = new System.Drawing.Point(143, 153);
            this.textBoxYTiles.Name = "textBoxYTiles";
            this.textBoxYTiles.Size = new System.Drawing.Size(100, 22);
            this.textBoxYTiles.TabIndex = 5;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(11, 71);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(100, 24);
            this.comboBox1.TabIndex = 9;
            this.comboBox1.SelectionChangeCommitted += new System.EventHandler(this.comboBox1GetString);
            // 
            // labelPlayer1
            // 
            this.labelPlayer1.AutoSize = true;
            this.labelPlayer1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayer1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(200)))), ((int)(((byte)(252)))));
            this.labelPlayer1.Location = new System.Drawing.Point(11, 15);
            this.labelPlayer1.Name = "labelPlayer1";
            this.labelPlayer1.Size = new System.Drawing.Size(69, 18);
            this.labelPlayer1.TabIndex = 10;
            this.labelPlayer1.Text = "Player 1";
            this.labelPlayer1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(147, 71);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(100, 24);
            this.comboBox2.TabIndex = 11;
            this.comboBox2.SelectionChangeCommitted += new System.EventHandler(this.comboBox2GetString);
            // 
            // Settingsform
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(384, 193);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.labelPlayer1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.labelXTiles);
            this.Controls.Add(this.labelYTiles);
            this.Controls.Add(this.textBoxXTiles);
            this.Controls.Add(this.textBoxYTiles);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.labelPlayer2);
            this.Controls.Add(this.textBoxPlayer2);
            this.Controls.Add(this.textBoxPlayer1);
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "Settingsform";
            this.RightToLeftLayout = true;
            this.ShowIcon = false;
            this.Text = "Reversi Settings";
            this.Load += new System.EventHandler(this.Settingsform_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    private void Settingsform_Load(object sender, EventArgs e)
    {

    }
}