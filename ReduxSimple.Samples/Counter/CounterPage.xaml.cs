﻿using ReduxSimple.Samples.Extensions;
using System;
using System.Reactive.Linq;
using Windows.UI.Xaml.Controls;

namespace ReduxSimple.Samples.Counter
{
    public sealed partial class CounterPage : Page
    {
        private static CounterStore _store = new CounterStore();

        public CounterPage()
        {
            InitializeComponent();

            // Observe changes on state
            _store.ObserveState()
                .Subscribe(state =>
                {
                    CounterValueTextBlock.Text = state.Count.ToString();
                });

            // Observe UI events
            IncrementButton.ObserveOnClick()
                .Subscribe(_ => _store.Dispatch(new IncrementAction()));

            DecrementButton.ObserveOnClick()
                .Subscribe(_ => _store.Dispatch(new DecrementAction()));

            // Initialize UI
            CounterValueTextBlock.Text = _store.State.Count.ToString();

            // Initialize Components
            HistoryComponent.Store = _store;
        }
    }
}
