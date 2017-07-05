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
using System.Globalization;

namespace PrincessLevels
{
    public class PrincessClass
    {
        public string FilePath { get; set; }  = "";
        public string Folder { get; set; } = "";
        public int width { get; set; } = 0;
        public int height { get; set; } = 0;
        public int size { get; internal set; }
        public uint[] pixels { get; internal set; }
        public bool processfailed = false;
    }
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public PrincessClass Pri;
        public MainWindow()
        {
            InitializeComponent();
            Pri = new PrincessClass();
            ListBoxCode.Items.Add("'Generated source code here");
        }

        private void SetFileToProcess(String PathAndFile)
        {
            Pri.FilePath = PathAndFile;
            Filename.Text = PathAndFile;
        }
        private void ButtonSelect_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog OpenFile = new Microsoft.Win32.OpenFileDialog();
            OpenFile.Multiselect = false;
            OpenFile.Title = "Open picture file...";
            //OpenFile.AddExtension = true;
            //OpenFile.Filter = "Any File|(*.*)";
            bool? result = OpenFile.ShowDialog();
            if (result != true) return;
            SetFileToProcess(OpenFile.FileName);            
        }

        //Checks if File is selected, also gathers Path
        private bool CheckIfFileSelected(bool IsAfile)
        {
            if (Pri.FilePath == "")
            {
                MessageBox.Show("Please select a file.", System.AppDomain.CurrentDomain.FriendlyName, MessageBoxButton.OK, MessageBoxImage.Error);
                Pri.processfailed = true;
                return false;
            }
            //check if file/folder exists
            if (IsAfile)
            {
                if (!File.Exists(Pri.FilePath))
                {
                    MessageBox.Show($"File not found!\n[{Pri.FilePath}]", System.AppDomain.CurrentDomain.FriendlyName, MessageBoxButton.OK, MessageBoxImage.Error);
                    Pri.processfailed = true;
                    return false;
                }
            } else
            {
                Pri.Folder = Directory.GetParent(Pri.FilePath).FullName;    //doesn't include \
                if (!Directory.Exists(Pri.Folder))
                {
                    MessageBox.Show($"Path not found!\n[{Pri.FilePath}]", System.AppDomain.CurrentDomain.FriendlyName, MessageBoxButton.OK, MessageBoxImage.Error);
                    Pri.processfailed = true;
                }                
            }
            return true;
        }
        private void ButtonLoadImage_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckIfFileSelected(true)) return;
            System.Uri u = new Uri(Pri.FilePath);         
            try 
            {
                pic.Source = BitmapFrame.Create(u);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening file [{Pri.FilePath}]\n{ex.InnerException.ToString()}", System.AppDomain.CurrentDomain.FriendlyName, MessageBoxButton.OK, MessageBoxImage.Error);
                Pri.processfailed = true;
            }          
        }

        // MarcoRLE
        // 00=item, 6-bit blank run (0-31). 
        // 0=Place baddy

        private void ButtonGenerate_Click(object sender, RoutedEventArgs e)
        {
            if ((pic.Source.Width != 20) || (pic.Source.Height !=12 ))
            {
                MessageBox.Show($"Image size must be 20x12\n{Pri.FilePath}");
                return;
            }
            Pri.width = (int)Math.Round(pic.Source.Width);
            Pri.height = (int)Math.Round(pic.Source.Height);
            Pri.size = Pri.width * Pri.height;

            Pri.pixels = new UInt32[Pri.size];
            BitmapSource bs = (BitmapSource)pic.Source;
            if (bs.Format != PixelFormats.Bgra32)   //force 32-bit RGB
            {
                bs = new FormatConvertedBitmap(bs, PixelFormats.Bgra32, null, 0);
            }
            int nstride = bs.PixelWidth * ((bs.Format.BitsPerPixel + 7) / 8);
            bs.CopyPixels(Pri.pixels,nstride,0);

            List<int> data = new List<int>();
            
            int run = -1, bit1 = -1, bit2 = -1;
            UInt32 pix1;
            int i = 0;
            do
            {
                //get pixel
                pix1 = Pri.pixels[i++];
                if (pix1 == 0xFFFFFFFF)
                {   
                    bit2 = 3;   //white      
                }
                else if (pix1 == 0xFFFF0000)
                {
                    bit2 = -1;  //red=place object,  no run. 
                }
                else if (pix1 == 0xFF00FF00)
                {
                    bit2 = 2;   //greeen
                }
                else if (pix1 == 0xFF0000FF)
                {
                    bit2 = 1;   //blue
                }
                else if (pix1 == 0xFF000000)
                {
                    bit2 = 0;   //black
                }
                if ((bit2 != bit1) || (run == 30) || (run== -1))    //I'll avoid runs of 31
                {   //End of run
                    if (run == -1)
                    { //start of run?
                        if (bit1==-1)
                        { //yes, initializing
                            bit1 = bit2;
                            run = 0;
                        } else
                        { //no, it's to place object, place, reset run. (I can't place objects adjacent)
                            data.Add(0);
                            bit1 = bit2; run = 0;
                        }
                    }
                    else
                    { //end of run, definitely
                        data.Add(bit1 | ((++run) * 8));
                        bit1 = bit2; run = 0;
                    }
                } else 
                {
                     run++; //keep running
                }
                //place object? no data, no run
                if (bit1==-1)
                {
                    run = -1; bit1 = 0;
                }
                
            } while (i < Pri.size);
            data.Add(bit1 + ((++run) * 8));  //last one 

            //===== done parsing, convert to data ===============
            ListBoxCode.Items.Add($"'Level: [{Pri.FilePath}] {DateTime.Now.ToString()}");
            StringBuilder dataline = new StringBuilder();
            dataline.Append("DATA ");
            
            if (Check8.IsChecked==true)
            {   //byte version: 
                foreach (int idx in data)
                {
                    dataline.Append($"${idx:x2}, ");
                }
                dataline.AppendLine("$FF");
            }
            else
            {   //word version 
                data.Add(0xFF);
                if ((data.Count & 1) == 1) data.Add(0xFF); //make it even
                int xword=0; bool hi = false;
                foreach (int idx in data)
                {
                    if (!hi)
                    {
                        xword = idx;
                        hi = !hi;
                    } else
                    {
                        xword += idx * 256;
                        dataline.Append($"${xword:x4}, ");
                        hi = !hi;
                    }                    
                }                
            }
            if (dataline.Length>0) dataline.Length--;  //remove last comma ,
            ListBoxCode.Items.Add(dataline.ToString());
        } //function

        private void ButtonCopy_Click(object sender, RoutedEventArgs e)
        { //
            //Clipboard.SetText(ListBoxCode.Items.ToString());
            StringBuilder s = new StringBuilder();
            foreach (String x in ListBoxCode.Items )
            {
                s.AppendLine(x);
            }
            Clipboard.SetText(s.ToString());
        }

        private void ButtonAll_Click(object sender, RoutedEventArgs e)
        {
            ListBoxCode.Items.Clear();
            Pri.processfailed = false;
            if (!CheckIfFileSelected(false)) return;

            //get all .ico and .png files
            string[] dir = Directory.GetFiles(Pri.Folder, "*.*");

            //only certain extensions, let's also sort to make troubleshooting easier
            var dir2 = from files in dir where
                            files.EndsWith(".png", true, CultureInfo.InvariantCulture) &&
                            files.EndsWith(".ico", true, CultureInfo.InvariantCulture) &&
                            files.EndsWith(".gif", true, CultureInfo.InvariantCulture) &&
                            files.EndsWith(".png", true, CultureInfo.InvariantCulture) &&
                            files.EndsWith(".bmp", true, CultureInfo.InvariantCulture)
                       orderby files 
                       select files;

            //keep backup of path
            string MyPath = Pri.Folder;
            int howmany = 0;

            //Process each one..
            foreach (string file in dir2)
            {                
                SetFileToProcess(MyPath + System.IO.Path.PathSeparator + file);
                ButtonLoadImage_Click(sender, e); if (Pri.processfailed) return;
                ButtonGenerate_Click(sender, e);  if (Pri.processfailed) return;
                howmany++;
            }
            MessageBox.Show($"Done! Processed {howmany} file(s).");
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            ListBoxCode.Items.Clear();
        }
    }
}
