
// AutoComplete - search

$(document).ready(function () {
    $('#search_text').autocomplete({
        //source: '@Url.Action("SearchProduct", "Shop")'
        source: function (req, res) {
            $.ajax({
                url: '/shop/SearchProduct',
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
})