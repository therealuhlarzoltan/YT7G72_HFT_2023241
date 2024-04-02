using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.WpfClient.Logic
{
    public class RestCollection<T> : INotifyCollectionChanged, IEnumerable<T>
    {
        public event NotifyCollectionChangedEventHandler? CollectionChanged;

        private NotifyService notify;
        private RestService rest;
        private List<T> items;
        private bool hasSignalR;
        private Type type = typeof(T);
        private string controllerEndpoint;


        public RestCollection(string baseurl, string controllerEndpoint, string hub = null)
        {
            hasSignalR = hub != null;
            this.rest = new RestService(baseurl, controllerEndpoint);
            this.controllerEndpoint = controllerEndpoint;
            if (hub != null)
            {
                this.notify = new NotifyService(baseurl + hub);
                this.notify.AddHandler<T>(type.Name + "Created", (T item) =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        items.Add(item);
                        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                    });
                });
                this.notify.AddHandler<T>(type.Name + "Deleted", (T item) =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        var element = items.FirstOrDefault(t => t.Equals(item));
                        if (element != null)
                        {
                            items.Remove(item);
                            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                        }
                        else
                        {
                            Init();
                        }
                    });

                });
                this.notify.AddHandler<T>(type.Name + "Updated", (T item) =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Init();
                    });
                });

                this.notify.Init();
            }
            Init();
        }

        private async Task Init()
        {
            items = await rest.GetAsync<T>(controllerEndpoint);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (items != null)
            {
                return items.GetEnumerator();
            }
            else return new List<T>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            if (items != null)
            {
                return items.GetEnumerator();
            }
            else return new List<T>().GetEnumerator();
        }

        public async Task Add(T item)
        {
            if (hasSignalR)
            {
                await this.rest.PostAsync(item, controllerEndpoint);
            }
            else
            {
                await this.rest.PostAsync(item, typeof(T).Name).ContinueWith((t) =>
                {
                    Init().ContinueWith(z =>
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                        });
                    });
                });
            }

        }

        public async Task Update(T item)
        {
            if (hasSignalR)
            {
                await this.rest.PutAsync(item, controllerEndpoint);
            }
            else
            {
                await this.rest.PutAsync(item, typeof(T).Name).ContinueWith((t) =>
                {
                    Init().ContinueWith(z =>
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                        });
                    });
                });
            }
        }

        public void Delete(int id)
        {
            if (hasSignalR)
            {
                this.rest.DeleteAsync(id, controllerEndpoint);
            }
            else
            {
                this.rest.DeleteAsync(id, typeof(T).Name).ContinueWith((t) =>
                {
                    Init().ContinueWith(z =>
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                        });
                    });
                });
            }

        }

        public async Task AddDirectly(T item)
        {
            await this.rest.PostAsync(item, controllerEndpoint);
        }

        public async Task UpdateDirectly(T item)
        {
            await this.rest.PutAsync(item, controllerEndpoint);
        }

        public async Task TriggerReset()
        {
            items = await rest.GetAsync<T>(controllerEndpoint);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

    }
}