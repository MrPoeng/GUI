using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace GUI
{
    public partial class Form1 : Form
    {
        // CONTROLS
        private Label lblTitle = null!;
        private RichTextBox rtbChat = null!;
        private TextBox txtName = null!;
        private TextBox txtMessage = null!;
        private Button btnStart = null!;
        private Button btnSend = null!;

        string userName = "";
        bool chatbotStarted = false;
        bool waitingForMood = false;

        public Form1()
        {
            InitializeComponent();
            CreateCustomGUI();

            this.Shown += Form1_Shown;
        }

        private void PlayWelcomeAudio()
        {
            string audioPath = Path.Combine(
                Application.StartupPath,
                "welcome.wav");

            if (File.Exists(audioPath))
            {
                SoundPlayer player = new SoundPlayer(audioPath);
                player.Play();
            }
            else
            {
                MessageBox.Show("File not found:\n" + audioPath);
            }
        }

        private void Form1_Shown(object? sender, EventArgs e)
        {
            PlayWelcomeAudio();

        }

        private void CreateCustomGUI()
        {
            // FORM 
            this.Text = "CyberSecurity Chatbot";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(230, 240, 255);

            // Header Panel
            Panel headerPanel = new Panel();
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 80;
            headerPanel.BackColor = Color.FromArgb(0, 102, 204);
            this.Controls.Add(headerPanel);

            //TITLE

            lblTitle = new Label();
            lblTitle.Text = "CyberSecurity Chatbot 🤖";
            lblTitle.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            lblTitle.ForeColor = Color.DeepSkyBlue;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(250, 20);
            headerPanel.Controls.Add(lblTitle);

            

            // CHAT AREA

            rtbChat = new RichTextBox();
            rtbChat.Location = new Point(30, 100);
            rtbChat.Size = new Size(920, 380);
            rtbChat.Font = new Font("Segoe UI", 12);
            rtbChat.ReadOnly = true;
            rtbChat.BackColor = Color.FromArgb(245, 250, 255);
            rtbChat.ForeColor = Color.Black;
            rtbChat.BorderStyle = BorderStyle.None;

            this.Controls.Add(rtbChat);

            // WELCOME MESSAGE
            DisplayMessage("🤖 Hello! My name is Friday.");
            DisplayMessage("🛡️ I'm here to help you stay safe online.");
            DisplayMessage("👤 Please enter your name and click Start.");


            // NAME LABEL

            Label lblName = new Label();
            lblName.Text = "Enter your name:";
            lblName.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblName.ForeColor = Color.FromArgb(0, 51, 153);
            lblName.Location = new Point(30, 510);
            lblName.AutoSize = true;

            this.Controls.Add(lblName);


            // NAME TEXTBOX

            txtName = new TextBox();
            txtName.Location = new Point(220, 508);
            txtName.Size = new Size(250, 35);
            txtName.Font = new Font("Segoe UI", 12);
            txtName.BackColor = Color.White;
            txtName.ForeColor = Color.Navy;

            this.Controls.Add(txtName);


            // START BUTTON

            btnStart = new Button();
            btnStart.Text = "Start";
            btnStart.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnStart.BackColor = Color.FromArgb(0, 120, 215);
            btnStart.ForeColor = Color.White;
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.FlatAppearance.BorderSize = 0;
            btnStart.Size = new Size(120, 40);
            btnStart.Location = new Point(500, 505);
            btnStart.Click += BtnStart_Click;

            this.Controls.Add(btnStart);


            // MESSAGE TEXTBOX

            txtMessage = new TextBox();
            txtMessage.Location = new Point(30, 580);
            txtMessage.Size = new Size(760, 35);
            txtMessage.Font = new Font("Segoe UI", 12);
            txtMessage.BackColor = Color.White;
            txtMessage.ForeColor = Color.Navy;

            this.Controls.Add(txtMessage);


            // SEND BUTTON

            btnSend = new Button();
            btnSend.Text = "Send";
            btnSend.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnSend.BackColor = Color.FromArgb(0, 153, 255);
            btnSend.ForeColor = Color.White;
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.FlatAppearance.BorderSize = 0;
            btnSend.Size = new Size(140, 40);
            btnSend.Location = new Point(810, 575);
            btnSend.Click += BtnSend_Click;

            this.Controls.Add(btnSend);

            txtMessage.KeyDown += TxtMessage_KeyDown;
        }


        // DISPLAY MESSAGE METHOD

        private void TxtMessage_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSend_Click(sender, e);
                e.SuppressKeyPress = true;
            }
        }

        private void DisplayMessage(string message)
        {
            string time = DateTime.Now.ToString("HH:mm");
            rtbChat.AppendText($"[{time}] {message}" + Environment.NewLine);
            rtbChat.ScrollToCaret();
        }


        // START BUTTON EVENT

        private void BtnStart_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Name cannot be empty.");
                return;
            }

            userName = txtName.Text.Trim();

            chatbotStarted = true;
            waitingForMood = true;

            DisplayMessage($"👋 Welcome, {userName}!");
            DisplayMessage($"{userName}, how are you today?");
        }


        // SEND BUTTON EVENT

        private void BtnSend_Click(object? sender, EventArgs e)
        {
            if (!chatbotStarted)
            {
                MessageBox.Show("Please enter your name and click Start first.");
                return;
            }
            string input = txtMessage.Text.ToLower().Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show("Please enter a message.");
                return;
            }

            DisplayMessage($"🧑 {userName}: {input}");


            // MOOD RESPONSE

            if (waitingForMood)
            {
                if (input.Contains("good") ||
                    input.Contains("fine") ||
                    input.Contains("great"))
                {
                    string response = $"That's great to hear, {userName}! Let's get started.";
                    DisplayMessage("😊 " + response);

                }
                else if (input.Contains("bad") ||
                         input.Contains("sad") ||
                         input.Contains("tired"))
                {
                    DisplayMessage($"💙 Sorry to hear that, {userName}. Hopefully I can cheer you up!");
                }
                else
                {
                    DisplayMessage($"👍 Thanks for sharing, {userName}! Let's get started.");
                }

                
                DisplayMessage("🛡️ CYBERSECURITY AWARENESS ASSISTANT 🛡️");
                

                DisplayMenu();

                waitingForMood = false;

                txtMessage.Clear();
                return;
            }

            
            // EXIT
            
            if (input == "exit")
            {
                DisplayMessage($"👋 Goodbye {userName}! Stay safe online.");
                Application.Exit();
            }

            
            // PASSWORDS
            
            else if (input.Contains("1") || input.Contains("password"))
            {
                DisplayMessage("🔐 Strong Password Tips:");
                DisplayMessage("- Use at least 12 characters");
                DisplayMessage("- Mix uppercase, lowercase, numbers, symbols");
                DisplayMessage("- Never reuse passwords");
                DisplayMessage("- Use a password manager");
            }

            
            // PHISHING
            
            else if (input.Contains("2") || input.Contains("phishing"))
            {
                DisplayMessage("🎣 Phishing Awareness:");
                DisplayMessage("- Do not click suspicious links");
                DisplayMessage("- Check sender email carefully");
                DisplayMessage("- Verify requests directly");
            }

            
            // PRIVACY
            
            else if (input.Contains("3") || input.Contains("privacy"))
            {
                DisplayMessage("🔒 Online Privacy Tips:");
                DisplayMessage("- Limit personal info online");
                DisplayMessage("- Use privacy settings");
                DisplayMessage("- Avoid public Wi-Fi");
            }

            
            // MALWARE
            
            else if (input.Contains("4") || input.Contains("malware"))
            {
                DisplayMessage("- Malware Protection:");
                DisplayMessage("- Install antivirus software");
                DisplayMessage("- Keep software updated");
                DisplayMessage("- Scan USB drives");
            }

            
            // HOW ARE YOU
            
            else if (input == "how are you")
            {
                DisplayMessage(" I'm an AI, so I don't have feelings, but I'm happy to assist!");
            }

           
            // BOT NAME
            
            else if (input == "whats your name" ||
                     input == "what's your name" ||
                     input.Contains("your name"))
            {
                DisplayMessage("Friday, I'm your CyberSecurity Awareness Bot!");
            }

            
            // WHO CREATED YOU
            
            else if (input == "who created you")
            {
                DisplayMessage("👨‍💻 I was built by IT students to promote cybersecurity awareness.");
            }

            
            // PURPOSE
            
            else if (input == "whats your purpose" ||
                     input == "what's your purpose")
            {
                DisplayMessage("🎯 My purpose is to educate users about cybersecurity topics.");
            }

            
            // JOKE
           
            else if (input.Contains("joke"))
            {
                DisplayMessage(" Why did the computer go to the doctor?");
                DisplayMessage("Because it had a virus!😂");
            }

            
            // UNKNOWN INPUT
            
            else
            {
                DisplayMessage("❌ Sorry, I didn't understand that.");
                DisplayMessage("Try choosing a topic number or asking a cybersecurity question.");
            }

            txtMessage.Clear();
        }

        // DISPLAY MENU

        private void DisplayMenu()
        {
            DisplayMessage("");
            DisplayMessage("📚 Choose a topic or ask a question:");
            DisplayMessage("1 - Passwords");
            DisplayMessage("2 - Phishing");
            DisplayMessage("3 - Privacy");
            DisplayMessage("4 - Malware");
            DisplayMessage("Type 'exit' to quit.");
            DisplayMessage("");
        }

        private void Form1_VisibleChanged(object? sender, EventArgs e)
        {

        }

        
    }

}