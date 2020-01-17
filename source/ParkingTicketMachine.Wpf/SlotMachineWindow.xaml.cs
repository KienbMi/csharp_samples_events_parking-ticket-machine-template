using System;
using System.Windows;
using ParkingTicketMachine.Core;

namespace ParkingTicketMachine.Wpf
{
    /// <summary>
    /// Interaction logic for SlotMachineWindow.xaml
    /// </summary>
    public partial class SlotMachineWindow
    {
        private SlotMachine _slotMachine;

        public SlotMachineWindow(string stationname, EventHandler<Ticket> ticketReady)
        {
            InitializeComponent();
            Title = stationname;

            _slotMachine = new SlotMachine(ticketReady, stationname);
            InitSlotMachine();
        }

        private void ButtonInsertCoin_Click(object sender, RoutedEventArgs e)
        {
            int[] coinValues = { 10, 20, 50, 100, 200 };
            if (ListBoxCoins.SelectedIndex < 0 || ListBoxCoins.SelectedIndex >= coinValues.Length)
            {
                MessageBox.Show("Bitte wählen Sie eine Münze aus!");
                return;
            }
            _slotMachine.InsertCoin(coinValues[ListBoxCoins.SelectedIndex]);
            
            if (_slotMachine.ValidUntil > FastClock.Instance.Time)
            {
                TextBoxTimeUntil.Text = _slotMachine.ValidUntil.ToShortTimeString();
                ButtonPrintTicket.IsEnabled = true;
            }
        }

        private void ButtonPrintTicket_Click(object sender, RoutedEventArgs e)
        {
            DateTime validUntil = _slotMachine.PrintTicket();
            if (validUntil > FastClock.Instance.Time)
            {
                MessageBox.Show($"Sie dürfen bis {validUntil} parken");
            }
            InitSlotMachine();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            _slotMachine.PrintTicket();
            InitSlotMachine();
        }

        private void InitSlotMachine()
        {
            TextBoxTimeUntil.Text = string.Empty;
            ButtonPrintTicket.IsEnabled = false;
        }

    }
}
