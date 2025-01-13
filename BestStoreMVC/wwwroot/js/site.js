
$('#textsearch').keyup(function () {
    debugger
    var typeValue = $(this).val();
    $('tbody tr').each(function () {
        if ($(this).text().search(new RegExp(typeValue, "i")) < 0) {
            $(this).fadeOut();
        }
        else {
            $(this).show();
        }
    })
});

var myIndex = 0;
carousel();

function carousel() {
    var i;
    var x = document.getElementsByClassName("mySlides");
    for (i = 0; i < x.length; i++) {
        x[i].style.display = "none";
    }
    myIndex++;
    if (myIndex > x.length) { myIndex = 1 }
    x[myIndex - 1].style.display = "block";
    setTimeout(carousel, 4000);
}
function showInfoDiv(product) {
    // Hide add to cart div
    document.getElementById('addToCartDiv').style.display = 'none';
    document.getElementById('showAdvertesment').style.display = 'none';
    // Populate product details
    var productInfoDiv = document.getElementById('productInfoContent');
    productInfoDiv.innerHTML = `
        <p><strong>Name:</strong> ${product.Name}</p>
        <p><strong>Brand:</strong> ${product.Brand}</p>
        <p><strong>Category:</strong> ${product.Category}</p>
        <p><strong>Price:</strong> ${product.Price}$</p>
        <p><strong>Created At:</strong> ${new Date(product.CreatedAt).toLocaleDateString()}</p>
        <img src="/Products/${product.ImageFileName}" style="max-width:200px;" />
    `;

    // Show the product info div
    document.getElementById('infoDiv').style.display = 'block';
}

function showAddToCartDiv(product) {
    // Hide product info div
    document.getElementById('infoDiv').style.display = 'none';
    document.getElementById('showAdvertesment').style.display = 'none';
    // Populate add to cart section
    var addToCartDiv = document.getElementById('addToCartContent');
    addToCartDiv.innerHTML = `
        <form id="addToCartForm" action="/Cart/AddToCart" method="post">
            <input type="hidden" name="id" value="${product.Id}" />
            <p><strong>Product:</strong> ${product.Name}</p>
            <p><strong>Price:</strong> ${product.Price}$</p>
            <img src="/Products/${product.ImageFileName}" style="max-width:200px;padding-bottom:15px;" />
            <div class="row mb-3">
                <label class="col-sm-4 col-form-label"><strong>Quantity</strong></label>
                <div class="col-sm-8">
                    <input class="form-control" name="Cuantity" type="number" min="1" style="border-radius: 8px; box-shadow: inset 0 1px 3px rgba(0, 0, 0, 0.1); font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;" required />
                    <span asp-validation-for="Cuantity" class="text-danger"></span>
                </div>
            </div>
            <button type="submit" class="btn btn-success">Add to Cart</button>
        </form>
    `;

    // Show the add to cart div
    document.getElementById('addToCartDiv').style.display = 'block';
}
function showNewProductDiv() {
    document.getElementById('editProductDiv').style.display = 'none';
    document.getElementById('newProductDiv').style.display = 'block';
}

function showEditProductDiv(id, name, brand, category, price, description, ImageFileName) {
    document.getElementById('newProductDiv').style.display = 'none';
    document.getElementById('editProductDiv').style.display = 'block';
    document.getElementById('ProductId').value = id;
    document.getElementById('ProductName').value = name;
    document.getElementById('ProductBrand').value = brand;
    document.getElementById('ProductCategory').value = category;
    document.getElementById('ProductPrice').value = price;
    document.getElementById('ProductDescription').value = description;
    document.getElementById('ProductFileName').value = ImageFileName; ProductId
}


function addToCart(productId) {
    // Implement the logic to add the product to the cart
    alert('Product ' + productId + ' added to cart.');
}
