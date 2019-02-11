using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Media;
using System.Globalization;

using System.Runtime.InteropServices;
using System.IO;

namespace countdownBuddy
{

    public partial class Form1 : Form
    {

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        bool settingsBtn = false;

        DateTime startTime = DateTime.Now;
        DateTime finishTime = DateTime.Now;
        TimeSpan duration = new TimeSpan(0);

        //Voice plan stuff
        SpeechSynthesizer synthesizer = new SpeechSynthesizer();
        string voicePlanLoadedName;
        string voicePlanErrorMsg;
        int voiceMsgNo = 0;
        TimeSpan playPeriod = new TimeSpan(0,0,1);

        bool hasSet = false;

        List<VoiceEvents> EventList = new List<VoiceEvents>();

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public Form1()
        {
            InitializeComponent();

            maskedTextBox1.Text = startTime.ToString("HH:mm:ss.ff");
            maskedTextBox2.Text = finishTime.ToString("HH:mm:ss.ff");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            synthesizer.SelectVoiceByHints(VoiceGender.Female);

            synthesizer.Volume = 100;  // 0...100
            synthesizer.Rate = 0;     // -10...10

            synthesizer.Speak(Math.PI.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SpeechSynthesizer synthesizer = new SpeechSynthesizer();
            synthesizer.SelectVoiceByHints(VoiceGender.Female);
            synthesizer.Volume = 100;  // 0...100
            synthesizer.Rate = 0;     // -10...10

            synthesizer.SpeakAsync("SwaG");
        }

        private void label9_Click(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.label17.Text = DateTime.Now.ToString("HH:mm:ss.ff");

            if (hasSet && (startTime - DateTime.Now) > TimeSpan.Zero)
            {
                //Big timer
                label1.Text = "T -" + (startTime - DateTime.Now).ToString("hh\\:mm\\:ss\\.ff");

                //Time to start
                if (hasSet) label19.Text = "-" + (startTime - DateTime.Now).ToString("hh\\:mm\\:ss\\.ff");

                //Time to halfway
                if (hasSet) label20.Text = "-" + (startTime + new TimeSpan(duration.Ticks / 2) - DateTime.Now).ToString("hh\\:mm\\:ss\\.ff");

                //Time to finish
                if (hasSet) label21.Text = "-" + (finishTime - DateTime.Now).ToString("hh\\:mm\\:ss\\.ff");
            } else
            {
                //Big timer
                label1.Text = "T +" + (startTime - DateTime.Now).ToString("hh\\:mm\\:ss\\.ff");

                //Time to start
                if (hasSet) label19.Text = "+" + (startTime - DateTime.Now).ToString("hh\\:mm\\:ss\\.ff");

                //Time to halfway
                if (hasSet) label20.Text = "+" + (startTime + new TimeSpan(duration.Ticks / 2) - DateTime.Now).ToString("hh\\:mm\\:ss\\.ff");

                //Time to finish
                if (hasSet) label21.Text = "+" + (finishTime - DateTime.Now).ToString("hh\\:mm\\:ss\\.ff");
            }

            //Voice plan
            if(voicePlanErrorMsg == null)voiceEventQueue();
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            //Start time
            if (DateTime.TryParse(maskedTextBox1.Text.Replace(' ', '0') + "00", out startTime))
            {
                //Setting start time:

                if (startTime < DateTime.Now) startTime = startTime.AddDays(1);

                if (finishTime == null && duration == null)
                {
                    finishTime = startTime;
                }

                if (finishTime != null && duration == null)
                {
                    duration = finishTime - startTime;
                }

                if (finishTime == null && duration != null)
                {
                    finishTime = startTime + duration;
                }

                if (finishTime != null && duration != null)
                {
                    finishTime = startTime + duration;
                }

                hasSet = true;

                //Update labels/textboxes
                //Duration
                label18.Text = duration.ToString("hh\\:mm\\:ss\\.ff");             //Maskboxes
                maskedTextBox2.Text = finishTime.ToString("HH:mm:ss.ff");
                maskedTextBox3.Text = duration.ToString("hh\\:mm\\:ss\\.ff");
            }
            else
            {
                hasSet = false;
                //Bad input
                maskedTextBox1.Text = "00:00:00.00";
                SystemSounds.Beep.Play();
            }
        }

        private void maskedTextBox2_TextChanged(object sender, EventArgs e)
        {
            //Finish time
            if (DateTime.TryParse(maskedTextBox2.Text.Replace(' ', '0') + "00", out finishTime))
            {
                //Setting finish time:

                if (finishTime < DateTime.Now) finishTime = finishTime.AddDays(1);

                if (startTime == null && duration == null)
                {
                    startTime = finishTime;
                }

                if (startTime == null && duration != null)
                {
                    if (finishTime - duration < DateTime.Now) duration = finishTime - DateTime.Now;
                    startTime = finishTime - duration;
                }

                if (finishTime < startTime)
                {
                    finishTime = finishTime.AddDays(1);
                }
                if (startTime != null && duration == null)
                {
                    duration = finishTime - startTime;
                }
                
                if (startTime != null && duration != null)
                {
                    duration = finishTime - startTime;
                }

                

                hasSet = true;

                //Update labels/textboxes
                //Duration
                label18.Text = duration.ToString("hh\\:mm\\:ss\\.ff");
                //Maskboxes
                maskedTextBox1.Text = startTime.ToString("HH:mm:ss.ff");
                maskedTextBox3.Text = duration.ToString("hh\\:mm\\:ss\\.ff");
            }
            else
            {
                hasSet = false;

                //Bad input
                maskedTextBox2.Text = "00:00:00.00";
                SystemSounds.Beep.Play();
            }
        }

        private void maskedTextBox3_TextChanged(object sender, EventArgs e)
        {
            //Duration
            if (TimeSpan.TryParse(maskedTextBox3.Text.Replace(' ', '0') + "00", out duration))
            {
                //Setting duration:

                if (startTime != null && finishTime == null)
                {
                    finishTime = startTime + duration;
                }

                if (startTime == null && finishTime != null)
                {
                    if (finishTime - duration < DateTime.Now) duration = finishTime - DateTime.Now;
                    startTime = finishTime - duration;
                }

                if (startTime != null && finishTime != null)
                {
                    finishTime = startTime + duration;
                }

                hasSet = true;

                //Update labels/textboxes
                //Duration
                label18.Text = duration.ToString("hh\\:mm\\:ss\\.ff");
                //Maskboxes
                maskedTextBox1.Text = startTime.ToString("HH:mm:ss.ff");
                maskedTextBox2.Text = finishTime.ToString("HH:mm:ss.ff");
            }
            else
            {
                hasSet = false;

                //Bad input
                maskedTextBox3.Text = "00:00:00.00";
                SystemSounds.Beep.Play();
            }
        }

        private void label9_DoubleClick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            if (settingsBtn)
            {
                this.Size = new Size(386, 193);
                maskedTextBox1.Enabled = false;
                maskedTextBox2.Enabled = false;
                maskedTextBox3.Enabled = false;
                settingsBtn = false;
            }
            else
            {
                this.Size = new Size(386, 318);
                maskedTextBox1.Enabled = true;
                maskedTextBox2.Enabled = true;
                maskedTextBox3.Enabled = true;
                settingsBtn = true;
            }
        }

