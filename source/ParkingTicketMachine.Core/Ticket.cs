using System;

namespace ParkingTicketMachine.Core
{
    public class Ticket
    {
        public string Stationname { get; set; }
        public DateTime ValidUntil { get; set; }
        public int AmountPaid { get; set; }
    }
}
