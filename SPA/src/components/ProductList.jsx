import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { useAuth } from '../hooks/useAuth';
import { API_BASE_URL } from '../config';

const ProductList = () => {
    const { currentUser } = useAuth();
    const [groupedProducts, setGroupedProducts] = useState({});
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [searchTerm, setSearchTerm] = useState('');

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

    const filteredGroupedProducts = Object.values(groupedProducts)
        .map(category => ({
            ...category,
            productos: category.productos.filter(product =>
                product.nombre.toLowerCase().includes(searchTerm.toLowerCase())
            ),
        }))
        .filter(category => category.productos.length > 0);

    const isAdmin = currentUser && currentUser.nombreUsuario === 'admin@gmail.com';

    return (
        <div className="product-list-container">
            {isAdmin && (
                <div className="admin-actions">
                    <Link to="/admin/create-product" className="auth-button">Crear Nuevo Producto</Link>
                </div>
            )}

            <div className="search-container">
                <input
                    type="text"
                    placeholder="Buscar producto por nombre..."
                    className="search-input"
                    value={searchTerm}
                    onChange={e => setSearchTerm(e.target.value)}
                />
            </div>

            {filteredGroupedProducts.length > 0 ? (
                filteredGroupedProducts.map(category => (
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
                ))
            ) : (
                <div className="no-results-message">
                    <h3>No se encontraron productos</h3>
                    <p>Intenta con otra búsqueda o revisa si hay algún error tipográfico.</p>
                </div>
            )}
        </div>
    );
};

export default ProductList;
