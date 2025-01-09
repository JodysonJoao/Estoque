function Login()
{
    
    fetch('/api/auth/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            username: document.getElementById('username').value,
            password: document.getElementById('password').value
        })
    })
        .then(response => response.json())
        .then(data => {
            if (data.token) {
                localStorage.setItem('authToken', data.token);
                window.location.href = '/paginainicial';
            } else {
                alert('Usuário ou senha inválidos');
            }
        })
}