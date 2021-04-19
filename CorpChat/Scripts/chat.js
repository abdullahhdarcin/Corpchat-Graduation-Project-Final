$(function () {
    var chat = $.connection.chatHub;
    chat.client.broadcastMessage = function (name, message) {
        var mesaj = "<li><div class='message-data'>" +
            "<span class='message-data-name'>" + name + "</span>" +
            "</div>" +
            "<div class='message my-message'>" + message + "</div></li>";
        $('#discussion').append(mesaj);
    };

    $('#userName').text(prompt('Enter your name:', ''));
    $('#groupName').text(prompt('Enter your group name:', ''));

    $('#message').focus();
    // server bağlantısı başlatılıyor.
    $.connection.hub.start().done(function () {
        console.log("Server'a bağlanıldı.");
        jQuery('#message').keydown(function (event) {
            if (event.keyCode == 13) {  // textarea da enter'a basınca mesajı gönderir.
                if ($('#kime').val() != "") {   // özel mesaj atılıyor.
                    chat.server.sendtouser($('#userName').text(), $('#message').val(), $('#kime').val(), $('#connID').text());
                }
                if ($('#grupMesaj').is(":checked")) {     //grup mesajı atılılıyor.                        
                    chat.server.sendgroup($('#userName').text(), $('#message').val(), $('#groupName').text());
                }
                if ($('#kime').val() == "" && !$('#grupMesaj').is(':checked')) {   // broadcast mesaj atılıyor.
                    chat.server.sendbroadcast($('#userName').text(), $('#message').val());
                }
                $('#message').val('').focus();
                return false;
            }
        });

        $(document).ready(function () {
            $('#connID').text($.connection.hub.id);
            chat.server.join($('#groupName').text());
        });
        $('#grupMesaj').click(function () {
            if ($(this).is(':checked')) {
                document.getElementById("kime").disabled = true;
                $("#kime").val("");
            }
            else {
                document.getElementById("kime").disabled = false;
            }
        });
    });
});