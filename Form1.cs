using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;

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
        private Panel headerPanel = null!;
        private Label lblName = null!;

        private string userName = "";
        private bool chatbotStarted = false;
        private bool waitingForMood = false;
        private string currentTopic = "";

        private int passwordIndex = 0;
        private int phishingIndex = 0;
        private int privacyIndex = 0;
        private int malwareIndex = 0;

        private readonly string[] passwordTips =
        {
            "Make sure to use strong, unique passwords for each account. Avoid using personal details in your passwords.",
            "A good password should contain a mix of uppercase letters, lowercase letters, numbers, and special characters.",
            "Never reuse the same password across multiple accounts. If one account is compromised, all your accounts could be at risk.",
            "Consider using a password manager to securely generate and store complex passwords."
        };

        private readonly string[] phishingTips =
        {
            "Phishing attacks trick users into revealing sensitive information through fake emails or websites.",
            "Always verify the sender's email address before clicking on links or downloading attachments.",
            "Be cautious of messages creating a sense of urgency or requesting personal information.",
            "If an email seems suspicious, contact the company directly through official channels."
        };

        private readonly string[] privacyTips =
        {
            "Protect your privacy by limiting the personal information you share online.",
            "Review your social media privacy settings regularly to control who can see your information.",
            "Avoid using public Wi-Fi networks for sensitive activities such as online banking.",
            "Always read app permissions carefully before granting access to your device data."
        };

        private readonly string[] malwareTips =
        {
            "Malware is malicious software designed to damage devices or steal information.",
            "Keep your operating system and software updated to reduce security vulnerabilities.",
            "Install trusted antivirus software and perform regular scans of your device.",
            "Avoid downloading files or software from untrusted websites."
        };

        public Form1()
        {
            InitializeComponent();
            CreateCustomGUI();

            this.Shown += Form1_Shown;
        }

        private void PlayWelcomeAudio()
        {
            string audioPath = Path.Combine(Application.StartupPath, "welcome.wav");

            if (File.Exists(audioPath))
            {
                // Task.Run ensures the UI thread never freezes while reading the file
                System.Threading.Tasks.Task.Run(() =>
                {
                    try
                    {
                        using (SoundPlayer player = new SoundPlayer(audioPath))
                        {
                            player.PlaySync(); // Plays safely inside the background thread
                        }
                    }
                    catch (Exception ex)
                    {
                        this.BeginInvoke(new Action(() =>
                            MessageBox.Show($"Audio playback error: {ex.Message}")));
                    }
                });
            }
            else
            {
                MessageBox.Show("File not found:\n" + audioPath, "Audio Missing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            headerPanel = new Panel();
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 80;
            headerPanel.BackColor = Color.FromArgb(0, 102, 204);
            this.Controls.Add(headerPanel);

            // TITLE
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
            lblName = new Label();
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

        private void TxtMessage_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSend_Click(sender, e);
                e.SuppressKeyPress = true; // Prevents Windows "Ding" beep on Enter key press
            }
        }

        private void DisplayMessage(string message)
        {
            string time = DateTime.Now.ToString("HH:mm");
            rtbChat.AppendText($"[{time}] {message}" + Environment.NewLine + Environment.NewLine);
            rtbChat.SelectionStart = rtbChat.Text.Length;
            rtbChat.ScrollToCaret();
        }

        private void ShowNextTip(string topic)
        {
            switch (topic)
            {
                case "password":
                    DisplayMessage("🔑 " + passwordTips[passwordIndex]);
                    passwordIndex = (passwordIndex + 1) % passwordTips.Length;
                    break;

                case "phishing":
                    DisplayMessage("🎣 " + phishingTips[phishingIndex]);
                    phishingIndex = (phishingIndex + 1) % phishingTips.Length;
                    break;

                case "privacy":
                    DisplayMessage("🔒 " + privacyTips[privacyIndex]);
                    privacyIndex = (privacyIndex + 1) % privacyTips.Length;
                    break;

                case "malware":
                    DisplayMessage("🪱 " + malwareTips[malwareIndex]);
                    malwareIndex = (malwareIndex + 1) % malwareTips.Length;
                    break;

                default:
                    DisplayMessage("Please ask about a cybersecurity topic first.");
                    break;
            }

            // Remind user of options after giving a tip
            DisplayMenu();
        }

        private void BtnStart_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Name cannot be empty.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            userName = txtName.Text.Trim();
            chatbotStarted = true;
            waitingForMood = true;

            DisplayMessage($"👋 Welcome, {userName}!");
            DisplayMessage($"{userName}, how are you today?");

            txtName.Enabled = false; // Prevent user from altering name mid-chat
            btnStart.Enabled = false;
        }

        private void BtnSend_Click(object? sender, EventArgs e)
        {
            if (!chatbotStarted)
            {
                MessageBox.Show("Please enter your name and click Start first.", "System Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string input = txtMessage.Text.ToLower().Trim();

            if (string.IsNullOrWhiteSpace(input))
            {
                return; // Silently drop empty enters to protect UX
            }

            DisplayMessage($"🧑 {userName}: {txtMessage.Text.Trim()}");
            txtMessage.Clear(); // Clear instantly for seamless chatting

            // MOOD RESPONSE
            if (waitingForMood)
            {
                if (input.Contains("good") || input.Contains("fine") || input.Contains("great"))
                {
                    DisplayMessage($"😊 That's great to hear, {userName}! Let's get started.");
                }
                else if (input.Contains("bad") || input.Contains("sad") || input.Contains("tired"))
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
                return;
            }

            // EXIT
            if (input == "exit" || input == "quit")
            {
                DisplayMessage($"👋 Goodbye {userName}! Stay safe online.");
                Application.Exit();
                return;
            }

            if (input.Contains("tell me more") ||
                input.Contains("another tip") ||
                input.Contains("another one") ||
                input.Contains("explain more"))
            {
                ShowNextTip(currentTopic);
                return;
            }

            // PASSWORDS
            if (input.Contains("1") || input.Contains("password"))
            {
                currentTopic = "password";
                ShowNextTip(currentTopic);
            }
            // PHISHING
            else if (input.Contains("2") || input.Contains("phishing"))
            {
                currentTopic = "phishing";
                ShowNextTip(currentTopic);
            }
            // PRIVACY
            else if (input.Contains("3") || input.Contains("privacy"))
            {
                currentTopic = "privacy";
                ShowNextTip(currentTopic);
            }
            // MALWARE
            else if (input.Contains("4") || input.Contains("malware"))
            {
                currentTopic = "malware";
                ShowNextTip(currentTopic);
            }

            // GENERAL CHAT RESPONSES
            else if (input == "how are you")
            {
                DisplayMessage("🤖 I'm an AI, so I don't have feelings, but I'm happy to assist!");
            }
            else if (input == "whats your name" || input == "what's your name" || input.Contains("your name"))
            {
                DisplayMessage("🤖 Friday, I'm your CyberSecurity Awareness Bot!");
            }
            else if (input == "who created you")
            {
                DisplayMessage("👨‍💻 I was built by IT students to promote cybersecurity awareness.");
            }
            else if (input == "whats your purpose" || input == "what's your purpose")
            {
                DisplayMessage("🎯 My purpose is to educate users about cybersecurity topics.");
            }
            else if (input.Contains("joke"))
            {
                DisplayMessage("Why did the computer go to the doctor?");
                DisplayMessage("Because it had a virus! 😂");
            }
            else
            {
                DisplayMessage("❌ Sorry, I didn't understand that.");
                DisplayMenu();
            }
        }

        private void DisplayMenu()
        {
            rtbChat.AppendText("📚 Choose a topic number or ask a question:" + Environment.NewLine +
                               "1️⃣ - Passwords" + Environment.NewLine +
                               "2️⃣ - Phishing" + Environment.NewLine +
                               "3️⃣ - Privacy" + Environment.NewLine +
                               "4️⃣ - Malware" + Environment.NewLine +
                               "Type 'exit' to quit." + Environment.NewLine + Environment.NewLine);
            rtbChat.SelectionStart = rtbChat.Text.Length;
            rtbChat.ScrollToCaret();
        }
        
    }
}