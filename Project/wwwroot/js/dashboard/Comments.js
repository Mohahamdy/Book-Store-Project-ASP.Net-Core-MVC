function redirectToShowComment(userId,bookId) {
    $.ajax({
        url: '/Dashboard/HideShowComment?userId=' + userId + '&bookId=' + bookId+'&status=show',
        type: 'GET',
        dataType: 'json',
        success: function (obj) {
            if (obj == "Done") {
                toster();
                toastr.success("The Comment is Shown Successfully.");
                document.getElementById("show_" + userId + bookId).setAttribute('hidden', true);
                document.getElementById("hide_" + userId + bookId).removeAttribute('hidden');
                document.getElementById("isAvail_" + userId + bookId).textContent = "True";
            }
        },
        error: function (xhr, status, error) {
            Swal.fire("Something wrong, Try again", "", "OK", "error");
        }
    });
}

function redirectToHideComment(userId, bookId) {
    $.ajax({
        url: '/Dashboard/HideShowComment?userId=' + userId + '&bookId=' + bookId + '&status=hide',
        type: 'GET',
        dataType: 'json',
        success: function (obj) {
            if (obj == "Done") {
                toster();
                toastr.success("The Comment is hidden Successfully.");
                document.getElementById("hide_" + userId + bookId).setAttribute('hidden', true);
                document.getElementById("show_" + userId + bookId).removeAttribute('hidden');
                document.getElementById("isAvail_" + userId + bookId).textContent = "False";
            }
        },
        error: function (xhr, status, error) {
            Swal.fire("Something wrong, Try again", "", "OK", "error");
        }
    });
}

function toster() {
    toastr.options = {
        "closeButton": true,
        "newestOnTop": false,
        "progressBar": true,
        "preventDuplicates": false,
        "onclick": null,
        "timeOut": "5000",
        "extendedTimeOut": "1000",
        "showEasing": "swing",
        "hideEasing": "linear",
        "showMethod": "fadeIn",
        "hideMethod": "fadeOut",
    }
}