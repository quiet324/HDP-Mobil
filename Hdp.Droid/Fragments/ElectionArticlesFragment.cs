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


using Hdp.CoreRx.Models;
using Hdp.CoreRx.ViewModels.ElectionArticles;
using Hdp.Droid.DroidExtensions;
using Hdp.Droid.Activities;

using ReactiveUI;
using ReactiveUI.AndroidSupport;
using Google.YouTube.Player;
using Android.Graphics;
using Android.Support.V4.Widget;
using System.Globalization;
using Splat;
using Hdp.CoreRx.Services;

namespace Hdp.Droid.Fragments
{
    public class ElectionArticlesFragment : BaseLoadingFragment<ElectionArticlesViewModel>
    {
        ReactiveListAdapter<ElectionArticleItemViewModel> _articlesAdapter;

        ListView _listView;
        SwipeRefreshLayout _refreshLayout;

        public ElectionArticlesFragment ()
        {
            var serviceConstructor = Locator.Current.GetService<IServiceConstructor> ();
            ViewModel = serviceConstructor.Construct<ElectionArticlesViewModel> ();

            _articlesAdapter = new ReactiveListAdapter<ElectionArticleItemViewModel> (ViewModel.ArticleItems, CreateItemView, InitItemView);
        }

        View CreateItemView (ElectionArticleItemViewModel itemViewModel, ViewGroup viewGroup)
        {
            var inflater = LayoutInflater.From (viewGroup.Context);
            var view = inflater.Inflate (Resource.Layout.ElectionArticleItemLayout, null);
            var playIcon = view.FindViewById<ImageView> (Resource.Id.playIcon);
            playIcon.Visibility = ViewStates.Gone;

            return view;
        }

        void InitItemView (ElectionArticleItemViewModel itemViewModel, View view)
        {
            var articleImage = view.FindViewById<ImageView> (Resource.Id.articleImage);
            var articleDate = view.FindViewById<TextView> (Resource.Id.articleDate);
            var articleBody = view.FindViewById<TextView> (Resource.Id.articleBody);
            var articleTitle = view.FindViewById<TextView> (Resource.Id.articleTitle);

            var playIcon = view.FindViewById<ImageView> (Resource.Id.playIcon);

            articleDate.Text = itemViewModel.CreatedAt.ToString ("dd MMMM yyyy", new CultureInfo ("tr-TR"));

            articleBody.Text = itemViewModel.Body;
            articleBody.Visibility = itemViewModel.Body == "" ? ViewStates.Gone : ViewStates.Visible;

            articleTitle.Text = itemViewModel.Title;
            articleTitle.Visibility = itemViewModel.Title == "" ? ViewStates.Gone : ViewStates.Visible;

            if (itemViewModel.MediaType == ElectionArticle.MediaType.Image) {
                playIcon.Visibility = ViewStates.Gone;
                Koush.UrlImageViewHelper.SetUrlDrawable (articleImage, itemViewModel.ImageUrl);
            } else if (itemViewModel.MediaType == ElectionArticle.MediaType.Video) {
                playIcon.Visibility = ViewStates.Visible;
                Koush.UrlImageViewHelper.SetUrlDrawable (articleImage, itemViewModel.VideoImageUrl);
            } else {
                playIcon.Visibility = ViewStates.Gone;
                articleImage.Visibility = ViewStates.Gone;
            }

            var isEvenRow = itemViewModel.Index % 2 == 0;
            var backgroundColor = Color.ParseColor (isEvenRow ? "#FAFAFA" : "#FFFFFF");

            view.SetBackgroundColor (backgroundColor);
        }

        protected override View OnCreateContentView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            _refreshLayout = new SwipeRefreshLayout (container.Context);
            _refreshLayout.Refresh += (sender, e) => ViewModel.RefreshContent.Execute(null);

            ViewModel.RefreshContent.IsExecuting.BindTo (_refreshLayout, y => y.Refreshing);

            _listView = new ListView (container.Context);
            _listView.Adapter = _articlesAdapter;
            _listView.SetSelector (Resource.Color.transparent);
            _listView.SetBackgroundColor (Color.ParseColor ("#FFFFFF"));

            _listView.ItemClick += (object sender, AdapterView.ItemClickEventArgs e) => {
                var itemViewModel = ViewModel.ArticleItems[e.Position];

                if (itemViewModel.MediaType == ElectionArticle.MediaType.Video) 
                {
                    var youtubeActivity = new Intent(container.Context, typeof(YoutubePlayerActivitiy));
                    youtubeActivity.PutExtra("VIDEO_ID", itemViewModel.VideoId);
                    StartActivity(youtubeActivity);   
                }
            };

            _refreshLayout.AddView (_listView);

            return _refreshLayout;
        }
    }
}

