


document.addEventListener("DOMContentLoaded", function () {
    const menuItems = document.querySelectorAll('.main-nav  ul .Link');
    // Set the active class on the menu item based on sessionStorage
    const activeMenuItemIndex = sessionStorage.getItem('activeMenuItemIndex');
    if (activeMenuItemIndex !== null) {
        menuItems[activeMenuItemIndex].classList.add('active');
    }
    menuItems.forEach(function (item) {
        item.addEventListener('click', function (e) {
            // Toggle 'active' class on the clicked menu item
            this.classList.toggle('active');

            // Remove 'active' class from all other menu items
            menuItems.forEach(function (menuItem) {
                if (menuItem !== item) {
                    menuItem.classList.remove('active');
                }
            });

            // Store the index of the clicked menu item in sessionStorage
            const index = Array.from(menuItems).indexOf(item);
            sessionStorage.setItem('activeMenuItemIndex', index);
        });
    });


});
