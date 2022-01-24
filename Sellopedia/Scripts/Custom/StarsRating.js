
// Stars rating system

$(document).ready(function () {
    $('.star').click(function (event) {
        console.log(`Score: ${event.target.id}`)
        // set the value of Score in the Review Model to the value of the star
        $('#Score').val(parseInt(event.target.id))
        // force the state of the selected star to be hovered
        $(this).toggleClass("hovered")
        $('.star').map(function () {
            if ($(this).hasClass("hovered") && this.id != event.target.id) {
                $(this).toggleClass("hovered")
            }
        })
    })
})