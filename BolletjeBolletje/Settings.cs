﻿using BolletjeBolletje;
using System;
using System.Drawing;
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
    private Label labelXTileWidth;
    private Label labelYTileWidth;
    private TextBox textBoxTileWidth;
    private TextBox textBoxTileHeight;

    // Colors
    private Color colorNormal = default(Color);
    private Color colorError = Color.FromArgb(245, 74, 74);

    // Variables
    private SettingsInitials currentSettings;

    public Settingsform(SettingsInitials currentSettings)
    {
        // Set at top to initialise components
        this.InitializeComponent();
        this.BackColor = Color.FromArgb(251, 239, 217);
        this.Paint += drawingSettingsLine;
   
        // Create icon name array for comboboxes
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

        // Set combobox values to allThings icon array
        this.comboBox1.Items.AddRange(allThings);
        this.comboBox2.Items.AddRange(allThings);

        // So the code can overwrite the currentsettings from the program.cs
        this.currentSettings = currentSettings;

        // Set al textboxes default values
        this.setTextboxes();

        // Click event on save (eventhandler). So the screen closes
        this.buttonSave.Click += this.saveSettings;
    }

    private void drawingSettingsLine(object obj, PaintEventArgs pea)
    {
        pea.Graphics.DrawLine(Pens.Black, 10, 120, 245, 120);
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
        // If there is an error the screen doesnt close on save
        Boolean noErrors = true;

        // Set all values in textboxes
        // And backcolors to default
        // In case of exceptoin set background to error color
        // Minimum comes from the minimum values in settings. There will be no red screen shown if the value is to low
        try
        {
            this.currentSettings.setP1Name(this.textBoxPlayer1.Text);
            this.textBoxPlayer1.BackColor = this.colorNormal;
        } catch (Exception e)
        {
            this.textBoxPlayer1.BackColor = this.colorError;
            noErrors = false;
        }

        try
        {
            this.currentSettings.setP2Name(this.textBoxPlayer2.Text);
            this.textBoxPlayer2.BackColor = this.colorNormal;
        }
        catch (Exception e)
        {
            this.textBoxPlayer2.BackColor = this.colorError;
            noErrors = false;
        }

        try
        {
            this.currentSettings.setTilesX(Convert.ToInt32(this.textBoxXTiles.Text) - 1);
            this.textBoxXTiles.BackColor = this.colorNormal;
        }
        catch (Exception e)
        {
            this.textBoxXTiles.BackColor = this.colorError;
            noErrors = false;
        }

        try
        {
            this.currentSettings.setTilesY(Convert.ToInt32(this.textBoxYTiles.Text) - 1);
            this.textBoxYTiles.BackColor = this.colorNormal;
        }
        catch (Exception e)
        {
            this.textBoxYTiles.BackColor = this.colorError;
            noErrors = false;
        }

        try
        {
            this.currentSettings.setTileSizeX(Convert.ToInt32(this.textBoxTileWidth.Text));
            this.textBoxTileWidth.BackColor = this.colorNormal;
        }
        catch (Exception e)
        {
            this.textBoxTileWidth.BackColor = this.colorError;
            noErrors = false;
        }

        try
        {
            this.currentSettings.setTileSizeY(Convert.ToInt32(this.textBoxTileHeight.Text));
            this.textBoxTileHeight.BackColor = this.colorNormal;
        }
        catch (Exception e)
        {
            this.textBoxTileHeight.BackColor = this.colorError;
            noErrors = false;
        }

        // If there are no errors close the screen on save
        if (noErrors)
        {
            this.Close();
        }
    }

    private void setTextboxes()
    {
        // Player names
        this.textBoxPlayer1.Text = this.currentSettings.getP1Name();
        this.textBoxPlayer2.Text = this.currentSettings.getP2Name();

        // X and Y tiles
        // Do plus one to be user friendly
        this.textBoxXTiles.Text = (this.currentSettings.getTilesX() + 1).ToString();
        this.textBoxYTiles.Text = (this.currentSettings.getTilesY() + 1).ToString();

        // Standard icon is Man and Fish
        this.comboBox1.Text = this.currentSettings.getP1Icon();
        this.comboBox2.Text = this.currentSettings.getP2Icon();

        // Tile sizes
        this.textBoxTileWidth.Text = this.currentSettings.getTileSizeX().ToString();
        this.textBoxTileHeight.Text = this.currentSettings.getTileSizeY().ToString();
    }

    public void openSettings(object obj, EventArgs pea)
    {
        ReversiForm screen = new ReversiForm();
        Application.Run(screen);
    }

    // Set all default component values
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
            this.labelXTileWidth = new System.Windows.Forms.Label();
            this.labelYTileWidth = new System.Windows.Forms.Label();
            this.textBoxTileWidth = new System.Windows.Forms.TextBox();
            this.textBoxTileHeight = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxPlayer1
            // 
            this.textBoxPlayer1.Location = new System.Drawing.Point(11, 43);
            this.textBoxPlayer1.Name = "textBoxPlayer1";
            this.textBoxPlayer1.Size = new System.Drawing.Size(100, 20);
            this.textBoxPlayer1.TabIndex = 0;
            // 
            // textBoxPlayer2
            // 
            this.textBoxPlayer2.Location = new System.Drawing.Point(147, 43);
            this.textBoxPlayer2.Name = "textBoxPlayer2";
            this.textBoxPlayer2.Size = new System.Drawing.Size(100, 20);
            this.textBoxPlayer2.TabIndex = 1;
            // 
            // labelPlayer2
            // 
            this.labelPlayer2.AutoSize = true;
            this.labelPlayer2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayer2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(252)))), ((int)(((byte)(107)))), ((int)(((byte)(107)))));
            this.labelPlayer2.Location = new System.Drawing.Point(147, 15);
            this.labelPlayer2.Name = "labelPlayer2";
            this.labelPlayer2.Size = new System.Drawing.Size(59, 15);
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
            this.buttonSave.Location = new System.Drawing.Point(263, 215);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(100, 22);
            this.buttonSave.TabIndex = 9;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            // 
            // labelXTiles
            // 
            this.labelXTiles.AutoSize = true;
            this.labelXTiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelXTiles.Location = new System.Drawing.Point(8, 133);
            this.labelXTiles.Name = "labelXTiles";
            this.labelXTiles.Size = new System.Drawing.Size(104, 15);
            this.labelXTiles.TabIndex = 8;
            this.labelXTiles.Text = "Horizontal tiles";
            // 
            // labelYTiles
            // 
            this.labelYTiles.AutoSize = true;
            this.labelYTiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelYTiles.Location = new System.Drawing.Point(144, 133);
            this.labelYTiles.Name = "labelYTiles";
            this.labelYTiles.Size = new System.Drawing.Size(86, 15);
            this.labelYTiles.TabIndex = 7;
            this.labelYTiles.Text = "Vertical tiles";
            // 
            // textBoxXTiles
            // 
            this.textBoxXTiles.Location = new System.Drawing.Point(11, 153);
            this.textBoxXTiles.Name = "textBoxXTiles";
            this.textBoxXTiles.Size = new System.Drawing.Size(100, 20);
            this.textBoxXTiles.TabIndex = 5;
            // 
            // textBoxYTiles
            // 
            this.textBoxYTiles.Location = new System.Drawing.Point(147, 153);
            this.textBoxYTiles.Name = "textBoxYTiles";
            this.textBoxYTiles.Size = new System.Drawing.Size(100, 20);
            this.textBoxYTiles.TabIndex = 6;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(11, 71);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(100, 21);
            this.comboBox1.TabIndex = 3;
            this.comboBox1.SelectionChangeCommitted += new System.EventHandler(this.comboBox1GetString);
            // 
            // labelPlayer1
            // 
            this.labelPlayer1.AutoSize = true;
            this.labelPlayer1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayer1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(107)))), ((int)(((byte)(200)))), ((int)(((byte)(252)))));
            this.labelPlayer1.Location = new System.Drawing.Point(11, 15);
            this.labelPlayer1.Name = "labelPlayer1";
            this.labelPlayer1.Size = new System.Drawing.Size(59, 15);
            this.labelPlayer1.TabIndex = 10;
            this.labelPlayer1.Text = "Player 1";
            this.labelPlayer1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(147, 71);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(100, 21);
            this.comboBox2.TabIndex = 4;
            this.comboBox2.SelectionChangeCommitted += new System.EventHandler(this.comboBox2GetString);
            // 
            // labelXTileWidth
            // 
            this.labelXTileWidth.AutoSize = true;
            this.labelXTileWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelXTileWidth.Location = new System.Drawing.Point(8, 195);
            this.labelXTileWidth.Name = "labelXTileWidth";
            this.labelXTileWidth.Size = new System.Drawing.Size(69, 15);
            this.labelXTileWidth.TabIndex = 15;
            this.labelXTileWidth.Text = "Tile width";
            // 
            // labelYTileWidth
            // 
            this.labelYTileWidth.AutoSize = true;
            this.labelYTileWidth.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelYTileWidth.Location = new System.Drawing.Point(144, 195);
            this.labelYTileWidth.Name = "labelYTileWidth";
            this.labelYTileWidth.Size = new System.Drawing.Size(75, 15);
            this.labelYTileWidth.TabIndex = 14;
            this.labelYTileWidth.Text = "Tile height";
            // 
            // textBoxTileWidth
            // 
            this.textBoxTileWidth.Location = new System.Drawing.Point(11, 215);
            this.textBoxTileWidth.Name = "textBoxTileWidth";
            this.textBoxTileWidth.Size = new System.Drawing.Size(100, 20);
            this.textBoxTileWidth.TabIndex = 7;
            // 
            // textBoxTileHeight
            // 
            this.textBoxTileHeight.Location = new System.Drawing.Point(147, 215);
            this.textBoxTileHeight.Name = "textBoxTileHeight";
            this.textBoxTileHeight.Size = new System.Drawing.Size(100, 20);
            this.textBoxTileHeight.TabIndex = 8;
            // 
            // Settingsform
            // 
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(384, 263);
            this.Controls.Add(this.labelXTileWidth);
            this.Controls.Add(this.labelYTileWidth);
            this.Controls.Add(this.textBoxTileWidth);
            this.Controls.Add(this.textBoxTileHeight);
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
            this.ResumeLayout(false);
            this.PerformLayout();

    }
}