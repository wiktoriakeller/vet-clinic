
$("#datetimepicker4").datetimepicker({
    format: "DD/MM/YYYY",
    minDate: moment().millisecond(0).second(0).minute(0).hour(0),
    maxDate: new Date(2025, 12),
    daysOfWeekDisabled: [0, 6]
});

$("#datetimepicker3").datetimepicker({
    format: "HH:mm",
    stepping: 30,
    enabledHours: [7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18]
});

$(document).ready(function () {
    let names = getActionAndController();
    const action = names[0], controller = names[1];

    if (controller == "Appointment") {
        if (action == "Update") {
            var selectedValue = $("#facilityId").val();
            var appointmentId = $("#appointmentId").val();
            updateAppointmentDropdowns(selectedValue, appointmentId, true);

            var minTime = updateTime(true);
            if (minTime[1] == true) {
                $("#datetimepicker3").datetimepicker("defaultDate", minTime[0]);
                $("#timePicker").val(minTime[0]);
            }
            else {
                updateDateAndTime(appointmentId);
            }
        }
        else if (action == "Create") {
            var minTime = updateTime(false);
            $("#timePicker").val(minTime[0]);
        }
    }
});

$("#datetimepicker4").on("change.datetimepicker", function () {
    let names = getActionAndController();
    const action = names[0], controller = names[1];

    if ((action == "Update" || action == "Create") && controller == "Appointment") {
        var minTime = updateTime(false);
        $("#timePicker").val(minTime[0]);
    }
});

function updateTime(useFullDate) {
    var selectedDate;
    var selectedTime;

    if (useFullDate) {
        var selected = $("#fullDate").val().split(" ");
        selectedDate = formatDate(selected[0]);
        selectedTime = selected[1];
    }
    else {
        selectedDate = $("#datePicker").val();
        selectedTime = $("#timePicker").val();
    }

    var currentDate = new Date();

    var day = currentDate.getDate();
    if (day <= 9)
        day = "0" + day.toString();

    var month = currentDate.getMonth() + 1;
    if (month <= 9)
        month = "0" + month.toString();

    var year = currentDate.getFullYear().toString();
    var currentDateStr = day + "/" + month + "/" + year;

    var hour = currentDate.getHours();
    var minutes = currentDate.getMinutes();
    var time;

    if (minutes <= 30) {
        if (hour <= 10)
            hour = "0" + currentHour.toString();
        time = hour + ":" + "30";
    }
    else {
        hour += 1;
        if (hour <= 10)
            hour = "0" + hour.toString();
        time = hour + ":" + "00";
    }

    var minTime;
    var change = false;

    console.log(selectedTime);

    if (selectedDate == currentDateStr && !(hour >= 19 || (hour == 18 && minutes > 30))) {
        $("#datetimepicker3").datetimepicker("minDate", time);
        minTime = time;

        var splittedSelTime = selectedTime.split(":");
        var selHour = parseInt(splittedSelTime[0]);
        var selMin = parseInt(splittedSelTime[1]);

        if ((selHour == hour && selMin < minutes) || selHour < hour)
            change = true;
    }
    else {
        $("#datetimepicker3").datetimepicker("minDate", "07:00");
        minTime = "07:00";
    }

    return [minTime, change];
}

function getCurrenDate() {
    var fullDate = $("#fullDate").val();
    var date = fullDate.split(" ")[0];
    return formatDate(date);
}

function getActionAndController() {
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

$("#facilityId").change(function () {
    var selectedValue = $(this).val();
    var appointmentId = $("#appointmentId").val();

    let names = getActionAndController();
    const action = names[0], controller = names[1];

    if (action == "Update" && controller == "Appointment") {
        updateAppointmentDropdowns(selectedValue, appointmentId, false);
    }
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

function formatDate(date) {
    var indexes = getAllIndexes(date, "/");
    var day = date.substring(indexes[0] + 1, indexes[1]);
    var month = date.substring(0, indexes[0]);
    var year = date.substring(indexes[1] + 1);
    return day + "/" + month + "/" + year;
}

function updateDateAndTime(appointmentId) {
    $.ajax({
        type: 'POST',
        dataType: 'JSON',
        url: '/Appointment/GetAppointmentDayAndTime',
        data: { appointmentId: appointmentId },
        success:
            function (response) {
                $("#datePicker").val(formatDate((response.date)));
                $("#timePicker").val(response.time);
            }
    });
}
