// =============================================
// SEARCH TASKS PAGE CLIENT-SIDE FUNCTIONALITY
// =============================================

(function () {
    'use strict';

    document.addEventListener('DOMContentLoaded', function () {
        initSearchButtonLoading();
        initFilterTasks();
    });

    /**
     * Initialize search button loading animation
     */
    function initSearchButtonLoading() {
        var searchBtn = getServerControl('btnSearch');
        if (searchBtn) {
            searchBtn.addEventListener('click', function () {
                var spinner = document.getElementById('loadingSpinner');
                if (spinner) spinner.style.display = 'block';
            });
        }
    }

    /**
     * Initialize filter tasks functionality
     */
    function initFilterTasks() {
        // Expose filterTasks and clearFilter to global scope for inline event handlers
        window.filterTasks = filterTasks;
        window.clearFilter = clearFilter;
    }

    /**
     * Filter tasks based on quick search input
     */
    function filterTasks() {
        var input = document.getElementById('txtQuickSearch');
        var filter = '';
        if (input) {
            filter = input.value.toUpperCase();
        }

        var gridView = document.getElementById('gvTasks');
        if (!gridView) return;

        var rows = gridView.getElementsByTagName('tr');
        var visibleCount = 0;
        var totalRows = 0;

        for (var i = 1; i < rows.length; i++) {
            var row = rows[i];
            var cells = row.getElementsByTagName('td');

            if (cells.length > 0) {
                totalRows++;
                var taskId = cells[0] ? cells[0].innerText || cells[0].textContent : '';
                var title = cells[1] ? cells[1].innerText || cells[1].textContent : '';
                var assignedTo = cells[2] ? cells[2].innerText || cells[2].textContent : '';
                var status = cells[3] ? cells[3].innerText || cells[3].textContent : '';

                if (filter === '') {
                    row.style.display = '';
                    visibleCount = totalRows;
                } else if (taskId.toUpperCase().indexOf(filter) > -1 ||
                    title.toUpperCase().indexOf(filter) > -1 ||
                    assignedTo.toUpperCase().indexOf(filter) > -1 ||
                    status.toUpperCase().indexOf(filter) > -1) {
                    row.style.display = '';
                    visibleCount++;
                } else {
                    row.style.display = 'none';
                }
            }
        }

        var filterCountSpan = document.getElementById('filterCount');
        if (filterCountSpan) {
            if (filter === '') {
                filterCountSpan.innerHTML = '<i class="fas fa-database me-2"></i>Showing all ' + totalRows + ' tasks';
                filterCountSpan.className = 'filter-count bg-light';
            } else {
                var icon = visibleCount > 0 ? 'fa-check-circle text-success' : 'fa-exclamation-circle text-danger';
                filterCountSpan.innerHTML = '<i class="fas ' + icon + ' me-2"></i>Showing ' + visibleCount + ' of ' + totalRows + ' tasks';
                filterCountSpan.className = visibleCount > 0 ? 'filter-count bg-success bg-opacity-10 text-success' : 'filter-count bg-danger bg-opacity-10 text-danger';
            }
        }
    }

    /**
     * Clear the quick filter input
     */
    function clearFilter() {
        var input = document.getElementById('txtQuickSearch');
        if (input) {
            input.value = '';
        }
        filterTasks();
    }

    /**
     * Get server control by client ID
     */
    function getServerControl(controlId) {
        var element = document.getElementById(controlId);
        if (element) return element;
        return document.querySelector('[id$="_' + controlId + '"]');
    }

})();