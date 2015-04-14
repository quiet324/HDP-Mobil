using System;
using ReactiveUI;
using Hdp.CoreRx.Models;

namespace Hdp.CoreRx.ViewModels.Events
{
    public class EventItemViewModel : EventViewModel, ICanGoToViewModel
	{
        public IReactiveCommand<object> GoToCommand { get; protected set; }

        public EventItemViewModel ()
        {
            
        }

        public EventItemViewModel (Event model, Action<EventItemViewModel> gotoCommand) : base()
        {
            Model = model;

            Title = model.Title;
            EventTitle = model.Title;
            Time = model.Time;
            Location = model.Location;

            GoToCommand = ReactiveCommand.Create ();
            GoToCommand.Subscribe (x => gotoCommand (this));
        }
	}
}

