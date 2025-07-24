// This script handles the search dropdown toggle
document.getElementById('searchToggle')?.addEventListener('click', function (e) {
    e.preventDefault();
    const dropdown = document.getElementById('searchDropdown');
    if (dropdown.style.display === 'none' || dropdown.style.display === '') {
        dropdown.style.display = 'block';
        dropdown.querySelector('input')?.focus();
    } else {
        dropdown.style.display = 'none';
    }
});

// Close the dropdown if clicked outside
document.addEventListener('click', function (event) {
    const dropdown = document.getElementById('searchDropdown');
    const toggleBtn = document.getElementById('searchToggle');
    if (!dropdown.contains(event.target) && !toggleBtn.contains(event.target)) {
        dropdown.style.display = 'none';
    }
});

// Show Bootstrap toasts on page load
document.addEventListener("DOMContentLoaded", function () {
    const toastElList = [].slice.call(document.querySelectorAll('.toast'));
    toastElList.forEach(function (toastEl) {
        new bootstrap.Toast(toastEl).show();
    });
});

// Function for get cart count and update the cart icon
document.addEventListener('DOMContentLoaded', () => {
    function updateCartCount() {
        fetch('/Order/GetCartCount', { credentials: 'include' })
            .then(res => res.json())
            .then(data => {
                let countElem = document.getElementById('cartCount');
                let cartIcon = document.getElementById('cartIcon');

                if (!cartIcon) return;

                if (data.cartCount > 0) {
                    if (!countElem) {
                        countElem = document.createElement('span');
                        countElem.id = 'cartCount';
                        countElem.className = 'position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger border border-light shadow-sm';
                        countElem.style.fontSize = '0.75rem';
                        countElem.style.zIndex = '99';
                        cartIcon.appendChild(countElem);
                    }
                    countElem.textContent = data.cartCount;
                } else {
                    if (countElem) {
                        countElem.remove();
                    }
                }
            })
            .catch(err => console.error('Error fetching cart count:', err));
    }

    updateCartCount();
    setInterval(updateCartCount, 30000);
});

// Function for get notification count and update the notification icon
document.addEventListener('DOMContentLoaded', () => {
    function updateNotificationCount() {
        fetch('/Notification/GetUnreadCount', { credentials: 'include' })
            .then(res => res.json())
            .then(data => {
                let countElem = document.getElementById('notificationCount');
                let notificationIcon = document.getElementById('notificationIcon');

                if (!notificationIcon) return;

                if (data.unreadCount > 0) {
                    if (!countElem) {
                        countElem = document.createElement('span');
                        countElem.id = 'notificationCount';
                        countElem.className = 'position-absolute top-0 start-100 translate-middle badge rounded-pill bg-danger border border-light shadow-sm';
                        countElem.style.fontSize = '0.75rem';
                        countElem.style.zIndex = '99';
                        notificationIcon.querySelector('a').appendChild(countElem);
                    }
                    countElem.textContent = data.unreadCount;
                } else {
                    if (countElem) {
                        countElem.remove();
                    }
                }
            })
            .catch(err => console.error('Error fetching notification count:', err));
    }

    updateNotificationCount();
    setInterval(updateNotificationCount, 30000);
});
