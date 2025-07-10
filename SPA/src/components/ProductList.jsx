import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { API_BASE_URL } from '../config';

const ProductList = () => {
    const [groupedProducts, setGroupedProducts] = useState({});
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    useEffect(() => {
        const fetchData = async () => {
            try {
                setLoading(true);
                const [productsResponse, categoriesResponse] = await Promise.all([
                    fetch(`${API_BASE_URL}/productos`),
                    fetch(`${API_BASE_URL}/categorias`)
                ]);

                if (!productsResponse.ok || !categoriesResponse.ok) {
                    throw new Error('Error al obtener los datos del servidor');
                }

                const products = await productsResponse.json();
                const categories = await categoriesResponse.json();

                const grouped = categories.reduce((acc, category) => {
                    const productsInCategory = products.filter(p => p.categoriaId === category.id);
                    if (productsInCategory.length > 0) {
                        acc[category.id] = {
                            ...category,
                            productos: productsInCategory
                        };
                    }
                    return acc;
                }, {});

                setGroupedProducts(grouped);
            } catch (e) {
                setError(e.message);
                console.error("Error fetching data:", e);
            } finally {
                setLoading(false);
            }
        };

        fetchData();
    }, []);

    if (loading) {
        return <div>Cargando productos...</div>;
    }

    if (error) {
        return <div>Error al cargar los productos: {error}</div>;
    }

    return (
        <div className="product-list-container">
            {Object.values(groupedProducts).map(category => (
                <div key={category.id} className="category-section">
                    <h2 className="category-title">{category.nombre}</h2>
                    <p className="category-description">{category.descripcion}</p>
                    <ul className="product-list">
                        {category.productos.map(product => (
                            <li key={product.id} className="product-item">
                                <Link to={`/product/${product.id}`} state={{ product }} className="product-link">
                                    <h3>{product.nombre}</h3>
                                    <p>{product.descripcion}</p>
                                    <p className="price">Precio: ${product.precio.toFixed(2)}</p>
                                    <p>Stock: {product.stock}</p>
                                </Link>
                            </li>
                        ))}
                    </ul>
                </div>
            ))}
        </div>
    );
};

export default ProductList;
