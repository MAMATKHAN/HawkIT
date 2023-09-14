let input = document.querySelectorAll(".form__inner__top__inp__value");
let label = document.querySelectorAll(".form__inner__top__inp__txt");
let btn = document.querySelector(".form__inner__btn");
btn.disabled = true;

console.log('test')
console.log(label)



for (let i = 0; i < input.length; i++) {
    input[i].addEventListener('click', function () {
        label[i].classList.add('label__active');
    });
}



for (let i = 0; i < input.length; i++) {
    input[i].addEventListener('input', () => {
        enableBtn();
    });
}


function enableBtn() {
    let nameIsValid = regCheck(input[0], mark[0], x[0], regName);
    let userNameIsValid = regCheck(input[1], mark[1], x[1], regUser);
    let mailIsValid = regCheck(input[2], mark[2], x[2], regMail);
    let numberIsValid = (regCheck(input[3], mark[3], x[3], regNum) || regCheck(input[3], mark[3], x[3], regNumT2));
    console.log(nameIsValid, userNameIsValid, mailIsValid, numberIsValid);
    if (nameIsValid && userNameIsValid && mailIsValid && numberIsValid) {
        btn.classList.add("form__inner__btn__enable")
        btn.disabled = false;
        console.log('enable');

    } else {
        btn.classList.remove("form__inner__btn__enable")
        btn.disabled = true;
        console.log('disable');
        
    }
}


function regCheck(input, mark, x, regExp) {
    if (input.value.length == 0) {
        return false;
    }

    if (input.value.match(regExp) == null) {
        mark.classList.remove('mark__active');
        x.classList.add('mark__active');

        return false;
    }
    x.classList.remove('mark__active');
    mark.classList.add('mark__active');

    return true

}





document.addEventListener('click', function (e) {
    
    let box = e.composedPath().includes(document.querySelector('#name'));
    if (!box && (input[0].value == "" || input[0].value == null)) {
        label[0].classList.remove('label__active')
    }
})

document.addEventListener('click', function (e) {
    let box = e.composedPath().includes(document.querySelector('#telegram'));
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
    let box = e.composedPath().includes(document.querySelector('#phone'));
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
    loader.style.display = "none";
}

function cleanForm() {
    document.getElementById("name").value = "";
    document.getElementById("email").value = "";
    document.getElementById("phone").value = "";
    document.getElementById("telegram").value = "";
    document.getElementById("message").value = "";

    btn.classList.remove("form__inner__btn__enable")
    btn.disabled = true;

}


/*   работа с регулярными выражениями   */


let x = document.querySelectorAll('.form__inner__top__inp__x')
let mark = document.querySelectorAll('.form__inner__top__inp__mark')

const regName = /^[a-zA-zа-яА-Я]+$/


const regUser = /^[@]([a-zA-zа-яА-Я0-9]{5,})$/

const regMail = /^[a-zA-z][a-zA-Z0-9]+@[a-zA-z]+\.[a-zA-z]+$/

const regNum = /^\+7\d{10}$/
const regNumT2 = /8\d{10}$/




console.log(input[0].value.match(regName))


/*   работа с успешным попатом   */


let succPopat = document.querySelector('.popat__succ');
let succCloseBtn = document.querySelector('.popat__succ__close');
let succBtn = document.querySelector('.popat__succ__btn');


succCloseBtn.addEventListener('click', closeSuccPopat);
succBtn.addEventListener('click', closeSuccPopat);



function closeSuccPopat() {
    succPopat.classList.remove('popat__succ__active');
    setTimeout(function () {
        succPopat.classList.add('popat__succ__anime');
    }, 200);
}


function openSuccPopat() {
    succPopat.classList.remove('popat__succ__anime');
    setTimeout(function () {
        succPopat.classList.add('popat__succ__active');
    }, 200);
}


/*   работа с ошибочным попатом   */


let errPopat = document.querySelector('.popat__err');
let errCloseBtn = document.querySelector('.popat__err__close');
let errBtn = document.querySelector('.popat__err__btn');


errCloseBtn.addEventListener('click', closeErrPopat);
errBtn.addEventListener('click', () => {
    closeErrPopat();
    openForm();
});



function closeErrPopat() {
    errPopat.classList.remove('popat__err__active');
    setTimeout(function () {
        errPopat.classList.add('popat__err__anime');
    }, 200);
}



function openErrPopat() {
    errPopat.classList.remove('popat__err__anime');
    setTimeout(function () {
        errPopat.classList.add('popat__err__active');
    }, 200);
}


//Заявка
async function TrySendBid(name, email, phone, telegram, message) {
    let url = GetHostUrl();
    let response = await fetch(`${url}/Home/TrySendBid?name=${name}&email=${email}&phone=${phone}&telegram=${telegram}&message=${message}`);
    if (response.ok) {
        let text = await response.text();
        if (text == "ok") return true;
    }
    return false;
}

function GetHostUrl() {
    return document.location.protocol + "//" + document.location.host;
}



let loader = document.querySelector('.form__inner__btn__block');


btn.addEventListener("click", () => {
    loader.style.display = 'block';

    let name = document.getElementById("name");
    let email = document.getElementById("email");
    let phone = document.getElementById("phone");
    let telegram = document.getElementById("telegram");
    let message = document.getElementById("message");
    
    let send = TrySendBid(name.value, email.value, phone.value, telegram.value, message.value);
    send.then(function (result) {
        console.log("message is" + result);
        closeForm();
        if (result) openSuccPopat();
        else openErrPopat();
        cleanForm();
    });

});









