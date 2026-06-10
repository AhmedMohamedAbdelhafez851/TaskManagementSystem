// =============================================
// MEMBER MASTER PAGE CLIENT-SIDE FUNCTIONALITY
// =============================================

(function () {
    'use strict';

    document.addEventListener('DOMContentLoaded', function () {
        initLoadingOverlay();
        initDarkMode();
        initActiveNavLink();
        initThemeToggle();
    });

    /**
     * Initialize loading overlay on navigation
     */
    function initLoadingOverlay() {
        var navLinks = document.querySelectorAll('a');
        var overlay = document.getElementById('loadingOverlay');

        for (var i = 0; i < navLinks.length; i++) {
            navLinks[i].addEventListener('click', function (e) {
                var link = this;
                if (link.href && link.href.indexOf('#') === -1 && !link.href.includes('javascript')) {
                    if (overlay) {
                        overlay.style.display = 'flex';
                    }
                }
            });
        }
    }

    /**
     * Initialize dark mode toggle functionality
     */
    function initDarkMode() {
        var savedMode = localStorage.getItem('darkMode');
        if (savedMode === 'enabled') {
            document.body.classList.add('dark-mode');
            var themeIcon = document.querySelector('#themeToggle i');
            if (themeIcon) {
                themeIcon.classList.remove('fa-moon');
                themeIcon.classList.add('fa-sun');
            }
        }
    }

    /**
     * Initialize active nav link highlighting
     */
    function initActiveNavLink() {
        var currentUrl = window.location.pathname;
        var navLinks = document.querySelectorAll('.nav-link');

        for (var i = 0; i < navLinks.length; i++) {
            var link = navLinks[i];
            var href = link.getAttribute('href');
            if (href && currentUrl.indexOf(href) !== -1) {
                link.classList.add('active');
            }
        }
    }

    /**
     * Initialize theme toggle button
     */
    function initThemeToggle() {
        var toggleBtn = document.getElementById('themeToggle');
        if (toggleBtn) {
            toggleBtn.onclick = function () {
                toggleDarkMode();
            };
        }
    }

    /**
     * Toggle dark mode
     */
    window.toggleDarkMode = function () {
        document.body.classList.toggle('dark-mode');
        var themeIcon = document.querySelector('#themeToggle i');

        if (themeIcon) {
            if (document.body.classList.contains('dark-mode')) {
                themeIcon.classList.remove('fa-moon');
                themeIcon.classList.add('fa-sun');
                localStorage.setItem('darkMode', 'enabled');
            } else {
                themeIcon.classList.remove('fa-sun');
                themeIcon.classList.add('fa-moon');
                localStorage.setItem('darkMode', 'disabled');
            }
        }
    };

})();