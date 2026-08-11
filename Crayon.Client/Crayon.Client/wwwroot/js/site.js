$(document).ready(function () {

    if ($.fn.DataTable) {
        $('.datatable').DataTable({
            pageLength: 25,
            order:      [],
            autoWidth:  false,
            language: {
                search:            '_INPUT_',
                searchPlaceholder: 'Search...',
                lengthMenu:        'Show _MENU_',
                info:              'Showing _START_ – _END_ of _TOTAL_',
                paginate: {
                    previous: '‹',
                    next:     '›'
                }
            }
        });
    }

});
