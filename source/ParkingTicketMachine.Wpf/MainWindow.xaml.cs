﻿using ParkingTicketMachine.Core;
using System;
using System.Text;
using System.Windows;

namespace ParkingTicketMachine.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, EventArgs e)
        {
            FastClock.Instance.Factor = 360;
            FastClock.Instance.IsRunning = true;
            FastClock.Instance.OneMinuteIsOver += OnOneMinuteIsOver;

            SlotMachineWindow slotMachineWindow1 = new SlotMachineWindow("LIMESSTRASSE", OnTicketReady) {Owner = this };
            slotMachineWindow1.Show();

            SlotMachineWindow slotMachineWindow2 = new SlotMachineWindow("LANDSTRASSE", OnTicketReady) {Owner = this };
            slotMachineWindow2.Show();
        }

        private void ButtonNew_Click(object sender, RoutedEventArgs e)
        {
            SlotMachineWindow slotMachineWindow = new SlotMachineWindow(TextBoxAddress.Text, OnTicketReady) { Owner = this };
            slotMachineWindow.Show();
        }

        private void OnTicketReady(object source, Ticket ticket)
        {
            string line = $"{ticket.Stationname}: {ticket.ValidUntil} {ticket.AmountPaid} Cent";
            AddLineToTextBox(line);
        }

        private void AddLineToTextBox(string line)
        {
            StringBuilder text = new StringBuilder(TextBlockLog.Text);
            text.Append(FastClock.Instance.Time.ToShortTimeString() + "\t");
            text.Append(line + "\n");
            TextBlockLog.Text = text.ToString();
        }

        private void OnOneMinuteIsOver(object source, DateTime actualTime)
        {
            this.Title = $"PARKSCHEINZENTRALE {actualTime.ToShortTimeString()}";
        }

    }
}
