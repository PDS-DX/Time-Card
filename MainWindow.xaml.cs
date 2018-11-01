using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TimeClock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer Timer = new System.Windows.Threading.DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            Timer.Tick += new EventHandler(Timer_Click);
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Start();
            Time.Text = DateTime.Now.ToShortTimeString();
        }

        bool isClockedIn = false;
        bool onBreak = false;
        DateTime start;
        DateTime stop;
        DateTime breakStart;
        DateTime breakStop;
        TimeSpan elapsed = new TimeSpan(0);
        List<string> lines = new List<string>();

        private void Clock_In_Click(object sender, RoutedEventArgs e)
        {
            if(isClockedIn)
            {
                if(onBreak)
                {
                    onBreak = false;
                    breakStop = DateTime.Now;
                    elapsed += breakStop.Subtract(breakStart);
                    lines.Add("\nEnd Break: " + breakStop.ToShortTimeString() + " : " + Note.Text);
                    Log.Text += lines.Last();
                    Break.Content = "Begin Break";
                }
                isClockedIn = false;
                Break.IsEnabled = false;
                TimeEnd.Text = DateTime.Now.ToShortTimeString();
                stop = DateTime.Now;
                ClockIn.Content = "Clock In";
                lines.Add("\nClocked Out: " + DateTime.Now + " : " + Note.Text);
                Log.Text += lines.Last();
                Note.Text = "";
                lines.Add("\nElapsed Time: " + (stop.Subtract(start) - elapsed) + "\n");
                Log.Text += lines.Last();
                Log.ScrollToEnd();
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                File.WriteAllLines(@path + "\\Time Cards\\" + start.Year + "_" + start.Month + "_" + start.Day + "_" + start.Hour + "_" + start.Minute + "_" + start.Second + ".txt", lines);
            } else {
                isClockedIn = true;
                Break.IsEnabled = true;
                TimeStart.Text = DateTime.Now.ToShortTimeString();
                start = DateTime.Now;
                ClockIn.Content = "Clock Out";
                lines.Add("Clocked In: " + DateTime.Now + " : " + Note.Text);
                Log.Text += lines.Last();
                Log.ScrollToEnd();
                Note.Text = "";
            }
        }

        private void Timer_Click(object sender, EventArgs e)
        {
            Time.Text = DateTime.Now.ToShortTimeString();
        }

        private void Break_Click(object sender, RoutedEventArgs e)
        {
            if(onBreak)
            {
                onBreak = false;
                Break.Content = "Begin Break";
                breakStop = DateTime.Now;
                elapsed += breakStop.Subtract(breakStart);
                lines.Add("\nEnd Break: " + breakStop.ToShortTimeString() + " : " + Note.Text);
                Log.Text += lines.Last();
                Note.Text = "";
            } else {
                onBreak = true;
                Break.Content = "End Break";
                breakStart = DateTime.Now;
                lines.Add("\nBegin Break: " + breakStart.ToShortTimeString() + " : " + Note.Text);
                Log.Text += lines.Last();
                Note.Text = "";
            }
        }
    }
}
