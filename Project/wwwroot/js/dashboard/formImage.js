function chooseImage(event) {
    const fileInput = document.getElementById('imageInput');
    fileInput.click();
}

// Function to preview the selected image
function previewImage(event) {
    const file = event.target.files[0];
    const reader = new FileReader();

    reader.onload = function (e) {
        const imgElement = document.getElementById('currentImage');
        imgElement.src = e.target.result;
    };

    reader.readAsDataURL(file);
}