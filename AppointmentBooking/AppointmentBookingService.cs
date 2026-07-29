using System;
using System.Collections.Generic;
using System.Text;

namespace AppointmentBooking
{
    public class AppointmentBookingService
    {
        public BookingResult BookAppointment(AppointmentRequest request)
        {
            if (request == null)
                return new BookingResult(false, "Appointment request is missing.");
            if (!request.Doctor.HasAvailableSlot())
            {
                return new BookingResult(
                false,
                $"Appointment cannot be booked because {request.Doctor.FullName} has no available slots.");
            }

            //Check if the patient is able to book an appointment with the doctor on the right day (cannot be the current date)
            if(request.Doctor.HasAvailableSlot() && Doctor.) {

            request.Doctor.ReserveSlot();
            return new BookingResult(
            true,
            $"Appointment booked successfully for {request.Patient.DisplayName} with {request.
            Doctor.FullName}.");

        }


    }


}
