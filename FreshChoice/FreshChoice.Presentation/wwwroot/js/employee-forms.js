// Employee Forms JavaScript - Form Validation and Enhancement

document.addEventListener('DOMContentLoaded', function() {

    // Password Toggle Functionality
    const passwordToggles = document.querySelectorAll('.password-toggle');

    passwordToggles.forEach(toggle => {
        toggle.addEventListener('click', function() {
            const input = this.previousElementSibling;
            const icon = this.querySelector('i');

            if (input.type === 'password') {
                input.type = 'text';
                icon.classList.remove('bi-eye');
                icon.classList.add('bi-eye-slash');
            } else {
                input.type = 'password';
                icon.classList.remove('bi-eye-slash');
                icon.classList.add('bi-eye');
            }
        });
    });

    // Form Validation Enhancement
    const forms = document.querySelectorAll('.universal-form, .employee-form');

    forms.forEach(form => {
        form.addEventListener('submit', function(e) {
            let isValid = true;
            const requiredFields = form.querySelectorAll('[required]');

            requiredFields.forEach(field => {
                if (!field.value.trim()) {
                    isValid = false;
                    field.classList.add('input-validation-error');
                } else {
                    field.classList.remove('input-validation-error');
                }
            });

            // Email validation (only if field exists and has value)
            const emailFields = form.querySelectorAll('input[type="email"]');
            emailFields.forEach(field => {
                if (field.value && !isValidEmail(field.value)) {
                    isValid = false;
                    field.classList.add('input-validation-error');
                    showError(field, 'Please enter a valid email address');
                }
            });

            // Password match validation
            const password = form.querySelector('input[name="Password"]');
            const confirmPassword = form.querySelector('input[name="ConfirmPassword"]');

            if (password && confirmPassword) {
                // Only validate if both have values OR if password has value (creating)
                const isCreating = !form.querySelector('input[name="Id"]');

                if (isCreating) {
                    // Creating new employee - passwords required and must match
                    if (password.value !== confirmPassword.value) {
                        isValid = false;
                        confirmPassword.classList.add('input-validation-error');
                        showError(confirmPassword, 'Passwords do not match');
                    }
                } else {
                    // Editing employee - only validate if changing password
                    if (password.value || confirmPassword.value) {
                        if (password.value !== confirmPassword.value) {
                            isValid = false;
                            confirmPassword.classList.add('input-validation-error');
                            showError(confirmPassword, 'Passwords do not match');
                        }
                    }
                }
            }

            if (!isValid) {
                e.preventDefault();
            }
        });

        // Remove error styling on input
        const inputs = form.querySelectorAll('.form-control-custom');
        inputs.forEach(input => {
            input.addEventListener('input', function() {
                this.classList.remove('input-validation-error');
                const errorMsg = this.parentElement.querySelector('.text-danger');
                if (errorMsg && !errorMsg.hasAttribute('data-valmsg-for')) {
                    errorMsg.remove();
                }
            });
        });
    });

    // Real-time email validation
    const emailInputs = document.querySelectorAll('input[type="email"]');
    emailInputs.forEach(input => {
        input.addEventListener('blur', function() {
            if (this.value && !isValidEmail(this.value)) {
                this.classList.add('input-validation-error');
                showError(this, 'Please enter a valid email address');
            }
        });
    });

    // Form submission loading state
    const submitButtons = document.querySelectorAll('button[type="submit"]');
    submitButtons.forEach(button => {
        button.addEventListener('click', function() {
            const form = this.closest('form');
            if (form.checkValidity()) {
                this.disabled = true;
                const originalContent = this.innerHTML;
                this.innerHTML = '<i class="bi bi-hourglass-split"></i> Saving...';

                // Re-enable after 5 seconds as a fallback
                setTimeout(() => {
                    this.disabled = false;
                    this.innerHTML = originalContent;
                }, 5000);
            }
        });
    });

    // Auto-save draft (optional - stores form data in sessionStorage)
    const autoSaveForms = document.querySelectorAll('.universal-form[data-autosave], .employee-form[data-autosave]');
    autoSaveForms.forEach(form => {
        const formId = form.getAttribute('data-autosave');

        // Load saved data
        const savedData = sessionStorage.getItem(`form_${formId}`);
        if (savedData) {
            const data = JSON.parse(savedData);
            Object.keys(data).forEach(key => {
                const input = form.querySelector(`[name="${key}"]`);
                if (input && input.type !== 'password') {
                    input.value = data[key];
                }
            });
        }

        // Save data on input
        form.addEventListener('input', function() {
            const formData = {};
            const inputs = form.querySelectorAll('.form-control-custom');
            inputs.forEach(input => {
                if (input.name && input.type !== 'password') {
                    formData[input.name] = input.value;
                }
            });
            sessionStorage.setItem(`form_${formId}`, JSON.stringify(formData));
        });

        // Clear saved data on successful submit
        form.addEventListener('submit', function() {
            sessionStorage.removeItem(`form_${formId}`);
        });
    });

    // Helper Functions
    function isValidEmail(email) {
        const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return re.test(email);
    }

    function showError(field, message) {
        // Remove existing error message (only custom ones, not validation framework ones)
        const existingError = field.parentElement.querySelector('.text-danger:not([data-valmsg-for])');
        if (existingError) {
            existingError.remove();
        }

        // Add new error message
        const errorMsg = document.createElement('span');
        errorMsg.className = 'text-danger';
        errorMsg.textContent = message;
        field.parentElement.appendChild(errorMsg);
    }

    // Smooth scroll to validation errors
    const validationSummary = document.querySelector('.validation-summary-errors');
    if (validationSummary) {
        validationSummary.scrollIntoView({ behavior: 'smooth', block: 'center' });
    }
});