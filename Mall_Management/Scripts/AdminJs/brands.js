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
$('#create__open').click(function () {
    createModal.modal('show');
});
$('#create__save').click(function () {
    const name = $('#create__input').val()
    if (name == "") {
        const Toast = Swal.mixin({
            toast: true,
            position: 'top',
            showConfirmButton: false,
            timer: 1500,
            didOpen: (toast) => {
                toast.addEventListener('mouseenter', Swal.stopTimer)
                toast.addEventListener('mouseleave', Swal.resumeTimer)
            }
        })
        Toast.fire({
            icon: 'warning',
            title: 'Nhập tên loại'
        })
        return;
    }
    return $.ajax({
        type: "POST",
        url: '/Brands/Create',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ brandName: name }),
        dataType: "json",
        success: function (result) {
            if (result == "exist") {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top',
                    showConfirmButton: false,
                    timer: 1500,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })
                Toast.fire({
                    icon: 'error',
                    title: 'Tên đã tồn tại'
                })
                return;
            }
            else {
                const Toast = Swal.mixin({
                    toast: true,
                    position: 'top-end',
                    showConfirmButton: false,
                    timer: 1000,
                    didOpen: (toast) => {
                        toast.addEventListener('mouseenter', Swal.stopTimer)
                        toast.addEventListener('mouseleave', Swal.resumeTimer)
                    }
                })
                Toast.fire({
                    icon: 'success',
                    title: 'Thêm thành công'
                })
                setTimeout(function () {
                    window.location.reload();
                }, 500);
                createModalLogin.modal('hide');
                return;
            }
        },
        error: function () {
            const Toast = Swal.mixin({
                toast: true,
                position: 'top-end',
                showConfirmButton: false,
                timer: 1500,
                didOpen: (toast) => {
                    toast.addEventListener('mouseenter', Swal.stopTimer)
                    toast.addEventListener('mouseleave', Swal.resumeTimer)
                }
            })
            Toast.fire({
                icon: 'error',
                title: 'Thêm không thành công'
            })
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
function saveEdit() {
    var brandData = {
        id: $('#brandId').val(),  // ID thương hiệu
        BrandName: $('#brandName').val(),  // Tên thương hiệu
        Image: $('#image').val(),  // Hình ảnh thương hiệu
        Description: $('#description').val(),  // Mô tả
        Url: $('#url').val()  // URL thương hiệu
    };

    console.log(brandData);  // In ra console để kiểm tra dữ liệu

    $.ajax({
        url: '/Brand/Edit',  // URL gửi yêu cầu AJAX
        type: 'POST',
        data: brandData,  // Dữ liệu được gửi lên server
        success: function (response) {
            if (response === 'success') {
                alert('Cập nhật thành công!');
            } else if (response === 'exist') {
                alert('Tên thương hiệu đã tồn tại!');
            } else {
                alert('Có lỗi xảy ra!');
            }
        },
        error: function (xhr, status, error) {
            console.log('Lỗi: ', error);  // In chi tiết lỗi ra console
        }
    });
}


function confirmDelete() {
    var brandID = document.getElementById('deleteBrandID').value;

    // Gửi AJAX request tới controller
    $.ajax({
        
        url: '/Brand/Delete', 
        type: 'POST',
        data: {
            id: brandID
        },
        success: function (response) {
            if (response === "success") {
                alert("Xóa thành công!");
                location.reload();  // Tải lại trang sau khi xóa thành công
            } else {
                alert("Có lỗi xảy ra.");
            }
        },
        error: function () {
            alert("Có lỗi trong quá trình xóa.");
        }
    });

    // Đóng modal sau khi gửi yêu cầu
    var deleteModal = bootstrap.Modal.getInstance(document.getElementById('deleteBrandModal'));
    deleteModal.hide();
}
