// переключение пунктов меню
toggle();



function toggle() {
    $('#menuTabs').children('li').each(function () { $(this).removeClass('active') })

    page = window.location.pathname.split('/')[1];
    idx = 0;
    switch (page) {
        case 'Sections':
            idx = 0;
            break;
        case 'Staff':
            idx = 1;
            break;
        case 'Home':
            if (window.location.pathname.split('/')[2] == "Help")
                idx = 2;
            break;
        default:
            return;
            break;
    }
    $('#menuTabs').children('li').get(idx).classList.add('active');
}