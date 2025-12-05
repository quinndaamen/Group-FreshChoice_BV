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
        // Remove any input restrictions that might interfere with typing
        wageInput.removeAttribute('maxlength');

        // Only format on blur, not on every keystroke
        wageInput.addEventListener('blur', function(e) {
            const value = parseFloat(e.target.value);
            if (!isNaN(value) && value >= 0) {
                e.target.value = value.toFixed(2);
            }
        });

        // Prevent the input from being affected by autosave initially
        wageInput.addEventListener('focus', function(e) {
            // Remove any autocomplete interference
            e.target.setAttribute('autocomplete', 'off');
        }, { once: true });
    }

    // Form Auto-save (Optional)
    const form = document.querySelector('.universal-form');
    if (form && form.dataset.autosave) {
        const autosaveKey = form.dataset.autosave;

        // Fields to exclude from autosave (can cause input issues)
        const excludeFields = ['WagePerHour', 'Password', 'ConfirmPassword'];

        // Load saved data
        const savedData = localStorage.getItem(autosaveKey);
        if (savedData) {
            try {
                const data = JSON.parse(savedData);
                Object.keys(data).forEach(key => {
                    if (!excludeFields.includes(key)) {
                        const input = form.querySelector(`[name="${key}"]`);
                        if (input && input.type !== 'password') {
                            input.value = data[key];
                        }
                    }
                });
            } catch (e) {
                console.error('Error loading autosaved data:', e);
            }
        }

        // Save data on input (exclude certain fields)
        const inputs = form.querySelectorAll('input:not([type="password"]), select, textarea');
        inputs.forEach(input => {
            input.addEventListener('input', function() {
                const formData = {};
                inputs.forEach(i => {
                    if (i.name && i.type !== 'password' && !excludeFields.includes(i.name)) {
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