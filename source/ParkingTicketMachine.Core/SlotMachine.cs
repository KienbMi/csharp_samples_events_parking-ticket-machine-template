using System;

namespace ParkingTicketMachine.Core
{
    public class SlotMachine
    {
        private const int MinPrice = 50;
        private const int MinParkingTime = 30;
        private const int MaxParkingTime = 90;

        public string Stationname { get; private set; }
        public int AmountPaid { get; private set; }
        public DateTime ValidUntil { get; private set; }


        public event EventHandler<Ticket> TicketPrinted;

        public SlotMachine(EventHandler<Ticket> ticketReady, string stationname)
        {
            Stationname = stationname;
            TicketPrinted += ticketReady;
        }

        public DateTime PrintTicket()
        {
            DateTime validUntil = DateTime.MinValue;
            
            if (AmountPaid >= MinPrice)
            {
                Ticket ticket = new Ticket() { Stationname = Stationname, AmountPaid = AmountPaid, ValidUntil = ValidUntil };
                TicketPrinted?.Invoke(this, ticket);
                validUntil = ValidUntil;
                Cancel();
            }
            return validUntil;
        }

        public string InsertCoin(int coinValue)
        {
            AmountPaid += coinValue;
            FastClock.Instance.IsRunning = false;
            string timeValid = string.Empty;

            if (AmountPaid >= MinPrice)
            {
                double minutes = Math.Min((double)AmountPaid / MinPrice * MinParkingTime, MaxParkingTime);
                ValidUntil = FastClock.Instance.Time.AddMinutes(minutes);
                timeValid = ValidUntil.ToShortTimeString();
            }
            return timeValid;
        }

        public void Cancel()
        {
            AmountPaid = 0;
            FastClock.Instance.IsRunning = true;
            ValidUntil = DateTime.MinValue;
        }
    }
}
