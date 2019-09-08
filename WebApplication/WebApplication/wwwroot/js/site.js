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
        error: function (xhr) {
            if (xhr.status === 400) {
                alert("Invalid URL provided. Please provide an absolute URL.");
            }
            else {
                alert("Fail to generate short URL.");
            }
        }
    });
}

function getLongUrl() {
    var shortUrl = document.getElementById("inputShortUrl").value;

    if (isEmptyOrSpaces(shortUrl)) {
        alert("Invalid short URL.");
    }

    $.ajax({
        url: getData("data-linkApiBaseAddress") + shortUrl,
        method: "GET",
        success: function (result) {
            alert("The long URL is [" + result + "]");
        },
        error: function (xhr) {
            if (xhr.status === 404) {
                alert("Long URL not found for " + shortUrl);
            }
            else {
                alert("Failed to get long URL.");
            }
        }
    });
}

$(document).ready(function () {
    document.getElementById("buttonShortenUrl").addEventListener("click", shortenUrl);
    document.getElementById("buttonLongUrl").addEventListener("click", getLongUrl);
});
