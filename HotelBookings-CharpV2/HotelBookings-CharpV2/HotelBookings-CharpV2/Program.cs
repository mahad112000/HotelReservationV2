using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HotelBookings_CharpV2
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread[] threads = new Thread[10];
            //Call three threads for testing where each thread will call doreservation method which will deal all room reservations
            for (int i = 0; i < 4; i++)
            {
                Thread t = new Thread(new ThreadStart(DoReservations));
                threads[i] = t;
            }
            for (int i = 0; i < 4; i++)
            {
                threads[i].Start();
            }
            Console.ReadKey();
        }

        static void DoReservations()
        {
            int[] rooms = { 101, 102, 201, 203 }; //rooms to test
            IBookingManager bm = new BookingManager();// create your manager here;

            for (int i = 0; i < 4; i++)
            {

                var today = new DateTime(2022, 4, 3);
                try
                {
                    var randomRoom = rooms[i];
                    var available = bm.IsRoomAvailable(randomRoom, today);
                    Console.WriteLine($"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId} - {available}"); // outputs true
                    bm.AddBooking("Patel", randomRoom, today);
                    Console.WriteLine($"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId} - {bm.IsRoomAvailable(101, today)}"); // outputs false");
                    //try to book that is already booked to see the exception
                    bm.AddBooking("Li", randomRoom, today); // throws an exception
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Thread id : {Thread.CurrentThread.ManagedThreadId} - {e.Message}");
                }
                //part 2. this is outside the try block becasue part 1 throw exception for booking a room that is already booked.
                var availableRooms = bm.getAvailableRooms(today).ToList();
                var availableRoomsMessage = availableRooms.Any() ? availableRooms.Select(r => r.ToString()).Aggregate((x, y) => x + ',' + y) : "none";
                Console.WriteLine($"Thread id : {System.Threading.Thread.CurrentThread.ManagedThreadId} - Available rooms {today} are :{availableRoomsMessage}");
            }

        }
    }
}
