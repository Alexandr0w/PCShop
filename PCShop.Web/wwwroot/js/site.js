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

// Show the cart count badge after a short delay
document.addEventListener('DOMContentLoaded', () => {
    const badge = document.getElementById('cartCount');
    if (badge) {
        setTimeout(() => badge.classList.add('show'), 100); 
    }
});

// Show Bootstrap toasts on page load
document.addEventListener("DOMContentLoaded", function () {
    const toastElList = [].slice.call(document.querySelectorAll('.toast'));
    toastElList.forEach(function (toastEl) {
        new bootstrap.Toast(toastEl).show();
    });
});