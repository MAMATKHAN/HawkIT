
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