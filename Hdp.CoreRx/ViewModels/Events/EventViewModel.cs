using System;
using System.Reactive.Linq;
using ReactiveUI;
using Hdp.CoreRx.Models;

namespace Hdp.CoreRx.ViewModels.Events
{
    public class EventViewModel : BaseViewModel
	{
        private string eventTitle;
        public string EventTitle {
            get { return this.eventTitle; }
            set { this.RaiseAndSetIfChanged (ref this.eventTitle, value); }
        }

        private DateTime time;
        public DateTime Time {
            get { return this.time; }
            set { this.RaiseAndSetIfChanged (ref this.time, value); }
        }

        private string location;
        public string Location {
            get { return this.location; }
            set { this.RaiseAndSetIfChanged (ref this.location, value); }
        }

        private DateTime createdAt;
        public DateTime CreatedAt {
            get { return this.createdAt; }
            set { this.RaiseAndSetIfChanged (ref this.createdAt, value); }
        }

        readonly ObservableAsPropertyHelper<string> dateText;
        public string DateText {
            get { return dateText.Value; }
        }

        readonly ObservableAsPropertyHelper<string> timeText;
        public string TimeText {
            get { return timeText.Value; }
        }

        public Event Model { get; set; }

        public EventViewModel (Event model)
        {
            Model = model;

            Title = model.Title;
            EventTitle = model.Title;
            Location = model.Location;
            Time = model.Time;
            CreatedAt = model.CreatedAt;

            this.WhenAnyValue (x => x.Time)
                .Select (x => x.ToString ("dd MMMM yyyy"))
                .ToProperty (this, x => x.DateText, out dateText);

            this.WhenAnyValue (x => x.Time)
                .Select (x => x.ToString ("HH:mm"))
                .ToProperty (this, x => x.TimeText, out timeText);
        }
	}
}

