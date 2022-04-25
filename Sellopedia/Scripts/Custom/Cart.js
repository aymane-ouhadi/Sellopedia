
// --------------- Cart & Checkout --------------- //

$(document).ready(function () {
    //------------- Add to cart jquery functionality
    $("#cart-count").text((JSON.parse(localStorage.getItem("Cart")) || []).length)
    $(".add-to-cart").click(function () {
        $.get("/Shop/AddToCart", { product_id: $(this).find(".product_id").val() }, function (data) {
            var cartItems = JSON.parse(localStorage.getItem("Cart")) || []
            if (!cartItems.some(item => item.CurrentProduct.ProductId === data.CurrentProduct.ProductId)) {
                cartItems.push(data)
                localStorage.setItem("Cart", JSON.stringify(cartItems))
            }
            $("#cart-count").text(cartItems.length)
        });
    })


    displayCartItems()
    updateCartSummary()
    displayCheckoutItems()

});

//------------- Cart displaying jquery functionality
const CartItem = (cartItem) => {
    return (
        `<tr>
            <td class="align-middle"><img src="${cartItem.CurrentProduct.ProductImage}" alt="${cartItem.CurrentProduct.ProductName}" style="width: 50px; margin-right: 2rem;">${cartItem.CurrentProduct.ProductName.substring(0, 15)}...</td>
            <td class="align-middle">${cartItem.CurrentProduct.ProductPrice}  DH</td>
            <td class="align-middle">
                <div class="input-group quantity mx-auto" style="width: 100px;">
                    <div class="input-group-btn">
                        <button class="btn btn-sm btn-primary btn-minus" onclick="handleDecrement(${cartItem.CurrentProduct.ProductId})">
                            <i class="fa fa-minus"></i>
                        </button>
                    </div>
                    <input type="text" class="form-control form-control-sm bg-secondary text-center item-quantity" value="${cartItem.Quantity}">
                    <div class="input-group-btn">
                        <button class="btn btn-sm btn-primary btn-plus" onclick="handleIncrement(${cartItem.CurrentProduct.ProductId})">
                            <i class="fa fa-plus"></i>
                        </button>
                    </div>
                </div>
            </td>
            <td class="align-middle">${cartItem.OrderPrice}  DH</td>
            <td class="align-middle">
                <button class="btn btn-sm btn-primary" onclick="handleRemove(${cartItem.CurrentProduct.ProductId})">
                    <i class="fa fa-times"></i>
                </button>
            </td>
        </tr>`
    )
}

const displayCartItems = () => {
    $("#cart-list").html((JSON.parse(localStorage.getItem("Cart")) || [])
        .map(item => CartItem(item)).join(""))
}

const updateCartSummary = () => {
    var cart = (JSON.parse(localStorage.getItem("Cart")) || [])
    var total = cart.reduce((previous, current) => previous + current.OrderPrice, 0)
    $(".total").text(`${total} DH`)
    if (cart.length == 0) $(".proceed-to-checkout").attr("disabled", true)
    else $(".proceed-to-checkout").attr("disabled", false)
}

//------------- Cart Items manipulating jquery functionality
const handleDecrement = (product_id) => {
    var cart = JSON.parse(localStorage.getItem("Cart")) || []
    cart = cart.map(item => {
        if (item.CurrentProduct.ProductId == product_id && item.Quantity > 1) {
            return { ...item, Quantity: item.Quantity - 1, OrderPrice: (item.Quantity - 1) * item.CurrentProduct.ProductPrice }
        }
        return item
    })
    localStorage.setItem("Cart", JSON.stringify(cart))
    displayCartItems()
    updateCartSummary()
}

const handleIncrement = (product_id) => {
    var cart = JSON.parse(localStorage.getItem("Cart")) || []
    cart = cart.map(item => {
        if (item.CurrentProduct.ProductId == product_id) {
            return { ...item, Quantity: item.Quantity + 1, OrderPrice: (item.Quantity + 1) * item.CurrentProduct.ProductPrice }
        }
        return item
    })
    localStorage.setItem("Cart", JSON.stringify(cart))
    displayCartItems()
    updateCartSummary()
}

const handleRemove = (product_id) => {
    var cart = JSON.parse(localStorage.getItem("Cart")) || []
    cart = cart.filter(item => item.CurrentProduct.ProductId != product_id)
    localStorage.setItem("Cart", JSON.stringify(cart))
    displayCartItems()
    $("#cart-count").text((JSON.parse(localStorage.getItem("Cart")) || []).length)
    updateCartSummary()
}


//------------- Checkout jquery functionality
const CheckoutItem = (checkoutItem) => {
    return (
        `<div class="d-flex justify-content-between">
            <p>${checkoutItem.CurrentProduct.ProductName.substring(0, 25)} ... | ${checkoutItem.Quantity}</p>
            <p>${checkoutItem.OrderPrice} DH</p>
        </div>`
    )
}

const displayCheckoutItems = () => {
    var cart = JSON.parse(localStorage.getItem("Cart")) || []
    var total = cart.reduce((previous, current) => previous + current.OrderPrice, 0)
    $(".checkout-card-header").after(
        cart.map(item => CheckoutItem(item)).join("")
    )
    $(".checkout-total").text(`${total} DH`)
}

const confirmOrder = () => {
    var orders = (JSON.parse(localStorage.getItem("Cart")) || [])
    orders = orders.map(order => ({
        UserId: order.UserId,
        ProductId: order.CurrentProduct.ProductId,
        OrderPrice: order.OrderPrice,
        Quantity: order.Quantity,
        CartId: 1
    }))
    var total = orders.reduce((previous, current) => previous + current.OrderPrice, 0)
    console.log(total)
    $.post("/Shop/ConfirmOrder", { Id: $("#Id").val(), Address: $("#Address").val(), Orders: orders, TotalPrice: total }, function (data) {
        if (data.result == 'Redirect') {
            window.location = data.url
        }
    });
}
