
// main data | JSON
var stats;

// get the data drom the Stats action in Admin Controller
$.ajax({
    async: false,
    type: 'post',
    url: '/Admin/Stats',
    success: function (data) {
        stats = data;
    }
});


var admins = stats.admins   // # of admins
var users = stats.users     // # of users depending on AccountType (particular | organisation)
var products = stats.products   // # of products (on sale | ordered)
var category_products = stats.products_category     // # of products in eac hcategory

/* 
 * use thes 2 functions and just pass different data to the target chartId
 * title is optional
 */
const chart_bar = (chartId, data, title) => {
    var xValues = data.map(item => item.Name.split(' '))
    var yValues = data.map(item => item.Count)
    var barColors = [
        'rgb(255, 99, 132)',
        'rgb(54, 162, 235)',
        'rgb(255, 205, 86)',
        "#b91d47",
        "#00aba9",
        "#2b5797",
        "#e8c3b9",
        "#1e7145"
    ];

    new Chart(chartId, {
        type: "bar",
        data: {
            labels: xValues,
            datasets: [{
                backgroundColor: barColors,
                data: yValues
            }]
        },
        options: {
            legend: { display: false },
            title: {
                display: title ? true : false,
                text: title ?? ""
            }
        }
    });
}

const chart_donught = (chartId, data, title) => {
    var xValues2 = Object.keys(data);
    var yValues2 = Object.values(data);
    var barColors2 = [
        'rgb(255, 99, 132)',
        'rgb(54, 162, 235)',
        'rgb(255, 205, 86)',
        "#b91d47",
        "#00aba9",
        "#2b5797",
        "#e8c3b9",
        "#1e7145"
    ];

    new Chart(chartId, {
        type: "doughnut",
        data: {
            labels: xValues2,
            datasets: [{
                backgroundColor: barColors2,
                data: yValues2
            }]
        },
        options: {
            title: {
                display: title ? true : false,
                text: title ?? ""
            }
        }
    });
}

/*
 * change the value of the client, admins & products count
 */
const totals = () => {
    $('#totalClients').text(Object.values(users).reduce((a, v) => a + v, 0))
    $('#totalAdmins').text(admins)
    $('#totalProducts').text(products.Products)
}

/*
 * Fill the data on the page
 */

totals()

chart_bar("products_by_category", category_products)
chart_donught("products", products)
chart_donught("users", users)