// --- get all the users in the page
var data = $('tbody > tr')  

// ----------- Filters --------- //
const filterOrderBy = () => {
    var selectValues = {
        "name": [
            'Asc',
            'Des'
        ],
        "gender": [
            ...new Set(
                data.map((index, value) => {
                    return value.dataset.gender
                })
            )
        ],
        "country": [
            ...new Set(
                data.map((index, value) => {
                    return value.dataset.country
                })
            )
        ],
        "city": [
            ...new Set(
                data.map((index, value) => {
                    return value.dataset.city
                })
            )
        ],
        "accounttype": [
            ...new Set(
                data.map((index, value) => {
                    return value.dataset.accounttype
                })
            )
        ],
        "whitelist": [
            ...new Set(
                data.map((index, value) => {
                    return value.dataset.whitelist
                })
            )
        ]
    };

    const applyFilter = () => {
        if ($filter.val() != null) {
            data
                .hide()
                .filter(function () {
                    return $(this).data($filter.val()) == $order.val()
                })
                .show()
        }
        else {
            data.show()
        }
    }

    var $filter = $('#filterby');
    var $order = $('#orderby');

    $filter.change(function () {
        console.log($filter.val())
        $order.empty().append(function () {
            var output = '';
            $.each(selectValues[$filter.val()], function (key, value) {
                output += '<option value=\"' + value + '\">' + value + '</option>';
            });
            return output;
        });

        // apply the filter on filterby change
        applyFilter()
    }).change();

    // apply the filter on orderby change
    $order.change(function () {
        applyFilter()
    })
}

// ----------- Search ----------- //
const search = () => {
    const search_filter = (search_text) => {
        data
            .hide()
            .filter(function () {
                return $(this).data('name').toLowerCase().includes(search_text.toLowerCase())
            })
            .show()
    }

    $("#searchLayout").on("input", function () {
        let search_text = $(this).val()

        search_filter(search_text)
    });
}

// ----------- Clear all filters ----------- //
const clearFilters = () => {
    $('#clearFilters').click(function (e) {
        e.preventDefault()
        $('#filterby').val('').trigger('change')
    })
}

// ----------- Main -------- //
$(document).ready(function () {
    $('#search_text').autocomplete({
        source: function (req, res) {
            $.ajax({
                url: '/Admin/SearchUser',
                type: 'POST',
                dataType: 'json',
                data: { search_text: req.term },
                success: function (data) {
                    res($.map(data, function (item) {
                        return { label: item, value: item }
                    }))
                }
            })
        }
    })

    filterOrderBy()
    search()
    clearFilters()
})