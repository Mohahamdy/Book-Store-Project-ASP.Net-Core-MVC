
const sideLinks = document.querySelectorAll('.sidebar .side-menu li a:not(.logout)');

sideLinks.forEach(item => {
    item.addEventListener('click', (e) => {

        // Remove 'active' class from all other items
        sideLinks.forEach(i => {
            i.parentElement.classList.remove('active');
        });
        // Add 'active' class to the clicked item's parent element
        item.parentElement.classList.add('active');
        console.log(item.textContent);
        // Store the id or any unique identifier of the clicked item in local storage
        localStorage.setItem('activeMenuItem', item.textContent.trim());
    });

    // Check if there's a stored active menu item and set its class accordingly
    const storedActiveMenuItem = localStorage.getItem('activeMenuItem');
    if (storedActiveMenuItem && item.textContent.trim() === storedActiveMenuItem) {
        item.parentElement.classList.add('active');
    }
});

const menuBar = document.querySelector('.content nav .bx.bx-menu');
const sideBar = document.querySelector('.sidebar');

menuBar.addEventListener('click', () => {
    sideBar.classList.toggle('close');
});

const searchBtn = document.querySelector('.content nav form .form-input button');
const searchBtnIcon = document.querySelector('.content nav form .form-input button .bx');
const searchForm = document.querySelector('.content nav form');

searchBtn.addEventListener('click', function (e) {
    if (window.innerWidth < 576) {
        e.preventDefault;
        searchForm.classList.toggle('show');
        if (searchForm.classList.contains('show')) {
            searchBtnIcon.classList.replace('bx-search', 'bx-x');
        } else {
            searchBtnIcon.classList.replace('bx-x', 'bx-search');
        }
    }
});

window.addEventListener('resize', () => {
    if (window.innerWidth < 768) {
        sideBar.classList.add('close');
    } else {
        sideBar.classList.remove('close');
    }
    if (window.innerWidth > 576) {
        searchBtnIcon.classList.replace('bx-x', 'bx-search');
        searchForm.classList.remove('show');
    }
});



const toggler = document.getElementById('theme-toggle');
const storedTheme = localStorage.getItem('Theme');
if (storedTheme === "Dark") {
    document.body.classList.add('dark');
    toggler.checked = true;
} else {
    document.body.classList.remove('dark');
    toggler.checked = false;

}

toggler.addEventListener('change', function () {
    if (this.checked) {
        document.body.classList.add('dark');
        localStorage.setItem('Theme', "Dark");
    } else {
        document.body.classList.remove('dark');
        localStorage.setItem('Theme', "light");

    }
});
