
$("#datetimepicker4").datetimepicker({
    format: "DD/MM/YYYY",
    minDate: new Date(),
    maxDate: new Date(2025, 12),
    daysOfWeekDisabled: [0, 6]
});

$(document).ready(function () {
    var selectedValue = $("#facilityId").val();
    var appointmentId = $("#appointmentId").val();

    let names = GetActionAndController();
    const action = names[0], controller = names[1];

    if ((action == "Update" || action == "Create") && controller == "Appointment") {
        UpdateTime(true);
        updateAppointmentDropdowns(selectedValue, appointmentId, true);
    }
});

$("#datetimepicker4").on("change.datetimepicker", function () {
    let names = GetActionAndController();
    const action = names[0], controller = names[1];

    if ((action == "Update" || action == "Create") && controller == "Appointment") {
        UpdateTime(false);
    }
});

function UpdateTime(initialize) {
    var selectedDate = $("#datePicker").val();
    var currentDate = new Date();

    var day = currentDate.getDate();
    if (day <= 9)
        day = "0" + day.toString();

    var month = currentDate.getMonth() + 1;
    if (month <= 9)
        month = "0" + month.toString();

    var year = currentDate.getFullYear().toString();
    var currentDateStr = day + "/" + month + "/" + year;
    var currentHour = currentDate.getHours();
    var currentMinutes = currentDate.getMinutes();
    var time;

    if (currentMinutes <= 30) {
        if (currentHour <= 10)
            currentHour = "0" + currentHour.toString();
        time = currentHour + ":" + "30";
    }
    else {
        currentHour += 1;
        if (currentHour < 7)
            currentHour = 7;

        if (currentHour <= 10)
            currentHour = "0" + currentHour.toString();
        time = currentHour + ":" + "00";
    }

    var dateFormat = "DD/MM/YYYY HH:mm";
    var minDate = moment(currentDateStr + " " + time, dateFormat);

    if (selectedDate == currentDateStr && initialize) {
        $("#datetimepicker3").datetimepicker({
            format: "HH:mm",
            stepping: 30,
            enabledHours: [7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18],
            minDate: minDate
        });
    }
    else if (initialize && selectedDate != currentDate && selectedDate != null) {
        $("#datetimepicker3").datetimepicker({
            format: "HH:mm",
            stepping: 30,
            enabledHours: [7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18],
            minDate: moment(selectedDate + " " + "07:00", dateFormat)
        });
    }

    if (selectedDate == currentDateStr && !initialize)
        $("#datetimepicker3").datetimepicker("minDate", minDate);
    else if (!initialize)
        $("#datetimepicker3").datetimepicker("minDate", moment(selectedDate + " " + "07:00", dateFormat));
}

$("#facilityId").change(function () {
    var selectedValue = $(this).val();
    var appointmentId = $("#appointmentId").val();

    let names = GetActionAndController();
    const action = names[0], controller = names[1];

    if (action == "Update" && controller == "Appointment") {
        updateAppointmentDropdowns(selectedValue, appointmentId, false);
    }
});

function GetActionAndController() {
    var url = window.location.pathname;
    var indexes = getAllIndexes(url, "/");

    if (indexes.length == 3) {
        var controller = url.substring(indexes[0] + 1, indexes[1]);
        var action = url.substring(indexes[1] + 1, indexes[2]);

        return [action, controller];
    }
    else if (indexes.length == 2) {
        var controller = url.substring(indexes[0] + 1, indexes[1]);
        var action = url.substring(indexes[1] + 1);

        return [action, controller];
    }

    return ["", ""];
}

function getAllIndexes(str, val) {
    var indexes = [], i;
    for (i = 0; i < str.length; i++)
        if (str[i] === val)
            indexes.push(i);
    return indexes;
}

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
