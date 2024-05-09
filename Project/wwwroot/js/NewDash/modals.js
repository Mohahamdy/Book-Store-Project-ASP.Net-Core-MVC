
//Books

if (window.location.pathname == "/newdashboard/books") {
    document.addEventListener("DOMContentLoaded", function () {
        //for inputs in the book info popup
        document.getElementsByClassName("btnShowComments")[0].addEventListener("click", function () {
            var bookid = parseInt(document.getElementById("bookId").innerHTML);
            redirectToBookComments(bookid);
        });
        document.getElementsByClassName("btnDeleteBook")[0].addEventListener("click", function () {
            var bookid = parseInt(document.getElementById("bookId").innerHTML);
            redirectToDeleteBook(bookid);
        });
        document.getElementsByClassName("btnAddBook")[0].addEventListener("click", function () {
            var bookid = parseInt(document.getElementById("bookId").innerHTML);
            redirectToAddBook(bookid);
        });
        document.getElementsByClassName("btnEditBook")[0].addEventListener("click", function () {
            var bookid = parseInt(document.getElementById("bookId").innerHTML);
            redirectToEditBook(bookid);
        });
    });
}

function BookInfo(bookId) {
    $.ajax({
        url: '/newDashboard/GetBookDeatils/' + bookId,
        type: 'GET',
        dataType: 'json',
        success: function (obj) {
            //src = "/images/shop/@item.Image"
            var imagePath = '/images/shop/' + obj.bookobj.image;
            document.getElementsByClassName("photo-main")[0].innerHTML = '<img style="height: 400px;width: 300px; " src="' + imagePath + '">';

            document.getElementById("bookId").textContent = obj.bookobj.id;
            document.getElementById("bookRate").textContent = obj.bookobj.rate;
            document.getElementById("bookTitle").textContent = obj.bookobj.name;
            document.getElementById("bookPrice").textContent = obj.bookobj.price;
            document.getElementById("bookDesc").textContent = obj.bookobj.description;
            document.getElementById("bookCategory").textContent = obj.category;
            document.getElementById("bookAuthor").textContent = obj.author;
            document.getElementById("bookDiscount").textContent = obj.discount;
            document.getElementById("bookAdmin").textContent = obj.admin;
            if (obj.bookobj.quantity == 0) {
                document.getElementById("bookQuant").style.color = "red";
                document.getElementById("bookQuant").textContent = obj.bookobj.quantity + " (Out Of Stock)";
            }
            else {
                document.getElementById("bookQuant").textContent = obj.bookobj.quantity;
                document.getElementById("bookQuant").style.color = "#757575";
            }
            if (obj.bookobj.isAvailable == true) {
                isAvailableTrue();
            }
            else {
                isAvailableFalse();
            }

            console.log(obj);
        },
        error: function (xhr, status, error) {
            // Handle error
            console.log("noooooo")

        }
    });

}


function redirectToBookComments(id) {
    // Construct the URL for the action
    let url = `/newDashboard/BookComments/${id}`;

    // Redirect the user to the URL
    window.location.href = url;
}


function redirectToDeleteBook(id) {
    Swal.fire({
        title: "Are You Sure to delete it?",
        showDenyButton: true,
        showCancelButton: true,
        showConfirmButton: false,
        denyButtonText: `Yes, delete it`
    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isDenied) {

            $.ajax({
                url: '/newDashboard/DeleteBook/' + id,
                type: 'GET',
                dataType: 'json',
                success: function (obj) {
                    if (obj == "Done") {
                        Swal.fire({ title: "The Book is Deleted Successfully, and you can get it back after clicking on Add", showCancelButton: true, showConfirmButton: false });
                        //for table
                        document.getElementById("delete_" + id).setAttribute('hidden', true);
                        document.getElementById("add_" + id).removeAttribute('hidden');
                        document.getElementById("isAvail_" + id).textContent = "False";
                        //for modal popup (book info popup)
                        isAvailableFalse();
                    }
                },
                error: function (xhr, status, error) {
                    Swal.fire("Something wrong, Try again", "", "OK", "error");
                }
            });
        }
    });
}


