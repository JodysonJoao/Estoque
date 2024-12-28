document.addEventListener('DOMContentLoaded', () => {
    const addProductForm = document.querySelector('.add-product-form');
    const saleForm = document.querySelector('.sale-form');
    const productContainer = document.getElementById('productContainer');
    const saleProductSelect = document.getElementById('sale-product');

    if (!addProductForm || !saleForm) {
        console.error("Formulários não encontrados.");
        return;
    }

    const updateProductList = () => {
        fetch('/api/produtos')
            .then((response) => response.json())
            .then((estoque) => {
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
            })
            .catch((error) => {
                console.error('Erro ao carregar produtos:', error);
            });
    };

    addProductForm.addEventListener('submit', (e) => {
        e.preventDefault();

        const nome = document.getElementById('product-name').value;
        const quantidade = parseInt(document.getElementById('product-quantity').value);
        const tamanho = document.getElementById('product-size').value;
        const cor = document.getElementById('product-color').value;
        const imagemInput = document.getElementById('product-image');

        if (nome && quantidade && tamanho && cor && imagemInput.files[0]) {
            const reader = new FileReader();
            reader.onload = function (e) {
                const novoProduto = {
                    nome,
                    quantidade,
                    tamanho,
                    cor,
                    imagem: e.target.result,
                };

                fetch('/api/produtos', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify(novoProduto),
                })
                    .then((response) => {
                        if (!response.ok) {
                            throw new Error('Erro ao adicionar produto.');
                        }
                        return response.json();
                    })
                    .then(() => {
                        updateProductList();
                        addProductForm.reset();
                    })
                    .catch((error) => {
                        console.error(error);
                        alert('Erro ao adicionar produto.');
                    });
            };
            reader.readAsDataURL(imagemInput.files[0]);
        } else {
            alert('Por favor, preencha todos os campos para adicionar um produto.');
        }
    });

    saleForm.addEventListener('submit', (e) => {
        e.preventDefault();

        const produtoId = parseInt(document.getElementById('sale-product').value);
        const quantidadeVenda = parseInt(document.getElementById('sale-quantity').value);

        if (produtoId && quantidadeVenda) {
            fetch('/api/produtos')
                .then((response) => response.json())
                .then((estoque) => {
                    const produto = estoque.find((p) => p.id === produtoId);

                    if (produto) {
                        if (produto.quantidade >= quantidadeVenda) {
                            produto.quantidade -= quantidadeVenda;

                            fetch(`/api/produtos/${produto.id}`, {
                                method: 'PUT',
                                headers: {
                                    'Content-Type': 'application/json',
                                },
                                body: JSON.stringify(produto),
                            })
                                .then(() => {
                                    alert('Venda realizada com sucesso!');
                                    updateProductList();
                                    saleForm.reset();
                                });
                        } else {
                            alert('Quantidade insuficiente em estoque.');
                        }
                    } else {
                        alert('Produto não encontrado.');
                    }
                });
        }
    });

    window.deleteProduct = (id) => {
        fetch(`/api/produtos/${id}`, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
            },
        })
            .then(() => updateProductList())
            .catch((error) => {
                console.error('Erro ao deletar produto:', error);
            });
    };

    updateProductList();
});
