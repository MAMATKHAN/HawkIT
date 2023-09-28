let input = document.querySelectorAll(".form__inner__top__inp__value");
let label = document.querySelectorAll(".form__inner__top__inp__txt");
let btn = document.querySelector(".form__inner__btn");
btn.disabled = true;




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
    if (nameIsValid && userNameIsValid && mailIsValid && numberIsValid) {
        btn.classList.add("form__inner__btn__enable")
        btn.disabled = false;
 

    } else {
        btn.classList.remove("form__inner__btn__enable")
        btn.disabled = true;
       
        
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
let body = document.querySelector('body');
let famBackgr = document.querySelector(".fam__backgr");


if (openBtn == null) {
   
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
    body.style.overflow = "hidden";
    form.classList.remove('form__anime');
    setTimeout(function () {
        form.classList.add('form__active');
    }, 200)
    famBackgr.style.display = "block";
}


function closeForm() {
    famBackgr.style.display = 'none'
    form.classList.remove('form__active');
    setTimeout(function () {
        form.classList.add('form__anime');
    }, 200)
    loader.style.display = "none";
    body.style.overflow = "inherit";
}

function cleanForm() {
    document.getElementById("name").value = "";
    document.getElementById("email").value = "";
    document.getElementById("phone").value = "";
    document.getElementById("telegram").value = "";
    document.getElementById("message").value = "";

    btn.classList.remove("form__inner__btn__enable")
    btn.disabled = true;

    x.forEach(item => item.classList.remove('mark__active'));
    mark.forEach(item => item.classList.remove('mark__active'));
    
}


/*   работа с регулярными выражениями   */


let x = document.querySelectorAll('.form__inner__top__inp__x')
let mark = document.querySelectorAll('.form__inner__top__inp__mark')

const regName = /^[a-zA-zа-яА-Я]+$/


const regUser = /^[@]([a-zA-zа-яА-Я0-9]{5,})$/

const regMail = /^[a-zA-z][a-zA-Z0-9]+@[a-zA-z]+\.[a-zA-z]+$/

const regNum = /^\+7\d{10}$/
const regNumT2 = /8\d{10}$/





/*   работа с успешным попатом   */


let succPopat = document.querySelector('.popat__succ');
let succCloseBtn = document.querySelector('.popat__succ__close');
let succBtn = document.querySelector('.popat__succ__btn');


succCloseBtn.addEventListener('click', closeSuccPopat);
succBtn.addEventListener('click', closeSuccPopat);



function closeSuccPopat() {
    famBackgr.style.display = 'none';
    succPopat.classList.remove('popat__succ__active');
    setTimeout(function () {
        succPopat.classList.add('popat__succ__anime');
    }, 200);
    body.style.overflow = "inherit";
}


function openSuccPopat() {
    body.style.overflow = "hidden";
    succPopat.classList.remove('popat__succ__anime');
    setTimeout(function () {
        succPopat.classList.add('popat__succ__active');
    }, 200);
    famBackgr.style.display = "block";
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
    famBackgr.style.display = 'none';
    errPopat.classList.remove('popat__err__active');
    setTimeout(function () {
        errPopat.classList.add('popat__err__anime');
    }, 200);
    body.style.overflow = "inherit";
}



function openErrPopat() {
    body.style.overflow = "hidden";
    errPopat.classList.remove('popat__err__anime');
    setTimeout(function () {
        errPopat.classList.add('popat__err__active');
    }, 200);
    famBackgr.style.display = "block";
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
        closeForm();
        if (result) openSuccPopat();
        else openErrPopat();
        cleanForm();
    });

});


/*   работа с формой и попапами в мобильной версии   */





let formTouch = document.querySelector('.form__title');
let succPopatTouch = document.querySelector('.popat__succ__block');
let errPopatTouch = document.querySelector('.popat__err__block');

let y1 = null;
let way;

/*   ивенты связанный с формой   */

formTouch.addEventListener('touchstart', handleTouch);
formTouch.addEventListener('touchmove', handleMoveForm);
formTouch.addEventListener('touchend', handleEndForm);

/*   ивенты связанный с формой   */

succPopatTouch.addEventListener('touchstart', handleTouch);
succPopatTouch.addEventListener('touchmove', handleMoveSuccPopat);
succPopatTouch.addEventListener('touchend', handleEndSuccPopat);


/*   ивенты связанный с формой   */

errPopatTouch.addEventListener('touchstart', handleTouch);
errPopatTouch.addEventListener('touchmove', handleMoveErrPopat);
errPopatTouch.addEventListener('touchend', handleEndErrPopat);






function handleTouch(event) {
    const firstTouch = event.touches[0];
    y1 = firstTouch.clientY;
}


// Функции связанные с формой


function handleMoveForm(event) {
    if (!y1) {
        return false;
    }
    form.style.transition = "all 0s ease";
    let y2 = event.touches[0].clientY;
    let yDiff = y2 - y1;
    way = 0 - yDiff;
    if (yDiff > 0) {
        form.style.bottom = way.toString() + "px";
    }
}


function handleEndForm() {
    if (way <= -200) {
        form.style.transition = "all 0.4s ease";
        closeFormMobile();
        
        
    } else {
        form.style.transition = "all 0.4s ease";
        form.style.bottom = "0";
    }
    
    
}

// Функции связанные с успешным попатом

function handleMoveSuccPopat(event) {
    if (!y1) {
        return false;
    }
    succPopat.style.transition = "all 0s ease";
    let y2 = event.touches[0].clientY;
    let yDiff = y2 - y1;
    way = 0 - yDiff;
    if (yDiff > 0) {
        succPopat.style.bottom = way.toString() + "px";
    }



}



function handleEndSuccPopat() {
    if (way <= -200) {
        succPopat.style.transition = "all 0.4s ease";
        closePopatSuccMobile();
        

    } else {
        succPopat.style.transition = "all 0.4s ease";
        succPopat.style.bottom = "0";
    }


}

// Функции связанные с провальным попатом


function handleMoveErrPopat(event) {
    if (!y1) {
        return false;
    }
    errPopat.style.transition = "all 0s ease";
    let y2 = event.touches[0].clientY;
    let yDiff = y2 - y1;
    way = 0 - yDiff;
    if (yDiff > 0) {
        errPopat.style.bottom = way.toString() + "px";
    }



}


function handleEndErrPopat() {
    if (way <= -200) {
        errPopat.style.transition = "all 0.4s ease";
        closePopatErrMobile();
        

    } else {
        errPopat.style.transition = "all 0.4s ease";
        errPopat.style.bottom = "0";
    }


}


function closePopatSuccMobile() {
    famBackgr.style.display = 'none'
    succPopat.style.bottom = "-600px";
    succPopat.classList.remove('popat__succ__active');

    setTimeout(function () {
        succPopat.classList.add('popat__succ__anime');
        succPopat.style.bottom = "0";
    }, 200)
    body.style.overflow = "inherit";
}


function closePopatErrMobile() {
    famBackgr.style.display = 'none'
    errPopat.style.bottom = "-600px";
    errPopat.classList.remove('popat__err__active');
    setTimeout(function () {
        errPopat.classList.add('popat__err__anime');
        errPopat.style.bottom = "0";
    }, 200);

    body.style.overflow = "inherit";
}


function closeFormMobile() {
    famBackgr.style.display = 'none'
    form.style.bottom = "-700px";
    form.classList.remove('form__active');
    
    setTimeout(function () {
        form.classList.add('form__anime');
        form.style.bottom = "0";
    }, 200)
    loader.style.display = "none";
    body.style.overflow = "inherit";
}







