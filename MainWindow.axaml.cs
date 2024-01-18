using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Threading.Tasks;
using Avalonia.Controls.Primitives;
using System.Timers;
using System.Diagnostics;
using System.IO;
using TagLib;
using NetCoreAudio;

namespace player
{
    public partial class MainWindow : Window
    {
        string plik = "toothless.mp3";
        bool isPlaying = false;
        Player music = new Player();
        byte volume = Convert.ToByte(3);
        private int sekundy = 0;
        System.Timers.Timer timer = new Timer();

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            try
            {
                sekundy++;
                slider1.Value = sekundy;
            }
            catch (Exception ex)
            {

            }
        }

        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = 1000;
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
        }

        public async void btn_open_click(Object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();

            dialog.Title = "Otwórz plik mp3";
            string[] result = await dialog.ShowAsync(this);
            if (result != null)
            {
                mp3.Text = result[0];
                TagLib.File file = TagLib.File.Create(mp3.Text);
                string title2 = await GetMp3Title(result[0]);
                string author2 = await GetMp3Author(result[0]);
                string length = await GetMp3Length(result[0]);
                czas.Text = length;
                Tytul.Text = title2;
                wykonawca.Text = author2;
            }
        }

        public void btn1_click(Object sender, RoutedEventArgs e)
        {
            music.Stop().Wait();
            
        }

        public void btn2_click(Object sender, RoutedEventArgs e)
        {
            if (!music.Playing)
            {
                if (mp3.Text != null)
                {
                    music.Play(mp3.Text).Wait();
                    slider1.Value=0;
                    btn2.Content = "𝄃𝄃";
                    timer.Start();
                }
                else
                {
                    slider1.Value=0;
                    music.Play(plik).Wait();
                    btn2.Content = "𝄃𝄃";
                }
            }
            else
            {
                music.Stop().Wait();
                btn2.Content = "𝅘𝅥𝅰";
                timer.Stop();
                this.Title = sekundy.ToString();
            }
        }

        private async Task<string> GetMp3Title(string filePath)
        {
            try
            {
                var file = await Task.Run(() => TagLib.File.Create(filePath));
                if (file.Tag != null && !string.IsNullOrEmpty(file.Tag.Title))
                {
                    return file.Tag.Title;
                }
                else
                {
                    return "Nieznany tytuł";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd odczytu pliku MP3: {ex.Message}");
                return "Nieznany tytuł";
            }
        }

        private async Task<string> GetMp3Author(string filePath)
        {
            try
            {
                var file = await Task.Run(() => TagLib.File.Create(filePath));
                if (file.Tag != null && !string.IsNullOrEmpty(file.Tag.FirstPerformer))
                {
                    return file.Tag.FirstPerformer;
                }
                else
                {
                    return "Nieznany autor";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd odczytu pliku MP3: {ex.Message}");
                return "Nieznany autor";
            }
        }

        private async Task<string> GetMp3Length(string filePath)
        {
            try
            {
                var file = await Task.Run(() => TagLib.File.Create(filePath));
                if (file.Properties != null)
                {
                    return $"{file.Properties.Duration:mm\\:ss}";
                }
                else
                {
                    return "Nieznany czas trwania";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Błąd odczytu pliku MP3: {ex.Message}");
                return "Nieznany czas trwania";
            }
        }
    }
}
