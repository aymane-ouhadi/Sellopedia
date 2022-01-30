$('#fancy_upload').click(function () {
    $('#basic_upload').click()
});

$('#basic_upload').change(function (e) {
    var reader = new FileReader()

    reader.onload = function (e) {
        $('#profile_image').attr('src', e.target.result);
    }

    reader.readAsDataURL(e.target.files[0]);

})
