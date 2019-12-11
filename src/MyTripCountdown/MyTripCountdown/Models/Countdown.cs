using System;
using Xamarin.Forms;

namespace MyTripCountdown.Models
{
    public class Countdown : BindableObject
    {
        TimeSpan _remainTime;

        public event Action Completed;
        public event Action Ticked;

        public DateTime EndDate { get; set; }
        public DateTime StartDate { get; set; }

        public double Progress 
        { 
            get 
            {
                var totalSeconds = (this.EndDate - this.StartDate).TotalSeconds;
                var remainSeconds = this.RemainTime.TotalSeconds;

                return remainSeconds / totalSeconds;
            }
        }

        public TimeSpan RemainTime
        {
            get { return _remainTime; }

            private set
            {
                _remainTime = value;
                OnPropertyChanged();
            }
        }

        public bool IsStopped { get; private set; }

        public void Start(int seconds = 1)
        {
            IsStopped = false;

            Device.StartTimer(TimeSpan.FromSeconds(seconds), () =>
            {
                RemainTime = (EndDate - DateTime.Now);

                var ticked = RemainTime.TotalSeconds > 1;

                if (ticked)
                {
                    Ticked?.Invoke();
                }
                else
                {
                    Completed?.Invoke();
                    IsStopped = true;
                }                

                return ticked; 
            });
        }
    }
}