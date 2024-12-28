document.addEventListener('DOMContentLoaded', (event) => {
    document.querySelector('form').addEventListener('submit', function (event) {
        event.preventDefault();
    });
});

window.addEventListener('DOMContentLoaded', (event) => {

    const nomedoperfil = document.getElementsByClassName('nomedouser');
    const nomeDoUser = localStorage.getItem('usuario');

    if (nomeDoUser) {
        for (let i = 0; i < nomedoperfil.length; i++) {
            nomedoperfil[i].textContent = nomeDoUser;
        }
    } else {
        console.log('Não fornecido.');
    }
});

function Register() {
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;

    if (username && password) {
        localStorage.setItem('usuario', username);
        localStorage.setItem('senha', password);
    } else {
        if (!username) {
            alert("Por favor, digite o seu nome de usuário.");
        } else if (!password) {
            alert("Por favor, digite a sua senha.");
        } else {
            alert("Por favor, preencha todos os campos.");
        }
    }
}

function Teste() {
    const username = localStorage.getItem('usuario');
    const password = localStorage.getItem('senha');
    console.log("Usuário: " + username, "Senha: " + password);
}

function Login() {
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;
    const storedUsername = localStorage.getItem('usuario');
    const storedPassword = localStorage.getItem('senha');

    if (username === storedUsername && password === storedPassword) {
        window.location.assign('/paginainicial');
    } else {
        alert("Usuário ou senha incorretos. Verifique e tente novamente.");
    }
}

function Logout() {
    localStorage.removeItem('usuario');
    localStorage.removeItem('senha');
    window.location.href = '/register';
}
