using System;

namespace ParkingTicketMachine.Core
{
    public class SlotMachine
    {
        private readonly int MinPrice = 50;
        private readonly int MinParkingTime = 30;
        private readonly int MaxParkingTime = 90;
        private readonly DateTime StartTime = DateTime.Parse("08:00");
        private readonly DateTime EndTime = DateTime.Parse("18:00");

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

                if (FastClock.Instance.Time.TimeOfDay < StartTime.TimeOfDay)
                {
                    ValidUntil = FastClock.Instance.Time.Date.AddMinutes(minutes + StartTime.TimeOfDay.TotalMinutes);
                }
                else if (FastClock.Instance.Time.TimeOfDay > EndTime.TimeOfDay)
                {
                    ValidUntil = FastClock.Instance.Time.AddDays(1);
                    ValidUntil = ValidUntil.Date.AddMinutes(minutes + StartTime.TimeOfDay.TotalMinutes);
                }
                else
                {
                    ValidUntil = FastClock.Instance.Time.AddMinutes(minutes);
                    if (ValidUntil.TimeOfDay > EndTime.TimeOfDay)
                    {
                        TimeSpan timeSpan = ValidUntil.TimeOfDay - EndTime.TimeOfDay;
                        ValidUntil = FastClock.Instance.Time.AddDays(1);
                        ValidUntil = ValidUntil.Date.AddMinutes(timeSpan.TotalMinutes + StartTime.TimeOfDay.TotalMinutes);
                    }
                }

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
