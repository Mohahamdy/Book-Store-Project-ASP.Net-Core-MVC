//dataTable
$('#myTable').DataTable();

//////////////////////////////////////////////////////////////////////////

var myDiv = document.getElementsByClassName("hoveredBlock")[0];
var links = myDiv.querySelectorAll("a");

// Get the current URL path
var currentPath = window.location.pathname;

// Loop through each <a> tag
links.forEach(function (link) {
    // Check if the href attribute matches the current path
    if (link.getAttribute("href") === currentPath) {
        // Add the "active" class to the matching <a> tag
        link.classList.add("active");
    }
});