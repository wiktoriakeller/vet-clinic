
$("#datetimepicker4").datetimepicker({
    format: "DD/MM/YYYY",
    minDate: new Date(),
    maxDate: new Date(2025, 12),
    daysOfWeekDisabled: [0, 6]
});

$("#datetimepicker3").datetimepicker({
    format: "HH:mm",
    stepping: 30,
    enabledHours: [7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18]
});

$(document).ready(function () {
    var selectedValue = $("#facilityId").val();
    var appointmentId = $("#appointmentId").val();
    updateAppointmentDropdowns(selectedValue, appointmentId, true);
    updateDateAndTime(appointmentId);
});

$("#facilityId").change(function () {
    var selectedValue = $(this).val();
    var appointmentId = $("#appointmentId").val();
    updateAppointmentDropdowns(selectedValue, appointmentId, false);
});

function updateAppointmentDropdowns(selectedValue, appointmentId, setSelected) {
    $.ajax({
        type: 'POST',
        dataType: 'JSON',
        url: '/Appointment/GetOfficesAndVeterinarians',
        data: { facilityId: selectedValue, appointmentId: appointmentId },
        success:
            function (response) {
                var officesMarkup = "<option value='0'>-- office --</option>";
                for (var i = 0; i < response.offices.length; i++) {
                    officesMarkup += "<option value=" + response.offices[i].officeId
                    if (response.selectedOffice == response.offices[i].officeId && setSelected)
                        officesMarkup += " selected";

                    officesMarkup += ">" + response.offices[i].officeNumber + "</option>";
                }

                var vetMarkup = "<option value='0'>-- veterinarian --</option>";
                for (var i = 0; i < response.veterinarians.length; i++) {
                    vetMarkup += "<option value=" + response.veterinarians[i].employeeId;
                    if (response.selectedVet == response.veterinarians[i].employeeId && setSelected)
                        vetMarkup += " selected";

                    vetMarkup += ">" + response.veterinarians[i].fullname + "</option>";
                }

                $("#OfficesFormDropdown").html(officesMarkup);
                $("#VeterinariansFormDropdown").html(vetMarkup);
            }
    });
}

function updateDateAndTime(appointmentId) {
    $.ajax({
        type: 'POST',
        dataType: 'JSON',
        url: '/Appointment/GetAppointmentDayAndTime',
        data: { appointmentId: appointmentId },
        success:
            function (response) {
                $("#Time").val(response.time);
            }
    });
}