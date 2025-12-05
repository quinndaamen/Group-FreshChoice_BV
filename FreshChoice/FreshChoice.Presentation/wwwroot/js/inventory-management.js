// Inventory Management JavaScript

document.addEventListener('DOMContentLoaded', function() {
    const searchInput = document.getElementById('searchInput');
    const categoryFilter = document.getElementById('categoryFilter');
    const inventoryRows = document.querySelectorAll('.inventory-row');
    const tableBody = document.getElementById('inventoryTableBody');
    const noResultsState = document.getElementById('noResultsState');
    const emptyState = document.getElementById('emptyState');

    // Initialize row animations
    initializeRowAnimations();

    // Filter Function
    function filterInventory() {
        const searchTerm = searchInput.value.toLowerCase();
        const selectedCategory = categoryFilter.value;
        let visibleCount = 0;

        inventoryRows.forEach(row => {
            const searchText = row.getAttribute('data-search').toLowerCase();
            const rowCategory = row.getAttribute('data-category');

            const matchesSearch = searchText.includes(searchTerm);
            const matchesCategory = selectedCategory === 'All Categories' || rowCategory === selectedCategory;

            if (matchesSearch && matchesCategory) {
                row.classList.remove('hidden-row');
                row.style.display = 'table-row';
                visibleCount++;
            } else {
                row.classList.add('hidden-row');
                row.style.display = 'none';
            }
        });

        // Show/hide no results message
        if (noResultsState) {
            if (visibleCount === 0 && inventoryRows.length > 0) {
                noResultsState.style.display = 'block';
            } else {
                noResultsState.style.display = 'none';
            }
        }

        // Hide empty state if we have rows
        if (emptyState && inventoryRows.length > 0) {
            emptyState.style.display = 'none';
        }
    }

    // Event Listeners
    if (searchInput) {
        searchInput.addEventListener('input', filterInventory);
    }

    if (categoryFilter) {
        categoryFilter.addEventListener('change', filterInventory);
    }

    // Animation on load
    function initializeRowAnimations() {
        inventoryRows.forEach((row, index) => {
            row.style.animationDelay = `${index * 0.03}s`;
        });
    }

    // Stock Status Update Function
    function updateStockStatus(row, quantity) {
        const statusBadge = row.querySelector('.status-badge');

        if (quantity === 0) {
            statusBadge.className = 'status-badge out-stock';
            statusBadge.textContent = 'Out of Stock';
        } else if (quantity < 10) {
            statusBadge.className = 'status-badge low-stock';
            statusBadge.textContent = 'Low Stock';
        } else {
            statusBadge.className = 'status-badge in-stock';
            statusBadge.textContent = 'In Stock';
        }
    }

    // Update Stock Alerts
    function updateStockAlerts() {
        const outOfStockAlert = document.querySelector('.alert-banner.alert-danger');
        const lowStockAlert = document.querySelector('.alert-banner.alert-warning');
        let outOfStockCount = 0;
        let lowStockCount = 0;

        inventoryRows.forEach(row => {
            const quantityCell = row.querySelector('.quantity-cell');
            const quantity = parseInt(quantityCell.getAttribute('data-quantity'));

            if (quantity === 0) {
                outOfStockCount++;
            } else if (quantity > 0 && quantity < 10) {
                lowStockCount++;
            }
        });

        // Update Out of Stock Alert
        if (outOfStockAlert) {
            if (outOfStockCount > 0) {
                outOfStockAlert.style.display = 'flex';
                const alertText = outOfStockAlert.querySelector('span');
                if (alertText) {
                    alertText.textContent = `${outOfStockCount} product(s) completely out of stock`;
                }
            } else {
                outOfStockAlert.style.display = 'none';
            }
        }

        // Update Low Stock Alert
        if (lowStockAlert) {
            if (lowStockCount > 0) {
                lowStockAlert.style.display = 'flex';
                const alertText = lowStockAlert.querySelector('span');
                if (alertText) {
                    alertText.textContent = `${lowStockCount} product(s) running low on stock`;
                }
            } else {
                lowStockAlert.style.display = 'none';
            }
        }
    }

    // Make adjustStock available globally
    window.adjustStock = function(itemId, adjustment) {
        // Find the row
        const row = document.querySelector(`[data-item-id=\"${itemId}\"]`);
        if (!row) return;

        const quantityCell = row.querySelector('.quantity-cell');
        const quantityValue = quantityCell.querySelector('.quantity-value');

        let currentQuantity = parseInt(quantityCell.getAttribute('data-quantity'));
        let newQuantity = Math.max(0, currentQuantity + adjustment);

        // Update the display
        quantityValue.textContent = newQuantity;
        quantityCell.setAttribute('data-quantity', newQuantity);

        // Update status badge
        updateStockStatus(row, newQuantity);

        // Update stock alerts
        updateStockAlerts();

        // Add a visual feedback animation
        quantityCell.style.transform = 'scale(1.1)';
        setTimeout(() => {
            quantityCell.style.transform = 'scale(1)';
        }, 200);

        // Save to database via AJAX
        fetch(`/Item/AdjustStock/${itemId}?adjustment=${adjustment}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || ''
            }
        })
            .then(response => {
                if (!response.ok) {
                    // If save failed, revert the change
                    quantityValue.textContent = currentQuantity;
                    quantityCell.setAttribute('data-quantity', currentQuantity);
                    updateStockStatus(row, currentQuantity);
                    updateStockAlerts();
                    alert('Failed to update stock. Please try again.');
                }
            })
            .catch(error => {
                console.error('Error updating stock:', error);
                // Revert on error
                quantityValue.textContent = currentQuantity;
                quantityCell.setAttribute('data-quantity', currentQuantity);
                updateStockStatus(row, currentQuantity);
                updateStockAlerts();
                alert('Failed to update stock. Please try again.');
            });
    };

    // Add transition to quantity cells
    document.querySelectorAll('.quantity-cell').forEach(cell => {
        cell.style.transition = 'transform 0.2s ease';
    });

    // Initialize low stock alert on page load
    updateStockAlerts();

    // Enhanced search with debouncing for better performance
    let searchTimeout;
    if (searchInput) {
        searchInput.addEventListener('input', function() {
            clearTimeout(searchTimeout);
            searchTimeout = setTimeout(filterInventory, 150);
        });
    }

    // Keyboard shortcuts
    document.addEventListener('keydown', function(e) {
        // Focus search on Ctrl/Cmd + K
        if ((e.ctrlKey || e.metaKey) && e.key === 'k') {
            e.preventDefault();
            if (searchInput) {
                searchInput.focus();
                searchInput.select();
            }
        }
    });

    // Add loading state for action buttons
    document.querySelectorAll('.btn-action').forEach(button => {
        button.addEventListener('click', function() {
            if (!this.classList.contains('btn-delete')) {
                this.style.opacity = '0.6';
                setTimeout(() => {
                    this.style.opacity = '1';
                }, 300);
            }
        });
    });
});

// Auto-save functionality
function setupAutoSave() {
    let saveTimeout;
    const autoSaveDelay = 500; // 1/2 seconds

    function scheduleSave(itemId, quantity) {
        clearTimeout(saveTimeout);
        saveTimeout = setTimeout(() => {
            // Make your AJAX call here
            console.log(`Auto-saving item ${itemId} with quantity ${quantity}`);
            // fetch(`/Item/Update/${itemId}`, { ... });
        }, autoSaveDelay);
    }
}

// Export functions if needed
if (typeof module !== 'undefined' && module.exports) {
    module.exports = {
        adjustStock: window.adjustStock
    };
}