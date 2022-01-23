
// Price Filter Slider

// Read a page's GET URL variables and return them as an associative array.
function getUrlVars() {
    var vars = [], hash;
    var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
    for (var i = 0; i < hashes.length; i++) {
        hash = hashes[i].split('=');
        vars.push(hash[0]);
        vars[hash[0]] = hash[1];
    }
    return vars;
}

// Slider ---------------------->
var min = 0, max = 5000;

$(function () {
    $("#slider-range").slider({
        range: true,
        min: min,
        max: max,
        values: [
            getUrlVars()['minPrice'] ? getUrlVars()['minPrice'] : 50,
            getUrlVars()['maxPrice'] ? getUrlVars()['maxPrice'] : 5000
        ],
        slide: function (event, ui) {
            $('#minPrice').val(ui.values[0]);
            $('#maxPrice').val(ui.values[1]);
            $('#price').val(`${ui.values[0]}dh - ${ui.values[1]}dh`)
        }
    });
    $('#price').val(`${$("#slider-range").slider("values", 0)}dh - ${$("#slider-range").slider("values", 1)}dh`)
});

// Form submit -------------------------->
var form = $('#price-filter-form')
form.submit(function (e) {
    //e.preventDefault()

    //get the querystring of category & search filters if available
    if (getUrlVars()["categoryId"]) {
        $('#category-filter').val(getUrlVars()["categoryId"])
        $('#category-filter').removeAttr("disabled")
    }
    if (getUrlVars()["search"]) {
        $('#search-filter').val(getUrlVars()["search"])
        $('#search-filter').removeAttr("disabled")
    }

    $.ajax({
        type: form.attr('method'),
        url: form.attr('action'),
        data: form.serialize(),
        success: function (data) {
            console.log('Success:')
            console.log(form.serialize())
            console.log(form.serializeArray())
        },
        error: function (err) {
            console.log('Error:')
            console.log(err)
        }
    })
})