// Custom Dropdown Functionality

class CustomDropdown {
    constructor(element) {
        this.element = element;
        this.select = element.querySelector('select');
        this.trigger = null;
        this.menu = null;
        this.isOpen = false;

        this.init();
    }

    init() {
        // Create custom dropdown UI
        this.createDropdown();

        // Add event listeners
        this.trigger.addEventListener('click', () => this.toggle());

        // Close on outside click
        document.addEventListener('click', (e) => {
            if (!this.element.contains(e.target)) {
                this.close();
            }
        });

        // Close on Escape key
        document.addEventListener('keydown', (e) => {
            if (e.key === 'Escape' && this.isOpen) {
                this.close();
            }
        });
    }

    createDropdown() {
        // Create trigger button
        const selectedOption = this.select.options[this.select.selectedIndex];
        this.trigger = document.createElement('div');
        this.trigger.className = 'custom-dropdown-trigger';
        this.trigger.innerHTML = `
            <span class="custom-dropdown-label">${selectedOption.text}</span>
            <svg class="custom-dropdown-arrow" xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <polyline points="6 9 12 15 18 9"></polyline>
            </svg>
        `;

        // Create dropdown menu
        this.menu = document.createElement('div');
        this.menu.className = 'custom-dropdown-menu';

        // Add options
        Array.from(this.select.options).forEach((option, index) => {
            const optionEl = document.createElement('div');
            optionEl.className = 'custom-dropdown-option';
            optionEl.textContent = option.text;
            optionEl.dataset.value = option.value;

            if (option.selected) {
                optionEl.classList.add('selected');
            }

            optionEl.addEventListener('click', () => this.selectOption(index));

            this.menu.appendChild(optionEl);
        });

        // Insert into DOM
        this.element.appendChild(this.trigger);
        this.element.appendChild(this.menu);
    }

    toggle() {
        if (this.isOpen) {
            this.close();
        } else {
            this.open();
        }
    }

    open() {
        this.isOpen = true;
        this.trigger.classList.add('active');
        this.menu.classList.add('active');
    }

    close() {
        this.isOpen = false;
        this.trigger.classList.remove('active');
        this.menu.classList.remove('active');
    }

    selectOption(index) {
        // Update the hidden select element
        this.select.selectedIndex = index;

        // Trigger change event
        const event = new Event('change', { bubbles: true });
        this.select.dispatchEvent(event);

        // Update UI
        const selectedOption = this.select.options[index];
        const label = this.trigger.querySelector('.custom-dropdown-label');
        label.textContent = selectedOption.text;

        // Update selected class
        this.menu.querySelectorAll('.custom-dropdown-option').forEach((opt, i) => {
            if (i === index) {
                opt.classList.add('selected');
            } else {
                opt.classList.remove('selected');
            }
        });

        this.close();
    }

    // Method to update options dynamically (if needed)
    updateOptions() {
        this.menu.innerHTML = '';

        Array.from(this.select.options).forEach((option, index) => {
            const optionEl = document.createElement('div');
            optionEl.className = 'custom-dropdown-option';
            optionEl.textContent = option.text;
            optionEl.dataset.value = option.value;

            if (option.selected) {
                optionEl.classList.add('selected');
            }

            optionEl.addEventListener('click', () => this.selectOption(index));

            this.menu.appendChild(optionEl);
        });
    }
}

// Initialize all custom dropdowns
function initCustomDropdowns() {
    const dropdowns = document.querySelectorAll('.custom-dropdown');
    dropdowns.forEach(dropdown => {
        new CustomDropdown(dropdown);
    });
}

// Initialize on DOM load
if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', initCustomDropdowns);
} else {
    initCustomDropdowns();
}