function redirectToAddBook(id) {
    Swal.fire({
        title: "Are You Sure to Add it?",
        showCancelButton: true,
        showConfirmButton: true,
        confirmButtonText: `Yes, Add it`
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/newDashboard/AddBook/' + id,
                type: 'GET',
                dataType: 'json',
                success: function (obj) {
                    if (obj == "Done") {
                        Swal.fire({ title: "The Book is Added Successfully", showCancelButton: true, showConfirmButton: false });
                        //for table
                        document.getElementById("add_" + id).setAttribute('hidden', true);
                        document.getElementById("delete_" + id).removeAttribute('hidden');
                        document.getElementById("isAvail_" + id).textContent = "True";
                        //for modal popup (book info popup)
                        isAvailableTrue();
                    }
                },
                error: function (xhr, status, error) {
                    Swal.fire("Something wrong, Try again", "", "OK", "error");
                }
            });
        }
    });

}


function redirectToEditBook(id) {
    let url = `/newDashboard/EditBook/${id}`;
    window.location.href = url;
}



function isAvailableTrue() {
    //document.getElementsByClassName("btnDeleteBook")[0].removeAttribute('hidden');
    //document.getElementsByClassName("btnAddBook")[0].setAttribute('hidden', true);
    document.getElementById("bookIsAvailable").textContent = "True";
    document.getElementById("bookIsAvailable").style.color = "#757575";
}
function isAvailableFalse() {
    document.getElementsByClassName("btnAddBook")[0].removeAttribute('hidden');
    document.getElementsByClassName("btnDeleteBook")[0].setAttribute('hidden', true);
    document.getElementById("bookIsAvailable").textContent = "False";
    document.getElementById("bookIsAvailable").style.color = "red";
}


//////////////////////////////////////////////////////////////////////////////////////////////////////

// Categories

function redirectToDeleteCategory(id) {
    Swal.fire({
        title: "Are You Sure to delete it?",
        showDenyButton: true,
        showCancelButton: true,
        showConfirmButton: false,
        denyButtonText: `Yes, delete it`
    }).then((result) => {
        /* Read more about isConfirmed, isDenied below */
        if (result.isDenied) {

            $.ajax({
                url: '/newDashboard/DeleteCategory/' + id,
                type: 'GET',
                dataType: 'json',
                success: function (obj) {
                    if (obj == "Done") {
                        Swal.fire({ title: "The Category is Deleted Successfully, and you can get it back after clicking on Add", showCancelButton: true, showConfirmButton: false });
                        //for table
                        document.getElementById("delete_" + id).setAttribute('hidden', true);
                        document.getElementById("add_" + id).removeAttribute('hidden');
                        document.getElementById("isAvail_" + id).textContent = "False";

                    }

                },
                error: function (xhr, status, error) {
                    Swal.fire("Something wrong, Try again", "", "OK", "error");
                }
            });
        }
    });
}


function redirectToAddCategory(id) {
    Swal.fire({
        title: "Are You Sure to Add it?",
        showCancelButton: true,
        showConfirmButton: true,
        confirmButtonText: `Yes, Add it`
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: '/newDashboard/AddCategory/' + id,
                type: 'GET',
                dataType: 'json',
                success: function (obj) {
                    if (obj == "Done") {
                        Swal.fire({ title: "The Category is Added Successfully", showCancelButton: true, showConfirmButton: false });
                        //for table
                        document.getElementById("add_" + id).setAttribute('hidden', true);
                        document.getElementById("delete_" + id).removeAttribute('hidden');
                        document.getElementById("isAvail_" + id).textContent = "True";

                    }


                },
                error: function (xhr, status, error) {
                    Swal.fire("Something wrong, Try again", "", "OK", "error");
                }
            });
        }
    });

}


function redirectToEditCategory(id) {
    let url = `/newDashboard/EditCategory/${id}`;
    window.location.href = url;
}


