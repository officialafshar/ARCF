using System;
using System.Drawing;
using System.Windows.Forms;


namespace TirKaman
{
    public class Game : Form
    {
        // Constants for the game parameters
        const int WINDOW_WIDTH = 800;
        const int WINDOW_HEIGHT = 600;
        const int ARROW_X = 100;
        const int ARROW_Y = 300;
        const int ARROW_LENGTH = 50;
        const int ARROW_WIDTH = 5;
        const int TARGET_X = 700;
        const int TARGET_Y_MIN = 100;
        const int TARGET_Y_MAX = 500;
        const int TARGET_WIDTH = 10;
        const int TARGET_HEIGHT = 100;
        const double GRAVITY = 90.8;
        const double PI = Math.PI;


        // Variables for the game state
        bool isShooting; // Whether the arrow is in motion
        double angle; // The angle of the arrow in radians
        double power; // The initial velocity of the arrow in pixels per second
        double time; // The elapsed time of the arrow flight in seconds
        double arrowX; // The current x position of the arrow tip in pixels
        double arrowY; // The current y position of the arrow tip in pixels
        double targetY; // The current y position of the target center in pixels
        double targetSpeed; // The speed of the target movement in pixels per second
        double flightTime;
        bool isHit; // Whether the arrow hit the target
        bool isMiss; // Whether the arrow missed the target

        // Controls for the game input and output
        Label angleLabel; // Shows the current angle value
        TrackBar angleBar; // Allows the user to adjust the angle value
        Label powerLabel; // Shows the current power value
        TrackBar powerBar; // Allows the user to adjust the power value
        Button shootButton; // Allows the user to shoot the arrow
        System.Windows.Forms.Timer timer; // Updates the game logic and graphics

        public Game()
        {
            // Initialize the form properties
            this.Text = "Archery game version 1";
            this.ClientSize = new Size(WINDOW_WIDTH, WINDOW_HEIGHT);
            this.BackColor = Color.White;

            // Initialize the angle label and bar
            angleLabel = new Label();
            angleLabel.Text = "Angle: 0";
            angleLabel.Location = new Point(10, 10);
            angleLabel.AutoSize = true;
            this.Controls.Add(angleLabel);

            angleBar = new TrackBar();
            angleBar.Minimum = 0;
            angleBar.Maximum = 180;
            angleBar.Value = 45;
            angleBar.TickFrequency = 10;
            angleBar.Location = new Point(10, 40);
            angleBar.Size = new Size(200, 50);
            angleBar.Scroll += AngleBar_Scroll;
            this.Controls.Add(angleBar);

            // Initialize the power label and bar
            powerLabel = new Label();
            powerLabel.Text = "Power: 0";
            powerLabel.Location = new Point(10, 100);
            powerLabel.AutoSize = true;
            this.Controls.Add(powerLabel);

            powerBar = new TrackBar();
            powerBar.Minimum = 1;
            powerBar.Maximum = 1000;
            powerBar.Value = 500;
            powerBar.TickFrequency = 100;
            powerBar.Location = new Point(10, 130);
            powerBar.Size = new Size(200, 50);
            powerBar.Scroll += PowerBar_Scroll;
            this.Controls.Add(powerBar);

            // Initialize the shoot button
            shootButton = new Button();
            shootButton.Text = "Shoot";
            shootButton.Location = new Point(10, 200);
            shootButton.Size = new Size(100, 50);
            shootButton.Click += ShootButton_Click;
            this.Controls.Add(shootButton);

            // Initialize the timer
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 17; // Update every 17 milliseconds
            timer.Tick += Timer_Tick;

            // Initialize the game state variables
            ResetGame();
            
        }

        private void ResetGame()
        {
            flightTime = 0;


            // Reset the game state variables to their initial values
            isShooting = false;
        //angle = PI / 4; // 45 degrees in radians
        //power = 500; 
        //time = 0;
        arrowX = ARROW_X + ARROW_LENGTH * Math.Cos(angle); 
        arrowY = ARROW_Y - ARROW_LENGTH * Math.Sin(angle); 
        //targetY = (TARGET_Y_MIN + TARGET_Y_MAX) / 2; 
        targetSpeed = (TARGET_Y_MAX - TARGET_Y_MIN) / 134; 
        isHit = false;
        isMiss= false;

        // Enable the input controls and disable the timer
        angleBar.Enabled= true;
        powerBar.Enabled= true;
        shootButton.Enabled= true;
        timer.Enabled= true;

        // Refresh the form to redraw the graphics
        this.Refresh();

        }

        private void AngleBar_Scroll(object sender, EventArgs e)
        {
        // Update the angle value based on the track bar position
        angle= PI * (angleBar.Value-90) /180;

        // Update the label text to show the angle value in degrees
        angleLabel.Text= "Angle: " + angleBar.Value;

        // Update the arrow position based on the new angle value
        arrowX= ARROW_X + ARROW_LENGTH * Math.Cos(angle); 
        arrowY= ARROW_Y - ARROW_LENGTH * Math.Sin(angle); 

        // Refresh the form to redraw the graphics
        this.Refresh();

        }

