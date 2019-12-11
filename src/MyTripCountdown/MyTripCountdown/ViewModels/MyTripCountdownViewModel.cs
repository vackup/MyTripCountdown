using MyTripCountdown.Models;
using MyTripCountdown.ViewModels.Base;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyTripCountdown.ViewModels
{
    public class MyTripCountdownViewModel : BaseViewModel
    {
        private Trip _trip;
        private Countdown _countdown;
        private int _seconds;
        private int _days;
        private int _hours;
        private int _minutes;
        private double _progress;

        public MyTripCountdownViewModel()
        {
            _countdown = new Countdown();
        }

        public Trip MyTrip
        {
            get => _trip;
            set => SetProperty(ref _trip, value);
        }

        public int Seconds
        {
            get => _seconds;
            set => SetProperty(ref _seconds, value);
        }

        public int Days
        {
            get => _days;
            set => SetProperty(ref _days, value);
        }

        public int Hours
        {
            get => _hours;
            set => SetProperty(ref _hours, value);
        }

        public int Minutes
        {
            get => _minutes;
            set => SetProperty(ref _minutes, value);
        }

        public double Progress
        {
            get => _progress;
            set => SetProperty(ref _progress, value);
        }

        public ICommand RestartCommand => new Command(Restart);

        public override Task LoadAsync()
        {
            LoadTrip();

            SetCountDown(MyTrip);            
            
            _countdown.Start();

            _countdown.Ticked += OnCountdownTicked;
            _countdown.Completed += OnCountdownCompleted;

            return base.LoadAsync();
        }

        private void SetCountDown(Trip myTrip)
        {
            _countdown.StartDate = MyTrip.Creation;
            _countdown.EndDate = MyTrip.Date;
        }

        public override Task UnloadAsync()
        {
            _countdown.Ticked -= OnCountdownTicked;
            _countdown.Completed -= OnCountdownCompleted;

            return base.UnloadAsync();
        }

        void OnCountdownTicked()
        {
            Seconds = _countdown.RemainTime.Seconds;
            Days = _countdown.RemainTime.Days;
            Hours = _countdown.RemainTime.Hours;
            Minutes = _countdown.RemainTime.Minutes;

            Progress = _countdown.Progress;
        }

        void OnCountdownCompleted()
        {
            Seconds = 0;
            Days = 0;
            Hours = 0;
            Minutes = 0;

            Progress = 0;
        }

        void LoadTrip()
        {
            var trip = new Trip()
            {
                Picture = "trip",
                Date = DateTime.Now.AddSeconds(15),
                Creation = DateTime.Now,                
            };

            MyTrip = trip;
        }

        void Restart()
        {
            LoadTrip();
            SetCountDown(MyTrip);

            if (_countdown.IsStopped)
            {
                _countdown.Start();
            }
        }
    }
}