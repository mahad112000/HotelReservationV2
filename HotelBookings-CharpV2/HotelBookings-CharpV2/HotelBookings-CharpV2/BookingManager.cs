using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookings_CharpV2
{
    public class BookingManager : IBookingManager
    {
        //initialize all rooms
        private int[] rooms = { 101, 102, 201, 203 };

        private Dictionary<int, List<BookingDetail>> reservations = new Dictionary<int, List<BookingDetail>>();
        private Object thisLock = new Object();


        public void AddBooking(string guest, int room, DateTime date)
        {
            lock (thisLock)
            {
                if (!rooms.Contains(room))
                    throw new ArgumentException($"Invalid room.");
                var booking = new BookingDetail { RoomId = room, Date = date, GuestSurName = guest };
                if (!reservations.ContainsKey(room))
                {
                    reservations.Add(room, new List<BookingDetail> { booking });
                }
                else
                {
                    var bookings = reservations[room];
                    foreach (var bk in bookings)
                    {
                        if (bk.Date.Date == date.Date)
                            throw new ArgumentException($"Room {room} is not available on {date.ToString()}");
                    }
                    reservations[room].Add(booking);
                }
            }
        }

        //part 2 - return available rooms for specific date
        public IEnumerable<int> getAvailableRooms(DateTime date)
        {
            lock (thisLock)
            {
                var availableRooms = rooms.Where(r => reservations.All(p => p.Key != r)).Select(r => r).ToList();

                //Get the reserved rooms where reserved date not equal to the needed date. 
                foreach (var booked in reservations)
                {
                    /*just compare the booked date with the interested. No need to check who is booked because we just need to know 
                    whether the room is available for this date regardless the guest. */
                    availableRooms.AddRange(booked.Value.Where(r => r.Date != date).Select(r => r.RoomId).ToList());
                }
                return availableRooms;
            }
        }

        //part 1 - return true/false depending whether room is available for the given date
        public bool IsRoomAvailable(int room, DateTime date)
        {
            lock (thisLock)
            {
                //check whether room id is valid first
                if (!rooms.Contains(room))
                    throw new ArgumentException($"Invalid room.");
                if (!reservations.ContainsKey(room))
                {
                    return true; //dictionary does contain this key so return true as it can be booked
                }
                //get bookedDate for this room
                var bookings = reservations[room];
                foreach (var bk in bookings)
                {
                    if (bk.Date.Date == date.Date)
                        return false; //booking exists for this date return false
                }
                return true; //Nothing matched return true
            }
        }
    }
}
