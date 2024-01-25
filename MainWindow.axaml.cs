using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using Avalonia.Threading;
using System.Threading.Tasks;
using Avalonia.Controls.Primitives;
using System.Timers;
using Avalonia.Media.Imaging;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using TagLib;
using NetCoreAudio;
using System.Drawing;

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

        private async void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (music.Playing)
                {
                    sekundy++;
                    await Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        slider1.Value++;
                    });
                }
            }
            catch (Exception ex)
            {
                // Obsługa błędu
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
                switch(title2)
                {
                    case "Driftveil City":
                    {
                        obrazek.Source=new Avalonia.Media.Imaging.Bitmap("Assets/tot.png");
                        break;
                    }
                    case "Friesenjung":
                    {
                        obrazek.Source=new Avalonia.Media.Imaging.Bitmap("Assets/junge.jpg");
                        break;
                    }
                    case "Power of the Saber Blade":
                    {
                        obrazek.Source=new Avalonia.Media.Imaging.Bitmap("Assets/dragon.jpg");
                        break;
                    }
                    default:
                    {
                        obrazek.Source=new Avalonia.Media.Imaging.Bitmap("Assets/1.jpg");
                        break;
                    }
                }
                string filePath = "ścieżka/do/pliku.mp3";
                byte[] coverData = await GetMp3Cover(result[0]);

                if (coverData != null)
                {
                    // Obsługa obrazu w tagu MP3
                }
                else
                {
                    // Brak obrazu w tagu MP3
                }

                czas.Text = length;
                Tytul.Text = title2;
                wykonawca.Text = author2;
            }
        }

        public void btn1_click(Object sender, RoutedEventArgs e)
        {
            music.Stop().Wait();
        }

        public async void btn2_click(Object sender, RoutedEventArgs e)
        {
            if (!music.Playing)
            {
                if (mp3.Text != null)
                {
                    music.Play(mp3.Text).Wait();
                    slider1.Value = 0;
                    btn2.Content = "𝄃𝄃";
                    timer.Start();
                }
                else
                {
                    slider1.Value = 0;
                    music.Play(plik).Wait();
                    btn2.Content = "𝄃𝄃";
                    this.Title = await GetMp3Title(mp3.Text);
                }
            }
            else
            {
                music.Stop().Wait();
                btn2.Content = "𝅘𝅥𝅰";
                timer.Stop();
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

        private async Task<byte[]> GetMp3Cover(string filePath)
        {
            try
            {
                var file = await Task.Run(() => TagLib.File.Create(filePath));
                if (file.Tag != null && file.Tag.Pictures.Length > 0)
                {
                    var picture = file.Tag.Pictures[0];
                    return picture.Data.Data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd odczytu pliku MP3: {ex.Message}");
                return null;
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
        // if(pizda)
        // {
        //     cout<<"cyce"<<endl;
        // }
        // else
        // {
        //     cout<<"janota ma cyce"<<endl;
        // }
    }
}
