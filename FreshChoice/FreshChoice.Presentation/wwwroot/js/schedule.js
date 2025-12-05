// Schedule Page JavaScript

document.addEventListener('DOMContentLoaded', function() {
    // Hover effects on shift blocks
    const shiftBlocks = document.querySelectorAll('.shift-block');

    shiftBlocks.forEach(block => {
        block.addEventListener('mouseenter', function() {
            this.style.zIndex = '20';
        });

        block.addEventListener('mouseleave', function() {
            this.style.zIndex = '1';
        });
    });

    // Optional: Add click to view shift details
    shiftBlocks.forEach(block => {
        block.addEventListener('click', function(e) {
            // Don't trigger if clicking on action buttons
            if (e.target.closest('.shift-actions')) {
                return;
            }

            // You can add a modal or details view here if needed
            console.log('Shift clicked:', this.dataset.shiftId);
        });
    });
});