document.addEventListener("DOMContentLoaded", function () {
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

$('#create__save').click(function () {
    let formData = {
        brandName: $('#brandName').val(),
        image: $('#image').val(),
        description: $('#description').val(),
        url: $('#url').val(),
        floor: $('#floor').val()
    };

    $.ajax({
        url: '/Brand/Create',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(formData),
        success: function (response) {
            if (response.success) {
                alert('Thương hiệu đã được tạo thành công!');
                $('#create-modal').modal('hide');
                location.reload();
            } else {
                alert(response.message);
                // Hiển thị lỗi chi tiết nếu có
                if (response.errors) {
                    response.errors.forEach(function (error) {
                        console.log(error.errorMessage); // Hiển thị lỗi ở console hoặc tùy chỉnh hiển thị ở giao diện
                    });
                }
            }
        },
        error: function (xhr, status, error) {
            console.error("Error occurred: " + error);
            alert('Có lỗi xảy ra, vui lòng thử lại!');
        }
    });
});

function createOpen() {
    console.log("Create Open Called");
    // Mở modal thêm mới
    var createModal = new bootstrap.Modal(document.getElementById('create-modal'));

    // Xóa sạch các giá trị trong các input trước khi mở modal
    document.getElementById('createBrandName').value = '';
    document.getElementById('createBrandImage').value = '';
    document.getElementById('createBrandFloor').value = '';
    document.getElementById('createBrandDescription').value = '';
    document.getElementById('createBrandUrl').value = '';
    createModal.show();
}


function createOpen(id, name, image, floor, description, url) {
    console.log("Edit Open Called with ID: ", id);
    // Mở modal chỉnh sửa
    var editModal = new bootstrap.Modal(document.getElementById('createBrandModal'));
    document.getElementById('editBrandID').value = id;
    document.getElementById('editBrandName').value = name;
    document.getElementById('editBrandImage').value = image;
    document.getElementById('editBrandFloor').value = floor;
    document.getElementById('editBrandDescription').value = description;
    document.getElementById('editBrandUrl').value = url;
    editModal.show();
}


function editOpen(id, name, image, floor, description, url) {
    console.log("Edit Open Called with ID: ", id);
    // Mở modal chỉnh sửa
    var editModal = new bootstrap.Modal(document.getElementById('editBrandModal'));
    document.getElementById('editBrandID').value = id;
    document.getElementById('editBrandName').value = name;
    document.getElementById('editBrandImage').value = image;
    document.getElementById('editBrandFloor').value = floor;
    document.getElementById('editBrandDescription').value = description;
    document.getElementById('editBrandUrl').value = url;
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
    var brandName = document.getElementById('createBrandName').value;
    var brandImage = document.getElementById('createBrandImage').value;
    var brandFloor = document.getElementById('createBrandFloor').value;
    var brandDescription = document.getElementById('createBrandDescription').value;
    var brandUrl = document.getElementById('createBrandUrl').value;

    // Kiểm tra các trường bắt buộc
    if (!brandName || !brandFloor) {
        alert("Vui lòng nhập đầy đủ thông tin các trường bắt buộc!");
        return;
    }

    // Tạo đối tượng dữ liệu cần gửi lên server
    var brandData = {
        BrandName: brandName,
        Image: brandImage,
        Floor: brandFloor,
        Description: brandDescription,
        Url: brandUrl
    };

    // Gọi AJAX để gửi dữ liệu lên server
    $.ajax({
        url: '/Brand/Create',  // URL đến controller để xử lý tạo mới
        type: 'POST',
        data: brandData,
        success: function (response) {
            if (response.success) {
                alert("Thêm thương hiệu thành công!");
                // Ẩn modal sau khi thành công
                var createModal = bootstrap.Modal.getInstance(document.getElementById('createBrandModal'));
                createModal.hide();
                // Làm mới lại danh sách thương hiệu (tùy vào cách bạn thực hiện load lại dữ liệu)
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
                alert('Thương hiệu đã được xóa thành công!');
                $('#editBrandModal').modal('hide');  // Đóng modal sau khi xóa thành công
                location.reload();  // Tải lại trang để cập nhật
            } else {
                alert(response.message);
            }
        },
    });
}