        private void label3_MouseHover(object sender, EventArgs e)
        {
            pictureBox2.Image = countdownBuddy.Properties.Resources.button_border_pressed;
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            pictureBox2.Image = countdownBuddy.Properties.Resources.button_border;
        }

        private void label16_MouseHover(object sender, EventArgs e)
        {
            pictureBox10.Image = countdownBuddy.Properties.Resources.button_border2_pressed;
        }

        private void label16_MouseLeave(object sender, EventArgs e)
        {
            pictureBox10.Image = countdownBuddy.Properties.Resources.button_border2;
        }

        private void label16_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Voice Plan Text File";
                dlg.Filter = "Text files (*.txt)|*.txt";

                if (dlg.ShowDialog() == DialogResult.OK)
                {

                    voicePlanLoadedName = dlg.SafeFileName;

                    voicePlanParse(dlg.FileName);
                }
            }
        }

        private void label22_MouseHover(object sender, EventArgs e)
        {
            pictureBox11.Image = countdownBuddy.Properties.Resources.button_border3_pressed;
        }

        private void label22_MouseLeave(object sender, EventArgs e)
        {
            pictureBox11.Image = countdownBuddy.Properties.Resources.button_border3;
        }

        private void label22_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Title = "Save Example Voice Plan Text File";
                dlg.Filter = "Text files (*.txt)|*.txt";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    //voicePlanLoadedName = dlg;
                    File.WriteAllText(dlg.FileName, "//-------------------------------------------------------\r\n//Example voice plan - Jameson Hartel - 10-02-2019\r\n//-------------------------------------------------------\r\n//This file is used to add voice events to your countdown\r\n//Comments can be made with // at the beginning of a line\r\n//\r\n//\r\n//The format for voice events is t-00:00:00.00,r0,hello world\r\n//All fields are seperated with a comma\r\n//\r\n//Time in countdown goes first\r\n//Leading with - sign indicates voiceEvent to play before event\r\n//\r\n//Time formats:\r\n//t-10:00:00 = 10 hours before event\r\n//t-10:00 = 10 minutes before event\r\n//t-10 = 10 seconds before event\r\n//t-00.10 = 10 microseconds before event\r\n//t20 = 20 seconds after event\r\n//t30:00 = 30 minutes after event\r\n//\r\n//An optional speech rate (within -10 to 10) can be specified\r\n//Speech rate controls how fast the message players\r\n//Leaving it blank will default it to zero\r\n//\r\n//The last field is the message you want spoken\r\n//Anything placed after the last comma will be spoken\r\n//\r\n//Voice plans only get executed once\r\n//To run a second time simply reload the voice plan\r\n//-------------------------------------------------------\r\n\r\nt-34,this is an example message\r\nt-31,by specifying a speech rate I can control how quickly messages are red\r\nt-26,r3,by specifying a speech rate of 3 I can say my messages faster\r\nt-23,r-3,or by specifying a slower speech rate of -3 I can say my messages slower\r\nt-16,by specifying no speech rate I will speak at the default rate of 0\r\nt-11,r3,t -10\r\nt-10,r3,9\r\nt-9,r3,8\r\nt-8,r3,7\r\nt-7,r3,6\r\nt-6,r3,5\r\nt-5,r3,4\r\nt-4,r3,3\r\nt-3,r3,2\r\nt-2,r3,1\r\nt-1,blast off\r\nt5,by typing in positive times I can have messages spoken after my event has occurred\r\nt10,10 seconds since takeoff\r\n");
                }
            }
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            //Close program
            Application.Exit();
        }

        private void pictureBox4_MouseHover(object sender, EventArgs e)
        {
            pictureBox4.Image = countdownBuddy.Properties.Resources.top_x_pressed;
        }

        private void pictureBox4_MouseLeave(object sender, EventArgs e)
        {
            pictureBox4.Image = countdownBuddy.Properties.Resources.top_x;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            //Minimise window
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox6_MouseHover(object sender, EventArgs e)
        {
            pictureBox6.Image = countdownBuddy.Properties.Resources.top___pressed;
        }

        private void pictureBox6_MouseLeave(object sender, EventArgs e)
        {
            pictureBox6.Image = countdownBuddy.Properties.Resources.top__;
        }

        private void voicePlanParse(string filePath)
        {
            //Start by resetting everything
            voicePlanErrorMsg = null;
            EventList.Clear();

            string[] voiceString = System.IO.File.ReadAllLines(filePath);

            foreach(string line in voiceString)
            {

                //Find position of first character in line
                if (!string.IsNullOrWhiteSpace(line))
                {
                    int i = line.TakeWhile(c => char.IsWhiteSpace(c)).Count();
                    if (line.Substring(i, 2) != "//")
                    {
                        string[] lineValues;
                        lineValues = line.Substring(i).Split(',');

                        //Are we on the first field?
                        bool firstValue = true;
                        //The timespan
                        TimeSpan? eventTime = null;
                        //Speech rate for event
                        int? eventRate = null;
                        //string to speak
                        string eventMsg = null;
                        voiceMsgNo = 0;

                        //Comparing stuff
                        bool? prevCountDown = null;
                        TimeSpan? prevEventTime = null;


                        foreach (string value in lineValues)
                        {
                            if (value.Length == 0)
                            {
                                voicePlanErrorMsg = "Error - Bad Comma";
                                break;
                            }
                            //Parse TimeSpan
                            if (firstValue)
                            {
                                string tmpTimeString = value.Substring(1);
                                int dividerCount = 0;

                                if (tmpTimeString.Substring(0, 1) == "-")
                                {
                                    tmpTimeString = value.Substring(2);
                                    if (tmpTimeString.Length > 0)
                                    {
                                        foreach (char c in tmpTimeString) if (c == ':') dividerCount++;

                                        if (dividerCount == 0) tmpTimeString = "00:00:" + tmpTimeString;
                                        else if (dividerCount < 2) tmpTimeString = "00:" + tmpTimeString;

                                        tmpTimeString = "-" + tmpTimeString;
                                    }
                                }
                                else
                                {
                                    if (tmpTimeString.Length > 0)
                                    {
                                        foreach (char c in tmpTimeString) if (c == ':') dividerCount++;

                                        if (dividerCount == 0) tmpTimeString = "00:00:" + tmpTimeString;
                                        else if (dividerCount < 2) tmpTimeString = "00:" + tmpTimeString;
                                    }
                                }
                                //Get timespan
                                TimeSpan tempEventTime;
                                if (TimeSpan.TryParse(tmpTimeString, CultureInfo.CurrentCulture, out tempEventTime))
                                {
                                    eventTime = tempEventTime;
                                }
                                else
                                {
                                    //Throw error
                                    voicePlanErrorMsg = "Error - Time Invalid";
                                    break;
                                }

                                TimeSpan eventTimeTmp = (TimeSpan)eventTime;
                                prevEventTime = eventTime;
                                firstValue = false;
                            }
                            else if ((value[0] == 'r' || value[0] == 'R') && (value[1] == '-' || value.Substring(1).All(char.IsNumber)) && char.IsNumber(value[value.Length - 1]) && value.Length < 5)
                            {
                                //Parse speech rate
                                int parseRate;
                                if (Int32.TryParse(value.Substring(1), out parseRate))
                                {
                                    if (parseRate >= -10 && parseRate <= 10)
                                    {
                                        eventRate = parseRate;
                                    }
                                    else
                                    {
                                        //Throw error
                                        voicePlanErrorMsg = "Error - Rate Bounds";
                                        break;
                                    }
                                }
                                else
                                {
                                    //Throw error
                                    voicePlanErrorMsg = "Error - Rate Invalid";
                                    break;
                                }
                            }
                            else
                            {
                                eventMsg = value;
                            }
                        }
                        if (eventTime != null && eventRate != null && eventMsg != null)
                        {
                            EventList.Add(new VoiceEvents() { MsgTime = (TimeSpan)eventTime, rate = (int)eventRate, msg = eventMsg });
                        } else if (eventTime != null && eventMsg != null)
                        {
                            EventList.Add(new VoiceEvents() { MsgTime = (TimeSpan)eventTime, msg = eventMsg });
                        }
                    }
                }

                //Set file label
                if (voicePlanErrorMsg != null)
                {
                    label15.Text = voicePlanErrorMsg;
                    break;
                } else
                {
                    label15.Text = filePath;
                    if (label15.Size.Width > 135)
                    {
                        label15.Text = filePath.Substring(Math.Max(0, filePath.Length - 19));
                        toolTip1.SetToolTip(label15, filePath);
                    }
                    else
                    {
                        label15.Text = filePath;
                    }
                }
            }
            TimeSpanComparer ts = new TimeSpanComparer();
            EventList.Sort(0,EventList.Count,ts);
        }

        private void voiceEventQueue()
        {

            //voiceMsgNo
            if (voiceMsgNo < EventList.Count)
            {
                if (EventList[voiceMsgNo].MsgTime <= DateTime.Now - startTime)
                {
                    if (EventList[voiceMsgNo].MsgTime > (DateTime.Now - startTime) - playPeriod) voiceSpeak(EventList[voiceMsgNo].rate, EventList[voiceMsgNo].msg);
                    voiceMsgNo++;
                }
            }
        }

        private void voiceSpeak (int rate, string msg)
        {
            synthesizer.SelectVoiceByHints(VoiceGender.Female);
            synthesizer.Volume = 100;  // 0...100
            synthesizer.Rate = rate;     // -10...10
            synthesizer.SpeakAsync(msg);
            //synthesizer.Speak(msg);
        }
    }
}
