
namespace AppointmentBooking.Tests;

[TestClass]
public class AppointmentBookingServiceTests
{
    [TestMethod]
    public void BookAppointment_WhenDoctorHasAvailableSlots_ReturnsSuccess()
    {
        var doctor = new Doctor("D001", "Dr Mark", 2);
        var patient = new Patient("P001", "Diana William");
        var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
        var service = new AppointmentBookingService();
        BookingResult result = service.BookAppointment(request);
        Assert.IsTrue(result.Success);
    }
    [TestMethod]
    public void BookAppointment_WhenDoctorHasNoAvailableSlots_ReturnsFailure()
    {
        var doctor = new Doctor("D001", "Dr Mark", 0);
        var patient = new Patient("P001", "Diana William");
        var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
        var service = new AppointmentBookingService();
        BookingResult result = service.BookAppointment(request);
        Assert.IsFalse(result.Success);
    }
    [TestMethod]
    public void BookAppointment_WhenSuccessful_DecreasesAvailableSlots()
    {
        var doctor = new Doctor("D001", "Dr Mark", 2);
        var patient = new Patient("P001", "Diana William");
        var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
        var service = new AppointmentBookingService();
        service.BookAppointment(request);
        Assert.AreEqual(1, doctor.AvailableSlots);
    }
    [TestMethod]
    public void BookAppointment_WhenFailed_DoesNotDecreaseAvailableSlots()
    {
        var doctor = new Doctor("D001", "Dr Mark", 0);
        var patient = new Patient("P001", "Diana William");
        var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
        var service = new AppointmentBookingService();
        service.BookAppointment(request);
        Assert.AreEqual(0, doctor.AvailableSlots);
    }
    [TestMethod]
    public void Doctor_WhenIdIsEmpty_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
        new Doctor("", "Dr Mark", 2));
    }
    [TestMethod]
    public void Doctor_WhenAvailableSlotsIsNegative_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
        new Doctor("D001", "Dr Mark", -1));
    }
    [TestMethod]
    public void Patient_WhenIdIsEmpty_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() =>
        new Patient("", "Diana William"));
    }
    [TestMethod]
    public void Patient_WhenPreferredNameExists_DisplayNameUsesPreferredName()
    {
        var patient = new Patient("P001", "Diana William", "Aroha");
        Assert.AreEqual("Aroha", patient.DisplayName);
    }
    [TestMethod]
    public void Patient_WhenPreferredNameMissing_DisplayNameUsesLegalName()
    {
        var patient = new Patient("P001", "Diana William");
        Assert.AreEqual("Diana William", patient.DisplayName);
    }
    [TestMethod]
    public void AppointmentRequest_WhenRequestedDateIsInPast_ThrowsException()
    {
        var doctor = new Doctor("D001", "Dr Mark", 2);
        var patient = new Patient("P001", "Diana William");
        Assert.Throws<ArgumentException>(() =>
        new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(-1)));
    }
    [TestMethod]
    public void BookAppointment_WhenSuccessful_ReturnsHelpfulMessage()
    {
        var doctor = new Doctor("D001", "Dr Mark", 2);
        var patient = new Patient("P001", "Diana William", "Aroha");    
        var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
        var service = new AppointmentBookingService();
        BookingResult result = service.BookAppointment(request);
        StringAssert.Contains(result.Message, "Appointment booked successfully");
        StringAssert.Contains(result.Message, "Aroha");
    }
    [TestMethod]
    public void BookAppointment_WhenNoSlots_ReturnsHelpfulMessage()
    {
        var doctor = new Doctor("D001", "Dr Mark", 0);
        var patient = new Patient("P001", "Diana William");
        var request = new AppointmentRequest(patient, doctor, DateTime.Today.AddDays(1));
        var service = new AppointmentBookingService();
        BookingResult result = service.BookAppointment(request);
        StringAssert.Contains(result.Message, "no available slots");
    }
}

