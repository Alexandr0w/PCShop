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

document.addEventListener('click', function (event) {
    const dropdown = document.getElementById('searchDropdown');
    const toggleBtn = document.getElementById('searchToggle');
    if (!dropdown.contains(event.target) && !toggleBtn.contains(event.target)) {
        dropdown.style.display = 'none';
    }
});

document.addEventListener('DOMContentLoaded', () => {
    const badge = document.getElementById('cartCount');
    if (badge) {
        setTimeout(() => badge.classList.add('show'), 100); 
    }
});