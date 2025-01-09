document.addEventListener('DOMContentLoaded', () => {
    const addProductForm = document.querySelector('.add-product-form');
    const saleForm = document.querySelector('.sale-form');
    const productContainer = document.getElementById('productContainer');
    const saleProductSelect = document.getElementById('sale-product');

    if (!addProductForm || !saleForm) {
        console.error("Formularios não encontrados");
        return;
    }

    const fetchData = async (url, options = {}) => {
        const token = localStorage.getItem('authToken');

        if (!token) {
            console.error('Token não encontrado.');
            alert('Você precisa estar logado para realizar esta ação.');
            return;
        }

        const headers = {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        };

        try {
            const response = await fetch(url, {
                ...options,
                headers: {
                    ...headers,
                    ...options.headers
                }
            });

            if (response.status === 401) {
                alert('Sessão expirada! Faça login novamente.');
                window.location.href = "/login"; 
                return;
            }

            if (!response.ok) throw new Error('Erro ao realizar a operação');
            const text = await response.text();
            return text ? JSON.parse(text) : {};
        } catch (error) {
            console.error('Erro na requisição:', error);
            alert(error.message);
        }
    };



    const updateProductList = async () => {
        const estoque = await fetchData('/api/produtos');
        if (!estoque) return;

        productContainer.innerHTML = '';
        saleProductSelect.innerHTML = '';

        estoque.forEach((produto) => {
            const productBox = document.createElement('div');
            productBox.classList.add('image-box');
            productBox.innerHTML = `
                <img src="${produto.imagem}" alt="${produto.nome}">
                <div class="name">ID: ${produto.id}</div>
                <div class="name">${produto.nome}</div>
                <div class="details">Quantidade: ${produto.quantidade}</div>
                <div class="details">Tamanho: ${produto.tamanho}</div>
                <div class="details">Cor: ${produto.cor}</div>
                <button onclick="deleteProduct(${produto.id})">Excluir</button>
            `;
            productContainer.appendChild(productBox);

            const option = document.createElement('option');
            option.value = produto.id;
            option.textContent = `${produto.nome} - Tamanho: ${produto.tamanho}, Cor: ${produto.cor}`;
            saleProductSelect.appendChild(option);
        });
    };

    addProductForm.addEventListener('submit', async (e) => {
        e.preventDefault();
        const nome = document.getElementById('product-name').value;
        const quantidade = parseInt(document.getElementById('product-quantity').value);
        const tamanho = document.getElementById('product-size').value;
        const cor = document.getElementById('product-color').value;
        const imagemInput = document.getElementById('product-image');

        if (nome && quantidade > 0 && tamanho && cor && imagemInput.files[0]) {
            const reader = new FileReader();
            reader.onload = async function (event) {
                const novoProduto = {
                    nome,
                    quantidade,
                    tamanho,
                    cor,
                    imagem: event.target.result
                };

                await fetchData('/api/produtos', {
                    method: 'POST',
                    body: JSON.stringify(novoProduto)
                });
                addProductForm.reset();
                updateProductList();
            };
            reader.readAsDataURL(imagemInput.files[0]);
        } else {
            alert('Preencha todos os campos corretamente');
        }
    });

    saleForm.addEventListener('submit', async (e) => {
        e.preventDefault();
        const produtoId = parseInt(saleProductSelect.value);
        const quantidadeVenda = parseInt(document.getElementById('sale-quantity').value);

        if (!produtoId || quantidadeVenda <= 0) {
            alert('Dados inválidos para a venda');
            return;
        }

        const produto = await fetchData(`/api/produtos/${produtoId}`);
        if (!produto) {
            alert('Produto não encontrado');
            return;
        }

        if (produto.quantidade < quantidadeVenda) {
            alert('Quantidade insuficiente em estoque');
            return;
        }

        const produtoAtualizado = {
            ...produto,
            quantidade: produto.quantidade - quantidadeVenda
        };

        await fetchData(`/api/produtos/${produtoId}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(produtoAtualizado)
        });

        alert('Venda realizada com sucesso');
        updateProductList();
        saleForm.reset();
    });

    window.deleteProduct = async (id) => {
        const confirmDelete = confirm('Deseja realmente excluir o produto?');
        if (!confirmDelete) return;

        await fetchData(`/api/produtos/${id}`, { method: 'DELETE' });
        updateProductList();
    };

    updateProductList();
});
