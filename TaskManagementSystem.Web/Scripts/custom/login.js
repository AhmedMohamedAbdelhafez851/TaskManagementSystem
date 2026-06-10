// =============================================
// LOGIN PAGE CLIENT-SIDE FUNCTIONALITY
// =============================================

(function() {
    'use strict';
    
    // Wait for DOM to be fully loaded
    document.addEventListener('DOMContentLoaded', function() {
        
        // 1. Password visibility toggle
        initPasswordToggle();
        
        // 2. Login button loading state
        initLoadingButton();
        
        // 3. Remember Me functionality
        initRememberMe();
        
        // 4. Enter key submission
        initEnterKeySubmit();
        
    });
    
    /**
     * Initialize password visibility toggle
     */
    function initPasswordToggle() {
        const togglePassword = document.getElementById('togglePassword');
        const passwordInput = getServerControl('txtPassword');
        
        if (togglePassword && passwordInput) {
            togglePassword.addEventListener('click', function() {
                const type = passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
                passwordInput.setAttribute('type', type);
                this.classList.toggle('fa-eye');
                this.classList.toggle('fa-eye-slash');
            });
        }
    }
    
    /**
     * Initialize loading state on login button click
     */
    function initLoadingButton() {
        const loginBtn = getServerControl('btnLogin');
        
        if (loginBtn) {
            loginBtn.addEventListener('click', function() {
                this.classList.add('loading');
                // Prevent getting stuck if server is slow
                setTimeout(() => {
                    this.classList.remove('loading');
                }, 10000);
            });
        }
    }
    
    /**
     * Initialize Remember Me functionality
     */
    function initRememberMe() {
        const rememberCheckbox = document.getElementById('rememberMe');
        const usernameInput = getServerControl('txtUsername');
        const loginBtn = getServerControl('btnLogin');
        
        if (rememberCheckbox && usernameInput) {
            // Load saved username
            const savedUsername = localStorage.getItem('savedUsername');
            if (savedUsername) {
                usernameInput.value = savedUsername;
                rememberCheckbox.checked = true;
            }
            
            // Save username on form submit
            if (loginBtn) {
                loginBtn.addEventListener('click', function() {
                    if (rememberCheckbox.checked && usernameInput.value) {
                        localStorage.setItem('savedUsername', usernameInput.value);
                    } else {
                        localStorage.removeItem('savedUsername');
                    }
                });
            }
        }
    }
    
    /**
     * Initialize Enter key submission
     */
    function initEnterKeySubmit() {
        const inputs = document.querySelectorAll('input');
        const loginBtn = getServerControl('btnLogin');
        
        inputs.forEach(input => {
            input.addEventListener('keypress', function(e) {
                if (e.key === 'Enter') {
                    e.preventDefault();
                    if (loginBtn) loginBtn.click();
                }
            });
        });
    }
    
    /**
     * Get server control by client ID
     * @param {string} controlId - Server control ID
     * @returns {HTMLElement|null}
     */
    function getServerControl(controlId) {
        const element = document.getElementById(controlId);
        if (element) return element;
        
        // Try to find by partial ID (for ASP.NET generated IDs)
        return document.querySelector(`[id$="_${controlId}"]`);
    }
    
})();