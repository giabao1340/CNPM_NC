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
})

document.getElementById('edit__save').addEventListener('click', function () {
    // Lấy các giá trị từ form
    const brandName = document.getElementById('brandName').value;
    const image = document.getElementById('image').value;
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

function editOpen(id, name, image, description, url) {
    console.log("Edit Open Called with ID: ", id);
    // Mở modal chỉnh sửa
    var editModal = new bootstrap.Modal(document.getElementById('editBrandModal'));
    document.getElementById('editBrandID').value = id;
    document.getElementById('editBrandName').value = name;
    document.getElementById('editBrandImage').value = image;
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


////1. Thêm mới 
////2. Chỉnh sửa
////3. Xóa
////4. Tìm kiếm gợi ý
////------------------------------------------------/
const createModal = $('#create-modal')
const editModal = $('#edit-modal')
const deleteModal = $('#delete-modal');
let brandID;
$('.dimis-modal').click(function () {
    createModal.modal('hide');
    editModal.modal('hide');
    deleteModal.modal('hide');
});
//1. thêm mới
function createBrand() {
    var brandData = {
        BrandName: $("#createBrandName").val(),
        Image: $("#createBrandImage").val(),
        Description: $("#createBrandDescription").val(),
        Url: $("#createBrandUrl").val()
    };

    $.ajax({
        url: '/Brand/Create',
        type: 'POST',
        data: brandData,
        success: function (response) {
            if (response === "success") {
                alert('Thương hiệu đã được thêm thành công!');
                location.reload(); // Reload page after success
            } else if (response === "exist") {
                alert('Tên thương hiệu đã tồn tại!');
            } else {
                alert('Đã có lỗi xảy ra!');
            }
        },
        error: function () {
            alert('Đã có lỗi xảy ra trong quá trình thêm thương hiệu.');
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
