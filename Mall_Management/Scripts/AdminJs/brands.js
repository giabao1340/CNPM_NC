﻿document.addEventListener("DOMContentLoaded", function () {
    // Khởi tạo các menu có data-kt-menu-trigger
    var menus = [].slice.call(document.querySelectorAll('[data-kt-menu-trigger="true"]'));
    menus.map(function (menu) {
        KTMenu.createInstances(menu);
    });
});

document.getElementById('create__save').addEventListener('click', function () {
    // Lấy các giá trị từ form
    const brandName = document.getElementById('brandName').value;
    const image = document.getElementById('image').value;
    const floor = document.getElementById('floor').value;
    const description = document.getElementById('description').value;
    const url = document.getElementById('url').value;

    // Thực hiện kiểm tra các giá trị nếu cần

    // Gửi form hoặc xử lý theo ý muốn
    console.log({
        brandName: brandName,
        image: image,
        floor: floor,
        description: description,
        url: url
    });
})

document.getElementById('edit__save').addEventListener('click', function () {
    // Lấy các giá trị từ form
    const brandName = document.getElementById('brandName').value;
    const image = document.getElementById('image').value;
    const floor = document.getElementById('floor').value;
    const description = document.getElementById('description').value;
    const url = document.getElementById('url').value;

    // Thực hiện kiểm tra các giá trị nếu cần

    // Gửi form hoặc xử lý theo ý muốn
    console.log({
        brandName: brandName,
        image: image,
        description: description,
        url: url
    });

});
document.getElementById('delete__save').addEventListener('click', function () {
    // Lấy giá trị của Brand ID từ input ẩn (hoặc bất kỳ nguồn nào khác mà bạn lưu trữ ID thương hiệu cần xóa)
    const brandID = document.getElementById('deleteBrandID').value;

    // Thực hiện thao tác xóa hoặc gửi yêu cầu AJAX để xóa
    console.log({
        brandID: brandID
    });

    // Ví dụ: Gửi yêu cầu xóa qua AJAX
    $.ajax({
        url: '/Brand/Delete',
        type: 'POST',
        data: { id: brandID },
        success: function (response) {
            if (response.success) {
                alert('Thương hiệu đã được xóa thành công!');
                location.reload(); // Tải lại trang để cập nhật sau khi xóa
            } else {
                alert('Đã có lỗi xảy ra khi xóa thương hiệu.');
            }
        },
        error: function () {
            alert('Đã có lỗi xảy ra trong quá trình xóa thương hiệu.');
        }
    });
});



function createOpen(id, name, image, floor, description, url) {
    console.log("Create Open Called with ID: ", id);

    // Mở modal tạo mới
    var createModal = new bootstrap.Modal(document.getElementById('createBrandModal'));

    // Thiết lập các trường input của modal về giá trị mặc định (rỗng)
    //document.getElementById('createBrandName').value = '';
    //document.getElementById('createBrandImage').value = '';
    //document.getElementById('createBrandFloor').value = '';
    //document.getElementById('createBrandDescription').value = '';
    //document.getElementById('createBrandUrl').value = '';

    // Hiển thị modal
    createModal.show();
}



function editOpen(id, name, image, floor, description, url) {
    console.log("Edit Open Called with ID: ", id);
    // Mở modal chỉnh sửa
    var editModal = new bootstrap.Modal(document.getElementById('editBrandModal'));

    document.getElementById('editBrandID').value = id;
    document.getElementById('editBrandName').value = name;
    document.getElementById('editBrandFloor').value = floor;
    document.getElementById('editBrandDescription').value = description;
    document.getElementById('editBrandUrl').value = url;

    // Kiểm tra xem phần tử currentImage có tồn tại không
    var currentImageElement = document.getElementById('editBrandImage');
    if (currentImageElement) {
        // Hiển thị ảnh hiện tại nếu phần tử tồn tại
        currentImageElement.src = image;
    } else {
        console.log('Element with id="currentImage" not found in the DOM.');
    }

    editModal.show();
}

function deleteOpen(id, name) {
    console.log("Delete Open Called with ID: ", id);
    // Mở modal xóa
    var deleteModal = new bootstrap.Modal(document.getElementById('deleteBrandModal'));
    document.getElementById('deleteBrandID').value = id;
    document.getElementById('deleteBrandName').textContent = name;
    deleteModal.show();
}