        private void PowerBar_Scroll(object sender, EventArgs e)
        {
        // Update the power value based on the track bar position
        power= powerBar.Value;

        // Update the label text to show the power value in pixels per second
        powerLabel.Text= "Power: " + power;

        }

        private void ShootButton_Click(object sender, EventArgs e)
        {
        // Start shooting by setting isShooting to true and resetting time to zero
        isShooting= true;
            flightTime = 0;

            // Disable the input controls and enable the timer
            angleBar.Enabled= false;
        powerBar.Enabled= false;
        shootButton.Enabled= false;
            timer.Enabled= true;
       


        }

        private void Timer_Tick(object sender, EventArgs e)
        {
        // Update time by adding one tick interval (in seconds)
        time+= timer.Interval /1000.0;

            // Update arrow position using kinematic equations of motion under gravity

            if (isShooting)
            {
                // Update flightTime by adding one tick interval (in seconds)
                flightTime += timer.Interval / 1000.0;

                // Update arrow position using kinematic equations of motion under gravity
                arrowX = ARROW_X + power * Math.Cos(angle) * flightTime;
                arrowY = ARROW_Y - (power * Math.Sin(angle) * flightTime - GRAVITY * flightTime * flightTime / 2);
                }

                // Update target position using a simple harmonic motion equation

                targetY = (TARGET_Y_MIN + TARGET_Y_MAX) /2 + (TARGET_Y_MAX - TARGET_Y_MIN) /2 * Math.Sin(targetSpeed * time);

        // Check for collision between arrow and target using simple bounding box method

        if (arrowX >= TARGET_X - TARGET_WIDTH /2 && arrowX <= TARGET_X + TARGET_WIDTH /2 && arrowY >= targetY - TARGET_HEIGHT /2 && arrowY <= targetY + TARGET_HEIGHT /2)
        {
        // Set isHit to true and stop shooting
        isHit= true;
        isShooting= false;

        // Change background color to green for two seconds and then reset game

        this.BackColor= Color.Green;

        timer.Interval=1000; 
        timer.Tick -= Timer_Tick; 
        timer.Tick += ResetTimer_Tick;

        }

        // Check for missing condition by checking if arrow goes out of bounds

        if (arrowX > WINDOW_WIDTH || arrowY > WINDOW_HEIGHT)
        {
        // Set isMiss to true and stop shooting

        isMiss= true;

        isShooting= false;

        // Change background color to red for two seconds and then reset game

        this.BackColor= Color.Red;

        timer.Interval=1000; 

        timer.Tick -= Timer_Tick; 

        timer.Tick += ResetTimer_Tick;

        }

        // Refresh form to redraw graphics

        this.Refresh();

        }

        private void ResetTimer_Tick(object sender, EventArgs e)
        {
        // Reset timer interval and event handler

        timer.Interval=10; 

        timer.Tick -= ResetTimer_Tick; 

        timer.Tick += Timer_Tick;

        // Reset game state and background color

        ResetGame();

        this.BackColor= Color.BlueViolet;

        }

        protected override void OnPaint(PaintEventArgs e)
        {
        base.OnPaint(e);

        // Get graphics object from event arguments

        Graphics g=e.Graphics;

        // Draw arrow as a line with a given color depending on hit or miss condition

        Color arrowColor=isHit ? Color.Green : isMiss ? Color.Red : Color.Black;

        Pen arrowPen=new Pen(arrowColor,ARROW_WIDTH);

        g.DrawEllipse(arrowPen, (float)arrowX + 5, (float)arrowY + 5, 5, 5);

        // Draw target as a rectangle with a black border and a red fill

        Pen targetPen=new Pen(Color.Black,TARGET_WIDTH);

        Brush targetBrush=new SolidBrush(Color.Red);

        g.DrawRectangle(targetPen,new Rectangle(TARGET_X - TARGET_WIDTH /2,(int)(targetY - TARGET_HEIGHT /2),TARGET_WIDTH,TARGET_HEIGHT));

        g.FillRectangle(targetBrush,new Rectangle(TARGET_X - TARGET_WIDTH /2,(int)(targetY - TARGET_HEIGHT /2),TARGET_WIDTH,TARGET_HEIGHT));

        // If the game is over, draw a message to show the result

        if (isHit || isMiss)
        {
            // Create a font and a brush for the message text
            Font messageFont = new Font("Arial", 32, FontStyle.Bold);
            Brush messageBrush = new SolidBrush(Color.White);

            // Create a string for the message text depending on hit or miss condition
            string messageText = isHit ? "You hit the target!" : "You missed the target!";

            // Measure the size of the message text
            SizeF messageSize = g.MeasureString(messageText, messageFont);

            // Calculate the position of the message text to center it on the form
            float messageX = (WINDOW_WIDTH - messageSize.Width) / 2;
            float messageY = (WINDOW_HEIGHT - messageSize.Height) / 2;

            // Draw the message text on the form
            g.DrawString(messageText, messageFont, messageBrush, messageX, messageY);
        }

        // Add a line to exit the game when the user presses any key
        this.KeyDown += (s, e) => Application.Exit();
        }
        }

static class Program
{
    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new Game());
    }
}
}