/* 
 * Below are focused MSTest test-case suggestions (names, short descriptions, setup, expected outcome, and rationale) you can add to improve reliability, usability, maintainability, accessibility, cultural coverage, and testability.
Reliability / correctness
•	Test_RejectsOverlappingAppointment
•	Setup: create an appointment A then attempt to create appointment B with overlapping times for the same resource.
•	Expect: second create fails with OverlapException or validation error.
•	Why: prevents double-booking.
•	Test_AllowsBackToBackAppointments
•	Setup: create appointment ending at T, create another starting exactly at T.
•	Expect: allowed.
•	Why: verifies boundary inclusivity/exclusivity.
•	Test_RejectsAppointmentsExceedingMaxDuration
•	Setup: attempt to create appointment longer than configured maximum.
•	Expect: validation error.
•	Why: enforces business rules.
•	Test_HandlesLeapDayAndFeb29
•	Setup: schedule appointment on Feb 29 in leap year and attempt in non-leap year.
•	Expect: succeeds in leap year, fails/normalized in non-leap year.
•	Why: date-edge-case coverage.
Time zones and daylight savings
•	Test_BookingAcrossDaylightSavingsTransitions
•	Setup: schedule appointment that starts before DST forward/backward shift and ends after.
•	Expect: correct persisted start/end in UTC and accurate duration.
•	Why: avoids time-shift bugs.
•	Test_TimeZoneNormalizationOnCreateAndRetrieve
•	Setup: create in timezone A, retrieve in timezone B.
•	Expect: correct conversion and same instants.
•	Why: ensures correct timezone handling.
Concurrency and idempotency
•	Test_ConcurrentCreateSameSlot_OnlyOneSucceeds
•	Setup: simulate concurrent requests (threads/tasks) creating appointment for same slot.
•	Expect: only one success; others receive conflict response.
•	Why: prevents race conditions.
•	Test_CreateIsIdempotentWhenRequestIdSame
•	Setup: send same request id twice.
•	Expect: second call returns existing appointment (no duplicate).
•	Why: supports retry-safe clients.
Persistence and recovery
•	Test_PersistenceSurvivesRepositoryRestart
•	Setup: create appointment, reinstantiate repository/service, read appointment.
•	Expect: appointment still present.
•	Why: verifies durable storage integration.
Usability / validations / friendly errors
•	Test_ReturnsUserFriendlyValidationMessages
•	Setup: submit invalid input (e.g., empty required field).
•	Expect: validation response contains clear, localized message keys and fields.
•	Why: improves UX and localization.
•	Test_MaxLengthAndSanitizationForTextFields
•	Setup: create appointment with overly long title and malicious characters.
•	Expect: truncation or rejection and proper escaping when returned.
•	Why: prevents UI breakage and XSS.
Accessibility (automated + contract)
•	Accessibility_ApiProvidesAccessibleLabelsForUi
•	Setup: verify API returns label metadata or resource keys for appointment fields (if applicable).
•	Expect: metadata present so UI can surface accessible labels.
•	Why: helps screen readers and consistent UI accessibility.
•	E2E_A11y_CheckWithAxeOnBookingPage
•	Setup: add automated UI test running Axe or similar to the booking page.
•	Expect: zero critical a11y violations.
•	Why: continuous accessibility assurance.
Cultural and internationalization
•	DataTestMethod_CultureSpecificDateParsing
•	Setup: run creation in several cultures (en-US, fr-FR, ar-SA, zh-CN) using DataRow.
•	Expect: dates parsed according to culture or API enforces ISO-8601 input and rejects ambiguous formats.
•	Why: prevents localization parsing errors.
•	Test_UnicodeAndRightToLeftNames
•	Setup: create appointments with names containing diacritics and Arabic/Hebrew text.
•	Expect: stored and returned without corruption; sorting/search works.
•	Why: supports global users and RTL scripts.
•	Test_FirstDayOfWeekAndBusinessHoursVaryByCulture
•	Setup: set business hours that depend on culture (e.g., Sunday vs Monday start); attempt bookings outside hours.
•	Expect: validated against culture-specific business rules.
•	Why: respects local conventions.
Maintainability / test structure
•	Test_PublicApiContractBackwardCompatibility
•	Setup: assert API schema/DTO contains required fields (use snapshot or contract tests).
•	Expect: test fails when removing fields inadvertently.
•	Why: prevents breaking clients.
•	Test_UsesInjectedClock_ForDeterministicTime
•	Setup: inject mock IClock/time provider and assert time-dependent behavior (e.g., reminders) uses it.
•	Expect: deterministic outputs based on mock clock.
•	Why: makes tests deterministic and maintainable.
Testability / patterns
•	Add DataTestMethods and TestDataBuilders
•	Use DataTestMethod + DataRow for multiple scenarios; implement builders to create appointments with defaults, overriding only relevant fields.
•	Why: reduces duplication and makes new test cases easy to add.
Implementation notes / suggestions
•	Use dependency injection for time, mailer, repository to enable mocks.
•	Prefer DataTestMethod for cultural and input-variation tests.
•	Add test helpers: AppointmentBuilder, FakeRepository, MockClock, ConcurrencyTestHelper.
•	Add explicit assertions for error codes/messages and for persisted UTC instants (not only string equality).
•	Where possible, prefer integration tests for persistence/timezone/DST and unit tests for validation/business rules.
•	Add automated accessibility checks to CI (axe-core for browser-based UI tests).
If you want, I can:
•	Provide concrete MSTest method stubs for a selected subset (e.g., DST, concurrency, cultures).
•	Or add data-driven test skeletons and test helpers (AppointmentBuilder, MockClock) into your test project. Which would you prefer?

 */