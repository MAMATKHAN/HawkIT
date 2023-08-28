let input = document.querySelectorAll(".form__inner__top__inp__value");
let label = document.querySelectorAll(".form__inner__top__inp__txt");
let btn = document.querySelector(".form__inner__btn");
btn.disabled = true;

console.log('test')
console.log(label)



for (let i = 0; i < input.length; i++) {
    console.log("1");
    input[i].addEventListener('click', function () {
        label[i].classList.add('label__active');
        console.log("add");
        if (label[0].classList.contains('label__active')){
            document.addEventListener('click', function () {
                if (input[0].value.match(regName) == null || input[0].value.match(regNameSym) != null) {
                    mark[0].classList.remove('mark__active');
                    x[0].classList.add('mark__active');
                } else {
                    console.log('правильно');
                    x[0].classList.remove('mark__active');
                    mark[0].classList.add('mark__active');
                    if (mark[0].classList.contains('mark__active') && mark[1].classList.contains('mark__active') && mark[2].classList.contains('mark__active') && mark[3].classList.contains('mark__active')) {
                        btn.classList.add("form__inner__btn__enable")
                        btn.disabled = false;
                    }
                }
            })
        }

        if (label[1].classList.contains('label__active')) {
            document.addEventListener('click', function () {
                if (input[1].value.match(regUser) == null || input[1].value.match(regUser)[0].length <= 6 || input[1].value.match(regUserSym) != null) {
                    mark[1].classList.remove('mark__active');
                    x[1].classList.add('mark__active');
                } else {
                    x[1].classList.remove('mark__active');
                    mark[1].classList.add('mark__active');
                    if (mark[0].classList.contains('mark__active') && mark[1].classList.contains('mark__active') && mark[2].classList.contains('mark__active') && mark[3].classList.contains('mark__active')) {
                        btn.classList.add("form__inner__btn__enable")
                        btn.disabled = false;
                    }
                }
            })
        }

        if (label[2].classList.contains('label__active')) {
            document.addEventListener('click', function () {
                if (input[2].value.match(regEmail) == null || input[2].value.match(regEmailSym) != null) {
                    mark[2].classList.remove('mark__active');
                    x[2].classList.add('mark__active');
                } else {
                    x[2].classList.remove('mark__active');
                    mark[2].classList.add('mark__active');
                    if (mark[0].classList.contains('mark__active') && mark[1].classList.contains('mark__active') && mark[2].classList.contains('mark__active') && mark[3].classList.contains('mark__active')) {
                        btn.classList.add("form__inner__btn__enable")
                        btn.disabled = false;
                    }
                }
            })
        }

        if (label[3].classList.contains('label__active')) {
            document.addEventListener('click', function () {
                if (input[3].value.match(regNum) == null && input[3].value.match(regNumT2) == null || input[3].value.match(regNumSym) != null) {
                    mark[3].classList.remove('mark__active');
                    console.log(input[3].value.match(regNum))
                    x[3].classList.add('mark__active');
                } else {
                    
                    x[3].classList.remove('mark__active');
                    mark[3].classList.add('mark__active');
                    if (mark[0].classList.contains('mark__active') && mark[1].classList.contains('mark__active') && mark[2].classList.contains('mark__active') && mark[3].classList.contains('mark__active')) {
                        btn.classList.add("form__inner__btn__enable")
                        btn.disabled = false;
                    }
                }
            })
        }
    });
}



document.addEventListener('click', function (e) {
    
    let box = e.composedPath().includes(document.querySelector('#name'));
    if (!box && (input[0].value == "" || input[0].value == null)) {
        label[0].classList.remove('label__active')
    }
})

document.addEventListener('click', function (e) {
    let box = e.composedPath().includes(document.querySelector('#username'));
    if (!box && (input[1].value == "" || input[1].value == null)) {
        label[1].classList.remove('label__active')
    }
})

document.addEventListener('click', function (e) {
    let box = e.composedPath().includes(document.querySelector('#email'));
    if (!box && (input[2].value == "" || input[2].value == null)) {
        label[2].classList.remove('label__active')
    }
})

document.addEventListener('click', function (e) {
    let box = e.composedPath().includes(document.querySelector('#number'));
    if (!box && (input[3].value == "" || input[3].value == null)) {
        label[3].classList.remove('label__active')
    }
})



let openBtn = document.querySelector('.button__type1');
let closeBtn = document.querySelector('.form__title__img');
let form = document.querySelector('.form');
let footerInner = document.querySelector('.footer__inner');

if (openBtn == null) {
    console.log(openBtn);
    footerInner.style.justifyContent = "center";
}




openBtn.addEventListener('click', openForm);
closeBtn.addEventListener('click', closeForm);



document.addEventListener('click', function (e) {
    let box = e.composedPath().includes(form);
    if (!box && form.classList.contains('form__active')) {
        closeForm();
    }
})


function openForm() {
    form.classList.remove('form__anime');
    setTimeout(function () {
        form.classList.add('form__active');
    }, 200)
}


function closeForm() {
    form.classList.remove('form__active');
    setTimeout(function () {
        form.classList.add('form__anime');
    }, 200)
}



/*   работа с регулярными выражениями   */


let x = document.querySelectorAll('.form__inner__top__inp__x')
let mark = document.querySelectorAll('.form__inner__top__inp__mark')

const regName = /[a-zA-zа-яА-Я]{1,}/
const regNameSym = /[^a-zA-zа-яА-Я]/

const regUser = /@([a-zA-zа-яА-Я0-9]{1,})/
const regUserSym = /[^@a-zA-zа-яА-Я0-9]/

const regEmail = /[a-zA-z]{1,}@[a-zA-z]{1,}\.[a-zA-z]{1,}/
const regEmailSym = /^[^a-zA-z]{1,}^@[a-zA-z]{1,}\^.[a-zA-z]{1,}/

const regNum = /\+7\d{10}/
const regNumT2 = /8\d{10}/
const regNumSym = /^\+[^0-9]/



console.log(input[0].value.match(regName))


/*   работа с успешным попатом   */


let succPopat = document.querySelector('.popat__succ');
let succCloseBtn = document.querySelector('.popat__succ__close');
let succBtn = document.querySelector('.popat__succ__btn');


succCloseBtn.addEventListener('click', closeSuccPopat);
succBtn.addEventListener('click', closeSuccPopat);



function closeSuccPopat() {
    succPopat.classList.remove('popat__succ__active');
    setTimeOut(function () {
        succPopat.classList.add('popat__succ__anime');
    }, 200);
}


function openSuccPopat() {
    succPopat.classList.remove('popat__succ__anime');
    setTimeOut(function () {
        succPopat.classList.add('popat__succ__active');
    }, 200);
}


/*   работа с ошибочным попатом   */


let errPopat = document.querySelector('.popat__err');
let errCloseBtn = document.querySelector('.popat__err__close');
let errBtn = document.querySelector('.popat__err__btn');


errCloseBtn.addEventListener('click', closeErrPopat);
errBtn.addEventListener('click', closeErrPopat);



function closeErrPopat() {
    errPopat.classList.remove('popat__err__active');
    setTimeOut(function () {
        errPopat.classList.add('popat__err__anime');
    }, 200);
}


function openErrPopat() {
    errPopat.classList.remove('popat__err__anime');
    setTimeOut(function () {
        errPopat.classList.add('popat__err__active');
    }, 200);
}