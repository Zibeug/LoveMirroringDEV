﻿"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;

connection.on("ReceiveMessage", function (user, message) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " dit " + msg;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    console.log(li.textContent);
    var eElement = document.getElementById("messagesList");
    eElement.insertBefore(li, eElement.firstChild);
});

connection.on("ImageReceive", function (user, message) {
    var li = document.createElement("li");
    var img = document.createElement("img");
    img.src = message;
    var encodedMsg = user + " dit ";
    li.textContent = encodedMsg;
    li.appendChild(img);
    var eElement = document.getElementById("messagesList");
    eElement.insertBefore(li, eElement.firstChild);
});

connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});

document.getElementById("show-all-connections").onclick = function () {
    connection.invoke("GetAllActiveConnectionsAsync")
    connection.on("ReceiveUser", function (userList) {
        while (document.getElementById("user-list").firstChild) {
            document.getElementById("user-list").removeChild(document.getElementById("user-list").firstChild);
        }
        var li = document.createElement("li");
        li.textContent = userList;
        document.getElementById("user-list").appendChild(li);
    });
};
