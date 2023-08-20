let comboBtn = document.querySelector('.combobox__inner__btn');
let comboList = document.querySelector('.combobox__list');
let combo = document.querySelector('.combobox');

console.log(comboBtn);

comboBtn.addEventListener('click', function () {
    if (comboList.classList.contains('combobox__list__anime')) {
        console.log('1')
        comboList.classList.remove('combobox__list__anime');
        setTimeout(function () {
            comboList.classList.add('combobox__list__active');
            combo.classList.add('combobox__active');
        }, 100)
    } else {
        comboList.classList.remove('combobox__list__active');
        combo.classList.remove('combobox__active');
        setTimeout(function () {
            comboList.classList.add('combobox__list__anime');
        }, 300)
    }

});