using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AlbumTager
{
    class Song
    {
        private string artist_;
        private string title_;
        private uint year_;
        private uint track_;
        private string album_;
        private string genre_;
        private string filePath_;
        private string status_;
        private string comment_;
        public event PropertyChangedEventHandler PropertyChanged;

        //public Song(string path)
        //{
        //    try
        //    {
        //        this._song = TagLib.File.Create(path);
        //    }
        //    catch (TagLib.CorruptFileException ex)
        //    {

        //       throw ex;
        //    }

        //    this.artist_ = this._song.Tag.FirstAlbumArtist;
        //    this.title_ = this._song.Tag.Title;
        //    this._year = this._song.Tag.Year;
        //    this.track_ = this._song.Tag.Track;
        //    this.album_ = this._song.Tag.Album;
        //    this.genre_ = this._song.Tag.FirstGenre;
        //    this.filePath_ = this._song.Name;
        //    this.status_ = "Neulozene";
        //}

        public Song(string path)
        {
            byte[] mp3binary = File.ReadAllBytes(path);                                             // precitanie suboru po bytoch a nasledne ulozenie do pola bytov
            int length = mp3binary.Length - 125;                                                    // inicializacia startovnej pozicie pre hladanie ID3v1 tagov

            byte[] title_temp = mp3binary.Skip(length).Take(30).ToArray();                          // posunutie sa v subore o 'length' bytov a nasledne precitanie 30 nasledujucich bytov a vytvorenie temp pola
            title_temp = title_temp.Where(val => val != '\0').ToArray();                            // vymazanie \0 v temp poli
            this.title_ = Encoding.ASCII.GetString(title_temp);                                     // prevod pola do string a encodovanie z ASCII

            byte[] artist_temp = mp3binary.Skip(length + 30).Take(30).ToArray();
            artist_temp = artist_temp.Where(val => val != '\0').ToArray();
            this.artist_ = Encoding.ASCII.GetString(artist_temp);

            byte[] album_temp = mp3binary.Skip(length + 60).Take(30).ToArray();
            album_temp = album_temp.Where(val => val != '\0').ToArray();
            this.album_ = Encoding.ASCII.GetString(album_temp);

            byte[] year_temp = mp3binary.Skip(length + 90).Take(4).ToArray();
            year_temp = year_temp.Where(val => val != '\0').ToArray();
            this.year_ = Convert.ToUInt32(Encoding.ASCII.GetString(year_temp));

            byte[] comment_temp = mp3binary.Skip(length + 94).Take(30).ToArray();
            comment_temp = comment_temp.Where(val => val != '\0').ToArray();
            this.comment_ = Encoding.ASCII.GetString(comment_temp);

            //byte[] track_temp = mp3binary.Skip(length + 125).Take(1).ToArray();
            //track_temp = track_temp.Where(val => val != '\0').ToArray();
            //this.track_ = Convert.ToUInt32(Encoding.ASCII.GetString(track_temp));

            byte[] genre_temp = mp3binary.Skip(length + 126).Take(1).ToArray();
            genre_temp = genre_temp.Where(val => val != '\0').ToArray();
            this.genre_ = Encoding.ASCII.GetString(genre_temp);

        }

        public string Title
        {
            get { return title_; }
            set
            {
                title_ = value;
                OnPropertyChanged("Názov piesne");
            }
        }

        public string Artist
        {
            get { return artist_; }
            set
            {
                artist_ = value;
                OnPropertyChanged("Umelec");
            }
        }

        public string Album
        {
            get { return album_; }
            set
            {
                album_ = value;
                OnPropertyChanged("Album");
            }
        }


        public uint Track
        {
            get { return track_; }
            set
            {
                track_ = value;
                OnPropertyChanged("#");
            }
        }

        public uint Year
        {
            get { return year_; }
            set
            {
                year_ = value;
                OnPropertyChanged("Rok");
            }
        }

        public string Genre
        {
            get { return genre_; }
            set
            {
                genre_ = value;
                OnPropertyChanged("Žáner");
            }
        }
        public string Path
        {
            get
            { return filePath_; }
            private set
            {
                filePath_ = value;
                OnPropertyChanged("Cesta k súboru");
            }
        }
        public string Status
        {
            get { return status_; }
            set
            {
                status_ = value;
                OnPropertyChanged("Stav");
            }
        }

        public void DeleteTags()
        {
            this.Artist = null;
            this.Track = 0;
            this.Year = 0;
            this.Album = null;
            this.Title = null;
            this.Genre = null;
        }

        //public void Update()
        //{

        //    try
        //    {
        //        this._song.Tag.Album = this.Album;
        //        this._song.Tag.Track = this.Track;
        //        this._song.Tag.Year = this.Year;
        //        this._song.Tag.Title = this.Title;
        //        this._song.Tag.AlbumArtists = null;
        //        this._song.Tag.AlbumArtists = new[] { this.Artist };
        //        this._song.Tag.Genres = null;
        //        this._song.Tag.Genres = new[] { this.Genre };
        //        this.Status = "Ulozene";
        //        this._song.Save();
        //    }
        //    catch (System.IO.IOException ex)
        //    {
        //        throw ex;
        //    }
        //    catch (System.NullReferenceException ex)
        //    {
        //        throw ex;
        //    }

        //}
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}