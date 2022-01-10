
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