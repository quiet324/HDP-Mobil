﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using Android.Support.V4.Widget;

using ReactiveUI;
using ReactiveUI.AndroidSupport;
using Hdp.CoreRx.ViewModels.Events;
using Splat;
using Hdp.CoreRx.Services;

namespace Hdp.Droid.Fragments
{
    public class EventsFragment : BaseLoadingFragment<EventsViewModel>
    {
        ReactiveListAdapter<EventItemViewModel> _eventsAdapter;

        ListView _listView;
        SwipeRefreshLayout _refreshLayout;

        public EventsFragment ()
        {
            var serviceConstructor = Locator.Current.GetService<IServiceConstructor> ();
            ViewModel = serviceConstructor.Construct<EventsViewModel> ();

            _eventsAdapter = new ReactiveListAdapter<EventItemViewModel> (ViewModel.EventItems, CreateItemView, InitItemView);
        }

        public View CreateItemView (EventItemViewModel itemViewModel, ViewGroup viewGroup)
        {
            var inflator = LayoutInflater.From (viewGroup.Context);
            return inflator.Inflate (Resource.Layout.EventItemLayout, null);
        }

        public void InitItemView (EventItemViewModel itemViewModel, View view)
        {
            var date = view.FindViewById<TextView> (Resource.Id.eventDate);
            var title = view.FindViewById<TextView> (Resource.Id.eventTitle);
            var time = view.FindViewById<TextView> (Resource.Id.eventTime);
            var location = view.FindViewById<TextView> (Resource.Id.eventLocation);

            date.Text = itemViewModel.DateText;
            title.Text = itemViewModel.Title;
            time.Text = itemViewModel.TimeText;
            location.Text = itemViewModel.Location;
        }

        protected override View OnCreateContentView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = inflater.Inflate (Resource.Layout.EventsLayout, null);
            view.SetBackgroundColor (Color.ParseColor ("#FFFFFF"));

            _refreshLayout = view.FindViewById<SwipeRefreshLayout> (Resource.Id.swipeContainer);
            _refreshLayout.Refresh += (sender, e) => ViewModel.RefreshContent.Execute(null);
            ViewModel.RefreshContent.IsExecuting.BindTo (_refreshLayout, x => x.Refreshing);

            _listView = view.FindViewById<ListView> (Resource.Id.eventsList);
            _listView.Adapter = _eventsAdapter;
            _listView.Divider = null;
            _listView.DividerHeight = 0;

            return view;
        }
    }
}

