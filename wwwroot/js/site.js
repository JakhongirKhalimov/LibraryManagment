function confirmDelete() {
    return confirm("Are you sure you want to delete this user?");
}

document.addEventListener('DOMContentLoaded', function () {
    const toggleLink = document.getElementById('toggleNotification');
    const notificationSection = document.querySelector('.notification');
    const toggleIcon = document.getElementById('toggleNotificationIcon');

    toggleLink.addEventListener('click', function (event) {
        event.preventDefault();
        notificationSection.classList.toggle('hidden');

        if (toggleIcon.src.includes('notification-active')) {
            toggleIcon.src = '/images/ui-icons/notification.png';
        } else {
            toggleIcon.src = '/images/ui-icons/notification-active.png';
        }
    });

    $(function () {
        $.ajax({
            type: "GET",
            url: '/Notification/GetNotifications',
            data: {},
            success: function (response) {
                console.log(response);
                if (response != null) {
                    if (response.isValid == true) {
                        var notificationsHTML = '';
                        $.each(response.list, function (index, notification) {
                            notificationsHTML += '<a class="notification-item" value="' + notification.id + '">';
                            notificationsHTML += '<div class="row px-4 py-3 m-0';
                            if (notification.isMarkedRead === false) {
                                notificationsHTML += ' unread-notification';
                            }
                            notificationsHTML += '">';
                            notificationsHTML += '<div class="col">' + notification.text + '</div>';
                            notificationsHTML += '</div></a>';
                        });

                        $('.notification').html(notificationsHTML);
                    }
                }
            },
            error: function (xhr, status, error) {
                console.error('Error fetching notifications:', error);
            }
        });
    });
});
