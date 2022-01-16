
$("#datetimepicker4").datetimepicker({
    format: "DD/MM/YYYY",
    minDate: moment().millisecond(0).second(0).minute(0).hour(0),
    maxDate: new Date(2025, 12),
    daysOfWeekDisabled: [0, 6]
});

$("#datetimepicker3").datetimepicker({
    format: "HH:mm",
    stepping: 30,
    enabledHours: [7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19]
});

$(document).ready(function () {
    let names = getActionAndController();
    const action = names[0], controller = names[1];

    var navbarItems = $(".menu-elements li"), i;
    console.log(navbarItems);

    for (i = 0; i < navbarItems.length; i++) {
        if (navbarItems[i].lastElementChild.innerHTML == controller) {
            navbarItems[i].className = "active";
        }
        else {
            navbarItems[i].className = "";
        }
    }

    if (controller == "Appointment") {
        if (action == "Update") {
            var selectedValue = $("#facilityId").val();
            var appointmentId = $("#appointmentId").val();
            updateAppointmentDropdowns(selectedValue, appointmentId, true);

            var minTimeDate = updateTimeAndDate(true);

            if (minTimeDate[2] == true) {
                $("#timePicker").val(minTimeDate[0]);

                if (minTimeDate[1] != null)
                    $("#datePicker").val(minTimeDate[1]);
            }
            else {
                setDateAndTimeFromDb(appointmentId);
            }
        }
        else if (action == "Create") {
            var minTimeDate = updateTimeAndDate(false);
            $("#timePicker").val(minTimeDate[0]);
        }
    }
});

$("#datetimepicker4").on("change.datetimepicker", function () {
    let names = getActionAndController();
    const action = names[0], controller = names[1];

    if ((action == "Update" || action == "Create") && controller == "Appointment") {
        var minTimeDate = updateTimeAndDate(false);
        $("#timePicker").val(minTimeDate[0]);
    }
});

function updateTimeAndDate(useFullDate) {
    var selectedDateStr;
    var selectedTimeStr;

    if (useFullDate) {
        var selected = $("#fullDate").val().split(" ");
        selectedDateStr = selected[0];
        selectedTimeStr = selected[1];
    }
    else {
        selectedDateStr = $("#datePicker").val();
        selectedTimeStr = $("#timePicker").val();
    }

    var curDateTime = new Date();

    var day = curDateTime.getDate();
    day = day <= 9 ? "0" + day.toString() : day.toString();
    var month = curDateTime.getMonth() + 1;
    month = month <= 9 ? "0" + month.toString() : month.toString();

    var year = curDateTime.getFullYear().toString();
    var currentDateStr = day + "/" + month + "/" + year;

    var hour = curDateTime.getHours();
    var minutes = curDateTime.getMinutes();
    var time;

    if (minutes >= 0 && minutes < 30) {
        hour = hour <= 9 ? "0" + hour.toString() : hour.toString();
        time = hour + ":" + "30";
    }
    else {
        hour += 1;
        hour = hour <= 9 ? "0" + hour.toString() : hour.toString();
        time = hour + ":" + "00";
    }

    var minTime = null;
    var minDate = null;
    var change = false;

    const selectedDate = toDate(selectedDateStr);
    const currentDate = toDate(currentDateStr);

    var splittedSelTime = selectedTimeStr.split(":");
    var selectedHour = parseInt(splittedSelTime[0]);
    var selectedMin = parseInt(splittedSelTime[1]);

    if (selectedDateStr == currentDateStr) {
        minTime = time;

        if ((selectedHour == hour && selectedMin < minutes) || selectedHour < hour)
            change = true;
    }
    else if (selectedDate < currentDate) {
        minTime = time;
        minDate = currentDateStr;
        change = true;
    }
    else {
        minTime = "07:00";
    }

    $("#datetimepicker3").datetimepicker("minDate", minTime);

    return [minTime, minDate, change];
}

const toDate = (dateStr) => {
    const [day, month, year] = dateStr.split("/")
    return new Date(year, month - 1, day)
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
    else if (indexes.length == 1) {
        var controller = url.substring(1);
        return ["", controller];
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

function setDateAndTimeFromDb(appointmentId) {
    $.ajax({
        type: 'POST',
        dataType: 'JSON',
        url: '/Appointment/GetAppointmentDayAndTime',
        data: { appointmentId: appointmentId },
        success:
            function (response) {
                $("#datePicker").val(response.date);
                $("#timePicker").val(response.time);
            }
    });
}

$(document).ready(function () {
    $("#addressInfo").popover({
        trigger: "focus",
        title: "Valid addresses examples:",
        content: `
        <ul>
            <li>Poznan Kwiatowa 2 61-386</li>
            <li>Poznan Kwiatowa 2 m.12 61-386</li>
            <li>Bukow 12c 65-040</li>
        </ul>
        `,
        placement: "right",
        html: true,
        animation: true
    });
});

$(document).ready(function () {
    $(".dismiss").on("click", function () {
        $(".sidebar").removeClass("active");
    });

    $(".open-menu").on("click", function (e) {
        e.preventDefault();
        $(".sidebar").addClass("active");
    });
});

$(".to-top a").on("click", function (e) {
    e.preventDefault();
    if ($(window).scrollTop() != 0) {
        $("html, body").stop().animate({ scrollTop: 0 }, 1000);

    }
});
