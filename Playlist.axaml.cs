using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Linq;
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

namespace player
{
    public partial class Playlist : Window
    {
        
        public Playlist()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
