

//let send = TrySendBid("test", "test@mail.ru", "phone", "tg", "message");


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

