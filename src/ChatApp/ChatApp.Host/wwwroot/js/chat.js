"use strict";

function sendMessage() {
    let msg = String(document.getElementById('message').value);

    let url = String(window.location.protocol + "//" + window.location.host + "/api/v1/message/send");

    console.log(msg);

    let data = { text: msg };

    fetch(url, {
        method: "POST",
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(data)
    }).then(res => {
        console.log("Request complete! response:", res);
    });

    document.getElementById('message').value = '';

    event.preventDefault();
}

document.getElementById("sendMessage").addEventListener("click", function () {
    sendMessage();
});

document.getElementById("message").addEventListener("keydown", function (e) {
    if (e.keyCode == 13) {
        sendMessage();
    }
});