using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace TrustCenterSearch.Presentation

{

    public class SilentObservableCollection<T> : ObservableCollection<T>

    {

        private bool _suppressNotification = false;



        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)

        {

            if (!this._suppressNotification)

                base.OnCollectionChanged(e);

        }



        public void AddRange(IEnumerable<T> list)

        {

            if (list == null)

                throw new ArgumentNullException($"Empty Lists are not supported:({nameof(list)})");



            this._suppressNotification = true;



            foreach (var item in list)

            {

                this.Add(item);

            }



            this._suppressNotification = false;

            this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));

        }

    }

}