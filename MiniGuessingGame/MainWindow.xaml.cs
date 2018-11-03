using System;
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
using System.IO;
using System.Windows.Threading;

namespace MiniGuessingGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int a;
        List<string> image_src = new List<string> { };
        Image[] img = new Image[2];
        DispatcherTimer timer_in = new DispatcherTimer(), timer_out = new DispatcherTimer();
        const int defaultsize = 250;
        const int newsize = 300;      
        int zoom_in_id, zoom_out_id;
        int score = 0, current_question = 1;
        public MainWindow()
        {
            InitializeComponent();
            string[] filepath = Directory.GetFiles(System.AppDomain.CurrentDomain.BaseDirectory + "resources\\", "*.png", SearchOption.AllDirectories);
            foreach (string filename in filepath)
            {
                image_src.Add(filename);
            }
            timer_in.Tick += new EventHandler(timer_in_Tick);
            timer_in.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer_out.Tick += new EventHandler(timer_out_Tick);
            timer_out.Interval = new TimeSpan(0, 0, 0, 0, 10);
            img[0] = imgChoice1;
            img[1] = imgChoice2;
            LoadNewQuestion();
        }

        private void btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            score = 0;
            current_question = 1;
            LoadNewQuestion();
        }
        private void LoadNewQuestion()
        {
            Random rnd = new Random();
            int i1 = rnd.Next(0, image_src.Count);
            int i2 = i1;
            while (i2 == i1)
                i2 = rnd.Next(0, image_src.Count);

            img[0].Source = new BitmapImage(new Uri((string)image_src[i1]));
            img[1].Source = new BitmapImage(new Uri((string)image_src[i2]));
            //imgChoice1.Source = new BitmapImage(new Uri("resources\\Kamex.PNG", UriKind.Relative));

            Random rndKey = new Random();
            if (rndKey.Next(1, 3) == 1)
                lblName.Content = System.IO.Path.GetFileNameWithoutExtension(image_src[i1]);
            else
                lblName.Content = System.IO.Path.GetFileNameWithoutExtension(image_src[i2]);
            UpdateResult();
        }
        private void UpdateResult()
        {
            lblResult.Content = "Score: " + score + "/" + current_question;
        }
        private void imgChoice1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectAnswer(1);
        }

        private void imgChoice2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectAnswer(2);
        }

        private void SelectAnswer(int n)
        {
            string key = System.IO.Path.GetFileNameWithoutExtension(img[n-1].Source.ToString());
            if (key == lblName.Content.ToString())
            {
                score++;
                //btnNext.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));                
            }
            else
                MessageBox.Show("Incorrect! " + lblName.Content + " doesn't look like that!", "1612602 - Mini Guessing Game - Pokédex",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            if (current_question==10)
            {
                MessageBox.Show("Complete! You scored " + score + "/" + current_question + Environment.NewLine
                    + "Choose OK to restart.", "1612602 - Mini Guessing Game - Pokédex",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                score = 0;
                current_question = 1;
                LoadNewQuestion();
                return;
            }
            current_question++;
            LoadNewQuestion();
        }

        private void imgChoice1_MouseEnter(object sender, MouseEventArgs e)
        {
            zoom_in_id = 0;
            timer_in.Start();
        }
        private void imgChoice2_MouseEnter(object sender, MouseEventArgs e)
        {
            zoom_in_id = 1;
            timer_in.Start();
        }
        private void imgChoice1_MouseLeave(object sender, MouseEventArgs e)
        {
            zoom_out_id = 0;
            timer_out.Start();
            timer_in.Stop();
        }
        private void imgChoice2_MouseLeave(object sender, MouseEventArgs e)
        {
            zoom_out_id = 1;
            timer_out.Start();
            timer_in.Stop();
        }
        private void timer_in_Tick(object sender, EventArgs e)
        {
            CommandManager.InvalidateRequerySuggested();
            img[zoom_in_id].Width = img[zoom_in_id].Height += 5;
            if (img[zoom_in_id].Width >= newsize)
            {
                timer_in.Stop();
            }
        }
        private void timer_out_Tick(object sender, EventArgs e)
        {
            CommandManager.InvalidateRequerySuggested();          
            img[zoom_out_id].Width = img[zoom_out_id].Height -= 5;
            if (img[zoom_out_id].Width <= defaultsize)
            {
                timer_out.Stop();
            }
        }
    }
}
