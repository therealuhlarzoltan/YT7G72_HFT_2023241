using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using YT7G72_HFT_2023241.WpfClient.Logic;

namespace YT7G72_HFT_2023241.WpfClient.ViewModels
{
    public class BestStudentsWindowViewModel : ObservableRecipient, IDisposable
    {
        private RestService restService;

        public BestStudentsWindowViewModel(RestService restService)
        {
            this.restService = restService;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}