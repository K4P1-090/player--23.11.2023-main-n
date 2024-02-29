using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Linq;
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
using Avalonia.Markup.Xaml;
using TagLib.Ape;
using Avalonia.Controls.Presenters;

namespace player
{
    public partial class Playlist : Window
    {
        
        public ListBox playlista = new ListBox();
        
        //this.FindControl<ListBox>("muzyka");
      
        private void InitializeComponent()
        {
            
            AvaloniaXamlLoader.Load(this);
        }
        public Playlist()
        {
            
            InitializeComponent();
        }
    }
}
