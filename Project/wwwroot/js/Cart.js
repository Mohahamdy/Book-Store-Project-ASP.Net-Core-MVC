

function addToCart(id, text) {
    let quantity;
    console.log("enterd");
    if (text == 'whithout quantity') { // just click on (Add to cart) button without select any quantity --> from any page except BookDetails
        quantity = 1;
    }
    else { //comes from BookDetails page & take the quantity from the input
        quantity = document.getElementById("quantity").value;
    }
    let flag = false; //to know if the book already exist and increse its quantity

    let arritems = JSON.parse(localStorage.getItem("cart") || "[]");

    if (arritems.length == 0) { //cart is empty
        setItem("cart", [{ "bookID": id, "quantity": quantity }]);
        toster();
        toastr.success("The Book is added to the Cart Successfully.");
    }
    else {  //cart has items --> we have some options (add book for the first time / book exist so <update quantity> or <say this is alredy exist and no action>)
        let newitems = [];
        arritems.forEach(obj => {
            if (obj.bookID == id) {  //selected book is exist in the cart
                flag = true;
                toster();
                if (text == 'whithout quantity') {   //comes from any page 
                    toastr.warning("The Book is already exist");
                }
                else {  //from bookdetails to update the quantity
                    obj.quantity = quantity;
                    toastr.success("The Book is already exist & update the quantity.");
                }
            }
            newitems.push(obj);
            if (newitems.length == arritems.length && flag == false) {  //the cart has items but this selected book isn't exist -->(add book for the first time)
                newitems.push({ "bookID": id, "quantity": quantity });
                toster();

                toastr.success("The Book is added to the Cart Successfully.");

            }

        });
        setItem("cart", newitems);
    }

}
function toster() {
    toastr.options = {
        "closeButton": true,
        "newestOnTop": false,
        "progressBar": true,
        "preventDuplicates": false,
        "onclick": null,
        "timeOut": "2000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut",
    }
}


function setItem(key, valueObj) {

    localStorage.setItem(key, JSON.stringify(valueObj));
}