function saveCreate() {
    // Lấy giá trị từ các trường nhập liệu trong modal
    var brandName = document.getElementById('brandName').value;
    var brandImage = document.getElementById('image').files[0]; // Lấy file từ input type="file"
    var brandFloor = document.getElementById('floor').value;
    var brandDescription = document.getElementById('description').value;
    var brandUrl = document.getElementById('url').value;

    // Kiểm tra các trường bắt buộc
    if (!brandName || !brandFloor) {
        alert("Vui lòng nhập đầy đủ thông tin các trường bắt buộc!");
        return;
    }

    // Tạo FormData để chứa dữ liệu cần gửi
    var formData = new FormData();
    formData.append("BrandName", brandName);
    formData.append("Image", brandImage);  // Thêm file ảnh vào FormData
    formData.append("Floor", brandFloor);
    formData.append("Description", brandDescription);
    formData.append("Url", brandUrl);

    // Gọi AJAX để gửi dữ liệu lên server
    $.ajax({
        url: '/Brand/Create',
        type: 'POST',
        data: formData,
        contentType: false, // Để jQuery không tự động đặt Content-Type
        processData: false, // Để jQuery không xử lý dữ liệu (vì đây là FormData)
        success: function (response) {
            if (response.success) {
                var createModal = bootstrap.Modal.getInstance(document.getElementById('createBrandModal'));
                createModal.hide();
                location.reload();  // Làm mới trang sau khi lưu thành công
            } else if (response.message === "exist") {
                alert("Tên thương hiệu đã tồn tại, vui lòng chọn tên khác.");
            } else {
                alert("Đã xảy ra lỗi khi thêm thương hiệu.");
            }
        },
        error: function () {
            alert("Đã xảy ra lỗi trong quá trình gửi yêu cầu.");
        }
    });
}



function saveEdit() {
    // Thu thập dữ liệu từ form
    var brandID = $("#editBrandID").val();
    var brandData = {
        BrandID: brandID,
        BrandName: $("#editBrandName").val(),
        Image: $("#editBrandImage").val(),
        Floor: $("#editBrandFloor").val(),
        Description: $("#editBrandDescription").val(),
        Url: $("#editBrandUrl").val()
    };

    // Gửi AJAX request để cập nhật thông tin thương hiệu
    $.ajax({
        url: '/Brand/Edit',  // Đảm bảo URL đúng với controller của bạn
        type: 'POST',
        data: { id: brandID, brand: brandData },  // Truyền id và dữ liệu brand
        success: function (response) {
            if (response === "success") {
                alert('Cập nhật thương hiệu thành công!');
                $('#editBrandModal').modal('hide'); // Đóng modal sau khi thành công
                location.reload();  // Tải lại trang để cập nhật
            } else if (response === "exist") {
                alert('Tên thương hiệu đã tồn tại!');
            } else {
                alert('Đã có lỗi xảy ra!');
            }
        },
        error: function () {
            alert('Đã có lỗi xảy ra trong quá trình cập nhật thương hiệu.');
        }
    });
}

function deleteOpen(id, name) {
    console.log("Delete Open Called with ID: ", id);
    // Mở modal xóa
    var deleteModal = new bootstrap.Modal(document.getElementById('deleteBrandModal'));
    document.getElementById('deleteBrandID').value = id;
    document.getElementById('deleteBrandName').textContent = name;
    deleteModal.show();
}

function confirmDelete() {
    var brandID = $("#editBrandID").val();  // Lấy ID của thương hiệu cần xóa
    // Gửi AJAX request để xóa thương hiệu
    $.ajax({
        url: '/Brand/Delete',  // Đảm bảo URL đúng với controller của bạn
        type: 'POST',
        data: { id: brandID },  // Gửi ID thương hiệu cần xóa
        success: function (response) {
            if (response.success) {
                //alert('Thương hiệu đã được xóa thành công!');
                $('#editBrandModal').modal('hide');  // Đóng modal sau khi xóa thành công
                location.reload();  // Tải lại trang để cập nhật
            } else {
                alert(response.message);
            }
        },
    });
}


document.getElementById('image').addEventListener('change', function () {
    var formData = new FormData();
    formData.append("image", this.files[0]);

    $.ajax({
        url: '/Brand/UploadImage',
        type: 'POST',
        data: formData,
        contentType: false,  // Không đặt Content-Type, FormData sẽ tự xử lý
        processData: false,  // Không xử lý dữ liệu theo kiểu mặc định của jQuery
        success: function (data) {
            if (data.filePath) {
                console.log('File đã upload thành công:', data.filePath);
                showUploadedImage(data.filePath);  // Hiển thị ảnh sau khi upload
            } else {
                console.log('Lỗi:', data.error);
            }
        },
        error: function (err) {
            console.error('Upload thất bại:', err);
        }
    });
});


function showUploadedImage(filePath) {
    document.getElementById('imagePreview').innerHTML = '<img src="' + filePath + '" style="max-width: 300px; height: auto;" />';
}


function previewImage() {
    var input = document.getElementById('image');
    var preview = document.getElementById('imagePreview');
    preview.innerHTML = ''; // Xóa ảnh xem trước cũ

    var file = input.files[0];
    if (file) {
        var reader = new FileReader();
        reader.onload = function (e) {
            var img = document.createElement('img');
            img.src = e.target.result;
            img.style.maxWidth = '300px'; // Kích thước ảnh xem trước
            img.style.height = 'auto';
            preview.appendChild(img);
        }
        reader.readAsDataURL(file);
    }
}