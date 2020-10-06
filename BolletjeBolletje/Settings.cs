using System;
using System.Drawing;
using System.Windows.Forms;

class Settingsform : Form
{

    public Settingsform()
    {
        // Set at top to initialise components
        this.InitializeComponent();
    }

    public void openSettings(object obj, EventArgs pea)
    {
        ReversiForm screen = new ReversiForm();
        Application.Run(screen);
        Console.WriteLine("Settings");
    }

    private void InitializeComponent()
    {
            this.SuspendLayout();
            // 
            // Settingsform
            // 
            this.ClientSize = new System.Drawing.Size(384, 361);
            this.Name = "Settingsform";
            this.ResumeLayout(false);

    }
}