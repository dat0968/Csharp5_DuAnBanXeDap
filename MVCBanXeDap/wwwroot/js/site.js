
﻿// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
﻿
async function changeWishlistStatus(idProduct, typeObject) {
    const response = await fetch(`/Wishlist/ChangeWishlist?idProduct=${idProduct}&typeObject=${typeObject}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        }
    });

    const result = await response.json();
    if (result.success) {
        toastr.success(result.message);
        // Tùy chỉnh logic cần thiết sau khi thay đổi wishlist thành công
    } else if (result.isLoginAgain) {
        toastr.warning(result.message);
        setTimeout(() => {
            window.location.href = "/Accounts/Login_Customer";
        }, 2000);
    } else {
        toastr.error(result.message);
    }
}

async function isProductInWishlist(idProduct) {
    const response = await fetch(`/Wishlist/IsOneInWishlist?idProduct=${idProduct}`);
    const result = await response.json();
    return result.data;
}

async function areProductsInWishlist(idProducts) {
    const response = await fetch('/Wishlist/IsManyInWishlist', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(idProducts)
    });

    const result = await response.json();
    return result.data;
}

