using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFASeekerApp.Services
{
    public class ConnectivityService
    {
        public ConnectivityService()
        {
            Connectivity.ConnectivityChanged += OnConnectivityChanged;
            CheckInternetStatus();
        }

        private void OnConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            CheckInternetStatus();
        }

        private void CheckInternetStatus()
        {
            bool isConnected = Connectivity.NetworkAccess == NetworkAccess.Internet;

            // Отправляем сообщение во всё приложение
            WeakReferenceMessenger.Default.Send(new ConnectivityMessage(isConnected));
        }
    }

    // Сообщение для уведомлений о сети
    public record ConnectivityMessage(bool IsConnected);
}
