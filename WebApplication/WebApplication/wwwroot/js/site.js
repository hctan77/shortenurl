// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function getData(dataAttribute) {
    try {
        return document.getElementById("javascript").getAttribute(dataAttribute);
    }
    catch (err) {
        console.error("Couldn't read data from " + dataAttribute, err);
    }
}

function isEmptyOrSpaces(str) {
    return str === null || str.match(/^ *$/) !== null;
}

function shortenUrl() {
    var longUrl = document.getElementById("inputLongUrl").value;

    if (isEmptyOrSpaces(longUrl)) {
        alert("Invalid long URL.");
    }

    $.ajax({
        url: getData("data-linkApiBaseAddress") + "?url=" + longUrl,
        method: "POST",
        success: function (result) {
            alert("Short URL generated - [" + result + "]");
        },
        fail: function () {
            alert("Fail to generate short URL.");
        }
    });
}

$(document).ready(function () {
    document.getElementById("buttonLongUrl").addEventListener("click", shortenUrl);
});
