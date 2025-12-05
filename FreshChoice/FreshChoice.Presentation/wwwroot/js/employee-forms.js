// Employee Forms JavaScript

document.addEventListener('DOMContentLoaded', function() {
    // Password Toggle Functionality
    const passwordToggles = document.querySelectorAll('.password-toggle');

    passwordToggles.forEach(toggle => {
        toggle.addEventListener('click', function() {
            const passwordField = this.previousElementSibling;
            const icon = this.querySelector('i');

            if (passwordField.type === 'password') {
                passwordField.type = 'text';
                icon.classList.remove('bi-eye');
                icon.classList.add('bi-eye-slash');
            } else {
                passwordField.type = 'password';
                icon.classList.remove('bi-eye-slash');
                icon.classList.add('bi-eye');
            }
        });
    });

    // Phone Number Formatting
    const phoneInput = document.querySelector('input[name="PhoneNumber"]');
    if (phoneInput) {
        phoneInput.addEventListener('input', function(e) {
            let value = e.target.value.replace(/\D/g, '');

            if (value.length > 0) {
                if (value.length <= 3) {
                    value = `(${value}`;
                } else if (value.length <= 6) {
                    value = `(${value.slice(0, 3)}) ${value.slice(3)}`;
                } else {
                    value = `(${value.slice(0, 3)}) ${value.slice(3, 6)}-${value.slice(6, 10)}`;
                }
            }

            e.target.value = value;
        });
    }

    // Wage Input Validation
    const wageInput = document.querySelector('input[name="WagePerHour"]');
    if (wageInput) {
        wageInput.addEventListener('input', function(e) {
            // Ensure only 2 decimal places
            const value = parseFloat(e.target.value);
            if (!isNaN(value)) {
                e.target.value = value.toFixed(2);
            }
        });
    }

    // Form Auto-save (Optional)
    const form = document.querySelector('.universal-form');
    if (form && form.dataset.autosave) {
        const autosaveKey = form.dataset.autosave;

        // Load saved data
        const savedData = localStorage.getItem(autosaveKey);
        if (savedData) {
            try {
                const data = JSON.parse(savedData);
                Object.keys(data).forEach(key => {
                    const input = form.querySelector(`[name="${key}"]`);
                    if (input && input.type !== 'password') {
                        input.value = data[key];
                    }
                });
            } catch (e) {
                console.error('Error loading autosaved data:', e);
            }
        }

        // Save data on input
        const inputs = form.querySelectorAll('input:not([type="password"]), select, textarea');
        inputs.forEach(input => {
            input.addEventListener('input', function() {
                const formData = {};
                inputs.forEach(i => {
                    if (i.name && i.type !== 'password') {
                        formData[i.name] = i.value;
                    }
                });
                localStorage.setItem(autosaveKey, JSON.stringify(formData));
            });
        });

        // Clear autosave on successful submit
        form.addEventListener('submit', function() {
            localStorage.removeItem(autosaveKey);
        });
    }

    // Confirm Password Validation
    const passwordInput = document.querySelector('input[name="Password"]');
    const confirmPasswordInput = document.querySelector('input[name="ConfirmPassword"]');

    if (passwordInput && confirmPasswordInput) {
        confirmPasswordInput.addEventListener('blur', function() {
            if (passwordInput.value && confirmPasswordInput.value) {
                if (passwordInput.value !== confirmPasswordInput.value) {
                    confirmPasswordInput.setCustomValidity('Passwords do not match');
                } else {
                    confirmPasswordInput.setCustomValidity('');
                }
            }
        });

        passwordInput.addEventListener('input', function() {
            if (confirmPasswordInput.value) {
                confirmPasswordInput.dispatchEvent(new Event('blur'));
            }
        });
    }
});