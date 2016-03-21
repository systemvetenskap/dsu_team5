$(function () {
    $(".calendar").datepicker({
        showOn: "both",
        buttonImage: "/Images/kalenderikon_v02.png",
        buttonImageOnly: true,
        buttonText: "Välj datum"
    }, $.datepicker.regional["sv"]);
});