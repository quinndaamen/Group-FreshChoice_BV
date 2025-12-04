// Employee Management Filter and Search Functionality

document.addEventListener('DOMContentLoaded', function() {
    const searchInput = document.getElementById('searchInput');
    const employeeCards = document.querySelectorAll('.employee-card');

    // Function to filter employees
    function filterEmployees() {
        const searchTerm = searchInput.value.toLowerCase();

        employeeCards.forEach(card => {
            const searchText = card.getAttribute('data-search').toLowerCase();

            // Check if card matches search
            const matchesSearch = searchText.includes(searchTerm);

            // Show or hide card
            if (matchesSearch) {
                card.style.display = 'grid';
            } else {
                card.style.display = 'none';
            }
        });

        // Show "no results" message if needed
        const visibleCards = Array.from(employeeCards).filter(card => card.style.display !== 'none');
        let noResultsMsg = document.getElementById('no-results-message');

        if (visibleCards.length === 0 && searchTerm) {
            if (!noResultsMsg) {
                noResultsMsg = document.createElement('div');
                noResultsMsg.id = 'no-results-message';
                noResultsMsg.className = 'alert alert-info';
                noResultsMsg.innerHTML = '<i class="bi bi-search"></i> No employees found matching your search.';
                document.querySelector('.employees-container').appendChild(noResultsMsg);
            }
        } else if (noResultsMsg) {
            noResultsMsg.remove();
        }
    }

    // Add event listener
    if (searchInput) {
        searchInput.addEventListener('input', filterEmployees);
    }

    // Optional: Add animation when cards appear
    employeeCards.forEach((card, index) => {
        card.style.opacity = '0';
        card.style.transform = 'translateY(20px)';

        setTimeout(() => {
            card.style.transition = 'opacity 0.3s ease, transform 0.3s ease';
            card.style.opacity = '1';
            card.style.transform = 'translateY(0)';
        }, index * 50);
    });
});