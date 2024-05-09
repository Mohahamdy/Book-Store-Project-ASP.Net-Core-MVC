

// for rating stars movement and hovering


var ratingsContainer;

//var isAuthenticated = document.getElementById('authStatus').getAttribute('data-is-authenticated');
var isAuthenticated = document.getElementById('authStatus');
if (isAuthenticated) {
    document.addEventListener("DOMContentLoaded", function () {
        ratingsContainer = document.querySelector(".addRating");

        ratingsContainer.addEventListener("mousemove", function (event) {
            if (ratingsContainer.querySelector(".rateTxt").value == "0") {
                var newWidth = getRate(event);
                ratingsContainer.querySelector(".ratings-val").style.width = newWidth + "%";
            }
        });

        ratingsContainer.addEventListener("mouseleave", function () {
            if (ratingsContainer.querySelector(".rateTxt").value == "0") {
                ratingsContainer.querySelector(".ratings-val").style.width = "0%"; // Reset to default width
            }
        });


        ratingsContainer.addEventListener("click", function (event) {

            var newWidth = getRate(event);
            var newRate = Math.round(newWidth);
            // Send the new rate to the server or perform any other action
            ratingsContainer.querySelector(".ratings-val").style.width = newWidth + "%";
            document.getElementsByClassName("rateTxt")[0].value = newRate;
        });
    });

    ////////////////////////////////////////////////////////////////////////////////////////////////////

    ///// To add Comment and rate /////
    document.getElementById("newsletter-btn").addEventListener("click", function (e) {
        e.preventDefault(); //to prevent refreshing the page because of type submit

        var bookid = document.getElementById("bookId").value;
        var comment = document.getElementById("txtComment").value;
        var rate = document.getElementById("rateTxt").value;

        if (comment == "") {
            toastr.error("You must add Comment.");
        }
        else if (rate == 0) {
            toastr.error("You must select Rate.");
        }
        else {
            $.ajax({
                url: '/Home/addReview',
                type: 'POST',
                dataType: 'json',
                data: { bookID: bookid, comment: comment, rate: rate },
                success: function (response) {

                    if (response == "no more than one") {
                        toastr.error("sorry, you mustn't add more than one review per book..");
                        clearReview();
                    }
                    else if (response == "no user") {
                        toastr.error("Please Login again...");
                    }
                    else {
                        toastr.success("your Review is Added Successfully.");
                        var newReview = '<div class="row no-gutters">'
                            + '<div class="col-auto">'
                            + '<div class="ratings-container">'
                            + '<div class="ratings">'
                            + '<div class="ratings-val" style="width: ' + (response.rate * 10) + '%"></div>'
                            + '</div>'
                            + '</div>'
                            + '<span class="review-date">Just Now</span>'
                            + '</div>'
                            + '<div class="col">'
                            + '<h4>' + response.userFName + ' ' + response.userLName + ' (You)</h4>'
                            + '<div class="review-content">'
                            + '<p>' + response.comment + '</p>'
                            + '</div>'
                            + '<div class="review-action">'
                            + '<a href="#"><i class="icon-thumbs-up"></i>Helpful (2)</a>'
                            + '<a href="#"><i class="icon-thumbs-down"></i>Unhelpful (0)</a>'
                            + '</div>'
                            + '</div>'
                            + '</div>';

                        var reviewDiv = document.createElement("div");
                        reviewDiv.className = "review animateReview";
                        reviewDiv.innerHTML = newReview;

                        document.getElementsByClassName("containerReviews")[0].prepend(reviewDiv);
                        clearReview();

                        if (document.getElementById("noReviews") != null) {
                            document.getElementById("noReviews").remove();
                        }
                    }
                },
                error: function (xhr, status, error) {
                    // Handle error
                    toastr.error("Something wrong, Try again!!");

                }
            });
        }
    });

}

function clearReview() {
    document.getElementById("txtComment").value = "";
    ratingsContainer.querySelector(".ratings-val").style.width = "0";
}
function getRate(event) {
    var containerRect = ratingsContainer.getBoundingClientRect();
    var mouseX = event.clientX - containerRect.left;
    var containerWidth = containerRect.width;
    return (mouseX / containerWidth) * 100;
}


////////////////////////////////////////////////////////////////////////////////////////////////////
