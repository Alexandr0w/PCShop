document.addEventListener('DOMContentLoaded', function () {
    const toggle = document.getElementById('searchToggle');
    const dropdown = document.getElementById('searchDropdown');

    if (toggle && dropdown) {
        toggle.addEventListener('click', function (e) {
            e.preventDefault();

            const isVisible = dropdown.style.display === 'block';
            dropdown.style.display = isVisible ? 'none' : 'block';

            if (!isVisible) {
                dropdown.querySelector('input')?.focus();
            }
        });

        document.addEventListener('click', function (e) {
            const isClickInside = toggle.contains(e.target) || dropdown.contains(e.target);
            if (!isClickInside) {
                dropdown.style.display = 'none';
            }
        });
    }
});
