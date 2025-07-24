function toggleSidebar() {
    const sidebar = document.getElementById('adminSidebar');
    const overlay = document.getElementById('sidebarOverlay');
    const isMobile = window.innerWidth <= 768;

    const titleText = document.getElementById('adminTitleFull');
    const titleIcon = document.getElementById('adminTitleIcon');

    if (isMobile) {
        sidebar.classList.toggle('active');
        overlay.style.display = sidebar.classList.contains('active') ? 'block' : 'none';
    } else {
        sidebar.classList.toggle('collapsed');

        if (sidebar.classList.contains('collapsed')) {
            titleText.style.display = 'none';
            titleIcon.classList.remove('d-none');
        } else {
            titleText.style.display = 'inline';
            titleIcon.classList.add('d-none');
        }
    }
}

// Ensure correct state on page load (optional)
document.addEventListener('DOMContentLoaded', function () {
    const sidebar = document.getElementById('adminSidebar');
    const titleText = document.getElementById('adminTitleFull');
    const titleIcon = document.getElementById('adminTitleIcon');

    if (sidebar.classList.contains('collapsed')) {
        titleText.style.display = 'none';
        titleIcon.classList.remove('d-none');
    } else {
        titleText.style.display = 'inline';
        titleIcon.classList.add('d-none');
    }
});

// Show Bootstrap toasts on page load
document.addEventListener("DOMContentLoaded", function () {
    const toastElList = [].slice.call(document.querySelectorAll('.toast'));
    toastElList.forEach(function (toastEl) {
        new bootstrap.Toast(toastEl).show();
    });
});