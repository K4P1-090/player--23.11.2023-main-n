using System;
using Avalonia.Controls;
using Avalonia.Dialogs;
using Avalonia.Input;
using Avalonia.Interactivity;
using NetCoreAudio;

namespace player;

public partial class MainWindow : Window
{
    string plik="toothless.mp3";
    bool isPlaying=false;
    Player music = new Player();
    byte volume = Convert.ToByte(3);
    public MainWindow()
    {
        InitializeComponent();
    }
    public async void btn_open_click(Object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog();

        dialog.Title="Otwórz plik mp3";
        string[] result = await dialog.ShowAsync(this);
        if(result!=null)
        {
            mp3.Text=result[0];
        }
    }
    public void btn1_click(Object sender, RoutedEventArgs e)
    {
        
    }
    public void btn2_click(Object sender, RoutedEventArgs e)
    {
        
        if(!music.Playing)
        {
           // music.SetVolume(volume).Wait();
           if(mp3.Text!=null)
           {
            music.Play(mp3.Text).Wait();
            slider1.Value=0;
            btn2.Content="𝄃𝄃";
           }
           else{
            music.Play(plik).Wait();
            slider1.Value=0;
            btn2.Content="𝄃𝄃";
           }
            
        }
           else{
            music.Stop().Wait();
            btn2.Content="𝅘𝅥𝅰";
        }
        

   
    }
    
    
}