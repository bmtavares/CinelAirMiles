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
        var res = "<ul class='list-group'>";

        $.ajax({
            url: "/Notifications/getNotifications",
            method: "GET",
            success: function (result) {
                $("#notificationCount").html(result.count);

                var notifications = result.userNotifications;
                notifications.forEach(element => {
                    res = res + "<li class='list-group-item'>" + element.notification.text + "</li>";
                });
                res = res + "</ul>";
                $("#notification-content").html(res);

                console.log(result);
            },
            error: function (error) {
                console.log(error);
            }
        });
    }

    getNotification();
});