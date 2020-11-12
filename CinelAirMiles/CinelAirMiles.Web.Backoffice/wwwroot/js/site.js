// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function () {
    //$('[data-show="popover"]').popover({
    //    placement: 'bottom',
    //    content: function () {
    //        return $("#notification-content").html();
    //    },
    //    html: true
    //});

    //$('body').append(`<div id="notification-content" class="hide"></div>`);

    function getNotification() {
        $.ajax({
            url: "/Notifications/getNotifications",
            method: "GET",
            success: function (result) {
                $("#notificationCount").html(result.count);
                console.log(result);
            },
            error: function (error) {
                console.log(error);
            }
        });
    }

    getNotification();
});