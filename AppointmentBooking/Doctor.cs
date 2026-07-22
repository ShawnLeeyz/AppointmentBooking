using System;
using System.Collections.Generic;
using System.Text;

/* Reliability: Preventing invalid slot counts. This helps with that because it helps with making sure that the avaliable slots cannot be a negative. 
 * This helps with making sure that the booking system ensuring that the doctor appoinment numbers are up to date and cannot be overbooked.
 * 
 * Maintainability: keeps slot rules inside Doctor. Keeping it in Doctor class allows future developers to easily change the rules for management of appoinments 
 * as the available slots are relevant towards the doctor class.
 * 
 * Encapsulation: Removing the set from the attributes Id, FullName, and AvailableSlots helps prevent external classes from being able to modify the doctor's information.
 * 
 * Testability: By adding the ArumentException to check for invalid inputs that the doctor class could have. The doctor attributes must contain valid information therefore this would prevent 
 * any doctor objects from being created with invalid information. And the arugments would let the developer know which attributes is invalid.
 */

namespace AppointmentBooking
{
    public class Doctor
    {
        public string Id { get; }

        public string FullName { get; }

        public int AvailableSlots { get; private set; }

        public Doctor(string id, string fullName, int availableSlots)
        {
            if (string.IsNullOrWhiteSpace(id)) //Check that id is not null or empty
            {
                throw new ArgumentException("Doctor ID is required.");
            }

            if (string.IsNullOrWhiteSpace(fullName)) //Check that fullName is not null or empty
            {
                throw new ArgumentException("Doctor name is required.");
            }

            if (availableSlots < 0) //Check that availableSlots is not negative
            {
                throw new ArgumentException("Available slots cannot be negative.");
            }

            Id = id;
            FullName = fullName;
            AvailableSlots = availableSlots;
        }

        // Method to check if the doctor has available appointment slots
        public bool HasAvailableSlot()
        {
            return AvailableSlots > 0;
        }

        // Method to reserve an appointment slot for the doctor
        public void ReserveSlot()
        {
            if (!HasAvailableSlot())
            {
                throw new InvalidOperationException("No appointment slots are available");
            }

            AvailableSlots--;
        }
    }
}
