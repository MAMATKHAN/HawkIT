let input = document.querySelectorAll(".form__inner__top__inp__value");
let label = document.querySelectorAll(".form__inner__top__inp__txt");

console.log('test')
console.log(label)

for (let i = 0; i < input.length; i++) {
    console.log("1");
    input[i].addEventListener('click', function () {
        label[i].classList.add('label__active');
        console.log("add");
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




