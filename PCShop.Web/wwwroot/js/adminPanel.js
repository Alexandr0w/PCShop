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

            // Close all opened submenus when collapsed
            const allCollapses = sidebar.querySelectorAll('.collapse.show');
            allCollapses.forEach(collapseEl => collapseEl.classList.remove('show'));
        } else {
            titleText.style.display = 'inline';
            titleIcon.classList.add('d-none');
        }
    }
}

document.addEventListener('DOMContentLoaded', function () {
    const sidebar = document.getElementById('adminSidebar');
    const overlay = document.getElementById('sidebarOverlay');
    const titleText = document.getElementById('adminTitleFull');
    const titleIcon = document.getElementById('adminTitleIcon');

    // Set initial sidebar title display
    if (sidebar.classList.contains('collapsed')) {
        titleText.style.display = 'none';
        titleIcon.classList.remove('d-none');
    } else {
        titleText.style.display = 'inline';
        titleIcon.classList.add('d-none');
    }

    // Hide sidebar when overlay is clicked (on mobile)
    if (overlay) {
        overlay.addEventListener('click', () => {
            sidebar.classList.remove('active');
            overlay.style.display = 'none';
        });
    }

    // Add click listeners for main menu links
    document.querySelectorAll('.main-menu-link').forEach(link => {
        link.addEventListener('click', function (e) {
            const isCollapsed = sidebar.classList.contains('collapsed');
            const targetId = this.getAttribute('data-target');
            const directUrl = this.getAttribute('data-direct-url');
            const isMobile = window.innerWidth <= 768;

            if (isCollapsed) {
                // When collapsed (desktop), navigate directly
                e.preventDefault();
                window.location.href = directUrl;
                return;
            }

            if (targetId) {
                // Menu with submenu – toggle submenu, do NOT close sidebar on mobile
                e.preventDefault();
                const submenu = document.querySelector(targetId);
                if (submenu) {
                    submenu.classList.toggle('show');
                }
                return;
            }

            if (directUrl) {
                // Menu without submenu – direct navigation and close sidebar on mobile
                e.preventDefault();
                if (isMobile && sidebar.classList.contains('active')) {
                    sidebar.classList.remove('active');
                    overlay.style.display = 'none';
                }
                window.location.href = directUrl;
            }
        });
    });
});

// Show Bootstrap toasts on page load - отделено от DOMContentLoaded
const toastElList = [].slice.call(document.querySelectorAll('.toast'));
    toastElList.forEach(function (toastEl) {
        new bootstrap.Toast(toastEl).show();
});
