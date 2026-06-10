// =============================================
// CREATE TASK PAGE CLIENT-SIDE FUNCTIONALITY
// =============================================

(function () {
    'use strict';

    document.addEventListener('DOMContentLoaded', function () {
        initFileUpload();
        initLoadingButton();
    });

    /**
     * Initialize file upload functionality
     */
    function initFileUpload() {
        const uploadArea = document.getElementById('uploadArea');
        const fileInput = getServerControl('fuAttachment');
        const fileInfo = document.getElementById('fileInfo');
        const fileNameSpan = document.getElementById('fileName');

        if (!uploadArea || !fileInput) return;

        // Click to upload
        uploadArea.addEventListener('click', function () {
            fileInput.click();
        });

        // File selection change
        fileInput.addEventListener('change', function () {
            if (this.files && this.files[0]) {
                const file = this.files[0];
                const maxSize = 5 * 1024 * 1024; // 5MB
                const allowedExtensions = /\.(pdf|doc|docx|jpg|jpeg|png)$/i;

                if (file.size > maxSize) {
                    alert('File size exceeds 5MB limit');
                    this.value = '';
                    return;
                }

                if (!allowedExtensions.test(file.name)) {
                    alert('Invalid file type. Allowed: PDF, DOC, DOCX, JPG, PNG');
                    this.value = '';
                    return;
                }

                if (fileNameSpan) fileNameSpan.textContent = file.name;
                if (fileInfo) fileInfo.style.display = 'inline-flex';
                if (uploadArea) uploadArea.style.borderColor = '#28a745';
            }
        });

        // Drag and drop
        uploadArea.addEventListener('dragover', function (e) {
            e.preventDefault();
            this.style.borderColor = '#667eea';
            this.style.background = '#f8f9ff';
        });

        uploadArea.addEventListener('dragleave', function (e) {
            e.preventDefault();
            this.style.borderColor = '#e0e0e0';
            this.style.background = '#fafafa';
        });

        uploadArea.addEventListener('drop', function (e) {
            e.preventDefault();
            this.style.borderColor = '#e0e0e0';
            this.style.background = '#fafafa';

            if (e.dataTransfer.files && e.dataTransfer.files[0]) {
                fileInput.files = e.dataTransfer.files;
                const event = new Event('change');
                fileInput.dispatchEvent(event);
            }
        });
    }

    /**
     * Initialize loading state on button click
     */
    function initLoadingButton() {
        const createBtn = getServerControl('btnCreate');

        if (createBtn) {
            createBtn.addEventListener('click', function () {
                this.classList.add('btn-loading');
                setTimeout(() => {
                    this.classList.remove('btn-loading');
                }, 5000);
            });
        }
    }

    /**
     * Clear file upload
     */
    window.clearFile = function () {
        const fileInput = getServerControl('fuAttachment');
        const fileInfo = document.getElementById('fileInfo');
        const uploadArea = document.getElementById('uploadArea');

        if (fileInput) fileInput.value = '';
        if (fileInfo) fileInfo.style.display = 'none';
        if (uploadArea) uploadArea.style.borderColor = '#e0e0e0';
    };

    /**
     * Get server control by client ID
     */
    function getServerControl(controlId) {
        const element = document.getElementById(controlId);
        if (element) return element;
        return document.querySelector(`[id$="_${controlId}"]`);
    }

})();