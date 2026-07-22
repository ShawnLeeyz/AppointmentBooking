using System;
using System.Collections.Generic;
using System.Text;

namespace AppointmentBooking
{
    public class AppointmentBookingService
    {
        public bool BookAppointment(AppointmentRequest request)
        {
            if(request.Doctor.AvailableSlots <= 0)
            {
                return false;
            }
            request.Doctor.AvailableSlots--;
            return true;
        }
    }
}
