using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Odbc;
using System.Linq;
using System.Runtime.InteropServices;
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
using Microsoft.Win32;

namespace AlbumTager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private OpenFileDialog Ofd = new OpenFileDialog();
        private ObservableCollection<Song> Songs= new ObservableCollection<Song>();

        public MainWindow()
        {
            InitializeComponent();
            Tabulka.ItemsSource = Songs;
            Ofd.Multiselect = true;
            Ofd.Filter = "Hudobné súbory MP3|*.mp3";
        }

        private void AddFile_click(object sender, RoutedEventArgs e)
        {
            if (Ofd.ShowDialog() == true)
            {
                string[] files = Ofd.FileNames;
                foreach (string s in files)
                {
                    try
                    {
                        this.Songs.Add(new Song(s));
                    }
                    catch (TagLib.CorruptFileException )
                    {
                        
                        this.Stav.Text = "Neplatné súbory";
                    }
                    
                }
                
            }
        }

        private void RemoveFiles_Click(object sender, RoutedEventArgs e)
        {
                var subory = Tabulka.SelectedCells;

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var i = Tabulka.SelectedIndex;
            try
            {
                //this.Songs[i].Update();
                this.Stav.Text = "Súbor bol úspešne uložený";
                this.Stav.Foreground = Brushes.Green;
            }
            catch (System.IO.IOException)
            {
                this.Stav.Text = "Nie je možné uložiť súbor. Je používaný iným programom";
                this.Stav.Foreground = Brushes.Red;
            }
            catch (System.ArgumentOutOfRangeException)
            {
                this.Stav.Text = "Nie je zvolený súbor";
                this.Stav.Foreground = Brushes.Red;
            }
            catch (System.NullReferenceException)
            {
                this.Stav.Text = "Boli zadané neplatné hodnoty";
                this.Stav.Foreground = Brushes.Red;
            }
            
        }

        private void SaveAll_Click(object sender, RoutedEventArgs e)
        {
            if (this.Songs.Count == 0)
            {
                this.Stav.Text = "Zoznam je prázdny";
                this.Stav.Foreground = Brushes.Red;
            }
            else
            {
                foreach (var s in this.Songs)
                {
                    try
                    {
                        //s.Update();
                    }
                    catch (System.IO.IOException)
                    {
                        this.Stav.Text = "Nie je možné uložiť súbor. Je používaný iným programom";
                        this.Stav.Foreground = Brushes.Red;
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {
                        this.Stav.Text = "Nie je zvolený súbor";
                        this.Stav.Foreground = Brushes.Red;
                    }
                    catch (System.NullReferenceException)
                    {
                        this.Stav.Text = "Boli zadané neplatné hodnoty";
                        this.Stav.Foreground = Brushes.Red;
                    }
                    this.Stav.Text = "Súbory boli úspešne uložené";
                    this.Stav.Foreground = Brushes.Green;
                }
            }
           
        }

        private void RemoveTagSelected_Click(object sender, RoutedEventArgs e)
        {
            var i = Tabulka.SelectedIndex;
            try
            {
                this.Songs[i].DeleteTags();
            }
            catch (System.ArgumentOutOfRangeException)
            {
                this.Stav.Text = "Nie je zvolený súbor";
                this.Stav.Foreground = Brushes.Red;
            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selected = this.comboBox.SelectedIndex;

            switch (selected)
            {
                case 0:
                    this.Songs = new ObservableCollection<Song>(from song in this.Songs orderby song.Title select song);
                    Tabulka.ItemsSource = Songs;
                    break;
                case 1:
                    this.Songs = new ObservableCollection<Song>(from song in this.Songs orderby song.Title select song);
                    this.Songs.Reverse();
                    Tabulka.ItemsSource = Songs;
                    break;
                case 2:
                    this.Songs = new ObservableCollection<Song>(from song in this.Songs orderby song.Track select song);
                    Tabulka.ItemsSource = Songs;
                    break;
                case 3:
                    this.Songs = new ObservableCollection<Song>(from song in this.Songs orderby song.Track select song);
                    this.Songs.Reverse();
                    Tabulka.ItemsSource = Songs;
                    break;
                case -1:
                default:
                    break;
            }
        }

        private void NumberSongs_OnClick(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < this.Songs.Count; i++)
            {
                this.Songs[i].Track = (uint) (i + 1);
            }
        }
    }
}
