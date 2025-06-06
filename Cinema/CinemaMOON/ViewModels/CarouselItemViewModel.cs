using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CinemaMOON.Models;

namespace CinemaMOON.ViewModels
{
    public class CarouselItemViewModel : INotifyPropertyChanged
    {
        private double _scaleFactor = 1.0;
        private int _zIndex = 0;
        private Movie _movie;

        public Uri ImageUri { get; }
        public Movie Movie => _movie;

        public CarouselItemViewModel(Movie movie)
        {
            _movie = movie ?? throw new ArgumentNullException(nameof(movie));
            ImageUri = new Uri(movie.Photo, UriKind.RelativeOrAbsolute);
        }

        public double ScaleFactor
        {
            get => _scaleFactor;
            set
            {
                if (Math.Abs(_scaleFactor - value) > 0.001)
                {
                    _scaleFactor = value;
                    OnPropertyChanged();
                }
            }
        }

        public int ZIndex
        {
            get => _zIndex;
            set
            {
                if (_zIndex != value)
                {
                    _zIndex = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}