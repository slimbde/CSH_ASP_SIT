// переключение пунктов меню
toggle();



function toggle() {
    //$('#menuTabs').children('li').each(function () { $(this).removeClass('active') })

    page = window.location.pathname.split('/')[1];

    var extra = window.location.pathname.split('/')[2];
    if (extra == "Help" || extra == "Register" || extra == "Login")
        page = extra;

    $('#menu_' + page).addClass('active');
}