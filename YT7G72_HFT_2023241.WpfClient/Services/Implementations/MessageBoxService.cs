using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using YT7G72_HFT_2023241.WpfClient.Services.Interfaces;

namespace YT7G72_HFT_2023241.WpfClient.Services.Implementations
{
    internal class MessageBoxService : IMessageBoxService
    {
        public void ShowInfo(string message)
        {
            MessageBox.Show(message, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowInfo(string message, string header)
        {
            MessageBox.Show(message, header, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowWarning(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
