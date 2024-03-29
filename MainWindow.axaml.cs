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
using MySql.Data.MySqlClient;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace player
{
    public partial class MainWindow : Window
    {
       
        

        Playlist win = new Playlist();
        string plik = "toothless.mp3";
        bool isPlaying = false;
        Player music = new Player();
        byte volume = Convert.ToByte(3);
        private int sekundy = 0;
        System.Timers.Timer timer = new Timer();

         private string GetFilePathFromDatabase(string songTitle)
        {
            DBConnector dbConnector = DBConnector.Instance();
            dbConnector.Server = "10.0.2.3";
            dbConnector.DatabaseName = "playlista";
            dbConnector.UserName = "kpawlowski";
            dbConnector.Password = "K4P1_090";

            if (dbConnector.IsConnect())
            {
                string query = "SELECT path FROM playlist WHERE title = @title";
                MySqlCommand command = new MySqlCommand(query, dbConnector.Connection);
                command.Parameters.AddWithValue("@title", songTitle);
                object result = command.ExecuteScalar();
                dbConnector.Close();

                if (result != null)
                {
                    return result.ToString();
                }
                else
                {
                    Console.WriteLine("File path not found for the selected song.");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Failed to connect to the database.");
                return null;
            }
        }

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
            win.Show();
        }

        public async void btn_open_click(Object sender, RoutedEventArgs e)
{
    var dialog = new OpenFileDialog();
    
    dialog.Title = "Otwórz plik mp3";
    string[] result = await dialog.ShowAsync(this);
    if (result != null)
    {
        if (result != null && result.Length > 0 && result[0] != null)
        {
            await LoadMp3Metadata(result[0]);
            //win.muzyka.Items.Add(result[0]);
            win.Add(System.IO.Path.GetFileName(result[0]));
        }
        mp3.Text = result[0];
        TagLib.File file = TagLib.File.Create(mp3.Text);
        string title2 = await GetMp3Title(result[0]);
        string author2 = await GetMp3Author(result[0]);
        string length = await GetMp3Length(result[0]);
        //win.Add(title2);
       // win.Add(mp3.Text);
        string filePath = result[0];
        //Add2Database(title2, filePath);
        
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
        }
         mp3.Text = result[0];
            // if (result != null)
            // {
            //     try{
            //         var db = new DBConnector();
            //         db.Server = "10.0.2.3";
            //         db.DatabaseName = "dwierzbicki";
            //         db.UserName = "dwierzbicki";
            //         db.Password = "Jui!#der7692@";
            //         if(db.IsConnect()){
            //             this.Title = "Połączono";
            //             if(db.InsertValues("songs",result[0],author2,title2)){
            //                 this.Title = "Wykonano Polecenie";
            //                 playlist.LoadSongsFromDatabase();
            //             }
            //             else{
            //                 this.Title = "Nie wykonano polecenia";
            //             }
            //         }
            //     }
            //     catch(Exception ex){
            //         Debug.WriteLine("Error: "+ ex.Message);
            //     }                
            // }
        
        
       // byte[] coverData = await GetMp3Cover(result[0]);

        // if (coverData != null)
        // {
        //     // Obsługa obrazu w tagu MP3
        // }
        // else
        // {
        //     // Brak obrazu w tagu MP3
        // }

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
                    music.Play(win.playlista.SelectedItem.ToString()).Wait();
                    slider1.Value = 0;
                    btn2.Content = "𝄃𝄃";
                    timer.Start();
                    string title2 = await GetMp3Title(win.playlista.SelectedItem.ToString());
        string author2 = await GetMp3Author(win.playlista.SelectedItem.ToString());
        string length = await GetMp3Length(win.playlista.SelectedItem.ToString());
        
        
        
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
        }
        czas.Text = length;
        Tytul.Text = title2;
        wykonawca.Text = author2;
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
        private async Task LoadMp3Metadata(string filePath)
{
    try
    {
        TagLib.File file = await Task.Run(() => TagLib.File.Create(filePath));
        if (file != null)
        {
            string title = string.IsNullOrEmpty(file.Tag.Title) ? "Unknown Title" : file.Tag.Title;
            string artist = string.IsNullOrEmpty(file.Tag.FirstPerformer) ? "Unknown Artist" : file.Tag.FirstPerformer;
            string duration = file.Properties.Duration.ToString(@"mm\:ss");
            // Update UI with metadata
            Tytul.Text = title;
            wykonawca.Text = artist;
            czas.Text = duration;
            // Load cover image if available
            byte[] coverData = file.Tag.Pictures.Length > 0 ? file.Tag.Pictures[0].Data.Data : null;
            // Display cover image in UI
        }
    }
    catch (Exception ex)
    {
        Debug.WriteLine($"Error reading MP3 file: {ex.Message}");
    }
}

         string server="localhost";
        string user="root";
        string pw="";
        string db="playlista";
        // private void Add2Database(string tyt, string sciezka)
        // {
        //     string connectionString = $"Server={server};Database={db};Uid={user};Pwd={pw};";
        //     using (MySqlConnection connection = new MySqlConnection(connectionString))
        //     {
        //         connection.Open();

        //         string query = "insert into lista(Tytul,Sciezka) values("+tyt+","+sciezka+")";
        //         MySqlCommand command = new MySqlCommand(query, connection);
        //         using (MySqlDataReader reader = command.ExecuteReader())
        //         {
        //             while (reader.Read())
        //             {
        //                 string data = reader.GetString(0);
        //                 win.Add(data);
        //             }
        //         }
        //     }
        // }
    }
}
