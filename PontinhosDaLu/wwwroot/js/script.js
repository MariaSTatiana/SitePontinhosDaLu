import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import Home from './components/Home';
import Products from './components/Products';
import Cart from './components/Cart';

function App() {
    return (
        <Router>
            <div className="App">
                <Routes>
                    <Route path="/" element={<Home />} />
                    <Route path="/products" element={<Products/>} />
                    <Route path="/cart" element={<Cart/>} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;

/* Backend (Node.js com Express e MySQL) */

const express = require('express');
const bodyParser = require('body-parser');
const mysql = require('mysql2');

const app = express();
const port = 3000;

// Configuração do MySQL
const db = mysql.createConnection({
    host: 'localhost',
    user: 'root',
    password: 'suaSenha',
    database: 'suaDatabase'
    waitForConnections: true,
    connectionLimit: 10,
    queueLimit: 0
});

db.getConnection((err, connection) => {
    if (err) {
        console.error('Erro ao conectar ao banco de dados MySQL:', err);
        return;
    }
    console.log('Conectado ao banco de dados MySQL');
    connection.release();
});

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));

// Rotas para interagir com o banco de dados
app.get('/api/products', (req, res) => {
    db.query('SELECT * FROM products', (err, result) => {
        if (err) {
            console.error('Erro ao buscar produtos:', err);
            return
            res.status(500).json({ error: 'Erro ao buscar produtos' });
        }
        res.json(result);
    });
});

app.post('/api/addToCart', (req, res) => {
    const { productId, quantity } = req.body;

    if (!productId || !quantity) {
        return res.status(400).json({ error: 'ID do produto e quantidade são obrigatórios' });
    }

    db.query('INSERT INTO cart (product_id, quantity) VALUES (?, ?)', [productId, quantity], (err) => {
        if (err) {
            console.error('Erro ao adicionar ao carrinho:', err);
            return res.status(500).json({ error: 'Erro ao adicionar ao carrinho' });
        }
        res.json({ message: 'Produto adicionado ao carrinho com sucesso!' });
    });
});

// Outras rotas...

app.listen(port, () => {
    console.log(`Servidor rodando na porta ${port}`);
});


/* Integração com API dos Correios (Node.js) */

const calcularFrete = async (peso, dimensoes, destino) => {
    try {
        const response = await axios.post('https://api.melhorenvio.com.br/v2/me/shipment/calculate', {
            from: { postal_code: '01001-000' },
            to: { postal_code: destino },
            weight: peso,
            width: dimensoes.width,
            height: dimensoes.height,
            length: dimensoes.length
        }, {
            headers: { Authorization: 'Bearer SEU_TOKEN' }
        });

        return response.data;
    } catch (error) {
        console.error('Erro ao calcular frete:', error);
        throw error;
    }
};


