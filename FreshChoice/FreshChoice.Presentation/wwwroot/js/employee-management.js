// Employee Management Search and Filter Functionality

document.addEventListener('DOMContentLoaded', function() {
    const searchInput = document.getElementById('searchInput');
    const roleFilter = document.getElementById('roleFilter');
    const statusFilter = document.getElementById('statusFilter');
    const employeeCards = document.querySelectorAll('.employee-card');

    function filterEmployees() {
        const searchTerm = searchInput ? searchInput.value.toLowerCase() : '';
        const roleValue = roleFilter ? roleFilter.value.toLowerCase() : '';
        const statusValue = statusFilter ? statusFilter.value.toLowerCase() : '';

        employeeCards.forEach(card => {
            const searchData = card.getAttribute('data-search')?.toLowerCase() || '';
            const roleData = card.getAttribute('data-role')?.toLowerCase() || '';
            const statusData = card.getAttribute('data-status')?.toLowerCase() || '';

            const matchesSearch = searchData.includes(searchTerm);
            const matchesRole = roleValue === '' || roleData === roleValue;
            const matchesStatus = statusValue === '' || statusData === statusValue;

            if (matchesSearch && matchesRole && matchesStatus) {
                card.classList.remove('hidden');
            } else {
                card.classList.add('hidden');
            }
        });
    }

    // Add event listeners
    if (searchInput) {
        searchInput.addEventListener('input', filterEmployees);
    }

    if (roleFilter) {
        roleFilter.addEventListener('change', filterEmployees);
    }

    if (statusFilter) {
        statusFilter.addEventListener('change', filterEmployees);
    }
});