﻿
$(".rotate").click(function () {
        $(this).toggleClass("down");
})


// Skickar bild till Modal
$('.getSrc').click(function () {
    var src = $(this).attr('src');
    $('.showPic').attr('src', src);
});

$('.getSrc').click(function () {
    var src = $(this).attr('name');
    $('.modal-title').attr('name', name);
});

(function (d, s, id) {
    var js, fjs = d.getElementsByTagName(s)[0];
    if (d.getElementById(id)) return;
    js = d.createElement(s); js.id = id;
    js.src = 'https://connect.facebook.net/sv_SE/sdk.js#xfbml=1&version=v2.12';
    fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));





       