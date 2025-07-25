// Function for sidebar in Admin Dashboard
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

            // Close all opened submenu on collapse
            const allCollapses = sidebar.querySelectorAll('.collapse.show');
            allCollapses.forEach(collapseEl => collapseEl.classList.remove('show'));
        } else {
            titleText.style.display = 'inline';
            titleIcon.classList.add('d-none');
        }
    }
}

// Navigation behavior for main menu links when sidebar is collapsed
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

    // Add click listeners for main menu links
    document.querySelectorAll('.main-menu-link').forEach(link => {
        link.addEventListener('click', function (e) {
            const isCollapsed = sidebar.classList.contains('collapsed');
            const targetId = this.getAttribute('data-target');
            const directUrl = this.getAttribute('data-direct-url');

            if (isCollapsed) {
                // When collapsed, navigate directly
                e.preventDefault();
                window.location.href = directUrl;
            } else {
                // When expanded, toggle submenu
                e.preventDefault();
                const submenu = document.querySelector(targetId);
                if (submenu) {
                    submenu.classList.toggle('show');
                }
            }
        });
    });

    // Show Bootstrap toasts on page load
    const toastElList = [].slice.call(document.querySelectorAll('.toast'));
    toastElList.forEach(function (toastEl) {
        new bootstrap.Toast(toastEl).show();
    });
});
