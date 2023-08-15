
/*   анимация мышки   */

console.log('curre');
let scroll = document.querySelector('.intro__mouse__scroll');
console.log('curre');



function animus() {
    scroll.classList.add('intro__mouse__scroll__active');
    setTimeout(function () {
        scroll.classList.remove('intro__mouse__scroll__active');
    }, 400)
}



setInterval(animus, 1200);



let infoBtns = document.querySelectorAll(".info__line__btn");
let infoServ = document.querySelector(".info__service");
let infoTeam = document.querySelector(".info__team");
let infoMiss = document.querySelector(".info__mission");

console.log(infoMiss);

infoBtns[0].addEventListener("click", function () {
    infoBtns[1].classList.remove('info__line__btn__active');
    infoBtns[2].classList.remove('info__line__btn__active');
    infoBtns[0].classList.add('info__line__btn__active');

    infoTeam.classList.remove('info__anime');
    infoMiss.classList.remove('info__anime');
    setTimeout(function () {
        infoTeam.classList.remove('info__active');
        infoMiss.classList.remove('info__active');
    }, 200);

    setTimeout(function () {
        infoServ.classList.add('info__active');
        setTimeout(function () {
            infoServ.classList.add('info__anime');
        }, 100);
    }, 200);
})



infoBtns[1].addEventListener("click", function () {
    infoBtns[0].classList.remove('info__line__btn__active');
    infoBtns[2].classList.remove('info__line__btn__active');
    infoBtns[1].classList.add('info__line__btn__active');

    console.log(infoMiss);

    infoServ.classList.remove('info__anime');
    infoMiss.classList.remove('info__anime');
    setTimeout(function () {
        infoServ.classList.remove('info__active');
        infoMiss.classList.remove('info__active');
    }, 200);

    setTimeout(function () {
        infoTeam.classList.add('info__active');
        setTimeout(function () {
            infoTeam.classList.add('info__anime');
        }, 100);
    }, 200);
})



infoBtns[2].addEventListener("click", function () {
    infoBtns[0].classList.remove('info__line__btn__active');
    infoBtns[1].classList.remove('info__line__btn__active');
    infoBtns[2].classList.add('info__line__btn__active');

    infoServ.classList.remove('info__anime');
    infoTeam.classList.remove('info__anime');
    setTimeout(function () {
        infoServ.classList.remove('info__active');
        infoTeam.classList.remove('info__active');
    }, 200);

    setTimeout(function () {
        infoMiss.classList.add('info__active');
        setTimeout(function () {
            infoMiss.classList.add('info__anime');
        }, 100);
    }, 200);
})