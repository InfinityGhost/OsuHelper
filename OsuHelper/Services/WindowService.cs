﻿// ------------------------------------------------------------------ 
//  Solution: <OsuHelper>
//  Project: <OsuHelper>
//  File: <WindowService.cs>
//  Created By: Alexey Golub
//  Date: 22/08/2016
// ------------------------------------------------------------------ 

using System.Windows;
using System.Windows.Threading;
using NegativeLayer.WPFExtensions;

namespace OsuHelper.Services
{
    public class WindowService
    {
        private static Dispatcher Dispatcher => Dispatcher.CurrentDispatcher;

        public void ShowError(string message)
        {
            Dispatcher.InvokeSafe(
                () => MessageBox.Show(message, "osu!helper - Error", MessageBoxButton.OK, MessageBoxImage.Error));
        }
    }
}