// ------------------ Icon Picker ------------------ //
// options
const iconPickerOptions = {
    title: 'Choose an icon',
}

//-------------- Create new Category
// set icon picker
$('#iconName').iconpicker(iconPickerOptions);

// change the icon in display
$('#iconName').on('iconpickerSelected', function (e) {
    $('.picker-target').get(0).className = 'picker-target fa-2x ' +
        e.iconpickerInstance.options.fullClassFormatter(e.iconpickerValue);
});

//-------------- Edit Category
$('#iconNameEdit').iconpicker(iconPickerOptions);
$('#iconNameEdit').on('iconpickerSelected', function (e) {
    $('.picker-target').get(1).className = 'picker-target fa-2x ' +
        e.iconpickerInstance.options.fullClassFormatter(e.iconpickerValue);
});
//-----------------------------------

// ---------------- Form Validation ---------------- //
// validate input takes the id of the inputs to validate
// and set the danger & success borders
const validateInput = (...input) => {
    var isValid = true;

    input.map(field => {
        if ($(field).val() == "") {
            $(field).addClass('border border-danger')
            $(field).removeClass('border border-success')
            isValid = false
        }
        else {
            $(field).removeClass('border border-danger')
            $(field).addClass('border border-success')
        }
    })

    return isValid
}

// handle submit validation
$('#submit').click(function () {
    return validateInput('#iconName', '#categoryName')
})

$('#submitEdit').click(function () {
    return validateInput('#iconNameEdit', '#categoryNameEdit')
})

// modal to edit category
$('.icon-list-demo').click(function (e) {
    //console.log(e.target.childNodes)

    // open the modal
    $('#editCategoryModal').modal('show');

    // get the inputs from the form inside the modal
    var EditFormInputs = $("#editCategoryForm :input")
    //console.log($(EditFormInputs))

    // get the selected category id, name & icon
    var categoryId = e.target.childNodes[1].value
    var categoryIcon = e.target.childNodes[3].className
    var categoryName = e.target.childNodes[4].innerHTML

    // fill the inputs with the current category
    $($(EditFormInputs)[0]).val(categoryId)
    $($(EditFormInputs)[1]).val(categoryName)
    $($(EditFormInputs)[2]).val(categoryIcon)

    var pickerTarget = $('#editCategoryForm .picker-target')[0]
    //console.log(pickerTarget)
    //pickerTarget.className = categoryIcon + ' fa-2x picker-targe'
})